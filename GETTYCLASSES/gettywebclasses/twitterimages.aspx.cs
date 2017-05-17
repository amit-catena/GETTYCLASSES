using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLib;
using Gettyclasses;
using System.IO;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;

namespace gettywebclasses
{
    public partial class twitterimages : System.Web.UI.Page
    {
        public string _strcookiename = string.Empty;
        public string _strSiteID = string.Empty;
        public string _strtweetimg = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["copy"] != null && Request.QueryString["copy"] == "y")
            {
                CopyTwitterImages();
            }
        }

        private void CopyTwitterImages()
        {
            DataTable dt = new DataTable();
            string dir = ConfigurationSettings.AppSettings["newImagePath"];
            string folder = ConfigurationSettings.AppSettings["TweetImageFolder"];
            try
            {
                using (apkadddetailsmgmt apk = new apkadddetailsmgmt())
                {
                    dt = apk.GetTwitterImagesData();
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        if (!Directory.Exists(dir + folder + "/" + Convert.ToString(dr["ImagePath"])))
                        {
                            Directory.CreateDirectory(dir + folder + "/" + Convert.ToString(dr["ImagePath"]));
                        }
                        
                        try
                        {
                            string imagepath = dir + folder + "/" + Convert.ToString(dr["ImagePath"]) + Convert.ToString(dr["ImageName"]);
                            Uri uri = new Uri("http://handle.racingtweets.com/Images/tweetimages/" + Convert.ToString(dr["ImageName"]));
                            //Uri uri = new Uri("http://192.168.10.54/twitterscheduler/Images/tweetimages/" + Convert.ToString(dr["ImageName"]));
                            using (WebClient client = new WebClient())
                                client.DownloadFile(uri, imagepath);
                        }
                        catch (Exception ex)
                        {
                        }


                        try
                        {
                            string imagepath = dir + folder + "/" + Convert.ToString(dr["ImagePath"]) + "TN_" + Convert.ToString(dr["ImageName"]);
                            Uri uri = new Uri("http://handle.racingtweets.com/Images/tweetimages/TN_" + Convert.ToString(dr["ImageName"]));
                            //Uri uri = new Uri("http://192.168.10.54/twitterscheduler/Images/tweetimages/TN_" + Convert.ToString(dr["ImageName"]));
                            using (WebClient client = new WebClient())
                                client.DownloadFile(uri, imagepath);
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    
                }
            }
            catch (Exception ex)
            { 
            }
        }

        protected void btnsubmit_click(object sender, EventArgs e)
        {
            string imagedate = "";
            string monthyearfolder = "";
            string dayfolder = "";
            string filename = "";
            try
            {
                if (null != Request.QueryString["randomID"])
                {
                    _strcookiename = Request.QueryString["randomID"].ToString();
                }

                if (null != Request.QueryString["timg"])
                    _strtweetimg = Request.QueryString["timg"].ToString();
                else
                    _strtweetimg = "y"; 

                if (imagefileupload.PostedFile.ContentLength > 0)
                {

                    ValidateDateFolder();
                    string dir = ConfigurationSettings.AppSettings["newImagePath"];
                    imagedate = GetImageFolderNameToUpload();

                    if (_strtweetimg == "y")
                    {
                        if (null != Request.QueryString["sst"] && Request.QueryString["sst"] == "y")
                            dir = dir + ConfigurationSettings.AppSettings["SportTweetImageFolder"] + "/";
                        else
                            dir = dir + ConfigurationSettings.AppSettings["TweetImageFolder"] + "/";
                    }
                    else
                        dir = dir + ConfigurationSettings.AppSettings["HandleImageFolder"] + "/";

                    string ext = Path.GetExtension(imagefileupload.PostedFile.FileName);
                    filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                    imagefileupload.PostedFile.SaveAs(dir + imagedate + filename);
                    imagefileupload.PostedFile.SaveAs(dir + imagedate + "org_" + filename);

                    string Tnname = Function.SaveThumbnailCompress("org_" + filename, filename, dir + imagedate, "TN_", 300, 225);
                    //Function.SaveThumbnailCompress("org_" + filename, filename, dir, "TN_TN_", 128, 85);

                    //SaveThumbnailCompress(filename, dir, true);

                    using (apkadddetailsmgmt objMgt = new apkadddetailsmgmt())
                    {
                        objMgt.SaveTwitterImage(_strcookiename, "0", imagedate, filename);
                    }
                }

                Page.RegisterStartupScript("onsave", "<script>closePOP();</script>");



            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, " ", ex);
            }
        }

        public static string GetImageFolderNameToUpload()
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
                string folder = string.Empty;

                if (_strtweetimg == "y")
                {
                    if (null != Request.QueryString["sst"] && Request.QueryString["sst"] == "y")
                        folder = ConfigurationSettings.AppSettings["SportTweetImageFolder"];
                    else
                        folder = ConfigurationSettings.AppSettings["TweetImageFolder"];
                }
                else
                    folder = ConfigurationSettings.AppSettings["HandleImageFolder"];

                if (!Directory.Exists(dir + folder + "/" + monthyearfolder + "/" + dayfolder))
                {

                    if (!Directory.Exists(dir + "Racingtweets"))
                    {
                        Directory.CreateDirectory(dir + "Racingtweets");
                    }

                    if (!Directory.Exists(dir + folder ))
                    {
                        Directory.CreateDirectory(dir + folder + "");
                    }
                    if (!Directory.Exists(dir +  folder + "/" + monthyearfolder))
                    {
                        Directory.CreateDirectory(dir + folder + "/" + monthyearfolder);
                    }
                    if (!Directory.Exists(dir + folder + "/" + monthyearfolder + "/" + dayfolder))
                    {
                        Directory.CreateDirectory(dir + folder + "/" + monthyearfolder + "/" + dayfolder);
                    }
                }


            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, " ", ex);
            }


        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void SaveThumbnailCompress(string orginalname, string folder, bool isserverimage)
        {
            // IMAGE Compression
            string ext = Path.GetExtension(orginalname);
            ImageCodecInfo jgpEncoder;
            switch (ext.ToUpper())
            {
                case ".JPEG":
                    jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                    break;
                case ".JPG":
                    jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                    break;

                case ".PNG":
                    jgpEncoder = GetEncoder(ImageFormat.Png);
                    break;
                default:
                    jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                    break;

            }


            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            // END COMPRESSION


            string imgpath = folder + orginalname;
            string albumPath = folder;

            //TN IMAGE START

            if (isserverimage)
            {
                imgpath = folder + orginalname;
                //albumPath = GetNewImagePathForSave();
            }


            int iwidth = System.Drawing.Image.FromFile(imgpath).Width;
            int iheight = System.Drawing.Image.FromFile(imgpath).Height;

            int twidth = 300;
            int theight = 300;

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


            string uriName = System.IO.Path.GetFileName(imgpath);
            string fnameTN = "TN_" + orginalname;

            Bitmap SourceBitmap = null;

            // create image object 
            System.Drawing.Image thmimage = System.Drawing.Image.FromFile(imgpath);
            // new code for smooth thumbnail

            try
            {

                //new code for thumbnail

                SourceBitmap = new Bitmap(twidth, theight);
                //new code for smooth image

                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(SourceBitmap);

                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, twidth, theight);

                gr.DrawImage(thmimage, rectDestination, 0, 0, iwidth, iheight, GraphicsUnit.Pixel);

                string filename = albumPath + fnameTN;

                SourceBitmap.Save(filename, jgpEncoder, myEncoderParameters);

                SourceBitmap.Dispose();
                thmimage.Dispose();
                gr.Dispose();

                SourceBitmap = null;
                thmimage = null;
                gr = null;

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Admin, "", ex);
            }
            finally
            {

            }

            //change here on 25 jan ---- fr displaying thumbnails on home page right corner [top stories]

            // TN_TN IMAGE START
            try
            {
                /*--------------------------------------------------------*/
                /*int jheight=105;
                int jwidth=148;*/
                int jheight = 95;
                int jwidth = 128;

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


                Bitmap SourceBitmap1 = null;


                // create image object 
                System.Drawing.Image thmcomimage = System.Drawing.Image.FromFile(imgpath);

                /*******************************************************************/

                try
                {
                    //new code for smooth image
                    SourceBitmap1 = new Bitmap(jwidth, jheight);

                    System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(SourceBitmap1);

                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                    System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, jwidth, jheight);

                    gr.DrawImage(thmcomimage, rectDestination, 0, 0, iwidth, iheight, GraphicsUnit.Pixel);

                    //pass destination of file
                    string compfilename = albumPath + "TN_" + fnameTN;

                    SourceBitmap1.Save(compfilename, jgpEncoder, myEncoderParameters);

                    SourceBitmap1.Dispose();
                    thmcomimage.Dispose();
                    gr.Dispose();
                    SourceBitmap1 = null;
                    thmcomimage = null;
                    gr = null;
                }
                catch (Exception ex)
                {

                    CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Admin, "", ex);
                }
                finally
                {


                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Admin, "", ex);
            }

        }
    }
}