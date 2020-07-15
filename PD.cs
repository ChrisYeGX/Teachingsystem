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
using DevExpress.XtraCharts;

namespace 主界面
{
    public partial class PD : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        //initialize datatable
        DataTable coursetablex = new DataTable();
        DataTable teachertablex = new DataTable();
        DataTable assignteacherX = new DataTable();
        DataTable ITgroup = new DataTable();
        //set mysql adapter
        MySqlDataAdapter k;
        MySqlDataAdapter p;
        MySqlDataAdapter q;
        MySqlDataAdapter t;
        MySqlConnection conn;
        //count the number of teacher
        int teachercount = 0;
        //count the number of course
        int coursecount = 0;
        //to confirm whether user click the comfirm and add button twice at one time
        int comfirm = 0;
        //initialize the pie chart
        Series _pieSeries = new Series("TeacherWorkload", ViewType.Pie);
        public PD()
        {
            InitializeComponent();
            //sql language which used in mysql
            string sql1 = "select * from course";
            string sql2 = "select * from teacher";
            string sql3 = "select * from checklist";
            string sql4 = "select * from itgroup";
            string sqlStr = "uid=liam;pwd=yilian723;database=liam;server=sql.m244.vhostgo.com";
            conn = new MySqlConnection(sqlStr);
            conn.Open();
            //assign value to each specific adapter
            k = new MySqlDataAdapter(sql1, sqlStr);
            k.Fill(coursetablex);
            p = new MySqlDataAdapter(sql2, sqlStr);
            p.Fill(teachertablex);
            q = new MySqlDataAdapter(sql3, sqlStr);
            q.Fill(assignteacherX);
            t = new MySqlDataAdapter(sql4, sqlStr);
            t.Fill(ITgroup);
            //due to sql to get teacher number
            teachercount = teachertablex.Rows.Count;
            //due to sql to get course number
            coursecount = coursetablex.Rows.Count;
            //make pie chart unvisible
            chartControl1.Visible = false;
            //add graph to it
            chartControl1.Series.Add(_pieSeries);        
        }
        public class Teacher
        {

            String Name;
            public Teacher(string name)
            {
                //set name
                this.Name = name;
            }

            public String getname()
            {
                //get name
                return this.Name;
            }
            public double Course(DataTable assignteacherX)
            {
                string total = "";
                int i;
                double j = 0;
                //calculate course number
                for (i = 0; i < assignteacherX.Rows.Count; i++)
                {
                    if (assignteacherX.Rows[i][2].ToString().Equals(this.Name))
                    {
                        //one duplicate course counts as 0.5
                        if (total.Contains(assignteacherX.Rows[i][0].ToString()))
                        {
                            j += 0.5;
                            total += assignteacherX.Rows[i][0].ToString();
                        }
                        //the courses which are not duplicated
                        else
                        {
                            total += assignteacherX.Rows[i][0].ToString();
                            j++;
                        }

                    }

                }
                //return course number that teacher have
                return j;
            }
            public int IT(DataTable ITgroup)
            {
                int i;
                int j = 0;
                //calculate IT course number 
                for (i = 0; i < ITgroup.Rows.Count; i++)
                {
                    if (ITgroup.Rows[i][1].ToString().Equals(this.Name))
                    {
                        j++;
                    }

                }
                //return IT course number that teacher have
                return j;
            }
            public double GetWorkload(DataTable assignteacherX, DataTable ITgroup)
            {
                //the situation that teacher didn't have IT course
                if (this.IT(ITgroup) == 0)
                {
                    //get workload
                    return this.Course(assignteacherX) * 1;
                }
                //the situation that teacher have IT course
                double IT1 = 1 + (this.IT(ITgroup) - 1) * 0.5;
                double Course = this.Course(assignteacherX) * 1;
                //get workload
                return IT1 + Course;
            }

        }

        private void Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        private void ribbonControl1_SelectedPageChanged_1(object sender, EventArgs e)
        {
            //when you click Course information
            if (ribbonControl1.SelectedPage.ToString().Equals("Course information"))
            {
                //show table information
                Coursetable.MainView = course;
                Coursetable.DataSource = coursetablex;
            }
            //when you click Assign
            else if (ribbonControl1.SelectedPage.ToString().Equals("Assign"))
            {
                repositoryItemComboBox1.Items.Clear();
                for (int x = 0; x < teachertablex.Rows.Count; x++)
                {
                    //add teacher inforamtion to combobox
                    repositoryItemComboBox1.Items.Add(teachertablex.Rows[x][0].ToString());
                }
                Coursetable.MainView = Assignteacher;
                Coursetable.DataSource = assignteacherX;
            }
            //when you click Teacher
            else if (ribbonControl1.SelectedPage.ToString().Equals("Teacher"))
            {
                MySqlCommandBuilder sb2 = new MySqlCommandBuilder(p);
                int i;
                //update teacher information
                for (i = 0; i < teachertablex.Rows.Count; i++)
                {   
                    Teacher weifeng = new Teacher(teachertablex.Rows[i][0].ToString());
                    String result1 = weifeng.GetWorkload(assignteacherX, ITgroup).ToString();
                    teachertablex.Rows[i][1] = result1;
                }
                //show teacher table
                Coursetable.MainView = TeacherInfo;
                Coursetable.DataSource = teachertablex;
            }
            //When you click IT
            else
            {
                for (int x = 0; x < teachertablex.Rows.Count; x++)
                {
                    //add teacher name to ITcombobox
                    repositoryItemComboBox1.Items.Add(teachertablex.Rows[x][0].ToString());
                }
                //show IT table
                Coursetable.MainView = SetItgroup;
                Coursetable.DataSource = ITgroup;
            }

        }

        private void 主界面_Load(object sender, EventArgs e)
        {
            //when main page load, it appear table
            Coursetable.MainView = course;
            Coursetable.DataSource = coursetablex;
        }

        private void Coursetable_Click(object sender, EventArgs e)
        {

        }
        private void Assignteacher_CellValueChanging_1(object sender, CellValueChangedEventArgs e)
        {   
            assignteacherX.Rows[e.RowHandle][2] = e.Value.ToString();
            for (int x = 0; x < teachertablex.Rows.Count; x++)
            {
                Teacher newone = new Teacher(teachertablex.Rows[x][0].ToString());
                //when workload much higher
                if (newone.GetWorkload(assignteacherX, ITgroup) >= 3)
                {
                    //it will show error
                    tip.Caption = "For " + newone.getname() + " has more than 3 workloads";
                }
            }
        }
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //add new IT group
            if (setnumber.EditValue.ToString() != "0"&&setnumber.EditValue.ToString()!="NULL")
            {
                int x = Convert.ToInt32(setnumber.EditValue.ToString());
                //update IT table
                for (int y = 1; y <= x; y++)
                {
                    int k =  ITgroup.Rows.Count+1 ;
                    DataRow u = ITgroup.NewRow();
                    u[0] = "Group" + k.ToString();
                    if(ITgroup.Rows.Count>=1)
                    u[2] = Convert.ToInt16(ITgroup.Rows[ITgroup.Rows.Count - 1][2]) + 1;
                    else u[2] = 1;                   
                    ITgroup.Rows.Add(u);

                }
            }
        }

        private void SetItgroup_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            ITgroup.Rows[e.RowHandle][1] = e.Value.ToString();
            for (int x = 0; x < teachertablex.Rows.Count; x++)
            {
                Teacher newone = new Teacher(teachertablex.Rows[x][0].ToString());
                //when workload much higher
                if (newone.GetWorkload(assignteacherX, ITgroup) >= 3)
                {
                    //it will show error
                    tip.Caption = "For " + newone.getname() + " has more than 3 workloads";
                }
            }
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //save table information
            ITgroup.PrimaryKey = new DataColumn[] { ITgroup.Columns["id"] };
            MySqlCommandBuilder sb1 = new MySqlCommandBuilder(t);
            //update ITgroup
            t.Update(ITgroup);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //save assign table
            MySqlCommandBuilder sb1 = new MySqlCommandBuilder(q);
            //update assignteacher table
            q.Update(assignteacherX);
        }
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //add new course
            coursetablex.Rows.Add();
            //this will reset the confirm parameter
            comfirm = 0;
            
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
                int hRowHandle = course.FocusedRowHandle;
                //delete course that you want
                if (hRowHandle >= 0&&coursecount==coursetablex.Rows.Count)
                {
                    String x = "delete from course where CourseName='" + coursetablex.Rows[hRowHandle][0].ToString() + "'";
                    coursetablex.Rows.RemoveAt(hRowHandle);
                    String y = "delete from checklist where CourseName='" + assignteacherX.Rows[hRowHandle][0].ToString() + "'";
                    assignteacherX.Rows.RemoveAt(hRowHandle);
                    //update information of two table in mysql
                    MySqlCommand comm = new MySqlCommand(x, conn);
                    MySqlCommand comd = new MySqlCommand(y, conn);
                    comm.ExecuteNonQuery();
                    comd.ExecuteNonQuery();
                }
            coursecount--;
        }
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int hRowHandle = SetItgroup.FocusedRowHandle;
            //delete IT group that you want
            if (hRowHandle >= 0)
            {
                //sql language
                String command = "delete from itgroup where Number ='" + ITgroup.Rows[hRowHandle][0].ToString() + "'";
                MySqlCommand comm = new MySqlCommand(command, conn);
                ITgroup.Rows.RemoveAt(hRowHandle);
                comm.ExecuteNonQuery();
            }
        }


        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //add new teacher
            teachertablex.Rows.Add();
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*for(int rows = teachercount; rows < teachertablex.Rows.Count; rows++)
            {
                //insert new teacher value in database
                String command = "insert teacher values ('"+teachertablex.Rows[rows][0].ToString()+"','0')";
                MySqlCommand comm = new MySqlCommand(command, conn);
                comm.ExecuteNonQuery();
            }*/
            p.Update(teachertablex);
            //update the number of teacher
            teachercount = teachertablex.Rows.Count;

        }

        private void Quit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //quit the program
            this.Dispose();
            Login x = new Login();
            x.Show();
        }
        private void EXCEL_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            string path = "";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                path = folder.SelectedPath;
            }
            StreamWriter sw;
            if (path != "")
            {
                sw = new StreamWriter(path + "/assgin.xls", false, Encoding.GetEncoding("gb2312"));

                StringBuilder sb = new StringBuilder();
                for (int k = 0; k < assignteacherX.Columns.Count - 1; k++)
                {
                    // add column to table 
                    sb.Append(assignteacherX.Columns[k].ColumnName.ToString() + "\t");
                }
                sb.Append(Environment.NewLine);
                // add row data 
                for (int i = 0; i < assignteacherX.Rows.Count; i++)
                {
                    DataRow row = assignteacherX.Rows[i];
                    for (int j = 0; j < assignteacherX.Columns.Count - 1; j++)
                    {
                        // depends on the specific column to add the data  
                        sb.Append(row[j].ToString() + "\t");
                    }
                    sb.Append(Environment.NewLine);                   

                }
                sw.Write(sb.ToString());
                sw.Flush();
                sw = new StreamWriter(path + "/workloadTeacher.xls", false, Encoding.GetEncoding("gb2312"));
                StringBuilder sd = new StringBuilder();
                for (int k = 0; k < teachertablex.Columns.Count; k++)
                {                     
                    sd.Append(teachertablex.Columns[k].ColumnName.ToString() + "\t");
                }
                sd.Append(Environment.NewLine);              
                for (int i = 0; i < teachertablex.Rows.Count; i++)
                {
                    DataRow row = teachertablex.Rows[i];
                    for (int j = 0; j < teachertablex.Columns.Count; j++)
                    {                       
                        sd.Append(row[j].ToString() + "\t");
                    }
                    sd.Append(Environment.NewLine);
                }
                sw.Write(sd.ToString());
                sw.Flush();
                sw = new StreamWriter(path + "/ITgroup.xls", false, Encoding.GetEncoding("gb2312"));
                StringBuilder sc = new StringBuilder();
                for (int k = 0; k < ITgroup.Columns.Count; k++)
                {
                    sc.Append(ITgroup.Columns[k].ColumnName.ToString() + "\t");
                }
                sc.Append(Environment.NewLine);
                for (int i = 0; i < ITgroup.Rows.Count; i++)
                {
                    DataRow row = ITgroup.Rows[i];
                    for (int j = 0; j < ITgroup.Columns.Count; j++)
                    {
                        sc.Append(row[j].ToString() + "\t");
                    }
                    sc.Append(Environment.NewLine);
                }
                sw.Write(sc.ToString());
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }
        private void Quit_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //exit
            System.Environment.Exit(0);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int hRowHandle = TeacherInfo.FocusedRowHandle;
            //delete teacher that you want
            if (hRowHandle >= 0)
            {
                //get name
                String name = teachertablex.Rows[hRowHandle][0].ToString();
                teachertablex.Rows.RemoveAt(hRowHandle);
                //delete teacher information in database
                String x = "delete from teacher where Instructor = '" + name + "'";
                MySqlCommand comm = new MySqlCommand(x, conn);
                comm.ExecuteNonQuery();
            }
            teachercount--;

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            comfirm++;
            //when you click confirm at first time
            if (comfirm == 1)
            {
                for (int rows = coursecount; rows < coursetablex.Rows.Count; rows++)
                {
                    int lastid;
                    if (rows >= 1)
                    {
                         lastid = Convert.ToInt16(coursetablex.Rows[rows - 1][3].ToString()) + 1;
                    }
                    else 
                    {
                         lastid = 1;
                    }
                    //update database course information
                    coursetablex.Rows[rows][3] = lastid;
                    String command = "insert course values ('" + coursetablex.Rows[rows][0].ToString() + "','" + coursetablex.Rows[rows][1].ToString() + "','" + coursetablex.Rows[rows][2].ToString() + "','"+lastid.ToString()+"')";
                    MySqlCommand comm = new MySqlCommand(command, conn);
                    comm.ExecuteNonQuery();
                    //set new data row
                    DataRow u = assignteacherX.NewRow();
                    u.SetField(0, coursetablex.Rows[rows][0].ToString());
                    u.SetField(1, coursetablex.Rows[rows][2]);
                    u.SetField(2, "Noone");
                    u.SetField(3, Convert.ToInt16(coursetablex.Rows[rows][3]));
                    assignteacherX.Rows.Add(u);                  
                }
                MySqlCommandBuilder x = new MySqlCommandBuilder(q);
                coursecount = coursetablex.Rows.Count;
                //update assignteacher table
                q.Update(assignteacherX);
            }           
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //show pie chart
            chartControl1.Visible = true;
            //value number:workload
            _pieSeries.ValueDataMembers[0] = "WorkLoad";
            //agrument:instructor
            _pieSeries.ArgumentDataMember = "Instructor";
            _pieSeries.DataSource = teachertablex;
            _pieSeries.LegendPointOptions.PointView = PointView.ArgumentAndValues;
        }

        private void chartControl1_Click(object sender, EventArgs e)
        {
            //when you click chart it will dispear
            chartControl1.Visible = false;
        }

        private void TeacherInfo_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {   if(e.Column.Name=="Instructor")
            teachertablex.Rows[e.RowHandle][0] = e.Value.ToString();
        }

        private void course_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.Name == "Courename1")
                coursetablex.Rows[e.RowHandle][0] = e.Value.ToString();
            else if (e.Column.Name == "gridColumn1")
                coursetablex.Rows[e.RowHandle][2] = e.Value.ToString() ;
        }
    }
}


