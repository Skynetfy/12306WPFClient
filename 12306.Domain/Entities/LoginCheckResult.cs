using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _12306.Domain.Entities
{
    public class LoginCheckResult
    {
        [JsonProperty(PropertyName = "flag")]
        public bool Flag { get; set; }
    }
}
