using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

using System.Collections;
using System.Configuration;
using System.IO;
using System.Data.SqlTypes;

namespace Sales_Automation_Distributor_System
{
    class DBOperations
    {
        private SqlConnection SQLDBConnection;
        private SqlCommand SQLDBCommand;
        private SqlTransaction SQLDBTransaction;
        public string _connectionString = string.Empty;

        public DBOperations()
        {
            try
            {
                SQLDBConnection = new SqlConnection();
                SQLDBCommand = new SqlCommand();
                SQLDBCommand.Connection = SQLDBConnection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SP_Transaction(string strConnection, string strTransactionCommand, string[,] strParameterValue)
        {
            try
            {
                if (strParameterValue == null || strTransactionCommand.Trim() == "" || strConnection.Trim() == "")
                {
                    MessageBox.Show("Some parameters are Missing in the given Query");
                    return "";
                }

                SQLDBCommand.CommandTimeout = 0;
                SQLDBCommand.CommandText = strTransactionCommand;
                SQLDBCommand.Parameters.Clear();

                for (int x = 0; x <= strParameterValue.GetUpperBound(0); ++x)
                {
                    if ((Convert.ToString(strParameterValue[x, 0]).ToUpper() == "NULL") || (Convert.ToString(strParameterValue[x, 1]).ToUpper() == "NULL"))
                    {
                        strParameterValue[x, 0] = DBNull.Value.ToString();
                        strParameterValue[x, 1] = DBNull.Value.ToString();
                    }
                    SQLDBCommand.Parameters.AddWithValue(strParameterValue[x, 0], strParameterValue[x, 1]);
                }
                
                SQLDBCommand.CommandType = CommandType.StoredProcedure;
                SQLDBCommand.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                throw err;
            }
            
            return "";
        }

        public string GetSqlConnStr()
        {
            try
            {
                _connectionString = "Data Source=DB-IT-USER;Initial Catalog=SFADistributor;User ID=sa;Password=dbit";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _connectionString;
        }

        public void Begin_transaction(string StrConnection)
        {
            try
            {
                if (SQLDBConnection.State == ConnectionState.Closed)
                {
                    SQLDBConnection.ConnectionString = StrConnection;
                    SQLDBConnection.Open();
                    SQLDBTransaction = SQLDBConnection.BeginTransaction();
                    SQLDBCommand.Transaction = SQLDBTransaction;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Commit_transaction()
        {
            try
            {
                if (SQLDBConnection.State == ConnectionState.Open)
                {
                    SQLDBTransaction.Commit();
                    SQLDBConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getRecordCountInUserTable()
        {
            string sqlQuery = "SELECT COUNT(user_ID) FROM tbl_UserDetails";
            
            SqlConnection myConnection;
            myConnection = new SqlConnection("Data Source=DB-IT-USER;Initial Catalog=SFADistributor;User ID=sa;Password=dbit");
            myConnection.Open();

            SqlCommand queryCommand = new SqlCommand(sqlQuery, myConnection);

            int rdCount;
            try
            {
                SqlDataReader reader = queryCommand.ExecuteReader();

                reader.Read();

                rdCount = reader.GetInt32(0);

                reader.Close();
                myConnection.Close();
                return rdCount;
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                myConnection.Close();
                return rdCount = 0;
            }
        }
    }
}
