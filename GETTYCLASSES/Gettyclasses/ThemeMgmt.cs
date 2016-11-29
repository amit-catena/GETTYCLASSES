using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Gettyclasses
{
   public class ThemeMgmt
    {


       public DataTable GetThemeDetails(string id)
       {
           DataTable dt = new DataTable();

           using (SQLHelper obj = new SQLHelper(ConfigurationSettings.AppSettings["Adsensestring"]))
           {
               dt = obj.ExecuteDataTable("SELECT ThemeId,ThemeName,ImageName,color,linkid FROM SlotTheme WHERE status = 'Y' and ThemeId =" + id);
           }
           return dt;

       }

       public bool SaveThemeDetail(string id, string name, string image,string color,string linkid)
       {
           bool flag = false;

           SqlParameter[] mypara ={
                                       new SqlParameter("@themeid",SqlDbType.Int),
                                       new SqlParameter("@themename",SqlDbType.VarChar),
                                       new SqlParameter("@imagename",SqlDbType.VarChar),
                                       new SqlParameter("@color",SqlDbType.VarChar),
                                       new SqlParameter("@linkid",SqlDbType.Int),
                                   };

           mypara[0].Value = id;
           mypara[1].Value = name;
           mypara[2].Value = image;
           mypara[3].Value = color;
           mypara[4].Value = linkid;
           using (SQLHelper obj = new SQLHelper(ConfigurationSettings.AppSettings["Adsensestring"]))
           {
               flag = obj.ExecuteNonQuery("SLOT_Theme_SAVEUPDATE", mypara);
           }
           return flag;
       }

       public DataTable GetWeloveHyperlink()
       {
           DataTable dt = new DataTable();

           using (SQLHelper obj = new SQLHelper(ConfigurationSettings.AppSettings["Network1"]))
           {
               string siteid=ConfigurationSettings.AppSettings["welovesiteid"].ToString();
               string strsql = "select LinkID,LinkName from HyperLinkManager where SiteID="+siteid+" and IsActive ='Y' order by LinkName";

               dt = obj.ExecuteDataTable(strsql);
                
           }
           return dt;
       }
    }
}
