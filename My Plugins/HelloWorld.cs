using System;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk;
using System.Runtime.Remoting.Contexts;

namespace MyPlugins
{
    public class HelloWorld : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                try
                {
                    String firstname = String.Empty;

                    if (entity.Attributes.Contains("firstname"))
                    {
                        firstname = entity.Attributes["firstname"].ToString();
                    }
                    String lastname = entity.Attributes["lasname"].ToString();

                    entity.Attributes.Add("description", "Hello World , HI  " + firstname + lastname);
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("Plugin execution is Invalid", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("My Plugin:{0}", ex.ToString());
                    throw;
                }

            }
        }
    }
}


//this is Hello World Plugin
