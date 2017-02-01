using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CommonLib;

namespace Gettyclasses
{
    public class newsliveupdatemgmt:IDisposable
    {
        SQLHelper objsql;
        #region :: Default Constructor ::
        public newsliveupdatemgmt()
        {
               
        }

        public newsliveupdatemgmt(string dbconn)
        {
            objsql = new SQLHelper(dbconn);
        }
        #endregion

        #region :: Dispose Method ::
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region :: Destructor  ::
        ~newsliveupdatemgmt()
        {
            Dispose();
        }
        #endregion

        #region :: public variable ::
        public string Title { get; set; }
        public string Description { get; set; }
        public string SiteId { get; set; }
        public string Image { get; set; }
        public string Addedby { get; set; }
        public string NewsId { get; set; }


        #endregion

        #region :: Method ::

        public int SaveLiveNewsUpdate()
        {
            int count = 0;
            try
            {
                SqlParameter[] param = { new SqlParameter("@title",SqlDbType.VarChar),
                                         new SqlParameter("@description",SqlDbType.NText),
                                         new SqlParameter("@siteid",SqlDbType.Int),
                                         new SqlParameter("@addedby",SqlDbType.Int),
                                         new SqlParameter("@image",SqlDbType.VarChar),
                                         new SqlParameter("@newsid",SqlDbType.Int),
                                         new SqlParameter("@retCount",SqlDbType.Int)
                                       };
                param[0].Value = Title;
                param[1].Value = Description;
                param[2].Value = SiteId;
                param[3].Value = Addedby;
                param[4].Value = Image;
                param[5].Value = NewsId;
                param[6].Value = 0;
                param[6].Direction = ParameterDirection.InputOutput;

                objsql.ExecuteNonQuery("AN_SP_NewsLiveUpdate_Save", param);
                count = Convert.ToInt32(param[6].Value);
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "newsliveupdatemgmt.cs SaveLiveNewsUpdate", ex);
            }
            return count;
        }

        public DataTable GetNewsLiveUpdateList(string newsid,string siteid)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] param = { new SqlParameter("@SiteId",SqlDbType.Int),
                                       new SqlParameter("@NewsId",SqlDbType.Int)};
                param[0].Value = siteid;
                param[1].Value = newsid;

                dt=objsql.ExecuteDataTable("AN_SP_GetNewsLiveUpdateList", param);
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "newsliveupdatemgmt.cs GetNewsLiveUpdateList", ex);
            }
            return dt;
        }

        public int DeleteNewsUpdateLive(string ids,string newsid)
        {
            int count = 0;
            try
            {
                SqlParameter[] param = { new SqlParameter("@id", SqlDbType.VarChar),
                                       new SqlParameter("@newsid",SqlDbType.Int),
                                       new SqlParameter("@retCount",SqlDbType.Int)};
                param[0].Value = ids;
                param[1].Value = newsid;
                param[2].Value = 0;
                param[2].Direction = ParameterDirection.InputOutput;
                objsql.ExecuteNonQuery("AN_SP_DeleteNewsLiveUpdate", param);
                count = Convert.ToInt32(param[2].Value);
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "newsliveupdatemgmt.cs DeleteNewsUpdateLive", ex);
            }
            return count;
        }
        

        #endregion
    }
}
