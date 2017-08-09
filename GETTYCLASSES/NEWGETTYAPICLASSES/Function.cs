using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web;

namespace NewGettyAPIclasses
{
    /// <summary>
    /// Public function class
    /// </summary>
    public class Function
    {
        /// <summary>
        /// Static function Tofilename
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToFileName(object input)
        {
            string output = string.Empty;
            try
            {
                output = input.ToString().Trim().Replace("'", "");
                output = output.ToString().Trim().Replace(" ", "-");
            }
            catch (Exception ex) { CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "ToFileName", ex); }
            return output;
        }

        /// <summary>
        /// Totitles for news foldername
        /// </summary>
        /// <param name="input"></param>
        /// <param name="_oldstatus"></param>
        /// <param name="_foldername"></param>
        /// <returns></returns>
        public static string toTitleNews(object input, string _oldstatus, string _foldername)
        {
            string output = string.Empty;
            try
            {
                if (_oldstatus == "N")
                {
                    output = _foldername;
                }
                else if (input.ToString() != "" && input != null)
                {
                    output = input.ToString().Trim().Replace("\"", @"");
                    output = output.ToString().Trim().Replace("’", "'");
                    output = output.ToString().Trim().Replace("‘", "'");
                    output = output.ToString().Trim().Replace("–", "-");
                    output = output.ToString().Trim().Replace("“", "");
                    output = output.ToString().Trim().Replace("”", "");
                    output = output.ToString().Trim().Replace("‘", "'");
                    output = output.ToString().Trim().Replace("¼", "1/4");
                    output = output.ToString().Trim().Replace("€", "");
                    output = output.ToString().Trim().Replace("…", "...");
                    output = output.ToString().Trim().Replace("'", @"");
                    output = output.ToString().Trim().Replace(".", " ");
                    output = output.ToString().Trim().Replace(":", " ");
                    output = output.ToString().Trim().Replace("?", " ");
                    output = output.ToString().Trim().Replace("? ", " ");
                    output = output.ToString().Trim().Replace(",", " ");
                    output = output.ToString().Trim().Replace("|", " ");
                    output = output.ToString().Trim().Replace("£", " ");
                    output = output.ToString().Trim().Replace("$", " ");
                    output = output.ToString().Trim().Replace("&", " ");
                    output = output.ToString().Trim().Replace("<", " ");
                    output = output.ToString().Trim().Replace(">", " ");
                    output = output.ToString().Trim().Replace(", ", " ");
                    output = output.ToString().Trim().Replace("/", " ");
                    output = output.ToString().Trim().Replace("%", " ");
                    output = output.ToString().Trim().Replace("#", " ");
                    output = output.ToString().Trim().Replace(" ", "-");
                    output = output.ToString().Trim().Replace("--", "-");
                    output = HTMLEncodeSpecialChars(output);
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "toTitleNews", ex);
            }
            return output;
        }

        /// <summary>
        /// totitle for filename
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string toTitle(object input)
        {
            string output = "";
            try
            {
                if (input.ToString() != "" && input != null)
                {
                    output = input.ToString().Trim().Replace("\"", @"");
                    output = output.ToString().Trim().Replace("’", "'");
                    output = output.ToString().Trim().Replace("‘", "'");
                    output = output.ToString().Trim().Replace("–", "-");
                    output = output.ToString().Trim().Replace("“", "");
                    output = output.ToString().Trim().Replace("”", "");
                    output = output.ToString().Trim().Replace("‘", "'");
                    output = output.ToString().Trim().Replace("¼", "1/4");
                    output = output.ToString().Trim().Replace("€", "");
                    output = output.ToString().Trim().Replace("…", "...");
                    output = output.ToString().Trim().Replace("'", @"");
                    output = output.ToString().Trim().Replace(".", " ");
                    output = output.ToString().Trim().Replace(":", " ");
                    output = output.ToString().Trim().Replace("?", " ");
                    output = output.ToString().Trim().Replace("? ", " ");
                    output = output.ToString().Trim().Replace(",", " ");
                    output = output.ToString().Trim().Replace("|", " ");
                    output = output.ToString().Trim().Replace("£", " ");
                    output = output.ToString().Trim().Replace("$", " ");
                    output = output.ToString().Trim().Replace("&", " ");
                    output = output.ToString().Trim().Replace("<", " ");
                    output = output.ToString().Trim().Replace(">", " ");
                    output = output.ToString().Trim().Replace(", ", " ");
                    output = output.ToString().Trim().Replace("/", " ");
                    output = output.ToString().Trim().Replace("%", " ");
                    output = output.ToString().Trim().Replace("#", " ");
                    output = output.ToString().Trim().Replace(" ", "-");
                    output = output.ToString().Trim().Replace("--", "-");
                    output = output.ToString().Trim().Replace(":", "");

                    output = HTMLEncodeSpecialChars(output);
                }
                //return output;
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "toTitle", ex);
            }
            return output;
        }


        /// <summary>
        /// Create Bitmap for Smoothimage  
        /// </summary>
        /// <param name="iwidth"></param>
        /// <param name="iheight"></param>
        /// <param name="jwidth"></param>
        /// <param name="jheight"></param>
        /// <param name="imgpath"></param>
        /// <param name="albumPath"></param>
        /// <returns></returns>
        public bool SmothImage(int iwidth, int iheight, int jwidth, int jheight, string imgpath, string albumPath)
        {
            //drawimage
            int cnt = 0;
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
                if (jheight >= 250)
                    jheight = 250;
            }

            string uriName = Path.GetFileName(imgpath);
            //change urname
            if (uriName.StartsWith("ORG_"))
                uriName = uriName.Replace("ORG_", "");
            Bitmap SourceBitmap = null;
            System.Drawing.Image thumbnail = null;
            string fnameHRW = "TN_" + uriName;

            Bitmap SourceBitmap1 = null;
            System.Drawing.Image thumbnail1 = null;
            System.Drawing.Image thmimage = System.Drawing.Image.FromFile(imgpath);

            /*******************************************************************/
            // create image object 
            try
            {

                SourceBitmap = new Bitmap(jwidth, jheight);

                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(SourceBitmap);

                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, jwidth, jheight);

                gr.DrawImage(thmimage, rectDestination, 0, 0, iwidth, iheight, GraphicsUnit.Pixel);

                string filename = albumPath + fnameHRW;
                SourceBitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                cnt++;
                SourceBitmap.Dispose();
                thmimage.Dispose();
            }
            catch (Exception exp)
            {
                //exp.ToString(); 
            }
            finally
            {
                SourceBitmap.Dispose();
                thmimage.Dispose();

            }
            if (cnt > 0)
                return true;
            else
                return false;

        }


        /// <summary>
        /// Save Created Thumbnails and compress it
        /// </summary>
        /// <param name="img"></param>
        /// <param name="_path"></param>
        /// <param name="_prefix"></param>
        /// <param name="_crtewidth"></param>
        /// <param name="_crtheight"></param>
        /// <returns></returns>
        public static string SaveThumbnailCompress(string img, string _path, string _prefix, int _crtewidth, int _crtheight)
        {

            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            string imgpath = _path + img;
            string albumPath = _path;

            int iwidth = System.Drawing.Image.FromFile(imgpath).Width;
            int iheight = System.Drawing.Image.FromFile(imgpath).Height;


            int twidth = _crtewidth;
            int theight = _crtheight;

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


            string ImgFilePath = img;
            string uriName = Path.GetFileName(imgpath);
            Bitmap SourceBitmap = null;
            //new 
            Bitmap imgbmt = (Bitmap)System.Drawing.Image.FromFile(imgpath);
            BitmapData sourceData;
            //create image object
            System.Drawing.Image thmimage = System.Drawing.Image.FromFile(imgpath);
            System.Drawing.Image thumbnail = null;
            string fnameTN = _prefix + uriName;
            //new code for smooth  thumbnail 
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

                SourceBitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                //SourceBitmap.Dispose();
                //thmimage.Dispose();
            }
            catch (Exception exp)
            {
                //exp.ToString(); 
            }
            finally
            {
                SourceBitmap.Dispose();
                SourceBitmap = null;
                thmimage.Dispose();
                imgbmt.Dispose();

            }

            //Orginal Image
            if (_crtewidth == 300)
            {
                try
                {
                    /*--------------------------------------------------------*/
                    /*int jheight=105;
                    int jwidth=148;*/
                    int jheight = 360;
                    int jwidth = 600;

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

                        gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, jwidth, jheight);

                        gr.DrawImage(thmcomimage, rectDestination, 0, 0, iwidth, iheight, GraphicsUnit.Pixel);

                        //pass destination of file
                        thmcomimage.Dispose();
                        try
                        {
                            File.Delete(imgpath);
                        }
                        catch (Exception ex)
                        {
                            //Response.Write("Error in DeletingFile");
                        }
                        SourceBitmap1.Save(imgpath, jgpEncoder, myEncoderParameters);

                        SourceBitmap1.Dispose();
                        thmcomimage.Dispose();
                        gr.Dispose();
                        SourceBitmap1 = null;
                        thmcomimage = null;
                        gr = null;
                    }
                    catch (Exception excp)
                    {


                    }
                    finally
                    {


                    }
                }
                catch (Exception exp)
                {
                 
                }
            }
            return _prefix + img;

        }

        /// <summary>
        /// Save thumbnail images
        /// </summary>
        /// <param name="orgfilename"></param>
        /// <param name="img"></param>
        /// <param name="_path"></param>
        /// <param name="_prefix"></param>
        /// <param name="_crtewidth"></param>
        /// <param name="_crtheight"></param>
        /// <returns></returns>
        public static string SaveThumbnailCompress(string orgfilename, string img, string _path, string _prefix, int _crtewidth, int _crtheight)
        {

            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            string imgpath = _path + orgfilename;
            string albumPath = _path;
            string mainfile = _path + img;
            int iwidth = System.Drawing.Image.FromFile(imgpath).Width;
            int iheight = System.Drawing.Image.FromFile(imgpath).Height;
            int twidth = _crtewidth;
            int theight = _crtheight;

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



            string uriName = Path.GetFileName(mainfile);
            Bitmap SourceBitmap = null;
            //new 
            Bitmap imgbmt = (Bitmap)System.Drawing.Image.FromFile(imgpath);
            BitmapData sourceData;
            //create image object
            System.Drawing.Image thmimage = System.Drawing.Image.FromFile(imgpath);
            System.Drawing.Image thumbnail = null;
            string fnameTN = _prefix + uriName;
            //new code for smooth  thumbnail 
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
                SourceBitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception exp)
            {
                //exp.ToString(); 
            }
            finally
            {
                SourceBitmap.Dispose();
                SourceBitmap = null;
                thmimage.Dispose();
                imgbmt.Dispose();

            }

            //Orginal Image
            if (_crtewidth == 300)
            {
                try
                {
                    /*--------------------------------------------------------*/
                    int jheight = 360;
                    int jwidth = 600;

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

                        gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, jwidth, jheight);

                        gr.DrawImage(thmcomimage, rectDestination, 0, 0, iwidth, iheight, GraphicsUnit.Pixel);

                        //pass destination of file
                        thmcomimage.Dispose();
                        try
                        {
                            File.Delete(mainfile);
                        }
                        catch (Exception ex)
                        {
                            //Response.Write("Error in DeletingFile");
                        }
                        SourceBitmap1.Save(mainfile, jgpEncoder, myEncoderParameters);

                        SourceBitmap1.Dispose();
                        thmcomimage.Dispose();
                        gr.Dispose();
                        SourceBitmap1 = null;
                        thmcomimage = null;
                        gr = null;
                    }
                    catch (Exception excp)
                    {


                    }
                    finally
                    {


                    }
                }
                catch (Exception exp)
                {
                    //Response.Write(exp);
                }
            }
            return _prefix + img;

        }

        /// <summary>
        /// Get image code info
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoder(ImageFormat format)
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


        public static string SaveoriginalImage(string img, string _path, string _prefix, int _crtewidth, int _crtheight)
        {
            string imgpath = _path + img;
            string albumPath = _path;

            int iwidth = System.Drawing.Image.FromFile(imgpath).Width;
            int iheight = System.Drawing.Image.FromFile(imgpath).Height;
            int twidth = _crtewidth;
            int theight = _crtheight;

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
            string ImgFilePath = img;
            string uriName = Path.GetFileName(imgpath);
            Bitmap SourceBitmap = null;
            //new 
            Bitmap imgbmt = (Bitmap)System.Drawing.Image.FromFile(imgpath);
            BitmapData sourceData;
            //create image object
            System.Drawing.Image thmimage = System.Drawing.Image.FromFile(imgpath);
            System.Drawing.Image thumbnail = null;
            string fnameTN = uriName.Substring(6);
            //new code for smooth  thumbnail 
            try
            {

                //new code for thumbnail

                SourceBitmap = new Bitmap(twidth, theight);
                //new code for smooth image
                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(SourceBitmap);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, twidth, theight);
                gr.DrawImage(thmimage, rectDestination, 0, 0, iwidth, iheight, GraphicsUnit.Pixel);
                string filename = albumPath + fnameTN;
                SourceBitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                //SourceBitmap.Dispose();
                //thmimage.Dispose();


            }
            catch (Exception exp)
            {
                //exp.ToString(); 
            }
            finally
            {
                SourceBitmap.Dispose();
                SourceBitmap = null;
                thmimage.Dispose();
                imgbmt.Dispose();

            }
            return img;

        }

        /// <summary>
        /// Get site networkID
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
        public static int GetnetworkID(string _ID)
        {
            int _nwtstring = 0;
            try
            {
                switch (_ID.ToLower())
                {
                    case "network1":
                        _nwtstring = 1;
                        break;
                    case "network2":
                        _nwtstring = 2;
                        break;
                    case "network3":
                        _nwtstring = 3;
                        break;
                    case "network4":
                        _nwtstring = 4;
                        break;
                    case "network5":
                        _nwtstring = 5;
                        break;
                    case "network6":
                        _nwtstring = 6;
                        break;
                    case "network7":
                        _nwtstring = 7;
                        break;
                    case "network8":
                        _nwtstring = 8;
                        break;
                    case "network9":
                        _nwtstring = 9;
                        break;
                    case "network10":
                        _nwtstring = 10;
                        break;
                }

            }
            catch (Exception ex)
            {

                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "Gettyimages-addimages-GetnetworkID-" + _ID, ex);

            }

            return _nwtstring;


        }



        /// <summary>
        /// Updated version with HTML encode Must be used for filtering client side content
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSql(object input)
        {
            string output = "";
            try
            {
                output = input.ToString();
                output = output.ToString().Trim().Replace("'", "");
                output = output.ToString().Trim().Replace("\'", "\'\'");
                output = output.ToString().Trim().Replace("“", "");
                output = output.ToString().Trim().Replace("”", "");
                output = output.ToString().Trim().Replace("%", "");
                output = output.ToString().Trim().Replace("\"", "");
                output =HttpContext.Current.Server.HtmlEncode(output.ToString());
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return output;
        }

        /// <summary>
        /// Sql parameter conversion
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSqlParameter(object input)
        {
            string output = "";
            try
            {
                output = input.ToString();
                //output = HttpContext.Current.Server.HtmlEncode(output.ToString());
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return output;
        }
        /// <summary>
        /// Temp SQL String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String totmpSql(object input)
        {
            string output = input.ToString().Trim().Replace(Convert.ToChar(146), Convert.ToChar(39));
            output = output.ToString().Trim().Replace(Convert.ToChar(147), Convert.ToChar(34));
            output = output.ToString().Trim().Replace(Convert.ToChar(148), Convert.ToChar(34));
            output = output.ToString().Trim().Replace(Convert.ToChar(10), Convert.ToChar(32));
            output = output.ToString().Trim().Replace(Convert.ToChar(13), Convert.ToChar(32));

            return output;
        }

        /// <summary>
        /// HtmlEncodespecials
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string HTMLEncodeSpecialChars(string text)
        {
            string t = System.Web.HttpUtility.UrlEncode(Encoding.GetEncoding("iso-8859-8").GetBytes(text));
            return t.ToString();
        }

        /// <summary>
        /// Get Site folder name
        /// </summary>
        /// <param name="siteurl"></param>
        /// <returns></returns>
        public static string GetSiteFolderName(string siteurl)
        {

            string strSiteName = "";
            string[] strarr = siteurl.Split('.');

            if (strarr.Length > 3)
            {
                strSiteName = string.Format("{0}-{1}", strarr[1].Trim(), strarr[3].Trim());
            }
            else
            {
                strSiteName = strarr[1];
            }

            return strSiteName;
        }

        /// <summary>
        /// Get site network connection strings
        /// </summary>
        /// <param name="NetworkID"></param>
        /// <returns></returns>
        public static string GetnetworkConnectionstring(string NetworkID)
        {
            string _nwtstring = string.Empty;
            try
            {
                switch (NetworkID)
                {
                    case "1":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network1"].ToString();
                        break;
                    case "2":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network2"].ToString();
                        break;
                    case "3":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network3"].ToString();
                        break;
                    case "4":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network4"].ToString();
                        break;
                    case "5":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network5"].ToString();
                        break;
                    case "6":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network6"].ToString();
                        break;
                    case "7":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network7"].ToString();
                        break;
                    case "8":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network8"].ToString();
                        break;
                    case "9":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network9"].ToString();
                        break;
                    case "10":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network10"].ToString();
                        break;
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GetnetworkConnectionstring :", ex);
            }

            return _nwtstring;


        }

        /// <summary>
        /// Get network connection string
        /// </summary>
        /// <param name="_ID"></param>
        /// <returns></returns>
        public static string GetnetworkIDwithconnectionstring(string _ID)
        {
            string _nwtstring = string.Empty;
            try
            {
                switch (_ID.ToLower())
                {
                    case "network1":
                        _nwtstring =System.Configuration.ConfigurationSettings.AppSettings["Network1"].ToString();
                        break;
                    case "network2":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network2"].ToString();
                        break;
                    case "network3":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network3"].ToString();
                        break;
                    case "network4":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network4"].ToString();
                        break;
                    case "network5":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network5"].ToString();
                        break;
                    case "network6":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network6"].ToString();
                        break;
                    case "network7":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network7"].ToString();
                        break;
                    case "network8":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network8"].ToString();
                        break;
                    case "network9":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network9"].ToString();
                        break;
                    case "network10":
                        _nwtstring = System.Configuration.ConfigurationSettings.AppSettings["Network10"].ToString();
                        break;
                    default:
                         _nwtstring =System.Configuration.ConfigurationSettings.AppSettings["Network1"].ToString();
                        break;
                
                }

            }
            catch (Exception ex)
            {

                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "Gettyimages-addimages-GetnetworkID-" + _ID, ex);

            }

            return _nwtstring;


        }

        /// <summary>
        /// Create image folder on server
        /// </summary>
        /// <param name="sitefolder"></param>
        /// <param name="monthyearfolder"></param>
        /// <param name="dayfolder"></param>
        /// <returns></returns>

        #region "Create MonthDayYearFolderin backend"
        public static bool CreateImagesFolderInBackend(string sitefolder, string monthyearfolder, string dayfolder)
        {
            string folderpath = ConstantAPI.downloadpath; 
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
        /// <summary>
        /// Get Date format image folders
        /// </summary>
        /// <returns></returns>
        public static string GetDateForImageToSave()
        {

            return string.Format("{0}/{1}/", DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("MMMdd"));
        }
        /// <summary>
        /// Valida site folders
        /// </summary>
        /// <param name="sitefolder"></param>
        /// <returns></returns>

        public static bool ValidateSiteFolder(string sitefolder)
        {
            string folderpath = System.Configuration.ConfigurationSettings.AppSettings["NewImagePath"];
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

        /// <summary>
        /// Write image
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void WriteImage(string path,string filename, int width, int height)
        {
            Bitmap srcBmp = new Bitmap(path + filename);
            float ratio = srcBmp.Width / srcBmp.Height;
            SizeF newSize = new SizeF(width, height * ratio);
            Bitmap target = new Bitmap((int)newSize.Width, (int)newSize.Height);
            using (Graphics graphics = Graphics.FromImage(target))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(srcBmp, 0, 0, newSize.Width, newSize.Height);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    target.Save(path+"new"+filename, ImageFormat.Jpeg);
                }
            }
            //Response.End();
        }
        #endregion

    }
}
