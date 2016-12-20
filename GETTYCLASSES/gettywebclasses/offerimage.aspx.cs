using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Net;
using Gettyclasses;
using System.Configuration;
 
 

namespace gettywebclasses
{
    public partial class offerimage : System.Web.UI.Page
    {

        public string baseurl = ConfigurationSettings.AppSettings["baseurl"];
        public string topImagepath = string.Empty;
        public static string MainFolder = "";
        string base64String = "";
        string strImageName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {

                    //Capture();
                }
                catch
                {

                }
               
            }
            else
            {
                strImageName = string.Format("TN_{0}.jpg", DateTime.Now.ToString("yyyyMMddhhmmss"));
                // Convert Base64 String to byte[]
                try
                {
                    string year,day;
                    year=DateTime.Now.ToString("yyyyMM");
                    day=DateTime.Now.ToString("MMMdd");
                    base64String = Request.Form["img64"];

                    base64String = base64String.Replace("data:image/png;base64,", "");
                    byte[] imageBytes = Convert.FromBase64String(base64String);
                    MemoryStream ms = new MemoryStream(imageBytes, 0,
                        imageBytes.Length);

                    // Convert byte[] to Image
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    ValidateDateTimeFolder(year,day);
                    image.Save(Server.MapPath(string.Format("~/Racingtweets/Tweets/{0}/{1}/",year,day)) + strImageName);

                    strImageName = string.Format("http://www.pix123.com/Racingtweets/Tweets/{0}/{1}/{2}",year,day,strImageName);
                }
                catch
                {
                    strImageName = "";
                }

                if (!string.IsNullOrEmpty(strImageName))
                {
                    Page.RegisterStartupScript("onsave", "<script>closePOP('" + strImageName + "');</script>");
                }

            }

            
        }

        protected void Capture()
        {
            string endurl = "";

            if (Request.QueryString["url"] != null && Request.QueryString["type"] != null)
            {
                endurl = GetURL(Request.QueryString["url"], Request.QueryString["type"]);

                Thread thread = new Thread(delegate()
                {
                    using (WebBrowser browser = new WebBrowser())
                    {
                        browser.ScrollBarsEnabled = false;
                        browser.AllowNavigation = true;
                        browser.Navigate(endurl);
                        browser.Width = 1024;
                        browser.Height = 768;
                        browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentCompleted);
                        //browser.DocumentCompleted += Browser_DocumentCompleted;
                        //tmrTick = new System.Web.UI.Timer();
                        //tmrTick.Interval = 10000;
                        //tmrTick.Enabled = true;
                        //tmrTick.Tick += new EventHandler<EventArgs>(Timer_Tick);
                        while (browser.ReadyState != WebBrowserReadyState.Complete)
                        {
                            System.Windows.Forms.Application.DoEvents();

                        }
                    }
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join(20000);
            }
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //timer.Enabled = false;

            WebBrowser browser = sender as WebBrowser;
            using (Bitmap bitmap = new Bitmap(browser.Width, browser.Height))
            {
                browser.DrawToBitmap(bitmap, new Rectangle(0, 0, browser.Width, browser.Height));
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] bytes = stream.ToArray();
                    
                   ltimg.Text  = string.Format("<img  id='img1' src='{0}'/>","data:image/png;base64," + Convert.ToBase64String(bytes));
                }                
            }
        }

        private void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = sender as WebBrowser;
            using (Bitmap bitmap = new Bitmap(browser.Width, browser.Height))
            {
                browser.DrawToBitmap(bitmap, new Rectangle(0, 0, browser.Width, browser.Height));
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] bytes = stream.ToArray();
                    ltimg.Text = string.Format("<img  id='img1' src='{0}'/>", "data:image/png;base64," + Convert.ToBase64String(bytes));

                    Page.RegisterStartupScript("imageload", "<script>updateImage('" + "test" + "');</script>");
                }
            }
        }

        public string GetURL(string id, string linktype)
        {
            string url = "";
            try
            {
                switch (linktype)
                {
                    case "1":
                        url = GetLinkRefFromHperlink(id);
                        break;
                       
                    case"2":
                        url = GetPromotionalLinkRef(id);
                        break;

                }
            }
            catch
            {

            }
            return url;
        }

        public string GetLinkRefFromHperlink(string id)
        {
            string url = "";
            try
            {

                using (SQLHelper obj = new SQLHelper(ConfigurationManager.AppSettings["Network2"].ToString()))
                {
                    url = Convert.ToString(obj.ExecuteScaler("select LinkReference from HyperLinkManager where linkid=" + id));
                }
            }
            catch
            {
            }
            return url;
        }

        public string GetPromotionalLinkRef(string id)
        {
            string url = "";
            try
            {

                using (SQLHelper obj = new SQLHelper(ConfigurationManager.AppSettings["Adsensestring"].ToString()))
                {
                    url = Convert.ToString(obj.ExecuteScaler("select LinkReference from OfferLinkManager where linkid=" + id));
                }
            }
            catch
            {
            }
            return url;
        }

        public void ValidateDateTimeFolder(string year ,string day)
        {
            string directoryPath = Server.MapPath(string.Format("~/Racingtweets/Tweets/{0}", year));
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                directoryPath = Server.MapPath(string.Format("~/Racingtweets/Tweets/{0}/{1}", year, day));

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }
            catch
            {

            }
        }
        
    }
}