using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gettyclasses;
using System.Data;
using System.Data.SqlClient;
using ConnectExample.Samples;
using System.Net;
using System.Configuration;
using CommonLib;
using System.Web.UI.HtmlControls;
using System.Web.Services;   

namespace gettywebclasses
{
    public partial class searchevents : System.Web.UI.Page
    {
        string _imagepathdownload = ConfigurationSettings.AppSettings["ImagePath"];
        string _baseURL = ConfigurationSettings.AppSettings["baseurl"];
        public int _networkid = 1;
        public int _intnewsID = 0;
        public string _strcookiename = string.Empty;
        public string _strSiteID = string.Empty;
        public string _strnews = string.Empty;
        public static string _redirecturl = string.Empty;
        public static int _intstartval;
        public static List<String> _lilist = new List<String>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null != Request.QueryString["NwtID"])
            {
                _networkid = Function.GetnetworkID(Request.QueryString["NwtID"].ToString());
            }

            if (null != Request.QueryString["SiteId"])
            {
                _strSiteID = Request.QueryString["SiteId"].ToString();
            }

            if (null != Request.QueryString["randomID"])
            {
                _strcookiename = Request.QueryString["randomID"].ToString();
            }
            if (null != Request.QueryString["NewsID"])
            {
                _intnewsID = Convert.ToInt32(Request.QueryString["NewsID"].ToString());
            }

            if (null != Request.QueryString["url"])
            {
                _redirecturl = HttpUtility.UrlDecode(Request.QueryString["url"].ToString());
                _redirecturl = _redirecturl.Replace("&amp;", "&");
            }
            if (!Page.IsPostBack)
            {
                _lilist = new List<String>();
                if (_intstartval > 75)
                {
                    prev.Visible = true;
                }
                loadmore.Visible = false;
                prev.Visible = false;
            }
            else
            {

                if (_intstartval > 75)
                {
                    prev.Visible = true;
                }

            }

        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                Getimagedata _objdata = new Getimagedata();
                if (txtsearch.Text != "")
                    _objdata._strserachterm = txtsearch.Text.Trim();
                _intstartval = 1;
                prev.Visible = false;
                loadmore.Visible = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                //Response.Write("_intstartval " + _intstartval+"</br>");  
                _objdata._intstartcnt = _intstartval;
                _objdata._strorientation = RDchkbox.SelectedValue;
                var _data = _objdata.GeteventimageDatalist();
                if (_data.Count > 0)
                {
                    gettydata.DataSource = _data;
                    gettydata.DataBind();
                    commonfn _objfn = new commonfn();
                    commonfn.setSession("token", _objdata._strtoken);
                    commonfn.setSession("securetoken", _objdata._strsecuretoken);
                    ltscript.Text = "";
                }
                else
                {
                    gettydata.DataSource = null;
                    ltscript.Text = "Sorry no records found.";
                }
                _data = null;
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
                ErrorLog.SaveErrorLog(_strSiteID, "Indexpage", "btnsearch_Click", "btnsearch_Click", ex.Message, _networkid.ToString());
            }
        }
        /// <summary>
        /// item data bound on datalist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gettydata_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            Literal _html = (Literal)e.Item.FindControl("lthtml");
            _html.Text = @"<a href='" + _baseURL + "alleventImages.aspx?eventid=" + (int)DataBinder.Eval(e.Item.DataItem, "_gotevent") + @"'  id='htmlA' >
                                <img src='" + HttpUtility.HtmlDecode((string)DataBinder.Eval(e.Item.DataItem, "UrlPreview")).Replace("&amp;", "&") + @"'   runat='server'    />
                            </a>";
            Literal _htmlcnt = (Literal)e.Item.FindControl("ltcount");
            _htmlcnt.Text = @"</br><a href='" + _baseURL + "alleventImages.aspx?eventid=" + (int)DataBinder.Eval(e.Item.DataItem, "_gotevent") + @"'  > " + (int)DataBinder.Eval(e.Item.DataItem, "_goteventcnt") + " images</a>";
            Literal _ltdate = (Literal)e.Item.FindControl("ltdate");
            // _ltdate.Text = @"<div class='dttxt'><b>Date:</b>" + DateTime.Parse((DateTime)DataBinder.Eval(e.Item.DataItem, "DateCreated")) + "</div>";
            //_ltdate.Text = DateTime.ParseExact(Convert.ToString((DateTime)DataBinder.Eval(e.Item.DataItem, "DateCreated")), "DD MMM YY", pr).ToString();
            //DateTime dt = DateTime.ParseExact(Convert.ToString((DateTime)DataBinder.Eval(e.Item.DataItem, "DateCreated")), "DD MMM yyyy", CultureInfo.InvariantCulture);
            //string _strdate = dt.ToString("DD MMM yyyy");
            _ltdate.Text = DateTime.Parse(Convert.ToString((DateTime)DataBinder.Eval(e.Item.DataItem, "DateCreated"))).ToString("dd MMM yyyy");


        }


        protected void btnmore_Click(object sender, EventArgs e)
        {
            try
            {
                Getimagedata _objdata = new Getimagedata();
                if (txtsearch.Text != "")
                    _objdata._strserachterm = txtsearch.Text.Trim();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                _intstartval = _intstartval + 75;
                prev.Visible = true;

                _objdata._intstartcnt = _intstartval;
                _objdata._strorientation = RDchkbox.SelectedValue;
                var _data = _objdata.GeteventimageDatalist();
                if (_data.Count > 0)
                {
                    gettydata.DataSource = _data;
                    gettydata.DataBind();
                    commonfn _objfn = new commonfn();
                    commonfn.setSession("token", _objdata._strtoken);
                    commonfn.setSession("securetoken", _objdata._strsecuretoken);
                    ltscript.Text = "";
                }
                else
                {
                    gettydata.DataSource = null;
                    ltscript.Text = "Sorry no records found.";
                }
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
                ErrorLog.SaveErrorLog(_strSiteID, "Indexpage", "btnmore_Click", "btnmore_Click", ex.Message, _networkid.ToString());
            }
        }
        protected void btnprev_Click(object sender, EventArgs e)
        {
            try
            {
                Getimagedata _objdata = new Getimagedata();
                if (txtsearch.Text != "")
                    _objdata._strserachterm = txtsearch.Text.Trim();
                _intstartval = _intstartval - 75;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                //Response.Write("_intstartval " + _intstartval+"</br>");  
                _objdata._intstartcnt = _intstartval;
                _objdata._strorientation = RDchkbox.SelectedValue;
                if (_intstartval == 1)
                    prev.Visible = false;

                var _data = _objdata.GeteventimageDatalist();
                if (_data.Count > 0)
                {
                    gettydata.DataSource = _data;
                    gettydata.DataBind();
                    commonfn _objfn = new commonfn();
                    commonfn.setSession("token", _objdata._strtoken);
                    commonfn.setSession("securetoken", _objdata._strsecuretoken);
                    ltscript.Text = "";
                }
                else
                {
                    gettydata.DataSource = null;
                    ltscript.Text = "Sorry no records found.";
                }
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
                ErrorLog.SaveErrorLog(_strSiteID, "Indexpage", "btnprev_Click", "btnprev_Click", ex.Message, _networkid.ToString());
            }
        }

        protected void RDchkbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Getimagedata _objdata = new Getimagedata();
                if (txtsearch.Text != "")
                    _objdata._strserachterm = txtsearch.Text.Trim();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                //Response.Write("_intstartval " + _intstartval+"</br>");  
                _intstartval = 1;
                _objdata._intstartcnt = _intstartval;
                _objdata._strorientation = RDchkbox.SelectedValue;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + RDchkbox.SelectedValue + ");", true);
                var _data = _objdata.GeteventimageDatalist();
                if (_data.Count > 0)
                {
                    gettydata.DataSource = _data;
                    gettydata.DataBind();
                    commonfn _objfn = new commonfn();
                    commonfn.setSession("token", _objdata._strtoken);
                    commonfn.setSession("securetoken", _objdata._strsecuretoken);
                    ltscript.Text = "";
                }
                else
                {
                    gettydata.DataSource = null;
                    ltscript.Text = "Sorry no records found.";
                }

                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
                ErrorLog.SaveErrorLog(_strSiteID, "Indexpage", "RDchkbox_SelectedIndexChanged", "RDchkbox_SelectedIndexChanged", ex.Message, _networkid.ToString());
            }

        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                _lilist = new List<string>();
                txtsearch.Text = "";
                _intstartval = 0;
                prev.Visible = false;
                loadmore.Visible = false;
                gettydata.DataSource = null;
                gettydata.DataBind();
                ltscript.Text = "Search it again.............!";
                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
                ErrorLog.SaveErrorLog(_strSiteID, "Indexpage", "btnclear_Click", "rd_selectedbtn", ex.Message, _networkid.ToString());
            }
        }

      
    }
}