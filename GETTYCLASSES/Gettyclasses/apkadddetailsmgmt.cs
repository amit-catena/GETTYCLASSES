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
     
    public class apkadddetailsmgmt:IDisposable
    {
        DAL dal;
        public apkadddetailsmgmt()
        {
            dal = new DAL();
        }
        public void Dispose()
        {
            dal.Dispose();
        }
        ~apkadddetailsmgmt()
        {
            Dispose();
        }

        #region :: public properties ::
        public int ApkFileId { get; set; }
        public string ApkFileName { get; set; }
        public string Version { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public int Addedby { get; set; }
        public int ApklinkId { get; set; }
        public int AppmasterId { get; set; }
        public string imagedate { get; set; }
        public string LinkName { get; set; }
        public string LinkReference { get; set; }
        public string filename { get; set; }
        public int LinkId { get; set; }

        #endregion

        #region :: Method ::
        public int SaveApkLink()
        {          
            int apklinkid = 0;
           
            try
            {               

                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["appstorelinkmanager"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_ApkLink_Save";
                cmd.Connection = con;
                SqlParameter[] param = { new SqlParameter("@LinkName", SqlDbType.VarChar),
                                           new SqlParameter("@LinkReference",SqlDbType.VarChar),
                                           new SqlParameter("@LinkID",SqlDbType.Int)
                                      };
                param[0].Value = LinkName;
                param[1].Value = LinkReference;
                param[2].Value = LinkId;
                param[2].Direction = ParameterDirection.InputOutput;
                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                con.Open();
                cmd.ExecuteNonQuery();
                apklinkid = Convert.ToInt32(param[2].Value);
                con.Close();
                 
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(Sections.BLL,"",ex);
            }
            return apklinkid;
        }

        public void SaveApkDetails()
        {
            int rowsaffected = 0;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_SaveApkDetails";
                cmd.Connection = con;
               
                SqlParameter[] param = { new SqlParameter("@apkfilename", SqlDbType.VarChar),
                                       new SqlParameter("@version",SqlDbType.VarChar),
                                       new SqlParameter("@size",SqlDbType.VarChar),
                                       new SqlParameter("@description",SqlDbType.VarChar),
                                       new SqlParameter("@addedby",SqlDbType.Int),
                                       new SqlParameter("@apklinkid",SqlDbType.Int),
                                       new SqlParameter("@appmasterid",SqlDbType.Int),
                                       new SqlParameter("@imagedate",SqlDbType.VarChar),
                                       new SqlParameter("@apkfileid",SqlDbType.Int),
                                       new SqlParameter("@filename",SqlDbType.VarChar)};

                param[0].Value = ApkFileName;
                param[1].Value = Version==null?"":Version;
                param[2].Value = Size==null?"":Size;
                param[3].Value = Description==null?"":Description;
                param[4].Value = Addedby;
                param[5].Value = ApklinkId;
                param[6].Value = AppmasterId;
                param[7].Value = (imagedate==null)?"":imagedate;
                param[8].Value = ApkFileId;
                param[9].Value = filename==null?"":filename;

                for (int i = 0; i < param.Length; i++)
                {
                    cmd.Parameters.Add(param[i]);
                }
                con.Open();
                rowsaffected=cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

                CommonLib.ExceptionHandler.WriteLog(Sections.BLL, "", ex);
            }
        }


        public DataTable GetApkDetails(string apkid)
        {
            DataTable dt = new DataTable();
           
           
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["gamingappstore"]);
                SqlCommand cmd = new SqlCommand("AN_SP_GetApkDetails", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AN_SP_GetApkDetails";
                cmd.Connection = con;
                cmd.Parameters.Add("@apkfileid", SqlDbType.Int).Value = apkid;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                  
            }
            catch (Exception ex)
            {

                CommonLib.ExceptionHandler.WriteLog(Sections.BLL, "", ex);
            }
            return dt;

        }

        #endregion


    }
}
