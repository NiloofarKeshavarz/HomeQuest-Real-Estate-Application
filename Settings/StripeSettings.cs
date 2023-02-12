namespace HomeQuest.Settings
{
    public class StripeSettings
    {
        public string PublishableKey { get; set; }
        public string SecretKey { get; set; }
        public string WebhookSecret { get; set; }
        public string MonthlyPriceId { get; set; }
        public string YearlyPriceId { get; set; }
    }
}
