using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleMvcApp.Models
{
    public class ConnectionDetails
    {
        public string name { get; set; }
        public string[] enabled_clients { get; set; }

    }

    public class ClientDetails
    {
        public string name { get; set; }
        public string client_id { get; set; }
    }

    public class ClientGrantDetails
    {
        public string client_id { get; set; }
        public string audience { get; set; }
    }

    public class FinalResult
    {
        public string Application { get; set; }
        public string Authentication { get; set; }
        public string Authorization { get; set; }
    }
    public class ClientDetailsVM
    {
        public List<ConnectionDetails> ConnectionDetailsCollection { get; set; }
        public List<ClientDetails> ClientDetailsCollection { get; set; }
        public List<ClientGrantDetails> ClientGrantDetailsCollection { get; set; }
        public List<FinalResult> FinalResultCollection { get; set; }
    }
}
