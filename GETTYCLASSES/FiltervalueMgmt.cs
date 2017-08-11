using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Gettyclasses
{
     public class FiltervalueMgmt:IDisposable
     {
          #region :: Default Constructor ::
        public FiltervalueMgmt()
        {

        }
        #endregion

        #region :: Dispose Method ::
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region :: Destructor  ::
        ~FiltervalueMgmt()
        {
            Dispose();
        }
        #endregion

        public void SaveFilterValueImage(string filtervalueid ,string imagename)
        {
            string strconn = ConfigurationSettings.AppSettings["Adsensestring"];
            try
            {
                SqlParameter[] param = { new SqlParameter("@FilterValueId",SqlDbType.Int),
                                       new SqlParameter("@ImageName",SqlDbType.VarChar)};
                param[0].Value = filtervalueid;
                param[1].Value = imagename;

                using (SQLHelper dal = new SQLHelper(strconn))
                {
                    dal.ExecuteNonQuery("AN_SP_FilterValueImage_Save", param);
                }
            }
            catch (Exception ex)
            {
                CommonLib.ExceptionHandler.WriteLog(CommonLib.Sections.BLL, "FilterValueMgmt.cs SaveFilterValueImage", ex);
            }
        }
    }
}
