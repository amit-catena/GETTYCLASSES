using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace gettywebclasses
{
    public partial class modelframe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                FillLinks(Session["liveupdatesiteid"].ToString());
            }

            catch (Exception exc)
            {
               
            }
            finally
            {

            }
        }

        public void FillLinks(string siteid)
        {
            DataTable dt = new DataTable();
            try
            {
                using (newsliveupdatemgmt obj = new newsliveupdatemgmt(Session["liveupdateconn"].ToString()))
                {
                    dt = obj.GetLinks(siteid);
                    if (dt.Rows.Count > 0)
                    {
                        sellink.DataSource = dt;
                        sellink.DataTextField = "LinkName";
                        sellink.DataValueField = "LinkID";
                        sellink.DataBind();
                    }
                    sellink.Items.Insert(0, new ListItem("-select-", "0"));
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "modelframelink8.aspx.cs FillLinks", ex);
            }
        }
    }
}