using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using CommonLib;
using Gettyclasses;
using System.IO;

namespace gettywebclasses
{
    public partial class UploadReviewImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ( !Page.IsPostBack)
            {
                FillLink();
            }
        }
        protected void btnsave_Click(object sender, EventArgs e)
        {
            if (file1.HasFile)
            {
               string  Name = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyhhmmss"), Path.GetExtension(file1.FileName));
               string imgpath = "http://anil-pc/gettyclasses/gettywebclasses/banner/" + Name;
               file1.SaveAs(Server.MapPath("~/banner/") + Name);
               Page.RegisterStartupScript("onsave", "<script>closePOP('"+imgpath+"','"+ddllink.SelectedValue+"');</script>");
            }
        }

        public void FillLink()
        {
            try
            {
                slotmgmt obj = new slotmgmt();
                DataTable dt = new DataTable();
                dt = obj.GetHyperlinkForReview();
                if (dt.Rows.Count > 0)
                {
                    ddllink.DataSource = dt;
                    ddllink.DataTextField = "LinkName";
                    ddllink.DataValueField = "LinkID";
                    ddllink.DataBind();
                }
                ddllink.Items.Insert(0, new ListItem("-select link-", "0"));
            }
            catch (Exception ex)
            {

            }
        }
    }
}