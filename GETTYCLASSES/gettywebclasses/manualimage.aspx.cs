using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using Gettyclasses;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace gettywebclasses
{
    public partial class manualimage : System.Web.UI.Page
    {
        string sitefolder = "";
        string monthyearfolder = "";
        string dayfolder = "";
        string strImg = "";
        string imagename = "";
        string imgpath = "";
        string original = "";
        string imageurl = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Page.IsPostBack)
            {
                sitefolder = Request.Form["sitefolder"];
                imagename =GetImageName(Request.Form["imagename"]);
                monthyearfolder = DateTime.Now.ToString("yyyyMM");
                dayfolder = DateTime.Now.ToString("MMMdd");
                Function.CreateImagesFolderInBackend(sitefolder, monthyearfolder, dayfolder);

                string ext = Path.GetExtension(imagename);
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

                strImg = Request.Form["imagetext"];
                strImg = strImg.Replace("data:image/jpeg;base64,", "");
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                original = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}/{3}", sitefolder, monthyearfolder, dayfolder, "org" + imagename));
                imgpath = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}/{3}", sitefolder, monthyearfolder, dayfolder, imagename));
                imageurl = string.Format("{4}{0}/{1}/{2}/{3}", sitefolder, monthyearfolder, dayfolder, imagename, ConfigurationManager.AppSettings["baseurl"]);
                if (strImg.Trim().Length > 0)
                {
                    Base64ToImage(strImg).Save(original, jgpEncoder, myEncoderParameters);
                    //Base64ToImage(strImg).Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //Function.SaveThumbnailCompress(imagename, string.Format("{0}/{1}/{2}/", sitefolder, monthyearfolder, dayfolder), "TN", 300, 225);
                   // Function.SaveThumbnailCompress(imagename, string.Format("{0}/{1}/{2}/", sitefolder, monthyearfolder, dayfolder), "TN_TN", 128, 85);

                    Page.ClientScript.RegisterStartupScript(GetType(), "testcall", string.Format("testcall('{0}');", imagename), true);
                }
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


        public string GetImageName(string name)
        {
            string filename = "";
            try
            {
                filename = DateTime.Now.ToString("yyMMddHHMMss") + Path.GetExtension(name);
            }
            catch
            {

            }
            return filename; 
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
        #endregion
    }
}