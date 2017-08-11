using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLib;
using Gettyclasses;
using System.IO;

namespace gettywebclasses
{
    public partial class UploadFilterValueImage : System.Web.UI.Page
    {
        string filtervalueid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                filtervalueid = Request.QueryString["filtervalueid"];
            }
        }
        protected void btnsave_Click(object sender, EventArgs e)
        {
            if (file1.HasFile)
            {
                string ImageName = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyhhmmss"), Path.GetExtension(file1.FileName));
                string imgpath = "http://www.pix123.com/commonreview/" + ImageName;
                file1.SaveAs(Server.MapPath("~/commonreview/") + ImageName);
                using (FiltervalueMgmt objfilter = new FiltervalueMgmt())
                {
                    objfilter.SaveFilterValueImage(Request.QueryString["filtervalueid"],ImageName);
                }
                Page.RegisterStartupScript("onsave", "<script>closePOP('" + imgpath + "','" + Request.QueryString["filtervalueid"]+ "');</script>");
            }
        }
    }
}