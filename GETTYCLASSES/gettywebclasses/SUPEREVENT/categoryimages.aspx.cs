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

namespace gettywebclasses.superevent
{
    public partial class categoryimages : System.Web.UI.Page
    {
        /// <summary>
        /// The baseurl
        /// </summary>
        public string baseurl = "";
        /// <summary>
        /// The jsondata
        /// </summary>
        public string jsondata = "";
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        
        public System.Drawing.Image imgPhoto;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                baseurl = ConfigurationSettings.AppSettings["baseurl"];

                if (!Page.IsPostBack)
                {
                    using (categoryimagemgmt objcat = new categoryimagemgmt())
                    {
                        if (Request.QueryString["catid"] != null)
                        {
                          //  jsondata = GetCategoryImage(Request.QueryString["catid"]);
                            ViewState["json"] = jsondata;
                        }
                        else
                            ViewState["json"] = jsondata;
                    }
                }
               
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "eventimage.aspx.cs Pageload ", ex);
            }
            jsondata = ViewState["json"].ToString();
            if (string.IsNullOrEmpty(jsondata))
            {
                jsondata = "null";
            }

        }


        /// <summary>
        /// Handles the Click event of the btnsave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnsave_Click(object sender, EventArgs e)
        {
           
          try
          {
              
              int i = 0;
              using (categoryimagemgmt objcat = new categoryimagemgmt())
              {
                  ValidateDateFolder();

                  if (hdnimage1.Value.Trim().Length > 0)
                  {
                      string imageName = DateTime.Now.ToString("ddMMyyyyHHmmsss") + "1.jpg";                        
                     // string dir = MapPath("../superevent/");
                      string dir = MapPath("../" + Request.QueryString["sitename"]+"/"+ GetImageFolderNameToUpload());
                      string imgpath = dir + imageName;
                      #region :: Base64 ::
                      string strImg = hdnimage1.Value;
                      strImg = strImg.Replace("data:image/png;base64,", "");
                      strImg = strImg.Replace("data:image/jpeg;base64,", "");
                      if (strImg.Trim().Length > 0)
                      {
                          Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                          SaveThumbnail(imageName, Request.QueryString["sitename"]);
                      }

                      #endregion
                  //    objsuper.SaveSuperEventImages(Request.QueryString["supereventid"], imageName);
                      objcat.AddImage(Request.QueryString["NetworkId"], Request.QueryString["catid"], Request.QueryString["siteid"], imageName, "", "", "", GetImageFolderNameToUpload());
                      i++;                     
                  }                 
              }

              if (i > 0)
              {
                  Page.RegisterStartupScript("onsave", "<script>closePOP();</script>");
              }

          }
          catch (Exception ex)
          {
              CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "eventimage.aspx.cs btnsave_Click ", ex);
          }         
              

        }


        private string SaveThumbnail(string img, string sitename)
        {

            string imgpath = "";
            string albumPath = "";

            /*
            if (isserverimage)
            {
                imgpath = Server.MapPath("..") + "/" + folder + img;
                albumPath = Server.MapPath("..") + "/" + folder;
            }
            else
            {
                imgpath = Server.MapPath("..") + "/" + sitename + "/category/" + catname + "/" + img;
                albumPath = Server.MapPath("..") + "/" + sitename + "/category/" + catname + "/";
            }
             */

           imgpath= MapPath("../" + Request.QueryString["sitename"] + "/" + GetImageFolderNameToUpload()+img);
           albumPath = MapPath("../" + Request.QueryString["sitename"] + "/" + GetImageFolderNameToUpload());

            //string imgpath = Server.MapPath("..")+"/categoryImages/"+img;
            //string albumPath = Server.MapPath("..")+"/categoryImages/";
            int iwidth = System.Drawing.Image.FromFile(imgpath).Width;
            int iheight = System.Drawing.Image.FromFile(imgpath).Height;
            int jheight = 105;
            int jwidth = 148;
            if (iwidth < jwidth)
                jwidth = iwidth;
            if (iheight < jheight)
                jheight = iheight;

            if (iheight > iwidth)
            {
                double xx = (iwidth * jheight) / iheight;
                jwidth = int.Parse(xx.ToString());
            }
            else
            {
                double xx = (iheight * jwidth) / iwidth;
                jheight = int.Parse(xx.ToString());
            }

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
                thumbnail = SourceBitmap.GetThumbnailImage(jwidth, jheight, null, IntPtr.Zero);
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
                SourceBitmap.GetThumbnailImage(jwidth, jheight, null, IntPtr.Zero).Dispose();
                thumbnail.Dispose();
                SourceBitmap.Dispose();
            }

            try
            {
                int kheight = 166;
                int kwidth = 240;
                //string fnameTN  = Path.GetFileName(imgpath);
                //create image object
                System.Drawing.Image newthumb = System.Drawing.Image.FromFile(imgpath);
                if (iwidth < kwidth)
                    kwidth = iwidth;
                if (iheight < kheight)
                    kheight = iheight;

                if (iheight > iwidth)
                {
                    double xx = (iwidth * kheight) / iheight;
                    kwidth = int.Parse(xx.ToString());
                }
                else
                {
                    double xx = (iheight * kwidth) / iwidth;
                    kheight = int.Parse(xx.ToString());
                }
                Bitmap objbitmap = null;
                try
                {
                    //new code for smooth image
                    objbitmap = new Bitmap(kwidth, kheight);

                    System.Drawing.Graphics objgr = System.Drawing.Graphics.FromImage(objbitmap);

                    //objgr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //objgr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    objgr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, kwidth, kheight);

                    objgr.DrawImage(newthumb, rectDestination);

                    //pass destination of file
                    string newimage = albumPath + "TNCat_" + fnameHRW;
                    objbitmap.Save(newimage);
                    //dispose objects


                  //  imgPhoto.Dispose();
                  //  newthumb.Dispose();
                }
                catch (Exception excp)
                {
                    Response.Write(excp.ToString());
                }
                finally
                {
                    objbitmap.Dispose();
                    newthumb.Dispose();
                }
            }
            catch (Exception excp)
            {

            }
            return "TN_" + img;
        }


        public string GetImageFolderNameToUpload()
        {
            return string.Format("{0}/{1}/", DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("MMMdd"));
        }

        public void ValidateDateFolder()
        {
            try
            {
                string dir = ConfigurationSettings.AppSettings["newImagePath"];
                string imagedate = GetImageFolderNameToUpload();
                string[] strarry = imagedate.Split('/');
                string monthyearfolder = strarry[0];
                string dayfolder = strarry[1];
                string sitename = Request.QueryString["sitename"].ToString();
                if (!Directory.Exists(dir +sitename+"/" + monthyearfolder + "/" + dayfolder))
                {

                    if (!Directory.Exists(dir + sitename))
                    {
                        Directory.CreateDirectory(dir+sitename);
                    }

                    if (!Directory.Exists(dir +sitename+"/" + monthyearfolder))
                    {
                        Directory.CreateDirectory(dir + sitename + "/" + monthyearfolder);
                    }
                    if (!Directory.Exists(dir + sitename + "/" + monthyearfolder+"/" + dayfolder))
                    {
                        Directory.CreateDirectory(dir + sitename + "/" + monthyearfolder+"/" + dayfolder);
                    }
                    
                }


            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, " ", ex);
            }

        
        
       }

        #region :: Base 64 functions ::
        /// <summary>
        /// Base64s to image.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <returns>System.Drawing.Image.</returns>
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

        /// <summary>
        /// Images to base64.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="format">The format.</param>
        /// <returns>System.String.</returns>
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
        #endregion

        #region :: GetAllImages ::
        /// <summary>
        /// Gets the super event images.
        /// </summary>
        /// <param name="supereventid">The supereventid.</param>
        /// <returns>System.String.</returns>
        public string GetCategoryImage(string catid)
        {
            string jsondata = "";
            DataTable dt = new DataTable();
            try
            {
                using (categoryimagemgmt obj = new categoryimagemgmt())
                {
                    dt = obj.GetCategoryImage(Request.QueryString["NetworkId"], catid, Request.QueryString["siteid"]);
                    if (dt.Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("{\"items\":[");
                        foreach (DataRow dr in dt.Rows)
                        {
                            sb.Append("{\"id\":" + dr["ImageID"].ToString() + ","+ "\"title\":\"" + dr["Image"].ToString() + "\"},");
                        }
                        sb.Append("]}");
                        jsondata = sb.ToString();
                        jsondata = jsondata.Remove(jsondata.Length - 3, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "eventimage.aspx.cs GetSuperEventImages ", ex);
            }
            return jsondata;
        }


        #endregion
    }
}