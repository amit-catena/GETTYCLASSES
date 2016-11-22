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
               dt = obj.ExecuteDataTable("SELECT ThemeId,ThemeName,ImageName FROM SlotTheme WHERE status<>'Y' and ThemeId =" + id);
           }
           return dt;

       }

       public bool SaveThemeDetail(string id, string name, string image)
       {
           bool flag = false;

           SqlParameter[] mypara ={
                                       new SqlParameter("@themeid",SqlDbType.Int),
                                       new SqlParameter("@themename",SqlDbType.VarChar),
                                       new SqlParameter("@imagename",SqlDbType.VarChar),
                                   };

           mypara[0].Value = id;
           mypara[1].Value = name;
           mypara[2].Value = image;
           using (SQLHelper obj = new SQLHelper(ConfigurationSettings.AppSettings["Adsensestring"]))
           {
               flag = obj.ExecuteNonQuery("SLOT_Theme_SAVEUPDATE", mypara);
           }
           return flag;
       }
    }
}
