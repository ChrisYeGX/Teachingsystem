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

namespace 主界面
{
    public partial class 主界面 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public 主界面()
        {
            InitializeComponent();
        }

        private void Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void ribbonControl1_SelectedPageChanged(object sender, EventArgs e)
        {
            string s = "";
            s = sender.GetType().GUID.ToString();
            MessageBox.Show(s);
        }
        public MySqlDataReader getdatatable(string x)
        { 
            string sql = "select * from" +" "+ x;
            string sqlStr = "server= sql.m244.vhostgo.com ;user=liam ;password=yilian723 ;database=liam"; //连接字符串
            MySqlConnection conStr = new MySqlConnection(sqlStr);//创建Connection对象
            conStr.Open();//打开数据库
            MySqlCommand command = new MySqlCommand(sql, conStr);
            MySqlDataReader reader = command.ExecuteReader();
            /*DataTable dt = new DataTable();
            dt.Columns.Add("CourseName", typeof(string)); //添加列集，下面都是
            dt.Columns.Add("Info", typeof(string));
            dt.Columns.Add("Year", typeof(int));*/
            return reader;
        }
        private void ribbonControl1_SelectedPageChanged_1(object sender, EventArgs e)
        {
            if (ribbonControl1.SelectedPage.ToString().Equals("Courseinformation"))
            {
                Coursetable.MainView = course;
                Coursetable.DataSource = getdatatable("course");
            }
            else if (ribbonControl1.SelectedPage.ToString().Equals("Assign"))
            {
               MySqlDataReader i = getdatatable("course");
               MySqlDataReader j = getdatatable("teacher");
               Coursetable.MainView = Assignteacher;
               Coursetable.DataSource = i;
               while (j.Read())
               {
                   repositoryItemCheckedComboBoxEdit1.Items.Add(j.GetValue(0).ToString());
               }
            }
            else if (ribbonControl1.SelectedPage.ToString().Equals("Teacher"))
            { 
                Coursetable.MainView = TeacherInfo;
                Coursetable.DataSource = getdatatable("teacher");               
            }
            else Coursetable.MainView = Check;
        }

        private void 主界面_Load(object sender, EventArgs e)
        {
            Coursetable.MainView = course;
            Coursetable.DataSource = getdatatable("course");
        }

        private void Coursetable_Click(object sender, EventArgs e)
        {

        }

        private void Assignteacher_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
                    e.Value.ToString();
        }
    }
}
