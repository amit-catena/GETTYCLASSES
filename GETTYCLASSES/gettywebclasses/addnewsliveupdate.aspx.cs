using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gettyclasses;
using System.Data;
using System.Text;
using System.IO;

namespace gettywebclasses
{
    public partial class addnewsliveupdate : System.Web.UI.Page
    {
        public string baseurl = "";
        public string networkid = "";
        public string siteid = "";
        public string newsid = "";
        public string userid = "";
        string networkconn = "";
        public string newstitle = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    baseurl = commonfn._baseURL;
                    networkid = Request.QueryString["networkid"];
                    siteid = Request.QueryString["siteid"];
                    newsid = Request.QueryString["newsid"];
                    userid = Request.QueryString["userid"];
                    newstitle = Request.QueryString["title"];
                    networkconn = System.Configuration.ConfigurationManager.AppSettings[networkid];
                    if (Request.QueryString["operation"] == "list")
                    {
                        divlist.Visible = true;
                        divadd.Visible = false;
                        GetNewsLiveUpdateList();
                    }
                    else
                    {
                        divlist.Visible = false;
                        divadd.Visible = true;
                    }
                   
                }
                catch (Exception ex)
                {
                    CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "addnewliveupdate.aspx.cs !Page.IsPostBack", ex);
                }

            }

           
        }


        public void GetNewsLiveUpdateList()
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sb = new StringBuilder();
                using (newsliveupdatemgmt obj = new newsliveupdatemgmt(networkconn))
                {
                    dt = obj.GetNewsLiveUpdateList(Request.QueryString["newsid"], Request.QueryString["siteid"]);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            sb.Append("<tr >");
                            sb.Append(string.Format("<td align= 'left' bgcolor='#FFFFFF' valign='middle' style='font-family:verdana;font-size:11px;'><input type='checkbox' id='{0}' class='source' value='{0}' name='{0}'></td>", dr["Id"].ToString()));
                            sb.Append(string.Format("<td align= 'left' bgcolor='#FFFFFF' valign='middle' style='font-family:verdana;font-size:11px;'>{0}</td>", dr["title"].ToString()));
                            sb.Append(string.Format("<td align= 'left' bgcolor='#FFFFFF' valign='middle' style='font-family:verdana;font-size:11px;'>{0}</td>", Convert.ToDateTime(dr["addedon"]).ToString("dd-MM-yyyy HH:mm tt")));
                            sb.Append(string.Format("<td align= 'left' bgcolor='#FFFFFF' valign='middle' style='font-family:verdana;font-size:11px;'>{0}</td>", dr["Name"].ToString()));
                            sb.Append("</tr>");
                        }

                    }
                    else
                    {
                        sb.Append("<tr><td style='color:red' colspan='4'>record not found </td></tr>");
                    }
                    ltlist.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "addnewliveupdate.aspx.cs GetNewsLiveUpdateList", ex);
            }
        }

        protected void btnsave_click(object sender, EventArgs e)
        {
            int count = 0;
            string imagename = "";
            
            try
            {
                networkid = Request.QueryString["networkid"];
                networkconn = System.Configuration.ConfigurationManager.AppSettings[networkid];
                using (newsliveupdatemgmt obj = new newsliveupdatemgmt(networkconn))
                {
                    if (file1.HasFile)
                    {
                        imagename = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyhhmmss"), Path.GetExtension(file1.FileName));

                        file1.SaveAs(Server.MapPath("~/newsliveupdate/") + imagename);                       
                    }
                    obj.NewsId = Request.QueryString["newsid"];
                    obj.Title = textTitle.Text;
                    obj.Description = textdesc.Text;
                    obj.Addedby = Request.QueryString["userid"];
                    obj.SiteId = Request.QueryString["siteid"];
                    obj.Image = imagename;
                    count= obj.SaveLiveNewsUpdate();
                    Page.RegisterStartupScript("onsave", "<script>closePOP('" + count + "','" + obj.NewsId + "');</script>");
                }               
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "addnewliveupdate.aspx.cs btnsave_click", ex);
            }
        }

        protected void btnadd_click(object sender, EventArgs e)
        {
            divlist.Visible = false;
            divadd.Visible = true;
            btndelete.Visible = false;
        }
    }
}