using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Gettyclasses;
using System.Drawing;
using System.IO;
using System.Data;
using System.Text;
using CommonLib;

namespace gettywebclasses
{
    public partial class themeimage : System.Web.UI.Page
    {
        public string _favcolor = "#ff0000";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillLink();
                if (Request.QueryString["themeid"] != null && Request.QueryString["themeid"]!="0")
                {

                    SetDetails();
                }
            }
        }

       public void SetDetails()
        {
            DataTable dt = new DataTable(); 
            ThemeMgmt obj = new ThemeMgmt();
            try
            {
                dt = obj.GetThemeDetails(Request.QueryString["themeid"]);                
                if (dt.Rows.Count > 0)
                {
                    txtthemeName.Text = dt.Rows[0]["ThemeName"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["ImageName"].ToString()))
                    {
                        ViewState["IMG"] = dt.Rows[0]["ImageName"].ToString();
                        ltimage.Text = "<img src='./commonreview/TN_" + dt.Rows[0]["ImageName"].ToString() + "' />";
                        _favcolor = dt.Rows[0]["color"].ToString();
                        try
                        {
                            ddllink.SelectedValue = dt.Rows[0]["linkid"].ToString();
                        }
                        catch { }
                    }
                }
            }
            catch
            {

            }
 

        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            string themeid = "0";
            string name = "";
            string imageName = "";
            string favcolor = "";
            try
            {

                int i = 0;

                if (Request.QueryString["themeid"] != null)
                {
                    themeid = Request.QueryString["themeid"];
                }
                name = txtthemeName.Text;
                if (file1.HasFile)
                {
                    imageName = GetimageName();
                    try
                    {
                        SaveThumbnail(imageName);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    imageName = ViewState["IMG"].ToString();
                }

                ThemeMgmt obj = new ThemeMgmt();
                favcolor = Request.Form["favcolor"].ToString();
                if (obj.SaveThemeDetail(themeid, txtthemeName.Text, imageName,favcolor,ddllink.SelectedValue))
                        i = 2;

                if (i > 0)
                {
                    Page.RegisterStartupScript("onsave", "<script>closePOP();</script>");
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "themeimage.aspx.cs btnsave_Click ", ex);
            }


        }


        public string GetimageName()
        {
            string Name = "";

            if (file1.HasFile)
            {
                try
                {
                    Name = string.Format("{0}{1}{2}", DateTime.Now.ToString("ddMMyyhhmmss"),Function.toTitle(txtthemeName.Text),Path.GetExtension(file1.FileName));
                    file1.SaveAs(Server.MapPath("~/commonreview/") + Name);
                }
                catch
                {
                }
            }

            return Name;
        }


        private string SaveThumbnail(string img)
        {

            string imgpath = "";
            string albumPath = "";
            imgpath = MapPath("~/commonreview/" + img);
            albumPath = MapPath("~/commonreview/");

            int jheight = System.Drawing.Image.FromFile(imgpath).Height;
            int jwidthAct = System.Drawing.Image.FromFile(imgpath).Width;
            double ratio = (double)jheight / (double)jwidthAct;
            int jwidth = 200;
            double calHeight = jwidth * ratio;

            string ImgFilePath = img;
            string uriName = Path.GetFileName(imgpath);
            Bitmap SourceBitmap = null;
            System.Drawing.Image thumbnail = null;
            string fnameHRW = "TN_" + uriName;
            Bitmap SourceBitmap1 = null;
            System.Drawing.Image thumbnail1 = null;
            try
            {
                SourceBitmap = new Bitmap(albumPath + uriName);
                thumbnail = SourceBitmap.GetThumbnailImage(jwidth, Convert.ToInt32(Math.Round(calHeight)), null, IntPtr.Zero);
                FileStream fs = File.Create(albumPath + fnameHRW);
                thumbnail.Save(fs, SourceBitmap.RawFormat);
                fs.Flush();
                fs.Close();
            }
            catch
            {
            }
            finally
            {
                SourceBitmap.GetThumbnailImage(jwidth, Convert.ToInt32(Math.Round(calHeight)), null, IntPtr.Zero).Dispose();
                thumbnail.Dispose();
                SourceBitmap.Dispose();
            }

            return "TN_" + img;
        }
        public void FillLink()
        {
            try
            {
                ThemeMgmt obj = new ThemeMgmt();
                DataTable dt = new DataTable();
                dt=obj.GetWeloveHyperlink();
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