using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CRMLeadCaptureExample.Controllers
{
    public class CRMLeadController : ApiController
    {
        private OrganizationService _orgService;

        [EnableCors(origins: "*", headers: "*", methods: "post")]
        public string Post([FromBody] FormDataCollection formValues)
        {
            string domain = HttpContext.Current.Request.Headers["Origin"].ToLower();
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            //if (!domain.Contains("jlleadform.azurewebsites.net") && !domain.Contains(host))
            //    return "fail!";

            CrmConnection connection = CrmConnection.Parse(
                ConfigurationManager.ConnectionStrings["CRMOnlineO365"].ConnectionString);

            using (_orgService = new OrganizationService(connection))
            {
                Entity lead = new Entity("lead");
                lead["firstname"] = formValues.Get("FirstName");
                lead["lastname"] = formValues.Get("LastName");

                _orgService.Create(lead);
            }
            return "success";
        }
    }
}
