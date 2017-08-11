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
using GettyImages.Api.Search;
using Newtonsoft.Json;

namespace newgettyimagesAPI
{
    /// <summary>
    /// Ajax class for image function
    /// </summary>
    public partial class imageajax : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                switch (Request.Form["type"].ToString())
                {
                    #region Switch_case
                    case "Getserchimages":
                        Getserchimages();
                        break;
                    case "Downloadimage":
                        Downloadimage();
                        break;
                    case "Downloadimage_Forsite":
                        Downloadimage_ForSite();
                        break;
                   case "Download_multiple_image_Forsite":
                        Download_multiple_image_ForSite();
                        break;
                   case "load_multiple_events":
                        load_multiple_events();
                        break;
                    default:
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Search images
        /// </summary>
        public async void Getserchimages()
        {
            string strsearch = Request.Form["search"];
            int page = Convert.ToInt32(Request.Form["page"]);
            string strmultiple = Request.Form["_muptilpe"];
            string strsort = Request.Form["sort"];
            string strorient = Request.Form["orient"];
            string strdate = Request.Form["date"];
            StringBuilder strstring = new StringBuilder(); 
            using (NewGettyAPIclasses.Getimagedata d = new NewGettyAPIclasses.Getimagedata())
            {
                if (string.IsNullOrEmpty(strmultiple))
                    strmultiple = "N";
                strstring = await NewGettyAPIclasses.Getimagedata.AccessTheWebAsync(strsearch, page, strmultiple, strsort, strorient, strdate);
                if (strstring.Length > 0)
                    lttext.Text = strstring.ToString();  
            }
        }

        /// <summary>
        /// Download images
        /// </summary>
        public async void Downloadimage()
        {
            string strID = Request.Form["ID"];
            string filename = string.Empty;
            string strstring = string.Empty;
            using (NewGettyAPIclasses.Getimagedata d = new NewGettyAPIclasses.Getimagedata())
            {
                strstring = await NewGettyAPIclasses.Getimagedata.AccessTheWebAsync_downloadimage(strID);
                if (!string.IsNullOrEmpty(strstring))
                {
                    filename=GoWebshot(strstring, strID);
                    if (!string.IsNullOrEmpty(filename))
                        lttext.Text = "download successfully....";
                }
            }

        }

        /// <summary>
        /// Download gettyimages for sites
        /// </summary>
        public async void Downloadimage_ForSite()
        {
            string strID = Request.Form["ID"];
            string strsiteIdID = Request.Form["SiteID"];
            string strnetworkdID = Request.Form["NetworkID"];
            string strcaption = Request.Form["caption"];
            string strlive = Request.Form["liveup"];
            string filename = string.Empty;
            string strstring = string.Empty;
            using (NewGettyAPIclasses.Getimagedata d = new NewGettyAPIclasses.Getimagedata())
            {
                d.strnetworkid = strnetworkdID;
                d.SiteID = Convert.ToInt32(strsiteIdID);
                strstring = await d.AccessTheWebAsync_downloadimage_fromID(strID, strcaption, strlive);
                if (!string.IsNullOrEmpty(strstring))
                {
                    lttext.Text = strstring;
                }
            }

        }
        /// <summary>
        /// Download multiple images for sites
        /// </summary>
        public async void Download_multiple_image_ForSite()
        {
            string strID = Request.Form["ID"];
            string strsiteIdID = Request.Form["SiteID"];
            string strnetworkdID = Request.Form["NetworkID"];
            string strjson = Request.Form["Strjson"];
            string filename = string.Empty;
            string strstring = string.Empty;
            int ID = 0;
            string cap = string.Empty;
            List<string> li = new List<string>();
            Dictionary<int, string> l = new Dictionary<int, string>();

            if (!string.IsNullOrEmpty(strjson))
            {
                dynamic array = JsonConvert.DeserializeObject(strjson);
                foreach(var d in array)
                {
                    var data = d.Value;
                    foreach(var innerd in data)
                    {
                        if(innerd.Name=="ID")
                        {
                            ID = Convert.ToInt32(innerd.Value);
                        }
                        if(innerd.Name=="caption")
                        {
                            cap = Convert.ToString(innerd.Value);
                        }
                    }
                    l.Add(ID, cap);
                    ID = 0; cap = string.Empty;
                }
            }
            using (NewGettyAPIclasses.Getimagedata d = new NewGettyAPIclasses.Getimagedata())
            {
                d.strnetworkid = strnetworkdID;
                d.SiteID = Convert.ToInt32(strsiteIdID);
                if(l.Count()>0)
                {
                    strstring = await d.AccessTheWebAsync_download_multiple_imagewithcaption(l);
                    if (!string.IsNullOrEmpty(strstring))
                    {
                        lttext.Text = strstring;
                    }
                }
            }
        }

        /// <summary>
        /// Load multiple events
        /// </summary>
        public void load_multiple_events()
        {
            int pageID = Convert.ToInt32(Request.Form["page"]);
            string filename = string.Empty;
            StringBuilder strstring = new StringBuilder();
            List<string> li = new List<string>();
            using (NewGettyAPIclasses.Getimagedata d = new NewGettyAPIclasses.Getimagedata())
            {
               strstring = d.Getimagesbytoken_events("", "", pageID); ;
               lttext.Text = Convert.ToString(strstring);
            }
        }

        /// <summary>
        /// Getwebshots for webrequest image
        /// </summary>
        /// <param name="strurl"></param>
        /// <param name="imgID"></param>
        /// <returns></returns>
        private string GoWebshot(string strurl, string imgID)
        {
            string _imagepathdownload = NewGettyAPIclasses.ConstantAPI.downloadpath;
            string sitefolder = "";
            string monthyearfolder = DateTime.Now.ToString("yyyyMM");
            string dayfolder = DateTime.Now.ToString("MMMdd");
            string filename = "";
            string orgfilename = "";
            string imgSrcURL = strurl;
            string path = null;
            string pathclient = null;
            string Tnname = string.Empty;
            string orgname = string.Empty;
            bool result = false;
            try
            {
                orgfilename = "getty_" + DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                filename = DateTime.Now.ToString("yyyyMMMddhhmmss") + "_" + imgID + ".jpg";
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadFile(imgSrcURL, _imagepathdownload + filename);
                webClient.Dispose();
                Tnname = filename;
            }
            catch (Exception exp)
            {
            }
            return Tnname;
        }

    }
}