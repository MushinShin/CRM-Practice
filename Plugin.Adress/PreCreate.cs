using System;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace Plugins
{
    public class PreCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity adress = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                //Service se usa para todas las llamadas al server 
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
                
                
                tracingService.Trace("Starting plug-in logic");

                try
                {
                    tracingService.Trace("Getting Entity Attributes");
                    string state = adress.GetAttributeValue<string>("custom_state");
                    string city = adress.GetAttributeValue<string>("custom_city");
                    string street = adress.GetAttributeValue<string>("custom_ street");
                    string number = adress.GetAttributeValue<int>("custom_number").ToString();

                    //Buenos Aires,La Plata, Calle 3, 771
                    string name = $"{state}, {city}, {street}, {number}";

                    tracingService.Trace($"Name created: {name}");

                    adress["name"] = name;
                }

            
                catch (Exception ex)
                {
                    tracingService.Trace($"FollowUpPlugin: {ex.Message}");
                    throw new InvalidPluginExecutionException("An error occurred adress PreCreate", ex);
                }
            }
        }
    }
}
