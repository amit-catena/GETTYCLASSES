﻿using System;
using System.Threading.Tasks;
using NUnit.Framework;
using TechTalk.SpecFlow;
using GettyImages.Api; 
namespace NewGettyAPIclasses.StepBindings
{
    [Binding]
    public class SharedSteps
    {
        [Given(@"I have an apikey")]
        [Given(@"an api key")]
        public void GivenIHaveAnApiKey()
        {
            ScenarioContext.Current.Set(ConstantAPI.API_KEY, "apikey");
        }


        [Given(@"an apisecret")]
        [Given(@"an api secret")]
        public void GivenAnApisecret()
        {
            ScenarioContext.Current.Set(ConstantAPI.API_SecretKEY, "apisecret");
        }

        [Given(@"a username")]
        [Given(@"a user name")]
        public void GivenAUsername()
        {
            ScenarioContext.Current.Set(ConstantAPI.Username, "username");
        }

        [Given(@"a password")]
        [Given(@"a user password")]
        public void GivenAPassword()
        {
            ScenarioContext.Current.Set(ConstantAPI.Password,
                "userpassword");
        }

        [Then(@"the status is success")]
        public void ThenTheStatusIsSuccess()
        {
            var task = ScenarioContext.Current["task"] as Task<dynamic>;

            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("NoAccessToProductType"))
                {
                    Assert.Pass("User does not have the correct agreement, but API responded properly");
                }
                else
                {
                    throw;
                }
            }
            Assert.Pass();
        }
    }
}