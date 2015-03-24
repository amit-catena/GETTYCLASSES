using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gettyclasses;
using System.Data;
using System.Data.SqlClient;
using ConnectExample.Samples;
using System.Net;
using System.Configuration;
using CommonLib;
using System.Web.UI.HtmlControls;
using System.Web.Services;

namespace gettywebclasses
{
    public partial class pollimages : System.Web.UI.Page
    {
        string json = "";//"jsonCallback({'result': {'imgname': '{0}','imgpath': '{1}'}})";
        public string _imgserver = ConfigurationSettings.AppSettings["imgserver"];
        public string isimgserver = "", sitefldname = "", isdefault = "", imgpath = "";
        public string imagename = "", returnimagepath = "", returnimagename = "";
        string monthyearfolder = DateTime.Now.ToString("yyyyMM");
        string dayfolder = DateTime.Now.ToString("MMMdd");
        string _imagepath = "";
        string imgExt = "";
        string largeimagename = string.Empty;
        string largereturnimagename = string.Empty;
        string largeimagenameURL = string.Empty;
        string largeimageId = string.Empty;
        List<string> _strIds = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (null != Request.QueryString["isimgserver"])
                    isimgserver = Request.QueryString["isimgserver"].ToString();
                if (null != Request.QueryString["sitefldname"])
                    sitefldname = Request.QueryString["sitefldname"].ToString();
                if (null != Request.QueryString["isdefault"])
                    isdefault = Request.QueryString["isdefault"].ToString();
                if (null != Request.QueryString["imgpath"])
                    imgpath = Request.QueryString["imgpath"].ToString();
                if (null != Request.QueryString["imageid"])
                    largeimageId = Request.QueryString["imageid"].ToString();


                if (imgpath.ToLower().Contains(".gif"))
                    imgExt = ".gif";

                if (imgpath.ToLower().Contains(".png"))
                    imgExt = ".png";

                if (imgpath.ToLower().Contains(".jpg"))
                    imgExt = ".jpg";

                if (imgpath.ToLower().Contains(".jpeg"))
                    imgExt = ".jpeg";

                if (imgExt.Trim().Length == 0)
                    imgExt = ".jpg";

                //imgExt = Path.GetExtension(imgpath);
                imagename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
                largeimagename = "large_" + imagename;
                //new code for large image
                if (!string.IsNullOrEmpty(largeimageId))
                {
                    Getimagedata _objnew = new Getimagedata();
                    _strIds.Add(largeimageId);
                    largeimagenameURL = _objnew.getlargeimage_download_DandD(_strIds);
                    _strIds.Clear();
                }
                //end code
                returnimagename = imagename;
                WebClient webClient = new WebClient();
                Function.ValidateSiteFolder(sitefldname);
                //Function.CreateImagesFolderInBackend(sitefldname, monthyearfolder, dayfolder);
                // Image Server Code
                _imagepath = Server.MapPath(string.Format("{0}/", sitefldname));
                //webClient.DownloadFile(imgpath, _imagepath + largeimagename);
                if (!string.IsNullOrEmpty(largeimageId))
                {
                    webClient.DownloadFile(largeimagenameURL, _imagepath + largeimagename);
                }
                else
                {
                    webClient.DownloadFile(imgpath, _imagepath + imagename);
                }
                webClient.Dispose();
                //large image download
                if (isimgserver.ToUpper() == "Y" && isdefault.ToUpper() == "Y")
                {
                    if (!string.IsNullOrEmpty(largeimageId))
                    {
                        //original large image
                       // largereturnimagename = Function.SaveThumbnailCompress(largeimagename, _imagepath, "DD_", 1200, 100);
                        //original news image
                        largereturnimagename = Function.SaveoriginalImage(largeimagename, _imagepath, "", 600, 500);
                    }
                  
                    //end code
                    returnimagename = Function.SaveThumbnailCompress(imagename, _imagepath, "", 300, 225);
                    //Function.SaveThumbnailCompress(imagename, _imagepath, "", 128, 85);
                    returnimagepath = string.Format("{0}{1}/{2}", _imgserver, sitefldname,returnimagename);
                }
                
                //json = string.Format("jsonCallback({'result':{'imgname':'{0}','imgpath':'{1}'}})", returnimagename, returnimagepath);

                json = "jsonCallback({'result':{'imgname':'" + returnimagename + "','imgpath':'" + returnimagepath + "'}})";
                largeimagenameURL = "";
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                json = "jsonCallback({'result':{'imgname':'','imgpath':''}})";
            }

            ltresult.Text = json;

        }
    }
}