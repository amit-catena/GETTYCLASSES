using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gettyclasses;

namespace gettywebclasses
{
      public partial class ajax_post : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //siteid = AdminBLL.Constants.SiteID;
                switch (Request.Form["type"].ToString())
                {
                    case "UpdateImageDetails":
                        UpdateImageDetails(Request.Form["imageid"], Request.Form["imagetitle"], Request.Form["imagealttext"], Request.Form["netid"]);
                        break;

                    case "BONUS":
                        GetBonusdata();
                        break;
                }
            }
            catch
            {
            }
        }
        public void UpdateImageDetails(string imageid, string imagetitle, string imagealttext, string networkid)
        {
            string i = string.Empty;

            try
            {
                using (Signup objsignup = new Signup())
                {

                    objsignup.ImageTitle = imagetitle;
                    objsignup.ImageAlttext = imagealttext;
                    objsignup.ImageID = Convert.ToInt32(imageid);
                    objsignup.NetworkID = networkid;
                    i = objsignup.UpdateImageDetails().ToString();
                }
            }
            catch
            { }
            ltresult.Text = "1";
        }

        public void GetBonusdata()
        {
            string bonusparent = "", bonusorder = "";
            string siteid = "", order = "", bonusfor = "", constr = "";
            string data = "";
            try
            {
                siteid=Request.Form["siteid"];
            }
            catch (Exception ex)
            {
                data = data +"siteid"+ex.Message;
            }
            try
            {
                order = Request.Form["order"];
            }
            catch (Exception ex)
            {
                data = data + "order" + ex.Message;
            }
            try
            {
                bonusfor = Request.Form["bonusfor"];
            }
            catch (Exception ex)
            {
                data = data + "bonusfor" + ex.Message;
            }

            try
            {
                constr = Request.Form["networkid"];
                data = data + "ajax load-constr" + constr;
            }
            catch (Exception ex)
            {
                data = data + "constr" + ex.Message;
            }

            try
            {

                CategoryTemplate obj = new CategoryTemplate();
                data = data +obj.GetBonusJson(siteid,order,bonusfor,constr);
                obj = null;
            }
            catch(Exception ex)
            {
                data = data + "ajax load" + ex.Message;
            }
            ltresult.Text = data;
        }
    }

}