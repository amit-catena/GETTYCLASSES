﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using GettyImages.Api;
namespace NewGettyAPIclasses.StepBindings
{
    public class StepDefinitionsBase
    {
        protected ApiClient GetApiClient()
        {
            ApiClient client;
            if (ScenarioContext.Current.ContainsKey("username"))
            {
                client =
                    ApiClient.GetApiClientWithResourceOwnerCredentials(ScenarioContext.Current.Get<string>("apikey"),
                        ScenarioContext.Current.Get<string>("apisecret"),
                        ScenarioContext.Current.Get<string>("username"),
                        ScenarioContext.Current.Get<string>("userpassword"));
            }
            else
            {
                client = ApiClient.GetApiClientWithClientCredentials(ScenarioContext.Current.Get<string>("apikey"),
                    ScenarioContext.Current.Get<string>("apisecret"));
            }
            return client;
        }
    }
}
