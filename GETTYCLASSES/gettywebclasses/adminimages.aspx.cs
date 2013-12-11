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
    public partial class adminimages : System.Web.UI.Page
    {
          string json = "";//"jsonCallback({'result': {'imgname': '{0}','imgpath': '{1}'}})";
        public string _imgserver = ConfigurationSettings.AppSettings["imgserver"];
        public string isimgserver="", sitefldname="", isdefault="", imgpath="";
        public string imagename = "",returnimagepath="",returnimagename="";
        string monthyearfolder = DateTime.Now.ToString("yyyyMM");
        string dayfolder = DateTime.Now.ToString("MMMdd");
        string _imagepath = "";
        string imgExt = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                isimgserver = Request.QueryString["isimgserver"].ToString();
                sitefldname = Request.QueryString["sitefldname"].ToString();
                isdefault = Request.QueryString["isdefault"].ToString();
                imgpath = Request.QueryString["imgpath"].ToString();

                if (imgpath.ToLower().Contains(".gif"))
                    imgExt = ".gif";

                if (imgpath.ToLower().Contains(".png"))
                    imgExt = ".png";

                if (imgpath.ToLower().Contains(".jpg"))
                    imgExt = ".jpg";

                if (imgpath.ToLower().Contains(".jpeg"))
                    imgExt = ".jpeg";

                if(imgExt.Trim().Length==0)
                    imgExt = ".jpg";

               

                //imgExt = Path.GetExtension(imgpath);
                imagename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
                returnimagename = imagename;
                WebClient webClient = new WebClient();
                Function.ValidateSiteFolder(sitefldname);
                Function.CreateImagesFolderInBackend(sitefldname, monthyearfolder, dayfolder);
                // Image Server Code
                _imagepath = Server.MapPath(string.Format("{0}/{1}/{2}/", sitefldname, monthyearfolder, dayfolder));
                webClient.DownloadFile(imgpath, _imagepath + imagename);
                webClient.Dispose();
                

                if (isimgserver.ToUpper() == "Y" && isdefault.ToUpper() == "Y")
                {
                    returnimagename = Function.SaveThumbnailCompress(imagename, _imagepath, "TN", 300, 225);
                    Function.SaveThumbnailCompress(imagename, _imagepath, "TN_TN", 128, 85);
                    returnimagepath = string.Format("{0}{1}/{2}/{3}/{4}", _imgserver, sitefldname, monthyearfolder, dayfolder, returnimagename);
                }
                else
                {
                    returnimagepath = string.Format("{0}{1}/{2}/{3}/{4}", _imgserver, sitefldname, monthyearfolder, dayfolder, returnimagename);
                }
                //json = string.Format("jsonCallback({'result':{'imgname':'{0}','imgpath':'{1}'}})", returnimagename, returnimagepath);

                json = "jsonCallback({'result':{'imgname':'"+returnimagename+ "','imgpath':'"+returnimagepath+"'}})";

            }
            catch(Exception ex)
            {
                Response.Write(ex.Message);
                json = "jsonCallback({'result':{'imgname':'','imgpath':''}})";
            }

            ltresult.Text = json;

        }
    }
 }
