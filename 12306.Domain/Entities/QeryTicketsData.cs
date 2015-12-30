using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _12306.Domain.Entities
{
    public class QeryTicketsData
    {
        [JsonProperty(PropertyName = "secretStr")]
        public string SecretStr { get; set; }

        [JsonProperty(PropertyName = "buttonTextInfo")]
        public string ButtonTextInfo { get; set; }

        [JsonProperty(PropertyName = "queryLeftNewDTO")]
        public QueryLeftNewDTO QueryLeftNewDTO { get; set; }
    }
}
