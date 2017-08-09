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
    /// Show getty image collection events
    /// </summary>
    public partial class events : System.Web.UI.Page
    {
        /// <summary>
        /// Public members
        /// </summary>
        public StringBuilder strtext = new StringBuilder();
        public string strmultiple = string.Empty;
        public int _networkid = 1;
        public int intsiteId = 0;
        public int inteventid = 0;
        public string strnetwork = string.Empty;
        /// <summary>
        /// On page load bind getty image events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (null != Request.QueryString["NwtID"])
                {
                    strnetwork = Convert.ToString(Request.QueryString["NwtID"]);
                }
                if (null != Request.QueryString["SiteId"])
                {
                    intsiteId = Convert.ToInt32(Request.QueryString["SiteId"].ToString());
                }
                if (null != Request.QueryString["multiple"])
                {
                    strmultiple = Convert.ToString(Request.QueryString["multiple"]);
                }
                else
                {
                    strmultiple = "N";
                }
                using (NewGettyAPIclasses.Getimagedata d = new NewGettyAPIclasses.Getimagedata())
                {
                    if (Request.QueryString["eventId"] == null)
                    {

                        strtext = d.Getimagesbytoken_events("", "", 1);
                    }
                    else
                    {
                        inteventid = Convert.ToInt32(Request.QueryString["eventId"]);
                        strtext = d.Getimagesbytoken_events("", "", 1, inteventid,"N");

                    }
                }
                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}