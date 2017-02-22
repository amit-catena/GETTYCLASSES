using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using gettywebclasses;
using System.Net;
using System.IO;

namespace gettywebclasses
{
    public partial class ajaxpost : System.Web.UI.Page
    {
        public string strgeneratexml = "/recacheXML.aspx?commentcacheid=N";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {                
                switch (Request.Form["type"].ToUpper())
                {
                    case "NEWSLIVEUPDATE":
                        SaveLiveNewsUpdate(Request.Form["newtworkid"], Request.Form["siteid"], Request.Form["newsid"], Request.Form["title"], Request.Form["desc"], Request.Form["addedby"], Request.Form["image"]);
                        break;
                    case "DELETENEWSLIVEUPDATE":
                        DeleteNewsLiveUpdate(Request.Form["ids"], Request.Form["networkid"],Request.Form["newsid"],Request.Form["siteurl"]);
                        break;

                }
            }
        }

        public void SaveLiveNewsUpdate(string networkid,string siteid,string newsid,string title,string desc,string addedby,string imagename)
        {
            int count = 0;
            try
            {
                string networkconn = System.Configuration.ConfigurationManager.AppSettings[networkid].ToString();
                using (Gettyclasses.newsliveupdatemgmt objnews = new Gettyclasses.newsliveupdatemgmt(networkconn))
                {
                    objnews.NewsId = newsid;
                    objnews.SiteId = siteid;
                    objnews.Title = title;
                    objnews.Description = desc;
                    objnews.Addedby = addedby;
                    objnews.Image = imagename;
                    objnews.NewsId = newsid;
                    count=objnews.SaveLiveNewsUpdate();
                    ltresult.Text = count.ToString();
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "ajaxpost.aspx.cs SaveLiveNewsUpdate", ex);
            }
        }

        public void DeleteNewsLiveUpdate(string ids,string networkid,string newsid,string siteurl)
        {
            try
            {
                string networkconn = System.Configuration.ConfigurationManager.AppSettings[networkid].ToString();
                using (Gettyclasses.newsliveupdatemgmt objnews = new Gettyclasses.newsliveupdatemgmt(networkconn))
                {
                    int count = objnews.DeleteNewsUpdateLive(ids, newsid);
                    ltresult.Text = count.ToString();
                    RefreshCache(siteurl + strgeneratexml);
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "ajaxpost.aspx.cs DeleteNewsLiveUpdate", ex);
            }
        }

        public  void RefreshCache(string siteurl)
        {

            System.Uri objURI;
            System.Net.WebRequest objWebRequest;
            System.Net.WebResponse objWebResponse;
            System.IO.Stream objStream;
            System.IO.StreamReader objStreamReader;

            string strHTML = "";
            try
            {
                objURI = new Uri(siteurl);
                objWebRequest = HttpWebRequest.Create(objURI);
                objWebResponse = objWebRequest.GetResponse();
                objStream = objWebResponse.GetResponseStream();
                objStreamReader = new StreamReader(objStream);
                strHTML = objStreamReader.ReadToEnd();

                objURI = null;

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "ajaxpost.aspx.cs RefreshCache", ex);
            }

        }
    }
}