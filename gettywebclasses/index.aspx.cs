﻿using System;
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
    public partial class index : System.Web.UI.Page
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
        public static  List<String> _lilist = new List<String>();  
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
                var _data = _objdata.GetimageDatalist();
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
            _html.Text = @"<a href='" + (string)DataBinder.Eval(e.Item.DataItem, "UrlPreview") + @"' id='htmlA' >
                                <img src='" + _baseURL + @"images/spacer.gif'    data-original='" + HttpUtility.HtmlDecode((string)DataBinder.Eval(e.Item.DataItem, "UrlThumb")).Replace("&amp;", "&") + @"'   class='lazy' id='_imgtemp' runat='server'    />
                            </a>";

            if (_lilist.Count > 0)
            {
                foreach (DataListItem _chk in gettydata.Items)
                {
                    HtmlInputCheckBox _selchk = _chk.FindControl("chkimg") as HtmlInputCheckBox;
                    if (_lilist.Contains(_selchk.Value))
                    {
                        _selchk.Checked = true; 
                    }
                }
            }
              

        }

        protected void btnalbum_Click(object sender, EventArgs e)
        {
            try
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
                if (null != Request.QueryString["news"])
                {
                    _strnews = Request.QueryString["news"].ToString();
                }
               /* List<string> _imagesselect = new List<string>();
                foreach (DataListItem _chk in gettydata.Items)
                {
                    HtmlInputCheckBox _selchk = _chk.FindControl("chkimg") as HtmlInputCheckBox;
                    if (_selchk.Checked)
                    {
                        _imagesselect.Add(_selchk.Value);
                    }
                }*/
                //get download seleted images
                Getimagedata _objdata = new Getimagedata();
                _objdata._strsiteID = !string.IsNullOrEmpty(_strSiteID) ? _strSiteID : "1";
                _objdata._strnetworkID = _networkid;
                _objdata._strrandomecookie = _strcookiename;
                _objdata._strnewsID = _intnewsID;
                _objdata._strnews = _strnews;
                //getdownload images.
                if (_lilist.Count > 0)
                {
                    if (_objdata.Getdownloadmultipleimages(_lilist))
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "javascript:self.close()", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "close", "javascript:window.opener.location.href = '" + commonfn._admin + _redirecturl + "';self.close();", true);
                    }
                }
                //_imagesselect.Clear();

            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.Client, "btnsearch_Click :", ex);
                ErrorLog.SaveErrorLog(_strSiteID, "Indexpage", "btnalbum_Click", "btnalbum_Click", ex.Message, _networkid.ToString());
            }
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
                var _data = _objdata.GetimageDatalist();
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

                var _data = _objdata.GetimageDatalist();
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
                var _data = _objdata.GetimageDatalist();
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

        [WebMethod]
        public static string  ProcessIT(string _val,bool _chk)
        {
            string result = "false";
            if (_lilist.Count > 0)
            {
                if (_chk)
                {
                    _lilist.Add(_val);
                    result = "true";
                }
                else
                {
                    if (_lilist.Contains(_val))
                        _lilist.Remove(_val);
                    result = "false";
                }

            }
            else
            {
                if (_chk)
                {
                    _lilist.Add(_val);
                    result = "true";
                }

            }

            return result;
        }

    
    }
}