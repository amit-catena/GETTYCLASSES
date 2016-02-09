#region :: namespaces ::
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Configuration;
using Gettyclasses;
#endregion

namespace gettywebclasses
{
    public partial class UploadImageInTemlpate : System.Web.UI.Page
    {
        #region :: variables ::
        public string _imgserver = ConfigurationSettings.AppSettings["imgserver"];
        int siteid = 0;
        int userid = 0;
        bool imgserver = true;
        public string str = string.Empty;
        string imageName = string.Empty;
        string retval = string.Empty;
        string img1 = string.Empty;
        string modifieddate = string.Empty;
        string sitefldname = "signup";
        public System.Drawing.Image imgPhoto;
        int imageid = 0;
        string dir = string.Empty;
        string _imagepath = string.Empty;
        string monthyearfolder = DateTime.Now.ToString("yyyyMM");
        string dayfolder = DateTime.Now.ToString("MMMdd");
        string returnimagepath = "", returnimagename = "";
        public string baseurl = ConfigurationSettings.AppSettings["baseurl"];
        //string ImageServerURL = ConfigurationSettings.AppSettings["ImageServerURL"];
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (null != Request.QueryString["siteid"])
                    siteid = Convert.ToInt32(Request.QueryString["siteid"].ToString());
                if (null != Request.QueryString["userid"])
                    userid = Convert.ToInt32(Request.QueryString["userid"].ToString());
                dir = Server.MapPath(string.Format("{0}/", "signup")); //BLL.Constants.SaveImagePathSignUp;
                CommonLib.CurrentPage.LinkCSS("http://www.developersllc.com/signup/css/style.css");
                CommonLib.CurrentPage.IncludeScript("http://www.developersllc.com/signup/js/jquery-1.4.4.min.js");
                if (IsPostBack && FileUpload1.PostedFile != null)
                {
                    string FName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    FName = FName.Replace(' ', '_');
                    FName = CommonLib.FileHandler.GetUniqueFileName(dir, FName);

                    string imgExt = Path.GetExtension(FName);


                    if (imgExt != string.Empty && (imgExt.ToLower() == ".jpg" || imgExt.ToLower() == ".jpeg" || imgExt.ToLower() == ".gif" || imgExt.ToLower() == ".png"))
                    {
                        imageName = DateTime.Now.ToString("yyyyMMddHHmmss") + imgExt;
                    }
                    else
                    {
                        imageName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    }
                    imgserver = true;
                    img1 = imageName;

                    modifieddate = DateTime.Now.ToString("yyyy/MM/dd");
                    ValidateSiteFolder(sitefldname);
                    CreateImagesFolderInBackend(sitefldname, Convert.ToDateTime(modifieddate).ToString("yyyyMM"), Convert.ToDateTime(modifieddate).ToString("MMMdd"));
                    dir = string.Format(dir + "{2}/{0}/{1}/", Convert.ToDateTime(modifieddate).ToString("yyyyMM"), Convert.ToDateTime(modifieddate).ToString("MMMdd"), sitefldname);
                    _imagepath = Server.MapPath(string.Format("{0}/{1}/{2}/", sitefldname, monthyearfolder, dayfolder));
                    using (FileStream fs = File.Create(_imagepath + imageName))
                    {
                        SaveFile(FileUpload1.PostedFile.InputStream, fs, imageName);
                        returnimagename = Function.SaveThumbnailCompress(imageName, _imagepath, "TN", 300, 170);                       
                        returnimagepath = string.Format("{0}{1}/{2}/{3}/{4}", _imgserver, sitefldname, monthyearfolder, dayfolder, returnimagename);
                        /*File.Copy(dir + imageName, dir + "org_" + imageName);
                        
                        string tnname = "";
                        string imgname = "";
                        string folder = "";
                        try
                        {
                            imgPhoto = System.Drawing.Image.FromFile(dir + "org_" + imageName);
                            int phWidth = imgPhoto.Width;
                            int phHeight = imgPhoto.Height;
                            
                            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);
                            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
                            Graphics grPhoto = Graphics.FromImage(bmPhoto);
                            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                            grPhoto.DrawImage(
                                imgPhoto,                               // Photo Image object
                                new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                                0,                                      // x-coordinate of the portion of the source image to draw. 
                                0,                                      // y-coordinate of the portion of the source image to draw. 
                                phWidth,                                // Width of the portion of the source image to draw. 
                                phHeight,                               // Height of the portion of the source image to draw. 
                                GraphicsUnit.Pixel);                    // Units of measure 


                            bmPhoto.Dispose();
                            grPhoto.Dispose();

                            tnname = this.SaveThumbnailCompress("org_" + imageName, imageName, dir, imgserver);
                            imgPhoto.Dispose();
                        }
                        catch
                        {
                        }*/
                        /* thumbnail */
                       /* #region ::  upload image ::
                        imgserver = true;
                        if (imgserver)
                        {
                            retval = UploadImageToImageServer(modifieddate);
                            //ltimagepath.Text = retval;
                        }
                        #endregion*/
                    }

                    /* downloaded code */
                    /* FileUpload1.SaveAs(Server.MapPath("~/" + name));*/
                    //Image1.ImageUrl = retval;
                    /* Label1.Text = retval;*/
                    if (!string.IsNullOrEmpty(returnimagepath))
                    {
                        using (Signup objsignup = new Signup())
                        {
                            objsignup.SiteID = Convert.ToInt32(siteid);
                            if (!string.IsNullOrEmpty(txttitle.Text))
                                objsignup.ImageTitle = txttitle.Text;
                            else
                                objsignup.ImageTitle = imageName;
                            objsignup.ImageAlttext = txtalttext.Text;
                            objsignup.ImageName = "TN" + imageName;
                            objsignup.ImageDate =  monthyearfolder + "/" + dayfolder + "/";
                            objsignup.AddedBy = Convert.ToInt32(userid);
                            objsignup.ImageID = Convert.ToInt32(idserver_image.Value);
                            if (objsignup.ImageID == 0)
                                imageid = objsignup.AddImageDetails();
                            /*else
                                objsignup.UpdateImageDetails();*/
                            ltsignupimages.Text = LoadAllImages();
                            CommonLib.JavaScriptHandler.RegisterScriptForSM("ShowLoading('N');", true);
                        }
                    }

                }
                else
                {
                    ltsignupimages.Text = LoadAllImages();
                }
            }
            catch(Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        #region :: methods ::
        protected void DeleteImage(object sender, EventArgs e)
        {
            try
            {
                using (Signup objsignup = new Signup())
                {

                    objsignup.ImageID = Convert.ToInt32(idserver_image.Value);
                    bool flag = objsignup.DeleteImage();
                    /*else
                        objsignup.UpdateImageDetails();*/
                    ltsignupimages.Text = LoadAllImages();
                    CommonLib.JavaScriptHandler.RegisterScriptForSM("ShowLoading('N');", true);
                }
            }
            catch
            {
            }
        }
        public string LoadAllImages()
        {
            string allimages = string.Empty;
            DataSet ds = new DataSet();
            StringBuilder str = new StringBuilder();
            string imageurl = string.Empty;
            try
            {
                using (Signup objsignup = new Signup())
                {
                    int siteid = Convert.ToInt32(Session["signup_siteid"].ToString());
                    ds = objsignup.GetsignupimageList(siteid);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            str.Append("<ul id='ul_image'>");
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                DateTime dt = new DateTime();
                                dt = Convert.ToDateTime(dr["addedon"].ToString());
                                string date = dt.ToString("MMMM d, yyyy");
                                imageurl = ConfigurationSettings.AppSettings["baseurl"] + sitefldname + "/" + dr["imagedate"].ToString() + dr["imagename"].ToString();
                                str.Append("<li data-date='" + date + "' data-alt='" + dr["ImageAlttext"].ToString() + "' data-name='" + dr["imagetitle"].ToString() + "' data-url='" + imageurl + "' id='" + dr["imageid"].ToString() + "'><img src='" + imageurl + "' border='0' width='200' height='200' /></li>");
                            }
                            str.Append("</ul>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            allimages = str.ToString();
            return allimages;
        }
        /*private string SaveThumbnailCompress(string dupimg, string img, string folder, bool isserverimage)
        {

            string imgpath = string.Empty;
            string albumPath = string.Empty;
            string img2 = string.Empty;
            string img3 = string.Empty;
            string img4 = string.Empty;

            try
            {
                imgpath = folder + dupimg;
                albumPath = folder;

                if (isserverimage)
                {                    
                    imgpath = folder + dupimg;
                    albumPath = folder;                 
                }
                else
                {
                    imgpath = BLL.Constants.dirpath + "/upload_images/" + dupimg;
                    albumPath = BLL.Constants.dirpath + "/upload_images/";
                }

                int iwidth = System.Drawing.Image.FromFile(imgpath).Width;
                int iheight = System.Drawing.Image.FromFile(imgpath).Height;
                

                

                
                int twidth = 300;
                int theight = 170;

                if (iwidth < twidth)
                    twidth = iwidth;
                if (iheight < theight)
                    theight = iheight;

                if (iheight > iwidth)
                {
                    double xx = (iwidth * theight) / iheight;
                    twidth = int.Parse(xx.ToString());
                }
                else
                {
                    double xx = (iheight * twidth) / iwidth;
                    theight = int.Parse(xx.ToString());
                }


                string uriName = Path.GetFileName(imgpath);
                string fnameTN = "TN" + uriName.Substring(4);
                img2 = fnameTN;

                Bitmap SourceBitmap = null;
                System.Drawing.Image thumbnail = null;

                
                System.Drawing.Image thmimage = System.Drawing.Image.FromFile(imgpath);
                

                try
                {

                

                    SourceBitmap = new Bitmap(twidth, theight);
                

                    System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(SourceBitmap);

                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                    System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, twidth, theight);

                    gr.DrawImage(thmimage, rectDestination, 0, 0, iwidth, iheight, GraphicsUnit.Pixel);

                    string filename = albumPath + fnameTN;

                    SourceBitmap.Save(filename);
                    SourceBitmap.Dispose();
                    thmimage.Dispose();


                }
                catch (Exception exp)
                {
                }
                finally
                {
                    SourceBitmap.Dispose();
                    thmimage.Dispose();
                }

               
            }
            catch
            {
            }
            return img;
        }*/
        public static bool ValidateSiteFolder(string sitefolder)
        {
            string folderpath = ConfigurationSettings.AppSettings["NewImagePath"];// NewImagePath
            bool flag = false;
            try
            {
                if (!Directory.Exists(string.Format("{0}{1}", folderpath, sitefolder)))
                {
                    Directory.CreateDirectory(string.Format("{0}{1}", folderpath, sitefolder));
                    flag = true;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return flag;
        }
        public static bool CreateImagesFolderInBackend(string sitefolder, string monthyearfolder, string dayfolder)
        {
            string folderpath = ConfigurationSettings.AppSettings["NewImagePath"];
            bool flag = false;
            try
            {
                if (!Directory.Exists(string.Format("{0}{1}\\{2}", folderpath, sitefolder, monthyearfolder)))
                {
                    Directory.CreateDirectory(string.Format("{0}{1}\\{2}", folderpath, sitefolder, monthyearfolder));
                }
                if (!Directory.Exists(string.Format("{0}{1}\\{2}\\{3}", folderpath, sitefolder, monthyearfolder, dayfolder)))
                {
                    Directory.CreateDirectory(string.Format("{0}{1}\\{2}\\{3}", folderpath, sitefolder, monthyearfolder, dayfolder));
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return flag;
        }
        private void SaveFile(Stream stream, FileStream fs, string FName)
        {
            try
            {

                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                }
                fs.Dispose();
                /*if (File.Exists(dir + FName))
                {
                    CommonLib.FileHandler.SaveThumbnail(FName, dir, "", "th_", 430, 320);
                    ltimagepath.Text = dir + "th_" + FName;
                }*/


                /*else
                    retval = UploadImage();*/

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "UploadReplyImage SaveFile()", ex);
            }
        }
       /* private string UploadImageToImageServer(string modifieddate)
        {

            string siteurl = string.Empty;
            string sitefolder = string.Empty;
            string imagpath = string.Empty;
            try
            {
                using (BLL.SiteMgt objst = new BLL.SiteMgt(str))
                {
                    sitefolder = BLL.Constants.ImageServerFolderName;
                    // if (Session["storify_networkid"] != null)
                    imagpath = string.Format(BLL.Constants.dirpath + "/upload_images/{2}/{0}/{1}/", Convert.ToDateTime(modifieddate).ToString("yyyyMM"), Convert.ToDateTime(modifieddate).ToString("MMMdd"), sitefldname);

                    try
                    {
                        BLL.UploadSectionMgmt objUp = new BLL.UploadSectionMgmt();

                        if (img1.Trim().Length > 0)
                        {
                            retval = objUp.Imagetransfer(modifieddate, sitefolder, img1, imagpath, BLL.ImageTypes.UserDrag);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Util.ErrorLog.SaveErrorLog(a_siteid,"Uploadimages","ListNews.cs","UploadNewsAjax",ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "gamingnetadminv2 ajax_post UploadSavedImages", ex);
            }
            return retval;
        }*/
        #endregion
    }
}