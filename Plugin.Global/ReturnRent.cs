using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Global
{
    public class ReturnRent : IPlugin
    {
        private const string inputRent = "Rent";
        private const string outputResult = "Result";


        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory serviceFactory =
                   (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            //Service se usa para todas las llamadas al server 
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            // The InputParameters collection contains all the data passed in the message request.  
            EntityReference rent = (EntityReference)context.InputParameters[inputRent];
            try
            {

                //El plugin deberia hacer un retrieve de la entidad renta a traves de su id y logical name.
                //statecode inactivo y statuscode finalizado (campos necesario)
                // y despues actualizarlo
                ColumnSet columns = new ColumnSet("statecode", "statuscode");
                Entity entidad = service.Retrieve(rent.LogicalName, rent.Id, columns);

                entidad.Attributes["statecode"] = new OptionSetValue(1);
                entidad.Attributes["statuscode"] = new OptionSetValue(690200002);

                service.Update(entidad);
                //
                tracingService.Trace("ok");
                context.OutputParameters[outputResult] = true;
            }
            catch (Exception ex)
            {
                context.OutputParameters[outputResult] = false;
                tracingService.Trace($"An error occurred : {ex.Message}");
            }
        }
    }
}
