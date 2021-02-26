using Newtonsoft.Json;
using System;

namespace COL.XMEN.Core.Domain
{
    public class Entity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { set; get; }
        public System.DateTimeOffset CreatedDate { set; get; }

        public System.DateTimeOffset? UpdatedDate { set; get; }
    }
}
