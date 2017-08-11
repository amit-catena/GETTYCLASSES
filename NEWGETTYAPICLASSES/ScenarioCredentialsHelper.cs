using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GettyImages.Api;
using Newtonsoft.Json;
using System.Configuration;
using System.Web;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.Web.Http;
using NUnit.Framework;
using TechTalk.SpecFlow;
using GettyImages.Api.Search;

namespace NewGettyAPIclasses
{
    internal class ScenarioCredentialsHelper
    {
        public static ApiClient GetCredentials()
        {


            if (ScenarioContext.Current.ContainsKey("apikey")
                  && ScenarioContext.Current.ContainsKey("apisecret")
                  && ScenarioContext.Current.ContainsKey("refreshtoken"))
            {
                return ApiClient.GetApiClientWithRefreshToken(ScenarioContext.Current.Get<string>("apikey"),
                    ScenarioContext.Current.Get<string>("apisecret"),
                    ScenarioContext.Current.Get<string>("refreshtoken"));
            }

            if (ScenarioContext.Current.ContainsKey("apikey")
                && ScenarioContext.Current.ContainsKey("apisecret")
                && !ScenarioContext.Current.ContainsKey("username"))
            {
                return ApiClient.GetApiClientWithClientCredentials(
                    ScenarioContext.Current.Get<string>("apikey"),
                    ScenarioContext.Current.Get<string>("apisecret"));
            }

            if (ScenarioContext.Current.ContainsKey("apikey")
                && ScenarioContext.Current.ContainsKey("apisecret")
                && ScenarioContext.Current.ContainsKey("username")
                && ScenarioContext.Current.ContainsKey("userpassword"))
            {
                return ApiClient.GetApiClientWithResourceOwnerCredentials(
                    ScenarioContext.Current.Get<string>("apikey"),
                    ScenarioContext.Current.Get<string>("apisecret"),
                    ScenarioContext.Current.Get<string>("username"),
                    ScenarioContext.Current.Get<string>("userpassword"));
            }

            Assert.Fail(
                "Unable to determine which credentials to use. Did you include the proper steps in your scenario?");

            return null;
        }
    }
}
