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

            }
        }

        public void DeleteSuperEventimages(string ids)
        {
            using (supereventimagesmgmt objsuper = new supereventimagesmgmt())
            {
                objsuper.DeleteSuperEventImages(ids);
            }
        }
       
    }
}