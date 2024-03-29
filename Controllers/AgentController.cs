using System;
using System.Security.Policy;
using HomeQuest.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;


namespace HomeQuest.Controllers
{
    public class AgentController : Controller
    {
        private readonly ILogger<AgentController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LinkGenerator _linkGenerator;
        private readonly StripeSettings _settings;
        private readonly IStripeClient _client;

        public AgentController(ILogger<AgentController> logger, UserManager<ApplicationUser> userManager, LinkGenerator linkGenerator, IOptions<StripeSettings> settings)
        {
            _logger = logger;
            _userManager = userManager;
            _linkGenerator = linkGenerator;
            _settings = settings.Value;
            _client = new StripeClient(_settings.SecretKey);

        }

        public async Task<IActionResult> Subscription(string? sessionId)
        {

            var user = await _userManager.GetUserAsync(User);
            var expirationDate = user.AgentExpirationDate;
            if (sessionId != null)
            {
                var session = await new SessionService(_client).GetAsync(sessionId);
                if (session == null)
                {
                    return NotFound();
                }
                var subscription = await new SubscriptionService(_client).GetAsync(session.SubscriptionId);
                expirationDate = subscription.CurrentPeriodEnd;
            }
            ViewData["SessionId"] = sessionId;
            ViewData["IsActive"] = expirationDate > DateTime.Now;
            ViewData["ExpirationDate"] = expirationDate;
            ViewData["MonthlyPriceId"] = _settings.MonthlyPriceId;
            ViewData["YearlyPriceId"] = _settings.YearlyPriceId;
            return View();
        }

        public async Task<IActionResult> Subscribe(string priceId)
        {
            var user = await _userManager.GetUserAsync(User);
            // To ensure proper recognition by Stripe, it is important to add the sessionId parameter as a trailing string to the URL generated by GetUriByAction. 
            // The {} characters must not be URL encoded, otherwise they will not be recognized.
            // {CHECKOUT_SESSION_ID} is a string literal; do not change it!
            // the actual Session ID is returned in the query parameter when your customer is redirected to the success page.
            var successUrl = _linkGenerator.GetUriByAction(HttpContext, "Subscription", "Agent") + "?sessionId={CHECKOUT_SESSION_ID}";
            var cancelUrl = _linkGenerator.GetUriByAction(HttpContext, "Subscription", "Agent");
            var options = new SessionCreateOptions
            {
                // See https://stripe.com/docs/api/checkout/sessions/create for additional parameters to pass.
                CustomerEmail = user.Email,
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                Mode = "subscription",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = priceId,
                        Quantity = 1,
                    },
                },
            };

            var service = new SessionService(_client);
            try
            {
                var session = await service.CreateAsync(options);

                // Redirect to the URL returned on the Checkout Session.
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            catch (StripeException e)
            {
                _logger.LogError(e.StripeError.Message, e);
                return BadRequest(e.StripeError.Message);
            }
        }

        [HttpPost("/webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _settings.WebhookSecret
                );
                // _logger.LogInformation($"Webhook notification with type: {stripeEvent.Type} found for {stripeEvent.Id}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Something failed {e}");
                return BadRequest();
            }
            
            switch (stripeEvent.Type)
            {
                case "customer.subscription.updated":
                    // Triggered when an existing subscription is updated. This event will give you the new subscription information, including the updated "CurrentPeriodEnd" date.
                    var subscription = stripeEvent.Data.Object as Stripe.Subscription;
                    if (subscription.Customer == null)
                    {
                        subscription.Customer = await new CustomerService(_client).GetAsync(subscription.CustomerId);
                    }
                    _logger.LogInformation($"Webhook notification {stripeEvent.Type} for User: {subscription.Customer.Email} with End Date: {subscription.CurrentPeriodEnd} == Stripe Subscription ID: {subscription.Id}");
                    var user = await _userManager.FindByEmailAsync(subscription.Customer.Email);
                    if (user == null)
                    {
                        _logger.LogError($"Webhook notification {stripeEvent.Type} - Unable to load user with Email '{subscription.Customer.Email}'.");
                        return BadRequest();
                    }
                    user.AgentExpirationDate = subscription.CurrentPeriodEnd;
                    await _userManager.UpdateAsync(user);
                    break;
                case "invoice.payment_failed":
                    // The payment failed or the customer does not have a valid payment method.
                    // The subscription becomes past_due. Notify your customer and send them to the
                    // customer portal to update their payment information.
                    // Triggered when a recurring payment fails. This event will give you information about the failed payment, including the reason for failure. You can use this information to take appropriate action, such as sending a notification to the user.
                    break;
            }

            return Ok();
        }

        public async Task<IActionResult> Billing()
        {
            var user = await _userManager.GetUserAsync(User);
            var customer = (await new CustomerService(_client).ListAsync(new CustomerListOptions { Email = user.Email })).FirstOrDefault();
            if (customer == null)
            {
                return NotFound();
            }


            // This is the URL to which your customer will return after
            // they are done managing billing in the Customer Portal.
            string? returnUrl = _linkGenerator.GetUriByAction(HttpContext, "Index", "Home"); // TODO: Agent Dashboard
            if (Request.Headers.ContainsKey("Referer"))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            var options = new Stripe.BillingPortal.SessionCreateOptions
            {
                Customer = customer.Id,
                ReturnUrl = returnUrl,
            };
            var session = await new Stripe.BillingPortal.SessionService(_client).CreateAsync(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }
}
