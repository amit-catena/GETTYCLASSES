using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Text;
using GettyImages.Api.Search;
using System.Net.Http;
using GettyImages.Api;
using Newtonsoft.Json;

namespace newgettyimagesAPI
{
    /// <summary>
    /// Index page 
    /// </summary>
    public partial class index : System.Web.UI.Page
    {
        public StringBuilder strtext = new StringBuilder();
        public int _networkid = 1;
        public int intsiteId = 0;
        public string strnetwork = string.Empty;
        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (null != Request.QueryString["NwtID"])
                {
                    strnetwork =Convert.ToString(Request.QueryString["NwtID"]);
                }

                if (null != Request.QueryString["SiteId"])
                {
                    intsiteId = Convert.ToInt32(Request.QueryString["SiteId"].ToString());
                }

                using (NewGettyAPIclasses.Getimagedata d = new NewGettyAPIclasses.Getimagedata())
                {
                    strtext = await NewGettyAPIclasses.Getimagedata.AccessTheWebAsync("",1,"N");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        async Task<string> AccessTheWebAsync()
        {
            // You need to add a reference to System.Net.Http to declare client.
              HttpClient client = new HttpClient();
            // GetStringAsync returns a Task<string>. That means that when you await the 
            var apikey = NewGettyAPIclasses.ConstantAPI.API_KEY;
            var apiscretkey = NewGettyAPIclasses.ConstantAPI.API_SecretKEY; 
            var clienttask =ApiClient.GetApiClientWithResourceOwnerCredentials(apikey, apiscretkey, NewGettyAPIclasses.ConstantAPI.Username, NewGettyAPIclasses.ConstantAPI.Password);
            var phraseToSearchFor = string.Empty;
            //var results =await clienttask.GetAccessToken();// .PostAsync(NewGettyAPIclasses.ConstantAPI.OauthURL + NewGettyAPIclasses.ConstantAPI.Genrate_tokenURL()); 
            var tokenstr = await clienttask.GetAccessToken();
            var token=tokenstr.AccessToken;
            var results = await clienttask.Search().Images().Editorial().WithResponseField("comp").WithPhrase("tennis").WithPageSize(100).ExecuteAsync();
            phraseToSearchFor = JsonConvert.SerializeObject(results, Formatting.Indented);
            // You can do work here that doesn't rely on the string from GetStringAsync.
            string urlContents = ""; 
            // The return statement specifies an integer result. 
            Response.Write(phraseToSearchFor);  
            return urlContents;
        }


       
    }
}