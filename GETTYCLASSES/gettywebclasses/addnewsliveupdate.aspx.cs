using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gettyclasses;
using System.Data;
using System.Text;
using System.IO;
using System.Net;
using System.Globalization;
using System.Drawing;

namespace gettywebclasses
{
    public partial class addnewsliveupdate : System.Web.UI.Page
    {
        public string baseurl = "";
        public string networkid = "";
        public string siteid = "";
        public string newsid = "";
        public string userid = "";
        string networkconn = "";
        public string newstitle = "";
        public string strgeneratexml = "/recacheXML.aspx?commentcacheid=N";       
        public string siteurl = "";
        public string autime = "";
        public string uktime = "";
       
      
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {
                ltimg.Text = "<img ID='imggetty'  src='' />";
                try
                {
                    autime = DateTime.UtcNow.AddHours(Convert.ToDouble(hdnAU.Value)).ToString("dd/MM/yyyy HH:mm");
                    uktime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    txtstartdate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    siteurl = Request.QueryString["siteurl"];
                    baseurl = commonfn._baseURL;
                    networkid = Request.QueryString["networkid"];
                    siteid = Request.QueryString["siteid"];
                    newsid = Request.QueryString["newsid"];
                    userid = Request.QueryString["userid"];
                    newstitle = Request.QueryString["title"];
                    networkconn = System.Configuration.ConfigurationManager.AppSettings[networkid];
                    Session["liveupdatesiteid"] = siteid;
                    Session["liveupdateconn"] = networkconn;
                    if (Request.QueryString["operation"] == "list")
                    {                      
                        GetNewsLiveUpdateList();                       
                    }
                    else
                    {                 
                                              
                        if (Request.QueryString["id"] != null)
                        {
                            GetDetails(Request.QueryString["id"]);
                        }
                    }
                   
                }
                catch (Exception ex)
                {
                    CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "addnewliveupdate.aspx.cs !Page.IsPostBack", ex);
                }
            }           
        }
        
       

        public void GetNewsLiveUpdateList()
        {
             networkid = Request.QueryString["networkid"];
             siteid = Request.QueryString["siteid"];
             userid = Request.QueryString["userid"];
            
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sb = new StringBuilder();
                using (newsliveupdatemgmt obj = new newsliveupdatemgmt(networkconn))
                {
                    dt = obj.GetNewsLiveUpdateList(Request.QueryString["newsid"], Request.QueryString["siteid"]);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            sb.Append("<tr >");
                            sb.Append(string.Format("<td align= 'left' bgcolor='#FFFFFF' valign='middle' style='font-family:verdana;font-size:11px;'><input type='checkbox' id='{0}' class='source' value='{0}' name = 'fcheck[]'></div></td>", dr["Id"].ToString()));
                            sb.Append(string.Format("<td align= 'left' bgcolor='#FFFFFF' valign='middle' style='font-family:verdana;font-size:11px;'>{0}</td>", dr["title"].ToString()));
                            sb.Append("<td class='text' align='center' bgcolor='#FFFFFF' valign='middle' style='font-family:verdana;font-size:11px;'><a title='click to manage' class='editlink' href='addnewsliveupdate.aspx?id=" + dr["Id"].ToString() + "&networkid=" + networkid + "&siteid=" + siteid + "&userid=" + dr["UID"].ToString() + "&newsid=" + dr["newsid"].ToString() + "'>Edit</a></td>");
                            sb.Append(string.Format("<td align= 'left' bgcolor='#FFFFFF' valign='middle' style='font-family:verdana;font-size:11px;'>{0}</td>", Convert.ToDateTime(dr["startdate"]).ToString("dd-MM-yyyy HH:mm tt")));
                            if (dr["ishighlight"].ToString() == "Y")
                            {
                                sb.Append(string.Format("<td align= 'center' bgcolor='#FFFFFF' valign='middle' style='font-family:verdana;font-size:11px;'>{0}</td>", "<img src='http://www.writersllc.com/images/icon_status_green.gif' />"));
                            }
                            else
                            {
                                sb.Append(string.Format("<td align= 'center' bgcolor='#FFFFFF' valign='middle' style='font-family:verdana;font-size:11px;'>{0}</td>", "<img src='http://www.writersllc.com/images/icon_status_red.gif' />"));

                            }
                            
                            sb.Append(string.Format("<td align= 'left' bgcolor='#FFFFFF' valign='middle' style='font-family:verdana;font-size:11px;'>{0}</td>", Convert.ToDateTime(dr["addedon"]).ToString("dd-MM-yyyy HH:mm tt")));
                            sb.Append(string.Format("<td align= 'left' bgcolor='#FFFFFF' valign='middle' style='font-family:verdana;font-size:11px;'>{0}</td>", dr["Name"].ToString()));
                            sb.Append("</tr>");
                        }

                    }
                    else
                    {
                        sb.Append("<tr><td style='color:red' colspan='6'>record not found </td></tr>");
                    }
                    ltlist.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "addnewliveupdate.aspx.cs GetNewsLiveUpdateList", ex);
            }
        }

        protected void btnsave_click(object sender, EventArgs e)
        {
            int count = 0;
            string imagename = "";
            
            try
            {
                networkid = Request.QueryString["networkid"];
                networkconn = System.Configuration.ConfigurationManager.AppSettings[networkid];
                using (newsliveupdatemgmt obj = new newsliveupdatemgmt(networkconn))
                {
                    if (file1.HasFile)
                    {
                        imagename = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyhhmmss"), Path.GetExtension(file1.FileName));

                        file1.SaveAs(Server.MapPath("~/newsliveupdate/") + imagename);
                    }
                    else
                    {
                        if (ViewState["img"] != null)
                        {
                            imagename = ViewState["img"].ToString(); 
                        }
                    }
                    if (!string.IsNullOrEmpty(Request.Form["hdngettyimg"].ToString()))
                    {
                        imagename = Request.Form["hdngettyimg"].ToString();
                    }
                   
                    if (!string.IsNullOrEmpty(imagename))
                    {
                        string imgpath = MapPath("../newsliveupdate/" + imagename);
                        int widthAct = System.Drawing.Image.FromFile(imgpath).Width;
                        if (widthAct > 600)
                        {
                            imagename = Function.SaveThumbnailCompress(imagename, HttpContext.Current.Server.MapPath("~/newsliveupdate/"), "TN_", 600, 402);
                        }
                    }
                    
                    obj.NewsId = Request.QueryString["newsid"];
                    obj.Title = textTitle.Text;
                    obj.Description = Request.Form["templateText8"];
                    obj.Addedby = Request.QueryString["userid"];
                    obj.SiteId = Request.QueryString["siteid"];
                    obj.Image = imagename;
                    obj.startdate = GetDate(txtstartdate.Text);
                    obj.region = ddlregion.SelectedValue;
                    if (Request.QueryString["Id"] != null)
                    {
                        obj.Id = Request.QueryString["Id"];
                    }
                    else
                    {
                        obj.Id = "0";
                    }
                    if (chkhighlight.Checked == true)
                    {
                        obj.ishighlight = "Y";
                    }
                    else
                    {
                        obj.ishighlight = "N";
                    }
                    obj.imagebelowtext = txtimgtext.Text;
                    count= obj.SaveLiveNewsUpdate();
                    if (Request.QueryString["siteurl"] != null)
                    {
                       RefreshCache(Request.QueryString["siteurl"]+strgeneratexml);
                    }
                    Page.RegisterStartupScript("onsave", "<script>closePOP('" + count + "','" + obj.NewsId + "');</script>");
                   
                }               
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "addnewliveupdate.aspx.cs btnsave_click", ex);
            }
        }


        private string SaveThumbnail(string img)
        {

            string imgpath = "";
            string albumPath = "";
            imgpath = MapPath("../newsliveupdate/" + img);
            albumPath = MapPath("../newsliveupdate/");

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



        public static void RefreshCache(string siteurl)
        {

            System.Uri objURI;
            System.Net.WebRequest objWebRequest;
            System.Net.WebResponse objWebResponse;
            System.IO.Stream objStream;
            System.IO.StreamReader objStreamReader;

            string strHTML = "";
            try
            {
                objURI = new Uri(siteurl);
                objWebRequest = HttpWebRequest.Create(objURI);
                objWebResponse = objWebRequest.GetResponse();
                objStream = objWebResponse.GetResponseStream();
                objStreamReader = new StreamReader(objStream);
                strHTML = objStreamReader.ReadToEnd();

                objURI = null;

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "addnewliveupdate.aspx.cs RefreshCache", ex);
            }

        }

        public string GetDate(string strdate)
        {
            string date = "";
            if (strdate.Trim().Length > 0)
            {
                string[] strarray;
                string[] arr;

                arr = strdate.Split(' ');
                string time = arr[1];
                strarray = arr[0].Split('/');

                if (strarray.Length > 2)
                {
                    date = string.Format("{0}-{1}-{2} {3}", strarray[2], strarray[1], strarray[0], time);
                }
            }
            else
            {
                date = string.Format("{0} ", DateTime.Now.ToString("yyyy-MM-dd"));
            }
            return date;

        }

       
        public void GetDetails(string id)
        {
            try
            {
                networkid = Request.QueryString["networkid"];
                networkconn = System.Configuration.ConfigurationManager.AppSettings[networkid];
                using (newsliveupdatemgmt obj = new newsliveupdatemgmt(networkconn))
                {
                    DataTable dt = new DataTable();
                    dt = obj.GetNewsLiveUpdateDetails(id);
                    if (dt.Rows.Count > 0)
                    {
                        textTitle.Text = dt.Rows[0]["title"].ToString();
                        ltback3.Text = dt.Rows[0]["description"].ToString();
                        txtstartdate.Text = Convert.ToDateTime(dt.Rows[0]["startdate"]).ToString("dd/MM/yyyy HH:mm");
                        ddlregion.SelectedValue = dt.Rows[0]["region"].ToString();
                        if (dt.Rows[0]["ishighlight"].ToString() == "Y")
                        {
                            chkhighlight.Checked = true;
                        }
                        else
                        {
                            chkhighlight.Checked = false;
                        }
                        txtimgtext.Text = dt.Rows[0]["imagebelowtext"].ToString();
                        if (dt.Rows[0]["image"].ToString() != "")
                        {
                            string imgpath = Gettyclasses.commonfn._baseURL+"newsliveupdate/" + dt.Rows[0]["image"].ToString();
                            ltimg.Text = "<img id='imggetty' src='" + imgpath + "' height='50px' width='50px'/>";
                            ViewState["img"] = dt.Rows[0]["image"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "addnewliveupdate.aspx.cs GetDetails", ex);
            }

        }
         
    }
}