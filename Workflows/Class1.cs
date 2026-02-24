using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Query;


namespace MYWORKFLOW
{
    public class Class1 : CodeActivity
    {
        [Input("City Abbreviation")]
        [RequiredArgument]
        public InArgument<string> CityAbbreviation { get; set; }

        [Output("Full City Name")]
        public OutArgument<string> FullCityName { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            // Get the execution context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            // Get the input city abbreviation
            string cityAbbreviation = CityAbbreviation.Get(executionContext);
            tracingService.Trace($"Received city abbreviation: {cityAbbreviation}");
            string fullCity = string.Empty;

            try
            {
                // Query to fetch the city abbreviation from the 'nw_countrydata' table
                QueryExpression query = new QueryExpression("nw_countriesdata")
                {
                    ColumnSet = new ColumnSet("nw_name", "nw_abbrevations")
                };

                // Add filter to match the input abbreviation
                query.Criteria.AddCondition("nw_abbrevations", ConditionOperator.Equal, cityAbbreviation.ToUpper());

                // Execute the query
                EntityCollection result = service.RetrieveMultiple(query);

                // Check if any records were found
                if (result.Entities.Count > 0)
                {
                    Entity cityRecord = result.Entities[0]; // Get the first match
                    fullCity = cityRecord.GetAttributeValue<string>("nw_name");
                    tracingService.Trace($"Mapped {cityAbbreviation} to {fullCity}.");


                    tracingService.Trace($"Mapped.");

                    tracingService.Trace($"Mapped.");

                    tracingService.Trace($"Mapped.");


                }
                else
                {
                    fullCity = "Unknown City";
                    tracingService.Trace($"No matching record found for city abbreviation '{cityAbbreviation}'. Defaulting to 'Unknown City'.");
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace($"Exception: {ex.Message}");
                fullCity = "Error Occurred";
            }

            // Set the output full city name
            FullCityName.Set(executionContext, fullCity);
            tracingService.Trace($"Setting output Full City Name to: {fullCity}");
        }
    }
}





//This is the Modified File