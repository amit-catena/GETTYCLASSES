#region Namespaces
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

#endregion
namespace Gettyclasses
{
    public class Function
    {
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

            //string ImgFilePath = img;
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
                SourceBitmap.Save(filename);
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



        public static string SaveThumbnailCompress(string img, string _path, string _prefix, int _crtewidth, int _crtheight)
        {
            //string	imgpath			=	Server.MapPath("../"  + sitename + "/category/" + parentname + "/" + catname + "/" +  ntitle.ToString() + "/" + img)  ;
            //string	albumPath			=	Server.MapPath("../"  + sitename + "/category/" + parentname + "/" + catname + "/" +  ntitle.ToString() + "/")  ;
            string imgpath = _path + img;
            string albumPath = _path;

            int iwidth = System.Drawing.Image.FromFile(imgpath).Width;
            int iheight = System.Drawing.Image.FromFile(imgpath).Height;


            //			int twidth = 300;
            //			int theight = 255;
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

                SourceBitmap.Save(filename);
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
            return _prefix + img;

        }


        public static string SaveoriginalImage(string img, string _path, string _prefix, int _crtewidth, int _crtheight)
        {
            //string	imgpath			=	Server.MapPath("../"  + sitename + "/category/" + parentname + "/" + catname + "/" +  ntitle.ToString() + "/" + img)  ;
            //string	albumPath			=	Server.MapPath("../"  + sitename + "/category/" + parentname + "/" + catname + "/" +  ntitle.ToString() + "/")  ;
            string imgpath = _path + img;
            string albumPath = _path;

            int iwidth = System.Drawing.Image.FromFile(imgpath).Width;
            int iheight = System.Drawing.Image.FromFile(imgpath).Height;


            //			int twidth = 300;
            //			int theight = 255;
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

                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, twidth, theight);

                gr.DrawImage(thmimage, rectDestination, 0, 0, iwidth, iheight, GraphicsUnit.Pixel);

                string filename = albumPath + fnameTN;

                SourceBitmap.Save(filename);
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
                output = HttpContext.Current.Server.HtmlEncode(output.ToString());
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return output;
        }
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
        public static String totmpSql(object input)
        {
            string output = input.ToString().Trim().Replace(Convert.ToChar(146), Convert.ToChar(39));
            output = output.ToString().Trim().Replace(Convert.ToChar(147), Convert.ToChar(34));
            output = output.ToString().Trim().Replace(Convert.ToChar(148), Convert.ToChar(34));
            output = output.ToString().Trim().Replace(Convert.ToChar(10), Convert.ToChar(32));
            output = output.ToString().Trim().Replace(Convert.ToChar(13), Convert.ToChar(32));

            return output;
            //input.ToString().Trim().Replace(Convert.ToChar(145),Convert.ToChar(39));

            //output	= output.ToString().Trim().Replace(Convert.ToChar(146),Convert.ToChar(39));
        }


        public static string HTMLEncodeSpecialChars(string text)
        {
            string t = System.Web.HttpUtility.UrlEncode(Encoding.GetEncoding("iso-8859-8").GetBytes(text));
            return t.ToString();
        }

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
                }

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "GetnetworkConnectionstring :", ex);
            }

            return _nwtstring;


        }


        #region "Create MonthDayYearFolderin backend"
        public static bool CreateImagesFolderInBackend(string sitefolder, string monthyearfolder, string dayfolder)
        {
            string folderpath = System.Configuration.ConfigurationSettings.AppSettings["NewImagePath"];


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

        public static string GetDateForImageToSave()
        {

            return string.Format("{0}/{1}/", DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("MMMdd"));
        }

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
        #endregion

    }
}
