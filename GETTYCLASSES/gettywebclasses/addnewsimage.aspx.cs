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

namespace gettywebclasses
{
    public partial class addnewsimage : System.Web.UI.Page
    {
        
        public string _strcookiename = string.Empty;
        public string _strSiteID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnsubmit_click(object sender, EventArgs e)
        {
            string imagedate = "";
            string monthyearfolder = "";
            string dayfolder = "";
            string filename = "";
            try
            {
                if (null != Request.QueryString["SiteId"])
                {
                    _strSiteID = Request.QueryString["SiteId"].ToString();
                }

                if (null != Request.QueryString["randomID"])
                {
                    _strcookiename = Request.QueryString["randomID"].ToString();
                }

                if (imagefileupload.PostedFile.ContentLength > 0)
                {

                    ValidateDateFolder();
                    string dir = ConfigurationSettings.AppSettings["newImagePath"];
                    imagedate = GetImageFolderNameToUpload();
                    string[] strarry = imagedate.Split('/');
                    monthyearfolder = strarry[0];
                    dayfolder = strarry[1];
                    dir = dir + "gamingappstore/NEWS/" + monthyearfolder + "/" + dayfolder + "/";
                    string ext = Path.GetExtension(imagefileupload.PostedFile.FileName);
                    filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                    imagefileupload.PostedFile.SaveAs(dir + filename);

                    SaveThumbnailCompress(filename, dir, true);

                    using (apkadddetailsmgmt objMgt = new apkadddetailsmgmt())
                    {
                        objMgt.SaveNewsImage(_strcookiename, _strSiteID, imagedate, filename);
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
                if (!Directory.Exists(dir + "gamingappstore/NEWS/" + monthyearfolder + "/" + dayfolder))
                {

                    if (!Directory.Exists(dir + "gamingappstore"))
                    {
                        Directory.CreateDirectory(dir + "gamingappstore");
                    }

                    if (!Directory.Exists(dir + "gamingappstore/NEWS"))
                    {
                        Directory.CreateDirectory(dir + "gamingappstore/NEWS");
                    }
                    if (!Directory.Exists(dir + "gamingappstore/NEWS/" + monthyearfolder))
                    {
                        Directory.CreateDirectory(dir + "gamingappstore/NEWS/" + monthyearfolder);
                    }
                    if (!Directory.Exists(dir + "gamingappstore/NEWS/" + monthyearfolder + "/" + dayfolder))
                    {
                        Directory.CreateDirectory(dir + "gamingappstore/NEWS/" + monthyearfolder + "/" + dayfolder);
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