using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace NewGettyAPIclasses
{
    [Binding]
    public class SharedSteps
    {
        [Given(@"I have an apikey")]
        [Given(@"an api key")]
        public void GivenIHaveAnApiKey()
        {
            ScenarioContext.Current.Set(Environment.GetEnvironmentVariable("GettyImagesApi_ApiKey"), ConstantAPI.API_KEY);
        }


        [Given(@"an apisecret")]
        [Given(@"an api secret")]
        public void GivenAnApisecret()
        {
            ScenarioContext.Current.Set(Environment.GetEnvironmentVariable("GettyImagesApi_ApiSecret"), ConstantAPI.API_SecretKEY);
        }

        [Given(@"a username")]
        [Given(@"a user name")]
        public void GivenAUsername()
        {
            ScenarioContext.Current.Set(Environment.GetEnvironmentVariable("GettyImagesApi_UserName"), ConstantAPI.Username);
        }

        [Given(@"a password")]
        [Given(@"a user password")]
        public void GivenAPassword()
        {
            ScenarioContext.Current.Set(Environment.GetEnvironmentVariable("GettyImagesApi_UserPassword"),
                ConstantAPI.Password);
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
