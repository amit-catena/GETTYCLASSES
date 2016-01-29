using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Gettyclasses
{
  public  class SQLHelper:IDisposable
    {
      SqlCommand SQLCmd = new SqlCommand();
        #region "Constructors  Destructor"
        public SQLHelper(string dbconn)
        {

            _connectionString = dbconn;

            _errormessage = "";
            SQLConn = new SqlConnection(_connectionString);
            SQLReader = null;
        }

        public void Dispose()
        {
      
            if (SQLConn != null)
            {
                if(SQLConn.State== ConnectionState.Open)
                        SQLConn.Close();
               SQLConn = null;
            }
        }
        #endregion

        #region "Private Members"

        SqlDataReader SQLReader=null;
        SqlConnection SQLConn;
        private string _errormessage = "";
        private string _connectionString ="";
        private int _recordaffect = 0;
        private bool _flag = false;

        #endregion

        #region "Properties"
        public bool HasError
        {
            get {

                if (!string.IsNullOrEmpty(_errormessage))
                    return true;
                else
                    return false;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return _errormessage;               
            }
        }
        #endregion

        

        #region "ExecuteNonQuery Methods"
        public bool ExecuteNonQuery(string StroredProcedureName, SqlParameter[] ParaMeterCollection)
        {
            SqlCommand SQLCmd = new SqlCommand();
            try
            {

                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                //Loop for Paramets
                SQLCmd.Parameters.Clear();
                for (int i = 0; i < ParaMeterCollection.Length; i++)
                {
                    SQLCmd.Parameters.Add(ParaMeterCollection[i]);
                }
                //End of for loop
                SQLConn.Open();
                _recordaffect = SQLCmd.ExecuteNonQuery();
                SQLConn.Close();
                if (_recordaffect > 0)
                    _flag = true;
                else
                    _flag = false;
            }
            catch (Exception e)
            {
                _errormessage = e.Message;
                throw e;
            }
            finally
            {
                SQLCmd = null;
            }
            return _flag;
        }
        public bool ExecuteNonQuery(string SqlQuery)
        {
            SqlCommand SQLCmd = new SqlCommand();
            try
            {

                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = SqlQuery;
                SQLCmd.CommandType = CommandType.Text;
                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();
                _recordaffect = SQLCmd.ExecuteNonQuery();
                SQLConn.Close();
                if (_recordaffect > 0)
                    _flag = true;
                else
                    _flag = false;

            }
            catch (Exception e)
            {
                _errormessage = e.Message;
                throw e;
            }
            finally
            {
                SQLCmd = null;
            }
            return _flag;
        }
        public bool ExecuteNonQuery(string StroredProcedureName, SqlParameter[] ParaMeterCollection, string OutPutParamerterName, out string OutPutParamerterValue)
        {
            SqlCommand SQLCmd = new SqlCommand();
            OutPutParamerterValue = "0";
            try
            {

                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                //Loop for Paramets
                for (int i = 0; i < ParaMeterCollection.Length; i++)
                {
                    SQLCmd.Parameters.Add(ParaMeterCollection[i]);
                }
                //End of for loop
                SQLCmd.Parameters[OutPutParamerterName].Direction = ParameterDirection.InputOutput;
                
                SQLConn.Open();
                _recordaffect = SQLCmd.ExecuteNonQuery();
                SQLConn.Close();
                OutPutParamerterValue = Convert.ToString(SQLCmd.Parameters[OutPutParamerterName].Value);
                if (_recordaffect > 0)
                    _flag = true;
                else
                    _flag = false;
            }
            catch (Exception e)
            {
                _errormessage = e.Message;
                throw e;
            }
            finally
            {
                SQLCmd = null;
            }
            return _flag;
        }
        #endregion

        #region "ExecuteScaler Methods"
        public object ExecuteScaler(string StroredProcedureName, SqlParameter[] ParaMeterCollection)
        {
            object SQlObj= null;
            SqlCommand SQLCmd = new SqlCommand();
            try
            {

                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                //Loop for Paramets
                for (int i = 0; i < ParaMeterCollection.Length; i++)
                {
                    SQLCmd.Parameters.Add(ParaMeterCollection[i]);
                }
                //End of for loop
                SQLConn.Open();
                SQlObj = SQLCmd.ExecuteScalar();
                SQLConn.Close();
            }
            catch (Exception e)
            {
                _errormessage = e.Message;
                throw e;
            }
            finally
            {
                SQLCmd = null;
            }
            return SQlObj;
        }
        public object ExecuteScaler(string SqlQuery)
        {
            object SQlObj = null;
            SqlCommand SQLCmd = new SqlCommand();
            try
            {
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = SqlQuery;
                SQLCmd.CommandType = CommandType.Text;
                SQLConn.Open();
                SQlObj = SQLCmd.ExecuteScalar();
                SQLConn.Close();
            }
            catch (Exception e)
            {
                _errormessage = e.Message;
                throw e;
            }
            finally
            {
                SQLCmd = null;
            }
            return SQlObj;
        }
        
        #endregion

        

        #region "ExecuteDataTable Methods"
        public DataTable ExecuteDataTable(string StroredProcedureName, SqlParameter[] ParaMeterCollection)
        {
            SqlDataAdapter SQLAdapter = new SqlDataAdapter();
            DataTable SQLDataTable = new  DataTable();
            SqlCommand SQLCmd = new SqlCommand();
            
            try
            {

                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                //Loop for Paramets
                for (int i = 0; i < ParaMeterCollection.Length; i++)
                {
                    SQLCmd.Parameters.Add(ParaMeterCollection[i]);
                }
                //End of for loop
                SQLAdapter.SelectCommand = SQLCmd;
                SQLConn.Open();
                SQLAdapter.Fill(SQLDataTable);
                SQLConn.Close();
            }
            catch (Exception e)
            {
                _errormessage = e.Message;
                throw e;
            }
            finally
            {
                SQLCmd = null; SQLAdapter = null;
            }
            return SQLDataTable;
        }
        public DataTable ExecuteDataTable(string SqlQuery)
        {
            SqlDataAdapter SQLAdapter = new SqlDataAdapter();
            DataTable SQLDataTable = new DataTable();
            SqlCommand SQLCmd = new SqlCommand();
            try
            {
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = SqlQuery;
                SQLCmd.CommandType = CommandType.Text;
                SQLAdapter.SelectCommand = SQLCmd;
                SQLConn.Open();
                SQLAdapter.Fill(SQLDataTable);
                SQLConn.Close();
            }
            catch (Exception e)
            {
                _errormessage = e.Message;
                throw e;
            }
            finally
            {
                SQLCmd = null; SQLAdapter = null;
            }
            return SQLDataTable;
        }
        #endregion

        #region "ExecuteDataSet Methods"
        public DataSet ExecuteDataSet(string StroredProcedureName, SqlParameter[] ParaMeterCollection)
        {
            SqlDataAdapter SQLAdapter = new SqlDataAdapter();
            DataSet SQLDataSet = new DataSet();
            SqlCommand SQLCmd = new SqlCommand();

            try
            {

                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = StroredProcedureName;
                SQLCmd.CommandType = CommandType.StoredProcedure;
                //Loop for Paramets
                for (int i = 0; i < ParaMeterCollection.Length; i++)
                {
                    SQLCmd.Parameters.Add(ParaMeterCollection[i]);
                }
                //End of for loop
                SQLAdapter.SelectCommand = SQLCmd;
                SQLConn.Open();
                SQLAdapter.Fill(SQLDataSet);
                SQLConn.Close();
            }
            catch (Exception e)
            {
                _errormessage = e.Message;
                throw e;
            }
            finally
            {
                SQLCmd = null; SQLAdapter = null;
            }
            return SQLDataSet;
        }
        public DataSet ExecuteDataSet(string SqlQuery)
        {
            SqlDataAdapter SQLAdapter = new SqlDataAdapter();
            DataSet SQLDataSet = new DataSet();
            SqlCommand SQLCmd = new SqlCommand();
            try
            {
                SQLCmd.Connection = SQLConn;
                SQLCmd.CommandText = SqlQuery;
                SQLCmd.CommandType = CommandType.Text;
                SQLAdapter.SelectCommand = SQLCmd;
                SQLConn.Open();
                SQLAdapter.Fill(SQLDataSet);
                SQLConn.Close();
            }
            catch (Exception e)
            {
                _errormessage = e.Message;
                throw e;
            }
            finally
            {
                SQLCmd = null; SQLAdapter = null;
            }
            return SQLDataSet;
        }
        #endregion




         
    }
}
