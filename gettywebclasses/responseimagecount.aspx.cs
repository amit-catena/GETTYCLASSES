using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gettyclasses;

namespace gettywebclasses
{
    public partial class responseimagecount : System.Web.UI.Page
    {
        int eventID = 0;
        string _strevent = string.Empty;
        public int _tlcnt = 0;
        string[] _strarray;
        public string _strval = string.Empty;
        public int _rtcnt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    Getimagedata _objdata = new Getimagedata();
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "print", "alert(" + _intstartval + ");", true);
                    if (null != Request.QueryString["eventid"])
                    {
                        _strevent = Request.QueryString["eventid"].ToString();
                        //_strevent = "533251361,514207643,484215707,516299701,499974343";
                    }
                    _objdata._intstartcnt = 1;
                    _objdata._strorientation = "horizontal";
                    _strarray = _strevent.Split(',');
                    if (_strarray.Length > 0)
                    {
                      
                        foreach (var i in _strarray)
                        {
                            _rtcnt = _objdata.Get_gettyevents_Responseimagecount(i.ToString());
                            if (_tlcnt == 0)
                            {
                                _strval += string.Format("{0}",_rtcnt);
                                _tlcnt++;
                            }
                            else
                            {
                                _strval +=","+string.Format("{0}",_rtcnt);
                            }
                            _rtcnt = 0;
                            //_tlcnt = _objdata.Get_gettyevents_Responseimagecount(i.ToString());
                        }
                    }
                    //Response.Write("_strval" + _strval);
                }
                catch (Exception ex)
                {
                    Response.Write(ex);
                }

                
            }
        }
    }
}