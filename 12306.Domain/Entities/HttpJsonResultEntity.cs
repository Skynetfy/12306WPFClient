using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _12306.Domain.Entities
{
    public class HttpJsonResultEntity<T>
    {
        [JsonProperty(PropertyName = "validateMessagesShowId")]
        public string ValidateMessagesShowId { get; set; }

        [JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }

        [JsonProperty(PropertyName = "httpstatus")]
        public int Httpstatus { get; set; }

        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }

        [JsonProperty(PropertyName = "messages")]
        public ICollection<string> Messages { get; set; }

        [JsonProperty(PropertyName = "validateMessages")]
        public T ValidateMessages { get; set; }
    }
}
