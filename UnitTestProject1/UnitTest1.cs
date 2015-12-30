using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;
using JKVideo.Core.Common;
using JKVideo.Core.Entities;
using JKVideo.Core.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using _12306.Domain.Entities;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var httpProvider = new HttpProvider();

            var url =
                string.Format(
                    "https://kyfw.12306.cn/otn/leftTicket/queryT?leftTicketDTO.train_date={0}&leftTicketDTO.from_station={1}&leftTicketDTO.to_station={2}&purpose_codes={3}", "2016-02-04", "SHH", "FYH", "ADULT");
            HttpResponseParameter responseParameter = httpProvider.Excute(new HttpRequestParameter
            {
                Url = url,
                IsPost = false
            });

            HttpJsonResult<QeryTicketsData> loginResult = responseParameter.Body.DeserializeObject<HttpJsonResult<QeryTicketsData>>();
        }

        [TestMethod]
        public void Method2()
        {
            using (FileStream fs = new FileStream(@"C:\Users\zhangzhiqiang\Documents\Tencent Files\369552841\FileRecv\Sample22.txt", FileMode.Open, FileAccess.Read))
            {
                StreamReader sr = new StreamReader(fs);
                var json = sr.ReadToEnd();
                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(json);

                doc.Save(@"E:\123.xml");
            }
        }
    }
}
