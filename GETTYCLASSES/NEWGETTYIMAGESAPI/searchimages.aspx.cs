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
    /// Searimages for gettyimages 
    /// </summary>
    public partial class searchimages : System.Web.UI.Page
    {
        /// <summary>
        /// public members
        /// </summary>
        public StringBuilder strtext = new StringBuilder();
        public string strmultiple = string.Empty;
        public int _networkid = 1;
        public int intsiteId = 0;
        public string strliveupdate = string.Empty;
        public string strnetwork = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null != Request.QueryString["multiple"])
            {
                strmultiple = Convert.ToString(Request.QueryString["multiple"]);
            }else
            {
                strmultiple = "N";
            }
            if (null != Request.QueryString["NwtID"])
            {
                strnetwork = Convert.ToString(Request.QueryString["NwtID"]);
            }
            if (null != Request.QueryString["SiteId"])
            {
                intsiteId = Convert.ToInt32(Request.QueryString["SiteId"].ToString());
            }

            if (null != Request.QueryString["liveupdate"])
            {
                strliveupdate = Request.QueryString["liveupdate"].ToString();
            }
            else
            {
                strliveupdate = "N";
            }
        }
    }
}