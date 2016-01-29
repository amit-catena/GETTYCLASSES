using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.IO;
using CommonLib;

namespace Gettyclasses
{
    public class CategoryTemplate
    {


        public string GetLink(string siteid, string networkid)
        {
            StringBuilder sb = new StringBuilder(); 
            string data = "",constr="";
            constr = commonfn.GetConnectionstring(networkid);

            DataTable dt = new DataTable();
            SqlParameter[] mypara ={
                                       new SqlParameter("@siteid",SqlDbType.Int)
                                   };
            mypara[0].Value = siteid;

            using (CommonLib.DAL obj = new DAL())
            {
                dt = obj.GetDataTable("Getty_GetLink", CommandType.StoredProcedure, mypara);
            }

            if (dt.Rows.Count > 0)
            {
                sb.Append("<select>");

                foreach (DataRow r in dt.Rows)
                {
                    sb.Append(string.Format("<option value=\"{0}\">{1}</option>", r["linkid"].ToString(), r["LinkName"].ToString().Replace("'", "")));
                }
                sb.Append("</select>");
            }

            data = sb.ToString(); sb = null;
            if (string.IsNullOrEmpty(data))
            {
                data = "<select><option value='0'>-- Select --</option></select>";
            }

            return data;
        }

        public string GetTemplateDetails(string catid,string siteid, string networkid)
        {
            string data = "", constr = "";
            constr = commonfn.GetConnectionstring(networkid);
            string sql = string.Format("SELECT templatetext FROM CategoryTemplate WHERE categoryid={0} and siteid={1}", catid, siteid);
            using (SQLHelper obj = new SQLHelper(constr))
            {
                data = Convert.ToString(obj.ExecuteScaler(sql));
            }

            return data;
        }

        public string SaveTemplate(string catid, string siteid, string networkid, string information)
        {
            string data = "", constr = "";
            constr = commonfn.GetConnectionstring(networkid);

            bool dt =false;
            SqlParameter[] mypara ={
                                       new SqlParameter("@categoryid",SqlDbType.Int),
                                       new SqlParameter("@siteid",SqlDbType.Int),
                                       new SqlParameter("@templatetext",SqlDbType.NText)
                                   };
            mypara[0].Value = catid;
            mypara[1].Value = siteid;
            mypara[2].Value = information;

            using (SQLHelper obj = new SQLHelper(constr))
            {
                dt = obj.ExecuteNonQuery("DD_SP_Cat_TemplateSave",mypara);
            }

            if (dt)
                data = "saved";
            else
                data = "fail";
            return data;
        }




    }
}
