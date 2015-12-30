using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKVideo.Core.Http
{
    public class HttpProvider : IHttpProvider
    {
        public HttpResponseParameter Excute(HttpRequestParameter requestParameter)
        {
            return HttpUtil.Excute(requestParameter);
        }
    }
}
