using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using gettywebclasses;

namespace gettywebclasses
{
    public partial class ajaxpost : System.Web.UI.Page
    {
       
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
                        DeleteNewsLiveUpdate(Request.Form["ids"], Request.Form["networkid"],Request.Form["newsid"]);
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

        public void DeleteNewsLiveUpdate(string ids,string networkid,string newsid)
        {
            try
            {
                string networkconn = System.Configuration.ConfigurationManager.AppSettings[networkid].ToString();
                using (Gettyclasses.newsliveupdatemgmt objnews = new Gettyclasses.newsliveupdatemgmt(networkconn))
                {
                    int count = objnews.DeleteNewsUpdateLive(ids, newsid);
                    ltresult.Text = count.ToString();
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "ajaxpost.aspx.cs DeleteNewsLiveUpdate", ex);
            }
        }
    }
}