using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Configuration;
using System.Net;
using GettyImages.Api;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Configuration;
using System.Threading.Tasks;
namespace newgettyimagesAPI
{
   
    public partial class _default : System.Web.UI.Page
    {
        public string getdata = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (NewGettyAPIclasses.Getimagedata d = new NewGettyAPIclasses.Getimagedata())
                {
                    string data = NewGettyAPIclasses.Getimagedata.Getsearchimages("football");
                    //string data = NewGettyAPIclasses.Getimagedata.Callwebrequestfortoken();
                    //getdata = MakeWebRequest("");
                    //MakeWebRequestfortoken("");
                    //Callwebrequestfortoken();
                    //Response.Write("data" + getdata);  
                }
            }
            catch(Exception ex)
            {
                ex.ToString(); 
            }
        }
        private static string MakeWebRequest(string requestUrl)
        {
            requestUrl = "https://api.gettyimages.com/v3/search/images/editorial?phrase=football&fields=comp";
            var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json charset=UTF-8";
            webRequest.UseDefaultCredentials = true;
            webRequest.Credentials = new NetworkCredential("BettingPro", "gettyimages", "https://api.gettyimages.com/v3/");
            webRequest.Headers.Add("Api-Key", ConfigurationSettings.AppSettings["apikey"]); 
             var jsonSerializer = new JavaScriptSerializer();
            var response = webRequest.GetResponse() as HttpWebResponse;
            string jsonResult;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResult = reader.ReadToEnd();
                reader.Close();
            }
            Debug.WriteLine(jsonResult);
            return jsonSerializer.Deserialize<string>(jsonResult);
        }

        private static string MakeWebRequestfortoken(string requestUrl)
        {
            //requestUrl = "https://api.gettyimages.com/oauth2/token/?client_id=10021&client_secret=ZtqGqIYkvGyqZT0y5ryqwwyyU/xcLoWK9GmS2eZmxvw=&grant_type=password&username=BettingPro&password=gettyimages";
            requestUrl = "https://api.gettyimages.com/oauth2/token/";
            var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            //webRequest.UseDefaultCredentials = true;
            //webRequest.Credentials = new NetworkCredential("BettingPro", "gettyimages", "https://api.gettyimages.com/oauth2/token/");
       
           /* webRequest.Headers.Add("client_id", "10021");
            webRequest.Headers.Add("client_secret", "ZtqGqIYkvGyqZT0y5ryqwwyyU/xcLoWK9GmS2eZmxvw=");
            webRequest.Headers.Add("grant_type", "password");
            webRequest.Headers.Add("username", "BettingPro");
            webRequest.Headers.Add("password", "gettyimages");*/

            var jsonSerializer = new JavaScriptSerializer();
            var response = webRequest.GetResponse() as HttpWebResponse;
            string jsonResult;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResult = reader.ReadToEnd();
                reader.Close();
            }
            Debug.WriteLine(jsonResult);
            return jsonSerializer.Deserialize<string>(jsonResult);
        }


        public string Callwebrequestfortoken()
        {
            string responseData = string.Empty;
            try
            {
                string URLAuth = "https://api.gettyimages.com/oauth2/token/";
                string postString = string.Format("client_id={0}&client_secret={1}&grant_type=password&username={2}&password={3}", 10021, "ZtqGqIYkvGyqZT0y5ryqwwyyU/xcLoWK9GmS2eZmxvw=", "BettingPro","gettyimages");
                const string contentType = "application/x-www-form-urlencoded";
               // System.Net.ServicePointManager.Expect100Continue = false;
                //CookieContainer cookies = new CookieContainer();
                HttpWebRequest webRequest = WebRequest.Create(URLAuth) as HttpWebRequest;
                webRequest.Method = "POST";
                webRequest.ContentType = contentType;
                //webRequest.CookieContainer = cookies;
                webRequest.ContentLength = postString.Length;
                //webRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
               // webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
               // webRequest.Referer = "https://accounts.craigslist.org";
                StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
                requestWriter.Write(postString);
                requestWriter.Close();
                var jsonSerializer = new JavaScriptSerializer();
                var response = webRequest.GetResponse() as HttpWebResponse;
                string jsonResult;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    jsonResult = reader.ReadToEnd();
                    reader.Close();
                }
                dynamic array = JsonConvert.DeserializeObject(jsonResult);
                foreach (var item in array)
                {
                  
                    if((item.Name=="access_token"))
                    {
                        //Response.Write(item.Name + "-:" + item.Value + "</br>");
                        //Getimagesbytoken("",Convert.ToString(item.Value));
                        GetimagesbyImageID("", Convert.ToString(item.Value),"");

                    }
                }

                //List<string> items = jsonSerializer.DeserializeObject<List<string>>(jsonResult);
                //jsonSerializer.Deserialize<string>(jsonResult);
               /* StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
                responseReader.Close();
                webRequest.GetResponse().Close();
                var gettoken = from n in responseData.ToList()
                               select new { };*/

            }
            catch(Exception ex)
            {
                ex.ToString(); 
            }
            return responseData;

        }


        private  void Getimagesbytoken(string requestUrl,string token)
        {
           // requestUrl = "https://api.gettyimages.com/v3/search/images/editorial?phrase=football&fields=preview&file_types=jpg&date_from=2017-01-01&date_to=2017-12-01&orientations=PanoramicHorizontal&editorial_segments=sport";

            //requestUrl = "https://api.gettyimages.com/v3/search/images/editorial?editorial_segments=sport&exclude_nudity=true&fields=thumb&file_types=jpg&minimum_size=large&orientations=Horizontal&product_types=editorial&phrase=football";
           // requestUrl = "https://api.gettyimages.com/v3/search/images/editorial?phrase=football&fields=comp,thumb&file_types=jpg&minimum_size=medium";

            //requestUrl = "https://api.gettyimages.com/v3/search/images/editorial?fields=&phrase=football";

            requestUrl = "https://api.gettyimages.com/v3/search/images/editorial?fields=preview&file_types=jpg&phrase=football";
            var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json charset=UTF-8";
            webRequest.UseDefaultCredentials = true;
           // webRequest.Credentials = new NetworkCredential("BettingPro", "gettyimages", "https://api.gettyimages.com/v3/");
            webRequest.Headers.Add("Api-Key", ConfigurationSettings.AppSettings["apikey"]);
            webRequest.Headers.Add("Bearer", token);
            var jsonSerializer = new JavaScriptSerializer();
            var response = webRequest.GetResponse() as HttpWebResponse;
            string jsonResult;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResult = reader.ReadToEnd();
                reader.Close();
            }
            dynamic array = JsonConvert.DeserializeObject(jsonResult);
            foreach (var item in array)
            {
              if(item.Name=="images")
              {
                  dynamic array2 = item.Value;
                 
                  foreach(var l in array2)
                  {
                      var innerl = l;
                      foreach (var r in innerl)
                      {
                          if (r.Name == "id")
                          {
                              Response.Write("ImageID"+r.Value);
                              GetimagesbyImageID("", token,Convert.ToString(r.Value));
                          }

                          if (r.Name == "display_sizes")
                          {
                              var display = r.Value;
                              foreach( var rr in display)
                              {
                                  //Response.Write(rr["is_watermarked"].Value);
                                  //Response.Write(rr["name"].Value);
                                  Response.Write("<img   src=\"" + rr["uri"].Value + "\"/></br>");
                              }
                             
                          }
                      }
                  }

              }
            }
        }


        private void GetimagesbyImageID(string requestUrl, string token,string IDS)
        {
            IDS ="605699334";
           // requestUrl = "https://api.gettyimages.com/v3/images?ids=" + IDS;
            requestUrl = "https://api.gettyimages.com/v3/downloads/" + IDS;
            var webRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Headers.Add("Api-Key", ConfigurationSettings.AppSettings["apikey"]);
            webRequest.Headers.Add("Bearer", token);
            string postString ="?auto_download=false";
            StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
            requestWriter.Write(postString);
            requestWriter.Close();
            var jsonSerializer = new JavaScriptSerializer();
            var response = webRequest.GetResponse() as HttpWebResponse;
            string jsonResult;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResult = reader.ReadToEnd();
                reader.Close();
            }
            dynamic array = JsonConvert.DeserializeObject(jsonResult);
            foreach (var item in array)
            {
               
            }
        }

        public class imagesdetail
        {

            public int ID { get; set; }
            public string URL { get; set; }
            public string wat { get; set; }
            public string imagename { get; set; }
            public string name { get; set; }
        }


    }
}