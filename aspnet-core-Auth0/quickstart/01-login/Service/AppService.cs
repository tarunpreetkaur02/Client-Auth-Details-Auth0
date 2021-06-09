using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace SampleMvcApp.Service
{
    public interface IAppService
    {
      
        public IRestResponse getClientsResponse(String token, String domain);
        public IRestResponse getConnectionsResponse(String token,String domain);
        public IRestResponse getGrantsResponse(String token, String domain);  

    }
    public class AppService : IAppService
    {
        //Put the value of your domain 
             
        public IRestResponse getClientsResponse(String token, String domain)
        {
            var client1 = new RestClient("https://"+ domain + "/api/v2/clients");
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("authorization", "Bearer " + token);
            IRestResponse response1 = client1.Execute(request1);
            return response1;
        }
        public IRestResponse getConnectionsResponse(String token, String domain)
        {
            var client2 = new RestClient("https://"+ domain + "/api/v2/connections");
            var request2 = new RestRequest(Method.GET);
            request2.AddHeader("authorization", "Bearer " + token);
            IRestResponse response2 = client2.Execute(request2);
            return response2;
        }
        public IRestResponse getGrantsResponse(String token, String domain)
        {
            var client3 = new RestClient("https://" + domain + "/api/v2/client-grants");
            var request3 = new RestRequest(Method.GET);
            request3.AddHeader("authorization", "Bearer " + token);
            IRestResponse response3 = client3.Execute(request3);
            return response3;
        }
    }
}
