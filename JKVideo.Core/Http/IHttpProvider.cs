using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKVideo.Core.Http
{
    public interface IHttpProvider
    {
        HttpResponseParameter Excute(HttpRequestParameter requestParameter);
    }
}
