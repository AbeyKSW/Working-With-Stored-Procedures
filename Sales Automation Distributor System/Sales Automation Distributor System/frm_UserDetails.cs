using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;

namespace Sales_Automation_Distributor_System
{
    public partial class frm_UserDetails : Form
    {
        
        public frm_UserDetails()
        {
            InitializeComponent();
        }

        /*public ArrayList GetEmpNameLike(string empName)
        {
            SqlConnection myConnection;
            myConnection = new SqlConnection("Data Source=DB-IT-USER;Initial Catalog=TRIOConstructionsInventory;User ID=sa;Password=dbit");
            myConnection.Open();

            ArrayList mtnList = new ArrayList();

            string sqlQuery = "select emp_userName from dbo.tbl_UserDetails WHERE emp_userName LIKE @empName";

            SqlCommand queryCommand = new SqlCommand(sqlQuery, myConnection);

            queryCommand.Parameters.AddWithValue("@empName", "%" + empName + "%");

            try
            {
                SqlDataReader reader = queryCommand.ExecuteReader();

                while (reader.Read())
                {
                    mtnList.Add(reader.GetString(0));
                }

                reader.Close();

                return mtnList;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                return mtnList;
            }
        }

        public void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string keyWrd = "";
            if (dataGridView1.Rows[0].Cells[2].Value != null)
            {
                keyWrd = dataGridView1.Rows[0].Cells[2].Value.ToString();
            }
            ListBox unames = new ListBox();
            ArrayList uname = GetEmpNameLike(keyWrd);

            for (int i = 0; i < uname.Count; i++)
            {
                unames.Items.Add(uname[i]);
            }
        }*/

        private void DBTransaction(string strStatmentType)
        {
            try
            {
                DBOperations SQLDBOperations = new DBOperations();
                string strEmpDetails = "";
                int rdCount = SQLDBOperations.getRecordCountInUserTable();

                string[,] AssighnParaValue = new string[10, 2]
                    {
                        {"@user_ID", txtEmpNumber.Text.ToString()},
                        {"@user_Name", txtUserName.Text.ToString()},
                        {"@full_Name", txtEmpName.Text.ToString()},
                        {"@addr", txtAddress.Text.ToString()},
                        {"@phone", txtPhone.Text.ToString()},
                        {"@password", txtPassword.Text.ToString()},
                        {"@rights_ID", txtEmpNumber.Text.ToString()},
                        {"@type", cmbEmpType.Text.ToString()},
                        {"@date", dtDate.Text.ToString()},
                        {"@StatementType", strStatmentType}
                    };

                SQLDBOperations.Begin_transaction(SQLDBOperations.GetSqlConnStr());
                strEmpDetails = SQLDBOperations.SP_Transaction(SQLDBOperations.GetSqlConnStr(), "sp_User", AssighnParaValue);
                SQLDBOperations.Commit_transaction();

                int newRdCount = SQLDBOperations.getRecordCountInUserTable();
                if (newRdCount == rdCount + 1)
                {
                    MessageBox.Show("Data insert Successfull.");
                }
                else
                {
                    MessageBox.Show("Data insert failed.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            DBTransaction("Insert");
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            DBTransaction("Update");
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            DBTransaction("Delete");
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            txtAddress.Text = "";
            txtEmpName.Text = "";
            txtEmpNumber.Text = "";
            txtPassword.Text = "";
            txtPhone.Text = "";
            txtUserName.Text = "";
            cmbEmpType.SelectedIndex = -1;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Form.ActiveForm.Close();
        }
    }
}
