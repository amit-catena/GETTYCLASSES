﻿using System;
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
using Newtonsoft.Json;
using System.Collections.Generic; 

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

            using (SQLHelper obj = new SQLHelper(constr))
            {
                dt = obj.ExecuteDataTable("Getty_GetLink",   mypara);
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
                data = "<select><option value=\"0\">-- Select --</option></select>";
            }

            return data;
        }

        public string GetSubcatJson(string sid, string nwid, string catalias, string cid)
        {
            StringBuilder sb = new StringBuilder();
            string data = "", constr = "";
            string catpath = "";
            constr = commonfn.GetConnectionstring(nwid);

            DataTable dt = new DataTable();
            SqlParameter[] mypara ={
                                       new SqlParameter("@siteid",SqlDbType.Int),
                                        new SqlParameter("@catid",SqlDbType.Int)
                                   };
            mypara[0].Value = sid;
            mypara[1].Value = cid;

            using (SQLHelper obj = new SQLHelper(constr))
            {
                dt = obj.ExecuteDataTable("GetCategoryList", mypara);
            }

            if (dt.Rows.Count > 0)
            {
                int count = 0;
                try
                {
                    sb.Append("{");
                    sb.Append(string.Format("\"category\":\"{0}\",\"subcategory\":[", catalias));
                    foreach (DataRow r in dt.Rows)
                    {
                        catpath = "";

                        if (r["cat_level"].ToString().Trim() == "1")
                        {
                            catpath = string.Format("{0}/{1}/{2}", r["SiteURL"].ToString(), "subcategory", r["CategoryPath"].ToString());
                        }
                        else
                        {
                            catpath = string.Format("{0}/{1}/{2}", r["SiteURL"].ToString(), "subcategory", r["CategoryPath"].ToString());
                        }

                        if (count != dt.Rows.Count-1)
                        {
                            sb.Append("{");
                            sb.Append(string.Format("\"catname\":\"{0}\",\"link\":\"{1}\"", r["CategoryAlias"].ToString().Replace("'",""), catpath));
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("{");
                            sb.Append(string.Format("\"catname\":\"{0}\",\"link\":\"{1}\"", r["CategoryAlias"].ToString().Replace("'",""), catpath));
                            sb.Append("}");
                        }
                        count++;

                       
                    }
                    sb.Append("]}");

                }
                catch
                {
                    sb = null;
                    sb = new StringBuilder();
                    sb.Append("{");
                    sb.Append(string.Format("\"category\":\"{0}\",\"subcategory\":[]", catalias.Replace("'","")));
                    sb.Append("}");
                }
            }
            else
            {
                sb.Append("{");
                sb.Append(string.Format("\"category\":\"{0}\",\"subcategory\":[]", catalias.Replace("'","")));
                sb.Append("}");
            }

            data = sb.ToString(); sb = null;
            

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

        public string CategoryDescription(string siteid, string networkid, string catid)
        {
            StringBuilder sb = new StringBuilder();
            string data = "", constr = "";
            constr = commonfn.GetConnectionstring(networkid);

            DataTable dt = new DataTable();
            SqlParameter[] mypara ={
                                       new SqlParameter("@siteid",SqlDbType.Int),
                                       new SqlParameter("@categoryid",SqlDbType.Int)                                       
                                   };
            mypara[0].Value = siteid;
            mypara[1].Value = catid;

            using (SQLHelper obj = new SQLHelper(constr))
            {
                dt = obj.ExecuteDataTable("Template_GetCategoryDescription", mypara);
            }

            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["Description"].ToString()))
                {
                   // data = HttpUtility.HtmlEncode(dt.Rows[0]["Description"].ToString());

                    data = dt.Rows[0]["Description"].ToString();
                }
            }

            return data;
        }


        public string CategoryQuickLink(string siteid, string networkid, string catid)
        {
            StringBuilder sb = new StringBuilder();
            string data = "", constr = "";
            constr = commonfn.GetConnectionstring(networkid);
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] mypara ={
                                       new SqlParameter("@categoryid",SqlDbType.Int),
                                       new SqlParameter("@SiteId",SqlDbType.Int)                                       
                                   };
                mypara[0].Value = catid;
                mypara[1].Value = siteid;
                string catpath = "", title = "";
                using (SQLHelper obj = new SQLHelper(constr))
                {
                    dt = obj.ExecuteDataTable("DD_SP_GetCheltenhamInformationTitle", mypara);
                }
                int count = 0;
                if (dt.Rows.Count > 0)
                {

                    sb.Append("{");
                    sb.Append("\"quicklink\":[");
                    foreach (DataRow dr in dt.Rows)
                    {
                        catpath = ""; title = "";

                        if (!string.IsNullOrEmpty(dr["externallink"].ToString()))
                        {
                            catpath = dr["externallink"].ToString();
                        }
                        else if (!string.IsNullOrEmpty(dr["linkid"].ToString()) && dr["linkid"].ToString() != "0")
                        {
                            catpath = "http://www.caledonianmedia.com/sitestat.aspx?siteurl=" + dr["linkid"].ToString();
                        }
                        else
                        {
                            catpath = string.Format("{0}/information/{1}{2}/", dr["siteurl"].ToString(), dr["CategoryPath"].ToString(), dr["Totitle"].ToString());
                        }



                        if (count != dt.Rows.Count - 1)
                        {
                            sb.Append("{");
                            sb.Append(string.Format("\"catname\":\"{0}\",\"link\":\"{1}\"", dr["Title"].ToString().Replace("'", ""), catpath));
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("{");
                            sb.Append(string.Format("\"catname\":\"{0}\",\"link\":\"{1}\"", dr["Title"].ToString().Replace("'", ""), catpath));
                            sb.Append("}");
                        }
                        count++;


                    }
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{");
                    sb.Append("\"quicklink\":[");
                    sb.Append("]}");
                }
            }
            catch
            {

                sb.Append("{");
                sb.Append("\"quicklink\":[");
                sb.Append("]}");
            }

            data = sb.ToString();

            return data;
        }


        public string GetAllCategorriesSubcategoriesJson(string siteid, string networkid)
        {
            DataTable dtcatlist = new DataTable();
            StringBuilder sb = new StringBuilder();
            string data="[]";
            int count = 0;
            dtcatlist = GetCategoryList(siteid, networkid);

            if (dtcatlist.Rows.Count > 0)
            {
                sb.Append("[");
                count = 0;
                foreach (DataRow dr in dtcatlist.Rows)
                {
                    
                    sb.Append("{");
                    sb.Append(string.Format("\"categoryid\":\"{3}\",\"category\":\"{0}\",\"caturl\":\"{1}/category/{2}\"", dr["CategoryAlias"].ToString(), dr["SiteUrl"].ToString(), dr["CategoryPath"].ToString(), dr["Categoryid"].ToString()));
                    sb.Append(GetSubcatJsonObject(dr["Categoryid"].ToString(), siteid, networkid));
                    if (count != dtcatlist.Rows.Count - 1)
                    {
                        sb.Append("},");
                    }
                    else
                    {
                        sb.Append("}");
                    }
                    count++;
                }
                sb.Append("]");
            }


            data = sb.ToString(); sb = null;

            return data;

        }

        public string GetSubcatJsonObject(string catid, string siteid, string networkid)
        {
            StringBuilder sb = new StringBuilder();
            string data = "", constr = "";
            string catpath = "";
            constr = commonfn.GetConnectionstring(networkid);

            DataTable dt = new DataTable();
            SqlParameter[] mypara ={
                                       new SqlParameter("@siteid",SqlDbType.Int),
                                        new SqlParameter("@catid",SqlDbType.Int)
                                   };
            mypara[0].Value = siteid;
            mypara[1].Value = catid;

            using (SQLHelper obj = new SQLHelper(constr))
            {
                dt = obj.ExecuteDataTable("GetCategoryList", mypara);
            }

            if (dt.Rows.Count > 0)
            {
                int count = 0;
                try
                {
                    //sb.Append(",{");
                    sb.Append(",\"subcategory\":[");
                    foreach (DataRow r in dt.Rows)
                    {
                        catpath = "";

                        if (r["cat_level"].ToString().Trim() == "1")
                        {
                            catpath = string.Format("{0}/{1}/{2}", r["SiteURL"].ToString(), "subcategory", r["CategoryPath"].ToString());
                        }
                        else
                        {
                            catpath = string.Format("{0}/{1}/{2}", r["SiteURL"].ToString(), "subcategory", r["CategoryPath"].ToString());
                        }

                        if (count != dt.Rows.Count - 1)
                        {
                            sb.Append("{");
                            sb.Append(string.Format("\"catname\":\"{0}\",\"link\":\"{1}\",\"subcatid\":\"{2}\"", r["CategoryAlias"].ToString().Replace("'", ""), catpath, r["CategoryId"].ToString()));
                            sb.Append(GetSubSubcatJsonObject(r["CategoryId"].ToString(), siteid, networkid)); 
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("{");
                            sb.Append(string.Format("\"catname\":\"{0}\",\"link\":\"{1}\",\"subcatid\":\"{2}\"", r["CategoryAlias"].ToString().Replace("'", ""), catpath, r["CategoryId"].ToString()));
                            sb.Append(GetSubSubcatJsonObject(r["CategoryId"].ToString(), siteid, networkid));
                            sb.Append("}");
                        }

                        count++;


                    }
                    sb.Append("]");

                }
                catch
                {
                    sb = null;
                    sb = new StringBuilder();
                    sb.Append(",\"subcategory\":[]");
                }
            }
            else
            {
               
                sb.Append(",\"subcategory\":[]");
                
            }

            data = sb.ToString(); sb = null;


            return data;
        }

        public DataTable GetCategoryList(string siteid, string networkid)
        {
            DataTable dt = new DataTable();
            string constr;
            constr = commonfn.GetConnectionstring(networkid);

            
            SqlParameter[] mypara ={
                                       new SqlParameter("@siteid",SqlDbType.Int)                               
                                   };
            mypara[0].Value = siteid;
         

            using (SQLHelper obj = new SQLHelper(constr))
            {
                dt = obj.ExecuteDataTable("GetCategoryListForTemplate", mypara);
            }
            return dt;

        }

        public string GetSubSubcatJsonObject(string catid, string siteid, string networkid)
        {
            StringBuilder sb = new StringBuilder();
            string data = "", constr = "";
            string catpath = "";
            constr = commonfn.GetConnectionstring(networkid);

            DataTable dt = new DataTable();
            SqlParameter[] mypara ={
                                       new SqlParameter("@siteid",SqlDbType.Int),
                                        new SqlParameter("@catid",SqlDbType.Int)
                                   };
            mypara[0].Value = siteid;
            mypara[1].Value = catid;

            using (SQLHelper obj = new SQLHelper(constr))
            {
                dt = obj.ExecuteDataTable("GetCategoryList", mypara);
            }

            if (dt.Rows.Count > 0)
            {
                int count = 0;
                try
                {
                    //sb.Append(",{");
                    sb.Append(",\"subsubcategory\":[");
                    foreach (DataRow r in dt.Rows)
                    {
                        catpath = "";

                        if (r["cat_level"].ToString().Trim() == "1")
                        {
                            catpath = string.Format("{0}/{1}/{2}", r["SiteURL"].ToString(), "subcategory", r["CategoryPath"].ToString());
                        }
                        else
                        {
                            catpath = string.Format("{0}/{1}/{2}", r["SiteURL"].ToString(), "subcategory", r["CategoryPath"].ToString());
                        }

                        if (count != dt.Rows.Count - 1)
                        {
                            sb.Append("{");
                            sb.Append(string.Format("\"catname\":\"{0}\",\"link\":\"{1}\",\"subcatid\":\"{2}\"", r["CategoryAlias"].ToString().Replace("'", ""), catpath, r["CategoryId"].ToString()));
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("{");
                            sb.Append(string.Format("\"catname\":\"{0}\",\"link\":\"{1}\",\"subcatid\":\"{2}\"", r["CategoryAlias"].ToString().Replace("'", ""), catpath, r["CategoryId"].ToString()));
                            sb.Append("}");
                        }
                        count++;


                    }
                    sb.Append("]");

                }
                catch
                {
                    sb = null;
                    sb = new StringBuilder();
                    sb.Append(",\"subsubcategory\":[]");
                }
            }
            else
            {

                sb.Append(",\"subsubcategory\":[]");

            }

            data = sb.ToString(); sb = null;


            return data;
        }


        public string Getallsports()
        {

            StringBuilder sb = new StringBuilder();
            List<string> Ls=new List<string>(); 
            string data = "", constr = "";
            string catpath = "";
            constr = commonfn.Getstreamconnstring();
            DataTable dt = new DataTable();
            SqlParameter[] mypara ={
                                       new SqlParameter("@siteid",SqlDbType.Int)
                                   };
            mypara[0].Value = 1;
            using (SQLHelper obj = new SQLHelper(constr))
            {
                dt = obj.ExecuteDataTable("GetALLsports", mypara);
                
           
                
                if(dt.Rows.Count>0)
                {
                    int count = 0;

                    DataView view = new DataView(dt);
                    DataTable distinctValues = view.ToTable(true, "sportid", "alias");

                     sb.Append("[");
                     foreach (DataRow d in distinctValues.Rows)
                    {
                        if (count != distinctValues.Rows.Count - 1)
                        {
                            sb.Append("{");
                            sb.Append(string.Format("\"sportid\":\"{0}\",\"sportname\":\"{1}\"", Convert.ToString(d["Sportid"]), Convert.ToString(d["alias"])));
                            sb.Append(getLeagues(dt, Convert.ToString(d["Sportid"])));
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("{");
                            sb.Append(string.Format("\"sportid\":\"{0}\",\"sportname\":\"{1}\"", Convert.ToString(d["Sportid"]), Convert.ToString(d["alias"])));
                            sb.Append(getLeagues(dt, Convert.ToString(d["Sportid"])));
                            sb.Append("}");
                        }
                        count++;

                    }
                    sb.Append("]");
                }
            }
            data = sb.ToString(); sb = null;
            return  data;
        }

        public string getLeagues(DataTable dtSp, string sportid)
        {
            DataRow[] dr;
            StringBuilder sb = new StringBuilder(); 
            string data = "";
            dr = dtSp.Select("sportid=" + sportid);

            if (dr.Length > 0)
            {
                int count = 0;
                sb.Append("[");
                foreach (DataRow d in dr)
                {
                    if (count != dr.Length - 1)
                    {
                        sb.Append("{");
                        sb.Append(string.Format("\"lid\":\"{0}\",\"lname\":\"{1}\"", Convert.ToString(d["eventgroupid"]), Convert.ToString(d["eventgroupname"]).Replace("'","")));
                        
                        sb.Append("},");
                    }
                    else
                    {
                        sb.Append("{");
                        sb.Append(string.Format("\"lid\":\"{0}\",\"lname\":\"{1}\"", Convert.ToString(d["eventgroupid"]), Convert.ToString(d["eventgroupname"]).Replace("'", "")));
                        sb.Append("}");
                    }
                    count++;

                }
                sb.Append("]");
                data = string.Format(",\"leagues\":{0}",sb.ToString());
                sb = null;
            }
            else
            {
                data = ",\"leagues\":[]";
            }

            return data;
        }





    }
}
