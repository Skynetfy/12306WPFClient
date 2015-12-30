using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKVideo.Core.Entities
{
    public class LoginResultEntity
    {
        public int status { get; set; }
        public string msg { get; set; }
        public string jumpUrl { get; set; }
    }
}
