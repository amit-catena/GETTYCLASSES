using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using System.Net;

namespace NewGettyAPIclasses
{
    public class ConstantAPI
    {
        /// <summary>
        /// Gettyimages credentials 
        /// </summary>
        public static readonly string API_KEY = ConfigurationSettings.AppSettings["apikey"];
        public static readonly string API_SecretKEY = ConfigurationSettings.AppSettings["apiscretkey"];
        public static readonly string Username = ConfigurationSettings.AppSettings["username"];
        public static readonly string Password = ConfigurationSettings.AppSettings["password"];
        public static readonly string basicURL = "https://api.gettyimages.com/v3/";
        public static readonly string OauthURL = "https://api.gettyimages.com/oauth2/token/";
        public static readonly string siteURL = ConfigurationSettings.AppSettings["SiteURL"];
        public static readonly string downloadpath = ConfigurationSettings.AppSettings["downloadpath"];

        /// <summary>
        /// Getrate token URL for getty images request
        /// </summary>
        /// <returns></returns>
        public static string Genrate_tokenURL()
        {
            string URL = string.Empty;
            URL = string.Format("client_id={0}&client_secret={1}&grant_type=password&username={2}&password={3}", ConfigurationSettings.AppSettings["apikey"], ConfigurationSettings.AppSettings["apiscretkey"], ConfigurationSettings.AppSettings["username"], ConfigurationSettings.AppSettings["password"]);
            return URL;
        }
        /// <summary>
        /// Refresh getty images URL
        /// </summary>
        /// <returns></returns>
         public static string Genrate_refreshtokenURL()
        {
            string URL = string.Empty;
            URL = string.Format("client_id={0}&client_secret={1}&grant_type=refresh_token&username={2}&password={3}", ConfigurationSettings.AppSettings["apikey"], ConfigurationSettings.AppSettings["apiscretkey"], ConfigurationSettings.AppSettings["username"], ConfigurationSettings.AppSettings["password"]);
            return URL;
        }
        /// <summary>
        /// Genrate client credential URL
        /// </summary>
        /// <returns></returns>
         public static string Genrate_clientcredential()
         {
             string URL = string.Empty;
             URL = string.Format("client_id={0}&client_secret={1}&grant_type=client_credentials", ConfigurationSettings.AppSettings["apikey"], ConfigurationSettings.AppSettings["apiscretkey"]);
             return URL;
         }
        /// <summary>
        /// Genrate image search ULR
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static string Genrate_imagesearchURL(string search)
        {
            string URL = string.Empty;
            URL = string.Format(ConstantAPI.basicURL + "search/images/editorial?fields=thumb,caption&orientations=Horizontal&file_types=jpg&phrase={0}", search);
            return URL;
        }
        /// <summary>
        /// Genrate image search URL event
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>

         public static string Genrate_imagesearchURL_event(string search,int page,int pagesize)
        {
            string URL = string.Empty;
            DateTime dt = System.DateTime.Now;
            string enddate = DateTime.Parse(dt.AddDays(30).ToString()).ToString("yyyy-MM-dd");
            string startdate = DateTime.Parse(dt.AddDays(-2).ToString()).ToString("yyyy-MM-dd");
            URL = string.Format(ConstantAPI.basicURL + "search/events?date_from={1}&date_to={2}&page={3}&page_size={4}&editorial_segment=sport&fields=id,hero_image,image_count,name,start_date{0}", "", startdate, enddate, page, pagesize);
            return URL;
        }

        /// <summary>
        /// Genrate image search with events
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
         public static string Genrate_imagesearchURL_event(int eventid)
         {
             string URL = string.Empty;
             DateTime dt = System.DateTime.Now;
             string enddate = DateTime.Parse(dt.AddDays(10).ToString()).ToString("yyyy-MM-dd");
             string startdate = DateTime.Parse(dt.AddDays(-20).ToString()).ToString("yyyy-MM-dd");
             URL = string.Format(ConstantAPI.basicURL + "search/images/editorial?event_ids={0}&fields=preview,caption,title", eventid);
             return URL;
         }

        /// <summary>
        /// Get constant value for members
        /// </summary>
        public static readonly string strTNsize = ConfigurationSettings.AppSettings["TNsize"];
        public static readonly string strTNTNsize = ConfigurationSettings.AppSettings["TNTNsize"];
        public static readonly string strspacer = ConfigurationSettings.AppSettings["spacerpath"];
        public static readonly string streventurl = ConstantAPI.siteURL + "events.aspx?eventId={0}&siteID=1&NwtID=1&multiple=N";
        public static readonly string downloadsitepath = ConfigurationSettings.AppSettings["downloadsitepath"];
        public static readonly string pixURL = ConfigurationSettings.AppSettings["pixURL"];
        
    }
}
