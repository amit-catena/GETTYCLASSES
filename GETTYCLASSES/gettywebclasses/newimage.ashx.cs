using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Net;
using Gettyclasses;
using System.Configuration;
using gettywebclasses;

namespace gettywebclasses
{
    /// <summary>
    /// Summary description for newimage
    /// </summary>
    public class newimage : IHttpHandler
    {
        private HttpContext _context;
        public void ProcessRequest(HttpContext context)
        {
            string endurl = "";
            //write your handler implementation here.
            _context = context;
            if (context.Request.QueryString["url"] != null && context.Request.QueryString["type"] != null)
            {
                endurl = GetURL(context.Request.QueryString["url"], context.Request.QueryString["type"]);

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

        public bool IsReusable
        {
            get
            {
                return false;
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
                    //ltimg.Text = string.Format("<img  id='img1' src='{0}'/>", "data:image/png;base64," + Convert.ToBase64String(bytes));

                    _context.Response.ContentType = "image/bmp";
                    _context.Response.OutputStream.Write(bytes, 0, bytes.Length);
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

                    case "2":
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
    }
}