using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.SqlServer.Server;

namespace Tester
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger("Tester");

        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback +=
    (sender, cert, chain, sslPolicyErrors) => true;

            Logger.Info("Begin Tester");

            while (true)
            {
                GetValues();
                Thread.Sleep(5000);
            }
        }

        private static void GetValues()
        {
            
            var client = new HttpClient();
            var stopwatch = new Stopwatch();

            var request = new HttpRequestMessage { RequestUri = new Uri(ConfigurationManager.AppSettings["url"]) };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                stopwatch.Start();
                using (var response = client.SendAsync(request).Result)
                {
                    stopwatch.Stop();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Logger.Info("Pass");
                    }
                    else
                    {
                        Logger.Error(string.Format("Fail {0}:{1}", response.StatusCode, response.RequestMessage));
                    }
                }
            }
            catch (Exception exc)
            {
                stopwatch.Stop();
                Logger.Error(exc);
            }
            finally
            {
                stopwatch.Stop();
                Logger.Info(string.Format("Duration: {0} ms", stopwatch.ElapsedMilliseconds));
            }
        }
    }
}
