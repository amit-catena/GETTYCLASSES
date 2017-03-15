using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gettyclasses;
using System.Net;
using System.IO;
using System.Text;
using System.Data;

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
                    case "HIGHLIGHTNEWSLIVEUPDATE":
                        SetHighlightNewsLiveUpdate(Request.Form["ids"], Request.Form["networkid"], Request.Form["siteurl"],Request.Form["status"]);
                        break;
                    case "PROMOTIONALLINK":
                        PROMOTIONALLINK();
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

        public void SetHighlightNewsLiveUpdate(string ids, string networkid,string siteurl,string status)
        {
            try
            {
                string networkconn = System.Configuration.ConfigurationManager.AppSettings[networkid].ToString();
                using (Gettyclasses.newsliveupdatemgmt objnews = new Gettyclasses.newsliveupdatemgmt(networkconn))
                {
                    objnews.SetHighlightNewsUpdateLive(ids,status);                  
                    RefreshCache(siteurl + strgeneratexml);
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "ajaxpost.aspx.cs SetHighlightNewsLiveUpdate", ex);
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

        public void PROMOTIONALLINK()
        {
            StringBuilder sb = new StringBuilder();
            string URL = "", region = "";

            region = Request.Form["region"];
            string conn = System.Configuration.ConfigurationManager.AppSettings["gamingnetAdsenseconn"];
          
            //new code  16/03/2015
            if (region == "GB")
                sb.Append(string.Format("<option value='{0}'>{1}</option>", "0", "Select GB Promotional Link"));
            else
                sb.Append(string.Format("<option value='{0}'>{1}</option>", "0", "Select AU Promotional Link"));
            try
            {
                DataTable dt = new DataTable();

                using (newsliveupdatemgmt objpromo = new newsliveupdatemgmt(conn))
                {
                    dt = objpromo.GetPromotionalLink(region);
                }
                //dt=obj.GetDataTable("SELECT LinkName+' (added '+Convert(varchar(10),Addedon,104)+')' as LinkName,shortenurl,RandomUniqueId as LinkID FROM OfferLinkManager where isactive='Y' and region='"+region+"' Order by LinkName");									
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        URL = "";
                        try
                        {
                            if (r["shortenurl"].ToString().Trim().StartsWith("f.ast.bet/go"))
                            {
                                URL = string.Format("http://{0}", r["shortenurl"].ToString().Trim());
                            }
                            else
                            {
                                URL = string.Format("//www.caledonianmedia.com/promotionalstat.aspx?siteurl={0}&siteid={1}", r["LinkID"].ToString(), Session["liveupdatesiteid"].ToString());
                            }
                        }
                        catch
                        {
                            URL = "";
                        }
                        if (URL.Length > 1)
                        {
                            sb.Append(string.Format("<option value='{0}'>{1}</option>", URL, r["LinkName"].ToString()));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "ajaxpost.aspx.cs RefreshCache", ex);
            }

            ltresult.Text = sb.ToString(); sb = null;


        }
    }
}