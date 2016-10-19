using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gettyclasses;

namespace gettywebclasses
{
    public partial class categorytemplate : System.Web.UI.Page
    {
        public string Strlinks = "",catalias="",siteid="",networkid="",catid="";
        string result = "";
        public string catjson = "{}";
        public string quicklink = "{}";
        public string catdesc = "";
        public string centraltableurl = "";
        public string allcatsubcatjson = "";
        public string Allsports = "[]";
        public string AllTicketSport = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (Request.QueryString["catname"] != null && Request.QueryString["siteid"] != null && Request.QueryString["networkid"] != null && Request.QueryString["catid"] != null)
            {
                networkid = Request.QueryString["networkid"];
                siteid = Request.QueryString["siteid"];
                catalias = Request.QueryString["catname"];
                catid= Request.QueryString["catid"];
                Strlinks = GetLink(siteid, networkid);
                catjson = GetSubcatJson(siteid, networkid, catalias, catid);
                quicklink = GetQuicklink(siteid, networkid, catid);
                catdesc = CategoryDescription(siteid, networkid, catid);
                allcatsubcatjson = GetAllcategorySubcategoryJson(siteid, networkid); 
                centraltableurl=string.Format("http://admin.writersllc.com/posttableindesc.aspx?siteid={0}&networkid={1}&userid=1&temp=1",siteid,networkid);
                if (!Page.IsPostBack)
                    SetTemplateData();
                else
                {
                    CategoryTemplate obj = new CategoryTemplate();
                    result = obj.SaveTemplate(catid, siteid, networkid, Request.Form["hddata"]);
                    if (result == "saved")
                        Page.RegisterStartupScript("onsave", "<script>closePOP();</script>");
                }
                Allsports = GetAllSportsJson();
                AllTicketSport = GetAllTicketSportsJson();
            }
        }

        public string GetLink(string site_id, string nwid)
        {
            string data = "";
            CategoryTemplate obj = new CategoryTemplate();
            data = obj.GetLink(site_id, nwid);
            return data;
        }

        public void SetTemplateData()
        {
            CategoryTemplate obj = new CategoryTemplate();
            ltData.Text = obj.GetTemplateDetails(catid, siteid, networkid);
        }
        public string GetSubcatJson(string sid, string nwid, string catalias, string cid)
        {
            string data = "";
            CategoryTemplate obj = new CategoryTemplate();
            data = obj.GetSubcatJson( sid,  nwid,  catalias,  cid);
            return data;
        }
        public string CategoryDescription(string sid, string nwid, string cid)
        {
            string data = "";
            CategoryTemplate obj = new CategoryTemplate();
            data = obj.CategoryDescription(sid, nwid, cid);
            return data;
        }
        public string GetQuicklink(string sid, string nwid, string cid)
        {
            string data = "";
            CategoryTemplate obj = new CategoryTemplate();
            data = obj.CategoryQuickLink(sid, nwid,cid);
            return data;
        }

        public string GetAllcategorySubcategoryJson(string sid, string nwid)
        {
            string data = "";
            CategoryTemplate obj = new CategoryTemplate();
            data = obj.GetAllCategorriesSubcategoriesJson(sid, nwid);
            return data;
        }

        public string GetAllSportsJson()
        {
            string data = "";
            CategoryTemplate obj = new CategoryTemplate();
            data = obj.Getallsports();
            if (string.IsNullOrEmpty(data))
                data = "[]";
            return data;
        }

        public string GetAllTicketSportsJson()
        {
            string data = "";
            CategoryTemplate obj = new CategoryTemplate();
            data = obj.GetallTicketsports();
            if (string.IsNullOrEmpty(data))
                data = "[]";
            return data;
        }
    }
}