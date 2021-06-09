using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SampleMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SampleMvcApp.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace SampleMvcApp.Controllers
{
    public class HomeController : Controller
    {
        string token;
        List<ConnectionDetails> connDetails;
        List<ClientDetails> clientDetails;
        List<ClientGrantDetails> clientGrantDetails;
        IAppService service1;
        private IConfiguration configuration;
        
        public HomeController(IAppService service, IConfiguration config)
        {
            service1 = service;
            configuration = config;
        }
      
        public async Task<IActionResult> Index()
        {
            ClientDetailsVM model = new ClientDetailsVM();
            // If the user is authenticated, then this is how you can get the access_token and id_token
            if (User.Identity.IsAuthenticated)
            {
                //Fetching the values of domain, client_id, client_secret and audience from appsettings.json file
                var domain = configuration.GetSection("Auth0").GetSection("Domain").Value;
                var client_id = configuration.GetSection("Auth0").GetSection("ClientId").Value;
                var client_secret = configuration.GetSection("Auth0").GetSection("ClientSecret").Value;
                var aud = configuration.GetSection("Auth0").GetSection("Audience").Value;

                //Making the call to get the access token
                var client = new RestClient("https://" + domain + "/oauth/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", "{\"client_id\":\"" + client_id + "\",\"client_secret\":\"" + client_secret + "\",\"audience\":\"" + aud + "\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                JObject jObject = JObject.Parse(response.Content);
                JToken jUser = jObject;
                token = (string)jUser["access_token"];
               
                //Call to get clientDetails 
                IRestResponse response1 = service1.getClientsResponse(token,domain);
                clientDetails = JsonConvert.DeserializeObject<List<ClientDetails>>(response1.Content);

                //Call to get connections 
                IRestResponse response2 = service1.getConnectionsResponse(token,domain);
                connDetails = JsonConvert.DeserializeObject<List<ConnectionDetails>>(response2.Content);

                //Call to get grants
                IRestResponse response3 = service1.getGrantsResponse(token,domain);
                clientGrantDetails = JsonConvert.DeserializeObject<List<ClientGrantDetails>>(response3.Content);

                List<FinalResult> details = new List<FinalResult>();

                //Iterating the rows in clientDetails
                foreach (var item in clientDetails)
                {
                    FinalResult result1 = new FinalResult();
                    result1.Application = item.name;
                    
                    IEnumerable<string> authnType = from connDetail in connDetails
                                                    where connDetail.enabled_clients.Contains(item.client_id)
                                                    select connDetail.name;
                    result1.Authentication = authnType.Count() == 0 ? null : string.Join(", ", authnType);
                    var authType = from grant in clientGrantDetails
                                   where grant.client_id == item.client_id
                                   select grant.audience;
                    result1.Authorization = authType.Count() == 0 ? null : string.Join(", ", authType);
                    details.Add(result1);
                }
                model.FinalResultCollection = details;
            }

            return View(model);
        }

        public IActionResult Error()
        {
            string errorMessage = "Something went wrong. Please have a look at the logs under Monitoring on the Auth0 dashboard for more details.";
            if (this.Response.StatusCode == StatusCodes.Status401Unauthorized|| this.Response.StatusCode == StatusCodes.Status500InternalServerError)
            {
                //custom error message goes here.
                errorMessage = "An error occured. Either you are not authorized or Internal Server occured. Please have a look at the logs under Monitoring on the Auth0 dashboard for more details.";
            }         
            return Content(errorMessage);
        }
    }
}


