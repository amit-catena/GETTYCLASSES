using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using Gettyclasses;
using System.Data;

namespace gettywebclasses
{
    public partial class UploadReviewImageNew : System.Web.UI.Page
    {
        public string _mstrImag64 = string.Empty;
        public string baseurl = ConfigurationSettings.AppSettings["baseurl"];
        public string topImagepath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    FillLink();
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                string imageName = DateTime.Now.ToString("ddMMyyyyHHmmsss") + ".jpg";
                string imgpath = MapPath("~/banner/" + imageName);
                string newimgpath = Gettyclasses.commonfn._baseURL + "banner/" + imageName;
                #region :: Base64 ::
                string strImg = Request.Form["templateText3"]; ;
                strImg = strImg.Replace("data:image/png;base64,", "");
                strImg = strImg.Replace("data:image/jpeg;base64,", "");
                if (strImg.Trim().Length > 0)
                {
                    Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    
                  //  Page.RegisterStartupScript("close", "<script>closepop();</script>");
                    Page.RegisterStartupScript("onsave", "<script>closePOP('" + newimgpath + "','" + ddllink.SelectedValue + "');</script>");
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
        }

        #region :: Base 64 functions ::
        public System.Drawing.Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
                imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }

        public string ImageToBase64(System.Drawing.Image image,
            System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        private void GetDatabaseImage(string slotid)
        {
            try
            {
                string strImageName = string.Empty;
                string imgpath = string.Empty;
                using (Gettyclasses.slotmgmt obj = new slotmgmt())
                {
                    strImageName = obj.GetSlotPosterImage(slotid);
                }
                if (strImageName != null && strImageName != "")
                {
                    imgpath = baseurl + "/commonreview/" + strImageName;
                }
                else
                {
                    imgpath = "";
                }

                topImagepath = imgpath;
                //Response.Write("3-networkID"+Session["networkID"].ToString() +"SQL"+strSQL +"topImagepath"+topImagepath);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
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