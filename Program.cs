using HomeQuest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HomeQuest.Services;
using HomeQuest.Models.Settings;
using System.Configuration;
using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stripe;
using HomeQuest.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HomeQuestDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("HomeQuestConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>
            (options => options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddEntityFrameworkStores<HomeQuestDbContext>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<HomeQuest.Services.IMailService, HomeQuest.Services.MailService>();
builder.Services.AddTransient<IEmailSender, HomeQuest.Services.MailService>();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

//add new authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAgentRole", policy => policy.RequireRole("Agent"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// these are middlewares in the pipeline

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope()) {
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    SeedUsersAndRoles(userManager, roleManager);
}

app.Run();

void SeedUsersAndRoles(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    Action<IdentityResult> handleResult = result => 
    {
        foreach (var error in result.Errors)
        {
            app.Logger.LogError("Error seeding users and roles: " + error.Description);
        }
        if (!result.Succeeded) {
            Environment.Exit(1);
        }
    };

    string[] roleNamesList = new String[] { "User", "Admin", "Agent" };
    foreach (var roleName in roleNamesList)
    {
        if (!roleManager.RoleExistsAsync(roleName).Result)
        {
            IdentityRole role = new IdentityRole();
            role.Name = roleName;
            handleResult(roleManager.CreateAsync(role).Result);
        }
    }

    // Create an Administrator account
    string defaultAdminEmail = "admin@homequest.com";
    string defaultAdminPass = "Admin123!";
    if (userManager.FindByNameAsync(defaultAdminEmail).Result == null)
    {
        ApplicationUser user = new ApplicationUser();
        user.UserName = defaultAdminEmail;
        user.Email = defaultAdminEmail;
        user.EmailConfirmed = true;
        handleResult(userManager.CreateAsync(user, defaultAdminPass).Result);
        handleResult(userManager.AddToRoleAsync(user, "Admin").Result);
    }

}