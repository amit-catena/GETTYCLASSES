using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gettyclasses;
using System.Data;

namespace gettywebclasses
{
    public partial class modelframepromolink : System.Web.UI.Page
    {
        public string BaseUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            BaseUrl = Gettyclasses.commonfn._baseURL;
            if (!Page.IsPostBack)
            {
                FillPromoLink();
            }
        }


        public void FillPromoLink()
        {
            string URL = "";
            string strconn = System.Configuration.ConfigurationManager.AppSettings["gamingnetAdsenseconn"];
            //new code  16/03/2015

            try
            {
                DataTable dt = new DataTable();

                //dt=obj.GetDataTable("SELECT LinkName+' (added '+Convert(varchar(10),Addedon,104)+')' as LinkName,shortenurl,RandomUniqueId as LinkID FROM OfferLinkManager where isactive='Y' and region='GB' Order by LinkName");									
                using (newsliveupdatemgmt objpromo = new newsliveupdatemgmt(strconn))
                {
                    dt = objpromo.GetPromotionalLink("GB");
                }
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
                            sellink.Items.Add(new ListItem(r["LinkName"].ToString(), URL));
                        }
                    }
                }
                sellink.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "modelIframepromotiolink.aspx.cs FillPromoLink", ex);
            }

        }
    }
}