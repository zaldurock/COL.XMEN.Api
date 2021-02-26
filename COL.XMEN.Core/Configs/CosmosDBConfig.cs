using System.Collections.Generic;

namespace COL.XMEN.Core.Configs
{
    public class CosmosDBConfig
    {

        public const string CosmosConfig = "CosmosDBConfig";
        public string Account { set; get; }
        public string DatabaseName { get; set; }
        public Dictionary<string, string> Containers { get; set; }
    }
}
