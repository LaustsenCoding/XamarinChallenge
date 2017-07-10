using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using XamarinChallengeDemoService.DataObjects;
using XamarinChallengeDemoService.Models;
using Owin;

using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Swagger;
using Swashbuckle.Application;

namespace XamarinChallengeDemoService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new XamarinChallengeDemoInitializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<XamarinChallengeDemoContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);
            ConfigureSwagger(config);
        }
    

        public static void ConfigureSwagger(HttpConfiguration config)
        {
            // Use the custom ApiExplorer that applies constraints. This prevents
            // duplicate routes on /api and /tables from showing in the Swagger doc.
            config.Services.Replace(typeof(IApiExplorer), new MobileAppApiExplorer(config));
            config
               .EnableSwagger(c =>
               {
                   c.SingleApiVersion("v1", "myService");

                       // Tells the Swagger doc that any MobileAppController needs a
                       // ZUMO-API-VERSION header with default 2.0.0
                       c.OperationFilter<MobileAppHeaderFilter>();

                       // Looks at attributes on properties to decide whether they are readOnly.
                       // Right now, this only applies to the DatabaseGeneratedAttribute.
                       c.SchemaFilter<MobileAppSchemaFilter>();
               })
               .EnableSwaggerUi();
        }
    }
    public class XamarinChallengeDemoInitializer : CreateDatabaseIfNotExists<XamarinChallengeDemoContext>
    {
        protected override void Seed(XamarinChallengeDemoContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            base.Seed(context);
        }
    }
}

