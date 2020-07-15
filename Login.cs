using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using System.IO;

namespace 主界面
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        public Login()
        {
            InitializeComponent();
            this.textEdit3.Properties.UseSystemPasswordChar = true;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {   
            //get TA information from the database if not exist then warning
            if (checkEdit1.Checked == true && checkEdit2.Checked == false)
            {
                string sqlStr = "uid=liam;pwd=yilian723;database=liam;server=sql.m244.vhostgo.com";
                MySqlConnection conn = new MySqlConnection(sqlStr);
                conn.Open();
                string sql1 = "select * from headta where Name = '" + textEdit1.Text + "' and Password ='" + textEdit3.Text + "'";
                MySqlCommand comd = new MySqlCommand(sql1,conn);
                MySqlDataReader newuser = comd.ExecuteReader();
                //if exist create a new ta form
                if (newuser.HasRows)
                {
                this.Visible = false;
                splashScreenManager1.ShowWaitForm();
                TA x = new TA();
                x.Visible = true;
                splashScreenManager1.CloseWaitForm();
                conn.Close();
                }
                else MessageBox.Show("wrong password or doesn't exist");conn.Close();
            }
            //get PD information from the database if not exist then warning
            else if (checkEdit1.Checked == false && checkEdit2.Checked == true)
            {
                string sqlStr = "uid=liam;pwd=yilian723;database=liam;server=sql.m244.vhostgo.com";
                MySqlConnection conn = new MySqlConnection(sqlStr);
                conn.Open();
                string sql1 = "select * from user where Name = '" + textEdit1.Text + "' and Password ='" + textEdit3.Text + "'";
                MySqlCommand comd = new MySqlCommand(sql1, conn);
                MySqlDataReader newuser = comd.ExecuteReader();
                //if exist create a new pd form
                if (newuser.HasRows)
                {
                this.Visible = false;
                splashScreenManager1.ShowWaitForm();
                PD x = new PD();
                x.Visible = true;
                splashScreenManager1.CloseWaitForm();
                conn.Close();
                }
                else MessageBox.Show("wrong password or doesn't exist");conn.Close();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
        }
    }
}