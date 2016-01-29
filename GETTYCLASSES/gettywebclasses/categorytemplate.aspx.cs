﻿using System;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (Request.QueryString["catname"] != null && Request.QueryString["siteid"] != null && Request.QueryString["networkid"] != null && Request.QueryString["catid"] != null)
            {
                networkid = Request.QueryString["networkid"];
                siteid = Request.QueryString["siteid"];
                catalias = Request.QueryString["catname"];
                catid= Request.QueryString["catid"];
                Strlinks = GetLink(siteid, networkid);
                if (!Page.IsPostBack)
                    SetTemplateData();
                else
                {
                    CategoryTemplate obj = new CategoryTemplate();
                    result = obj.SaveTemplate(catid, siteid, networkid, Request.Form["hddata"]);
                    if (result == "saved")
                        Page.RegisterStartupScript("onsave", "<script>closePOP();</script>");
                }
                

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
    }
}