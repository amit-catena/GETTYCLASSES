using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gettyclasses;

namespace gettywebclasses
{
    public partial class supereventajax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Request.Form["type"])
            {
                case "supereventimages":
                    DeleteSuperEventimages(Request.Form["supereventids"]);
                    break;
                case "appimages":
                    DeleteAppImagesimages(Request.Form["appids"], Request.Form["siteid"]);
                    break;
                case "screenshotimages":
                    DeleteScreenShotimages(Request.Form["slotids"]);
                    break;

            }
        }

        public void DeleteSuperEventimages(string ids)
        {
            using (supereventimagesmgmt objsuper = new supereventimagesmgmt())
            {
                objsuper.DeleteSuperEventImages(ids);
            }
        }

        public void DeleteAppImagesimages(string ids,string siteid)
        {
            using (appimagemgmt obj = new appimagemgmt())
            {
                obj.DeleteAppImages(ids,siteid);
            }
        }

        public void DeleteScreenShotimages(string ids)
        {
            using (slotmgmt obj = new slotmgmt())
            {
                obj.DeleteScreenshotImages(ids);
            }
        }
       
    }
}