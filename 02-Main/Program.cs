using Microsoft.AspNetCore.OutputCaching;
using StairwayDesigns.Services;
using StairwayDesigns.Models;
using Polly;
using Polly.Registry;
using System.IO;

namespace StairwayDesigns;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        
        // Configure AppConfig from appsettings.json
        builder.Services.Configure<AppConfig>(options =>
        {
            var appConfig = builder.Configuration.GetSection("AppConfig");
            options.OnrEmail = appConfig["OwnerEmailID"] ?? "";
            options.AppOwner = appConfig["CompnayName"] ?? "";
            options.SourceID = appConfig["SubscriptionID"] ?? "";
        });

        // Register the AppConfig as IAppConfig
        builder.Services.AddScoped<IAppConfig, AppConfig>();

        // Add resilience pipeline for email operations
        builder.Services.AddResiliencePipeline("default", pipeline =>
        {
            pipeline.AddRetry(new Polly.Retry.RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = Polly.DelayBackoffType.Exponential
            })
            .AddTimeout(TimeSpan.FromMinutes(2));
        });

        // Register JSON formatter
        builder.Services.AddScoped(typeof(IFormatJSON<>), typeof(FormatJSON<>));

        // Register email service
        builder.Services.AddScoped<INotifyUser, NotifyUser>();

        // Add HTTP client for SMS and other HTTP operations
        builder.Services.AddHttpClient();

        // (Optional) Live reload services can be added here if package installed.

        // Add output caching only outside Development so Hot Reload & rapid edits are visible
        if (!builder.Environment.IsDevelopment())
        {
            builder.Services.AddOutputCache(options =>
            {
                options.AddBasePolicy(policy => policy
                    .Expire(TimeSpan.FromMinutes(5))
                    .SetVaryByRouteValue("page"));
            });
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            // No aggressive static file caching in Dev so changes show immediately
            app.UseStaticFiles();
        }
        else
        {
            // Add static file middleware with aggressive client caching for css/js/images in non-Dev
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    var ext = Path.GetExtension(ctx.File.Name).ToLowerInvariant();
                    if (ext is ".css" or ".js" or ".png" or ".jpg" or ".jpeg" or ".gif" or ".svg" or ".webp" or ".ico")
                    {
                        const int days = 30; // cache duration
                        ctx.Context.Response.Headers["Cache-Control"] = $"public,max-age={days * 24 * 60 * 60},immutable";
                        ctx.Context.Response.Headers["Expires"] = DateTime.UtcNow.AddDays(days).ToString("R");
                    }
                }
            });
        }

        app.UseRouting();

        app.UseAuthorization();

        // Enable output caching middleware only if registered
        if (!app.Environment.IsDevelopment())
        {
            app.UseOutputCache();
        }

        // Map Razor Pages (apply output cache metadata only outside Development)
        var pages = app.MapRazorPages();
        if (!app.Environment.IsDevelopment())
        {
            pages.WithMetadata(new OutputCacheAttribute());
        }

        app.Run();
    }
}
