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
    public partial class TA : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        //create datatable
        DataTable coursetablex = new DataTable();
        DataTable tatable = new DataTable();
        DataTable Timetable = new DataTable();
        //set mysql adapter
        MySqlDataAdapter k;
        MySqlDataAdapter p;
        MySqlDataAdapter q;
        //record the operating cell row
        int tablecellrow = 0;
        // record the operating cell column
        string tablecellcolunm = "";
        //the number of teacher
        int teachercount = 0;
        MySqlConnection conn;
        //initialize pie chart
        Series _pieSeries = new Series("TAWorkload", ViewType.Pie);
        public TA()
        {
            InitializeComponent();
            repositoryItemTimeSpanEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            repositoryItemTimeSpanEdit1.DisplayFormat.FormatString = "{0:dd} Weekdays, {0:hh} beginhours";
            //sql language to operate database
            string sql1 = "select * from coursetablear";
            string sql2 = "select * from assistant";
            string sql3 = "select * from timetable";
            string sqlStr = "uid=liam;pwd=yilian723;database=liam;server=sql.m244.vhostgo.com";  
            //get conn
            conn = new MySqlConnection(sqlStr);
            conn.Open();
            // assign data adapter
            k = new MySqlDataAdapter(sql1, sqlStr);
            k.Fill(coursetablex);
            p = new MySqlDataAdapter(sql2, sqlStr);
            p.Fill(tatable);
            q = new MySqlDataAdapter(sql3, sqlStr);
            q.Fill(Timetable);
            //
            for (int x = 0; x < tatable.Rows.Count; x++)
            {
                assistant newone = new assistant(tatable.Rows[x]["Name"].ToString(), coursetablex);
                repositoryItemCheckedComboBoxEdit1.Items.Add(newone.getname() + " current workload is: " + newone.getworkload().ToString());
            }
            //get the number of teacher
            teachercount = tatable.Rows.Count;
            
        }
        public class assistant
        {
            //initialize value
            //name
            String Name;
            //the first hour for each course which apply to the ta
            String[] normalcoursetime1 = new String[6];
            //the second hour for each course which apply to the ta
            String[] normalcoursetime2 = new String[6];
            //the second hour for each course which apply to the ta
            String[] normalcoursetime3 = new String[6];
            //the tutorial hour for each course which apply to the ta
            String[] tutorialtime = new String[6];
            //this is used for checking 
            String[] alltime = new String[24];
            int[] rows = new int[6];
            int workload = 0;
            int number = 0;

            public assistant(string name, DataTable x)
            {
                //set name
                this.Name = name;
                for (int p = 0; p < 6; p++)
                {
                    //initialize course time
                    normalcoursetime1[p] = "nil";
                    normalcoursetime2[p] = "nil";
                    normalcoursetime3[p] = "nil";
                    tutorialtime[p] = "nil";
                }
                //add all hours to the alltime array
                normalcoursetime1.CopyTo(alltime, 0);
                normalcoursetime2.CopyTo(alltime, 6);
                normalcoursetime3.CopyTo(alltime, 12);
                tutorialtime.CopyTo(alltime, 18);
                //calculate ta workload
                for (int i = 0; i < x.Rows.Count; i++)
                {
                    string total = x.Rows[i][6].ToString();
                    if (total.Contains(this.Name))
                    {
                        workload++;
                        //when ta workload smaller than 6
                        if (workload <= 6)
                        {
                            //make information to table
                            normalcoursetime1[number] = x.Rows[i][2].ToString();
                            normalcoursetime2[number] = x.Rows[i][3].ToString();
                            normalcoursetime3[number] = x.Rows[i][4].ToString();
                            tutorialtime[number] = x.Rows[i][7].ToString();
                            normalcoursetime1.CopyTo(alltime, 0);
                            normalcoursetime2.CopyTo(alltime, 6);
                            normalcoursetime3.CopyTo(alltime, 12);
                            tutorialtime.CopyTo(alltime, 18);
                            rows[number] = i;
                            number++;
                        }
                        //when workload lager than requirement
                        else
                        {
                            workload--;
                            x.Rows[i][6] = "";
                            x.Rows[i][7] = "nil";
                        }
                    }
                }
            }
            public int getworkload()
            {
                //get workload
                return workload;
            }
            public String getname()
            {
                //get name
                return this.Name;
            }
            public bool checktacourse(DataTable x)
            {
                bool right = true;
                //go through all time in table
                foreach (string k in alltime)
                {
                    int current = 0;
                    int index = 0;
                    //check normalcoursetime1
                    foreach (string l in normalcoursetime1)
                    {
                        if (k.Equals(l)) current++;
                        //if normalcoursetime1  happen conflicts, the row data turn to nil
                        if (!k.Equals("nil") && !l.Equals("nil") && k.Equals(l) && current > 1)
                        {
                            x.Rows[rows[index]][2] = "nil";
                            x.Rows[rows[index]][3] = "nil";
                            x.Rows[rows[index]][4] = "nil";
                            x.Rows[rows[index]][7] = "nil";
                            right = false;
                            break;
                        }
                        index++;
                    }
                    index = 0;
                    //check normalcoursetime2
                    foreach (string l in normalcoursetime2)
                    {
                        if (k.Equals(l)) current++;
                        //if normalcoursetime2  happen conflicts, the row data turn to nil
                        if (!k.Equals("nil") && !l.Equals("nil") && k.Equals(l) && current > 1)
                        {
                            x.Rows[rows[index]][2] = "nil";
                            x.Rows[rows[index]][3] = "nil";
                            x.Rows[rows[index]][4] = "nil";
                            x.Rows[rows[index]][7] = "nil";
                            right = false;
                            break;
                        }
                        index++;
                    }
                    index = 0;
                    //check normalcoursetime3
                    foreach (string l in normalcoursetime3)
                    {
                        if (k.Equals(l)) current++;
                        //if normalcoursetime3  happen conflicts, the row data turn to nil
                        if (!k.Equals("nil") && !l.Equals("nil") && k.Equals(l) && current > 1)
                        {
                            x.Rows[rows[index]][2] = "nil";
                            x.Rows[rows[index]][3] = "nil";
                            x.Rows[rows[index]][4] = "nil";
                            x.Rows[rows[index]][7] = "nil";
                            right = false;
                            break;
                        }
                        index++;
                    }
                    index = 0;
                    //check if it happens conflicts on tutorialtime
                    foreach (string l in tutorialtime)
                    {
                        if (k.Equals(l)) current++;
                        //when it happens conflicts recover value by nil
                        if (!k.Equals("nil") && !l.Equals("nil") && k.Equals(l) && current > 1)
                        {
                            x.Rows[rows[index]][2] = "nil";
                            x.Rows[rows[index]][3] = "nil";
                            x.Rows[rows[index]][4] = "nil";
                            x.Rows[rows[index]][7] = "nil";
                            right = false;
                            break;
                        }
                        index++;
                    }

                }
                //return bool value of check function
                return right;
            }
        }


        private void TA_Load(object sender, EventArgs e)
        {
            TAgirdviewcontrol.MainView = TAinformation;
            TAgirdviewcontrol.DataSource = tatable;
            //make chart unvisible
            chartControl1.Visible = false;
            //add pie cahrt
            chartControl1.Series.Add(_pieSeries);
        }

        private void ribbon_SelectedPageChanged(object sender, EventArgs e)
        {
            //when you click Coursetable
            if (ribbon.SelectedPage.ToString().Equals("Coursetable"))
            {
                //show course information
                TAgirdviewcontrol.MainView = Coursetable;
                TAgirdviewcontrol.DataSource = coursetablex;
            }
            //when you click TA information
            else if (ribbon.SelectedPage.ToString().Equals("TA information"))
            {
                //get workload of ta
                for (int i = 0; i < tatable.Rows.Count; i++)
                {
                    assistant newone = new assistant(tatable.Rows[i]["Name"].ToString(), coursetablex);
                    tatable.Rows[i]["Workload"] = newone.getworkload();
                }
                TAgirdviewcontrol.MainView = TAinformation;
                TAgirdviewcontrol.DataSource = tatable;
            }
            else
            {
                TAgirdviewcontrol.MainView = TAinformation;
                TAgirdviewcontrol.DataSource = tatable;
            }
        }

        private void TAgirdviewcontrol_Click(object sender, EventArgs e)
        {

        }
        private void Coursetable_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            tablecellrow = e.RowHandle;
            tablecellcolunm = e.Column.Name.ToString();
            //when you click timetable cell
            if (e.Column.Name.ToString() == "Time1" || e.Column.Name.ToString() == "Time2" || e.Column.Name.ToString() == "Time3")
            {
                //go to another table to choose time
                TAgirdviewcontrol.MainView = timetable;
                TAgirdviewcontrol.DataSource = Timetable;
            }
            //when you click tutorialtime celll
            else if (e.Column.Name.ToString() == "tutorialtime")
            {
                if (coursetablex.Rows[e.RowHandle][6].ToString() != "")
                {
                    //go to another table to choose time
                    TAgirdviewcontrol.MainView = timetable;
                    TAgirdviewcontrol.DataSource = Timetable;
                }
            }
        }

        private void timetable_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.Name.ToString().Equals("TimeX"))
            {
                //do nothing
            }
            else
            {
                //add information to table
                string x = e.Column.Name.ToString() + " " + Timetable.Rows[e.RowHandle][0].ToString();
                coursetablex.Rows[tablecellrow][tablecellcolunm] = x;
                TAgirdviewcontrol.MainView = Coursetable;
                TAgirdviewcontrol.DataSource = coursetablex;
            }
            for (int k = 0; k < tatable.Rows.Count; k++)
            {
                assistant newone = new assistant(tatable.Rows[k]["Name"].ToString(), coursetablex);
                //when it happens conflict
                if (!newone.checktacourse(coursetablex))
                    //show error message
                    Error.Caption = "Can not choose the time as " + newone.getname() + " has a time collision";
            }
            String[] time = new String[4];
            time[0] = coursetablex.Rows[tablecellrow][2].ToString();
            time[1] = coursetablex.Rows[tablecellrow][3].ToString();
            time[2] = coursetablex.Rows[tablecellrow][4].ToString();
            time[3] = coursetablex.Rows[tablecellrow][7].ToString();
            int current = 0;
            //identify the same time in table
            foreach(String sentence1 in time)
            {
                foreach (String sentence2 in time)
                {
                    //it happens conflicts
                    if (sentence1==sentence2&&sentence1!="nil"&&sentence2!="nil")
                    {
                        current++;
                        //turn value to nil
                        if (current > 1)
                        {
                            coursetablex.Rows[tablecellrow][2] = "nil";
                            coursetablex.Rows[tablecellrow][3] = "nil";
                            coursetablex.Rows[tablecellrow][4] = "nil";
                            coursetablex.Rows[tablecellrow][7] = "nil";
                            Error.Caption = coursetablex.Rows[tablecellrow][1].ToString() + " has a time collision";
                        }
                    }
                    
                }
                current = 0;
            }   
            //check conflict in table
           for(int k =0; k < coursetablex.Rows.Count; k++)
            {
                String a = coursetablex.Rows[k][1].ToString();
                String[] atime = new String[4];
                atime[0] = coursetablex.Rows[k][2].ToString();
                atime[1] = coursetablex.Rows[k][3].ToString();
                atime[2] = coursetablex.Rows[k][4].ToString();
                atime[3] = coursetablex.Rows[k][7].ToString();
                for(int b = k + 1; b < coursetablex.Rows.Count;b++)
                {
                    if (a == coursetablex.Rows[b][1].ToString())
                    {
                        //when it happen conflict
                        if (coursetablex.Rows[b][2].ToString() != "nil")
                        {
                            foreach(String m in atime)
                            {
                                //turn it into nil
                                if (coursetablex.Rows[b][2].ToString() == m && m != "nil")
                                {
                                    coursetablex.Rows[b][2] = "nil";
                                    Error.Caption = "You can not set duplicate time for this course";
                                }
                            }
                        }
                        //when it happen conflict
                        if (coursetablex.Rows[b][3].ToString() != "nil")
                        {
                            foreach (String m in atime)
                            {
                                //turn it into nil
                                if (coursetablex.Rows[b][3].ToString() == m && m != "nil")
                                {
                                    coursetablex.Rows[b][3] = "nil";
                                    Error.Caption = "You can not set duplicate time for this course";
                                }
                            }
                        }
                        //when it happen conflict
                        if (coursetablex.Rows[b][4].ToString() != "nil")
                        {
                            foreach (String m in atime)
                            {
                                //turn it into nil
                                if (coursetablex.Rows[b][4].ToString() == m && m != "nil")
                                {
                                    coursetablex.Rows[b][4] = "nil";
                                    Error.Caption = "You can not set duplicate time for this course";
                                }
                            }
                        }
                        //when it happen conflict
                        if (coursetablex.Rows[b][7].ToString() != "nil")
                        {
                            foreach (String m in atime)
                            {
                                //turn it into nil
                                if (coursetablex.Rows[b][7].ToString() == m && m != "nil")
                                {
                                    coursetablex.Rows[b][7] = "nil";
                                    Error.Caption = "You can not set duplicate time for this course";
                                }
                            }
                        }
                    }
                }
            }       
        }
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //save course table
            MySqlCommandBuilder sb1 = new MySqlCommandBuilder(k);
            //update database information
            k.Update(coursetablex);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MySqlCommandBuilder sb1 = new MySqlCommandBuilder(p);
            //update database information
            p.Update(tatable);
        }
        private void repositoryItemCheckedComboBoxEdit1_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            
            repositoryItemCheckedComboBoxEdit1.Items.Clear();
            for (int x = 0; x < tatable.Rows.Count; x++)
            {
                assistant newone = new assistant(tatable.Rows[x]["Name"].ToString(), coursetablex);
                repositoryItemCheckedComboBoxEdit1.Items.Add(newone.getname() + " current workload is: " + newone.getworkload().ToString());
                //check whether ta go over time
                if (newone.getworkload() == 6)
                {
                    Error.Caption = "For " + newone.getname() + " you can not assign any more tutorial course";
                }
            }
            //show workload
            e.Value = e.Value.ToString().Replace("current workload is: ", "");
            //replace to " "
            for (int j = 0; j < 7; j++)
            {
                e.Value = e.Value.ToString().Replace(j.ToString(), "");
            }
        }
        private void repositoryItemCheckedComboBoxEdit1_QueryCloseUp(object sender, CancelEventArgs e)
        {
            repositoryItemCheckedComboBoxEdit1.Items.Clear();
            for (int x = 0; x < tatable.Rows.Count; x++)
            {
                assistant newone = new assistant(tatable.Rows[x]["Name"].ToString(), coursetablex);
                repositoryItemCheckedComboBoxEdit1.Items.Add(newone.getname() + " current workload is: " + newone.getworkload().ToString());
                //check whether ta go over time
                if (newone.getworkload() == 6)
                {
                    Error.Caption = "For " + newone.getname() + " you can not assign any more tutorial course";
                }
            }
        }

        private void repositoryItemCheckedComboBoxEdit1_QueryPopUp(object sender, CancelEventArgs e)
        {
            repositoryItemCheckedComboBoxEdit1.Items.Clear();
            for (int x = 0; x < tatable.Rows.Count; x++)
            {
                assistant newone = new assistant(tatable.Rows[x]["Name"].ToString(), coursetablex);
                repositoryItemCheckedComboBoxEdit1.Items.Add(newone.getname() + " current workload is: " + newone.getworkload().ToString());
                //check whether ta go over time
                if (newone.getworkload() == 6)
                {
                    Error.Caption = "For " + newone.getname() + " you can not assign any more tutorial course";
                }
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //save ta information
            for (int rows = teachercount; rows < tatable.Rows.Count; rows++)
            {
                int k = Convert.ToInt16(tatable.Rows[rows-1][0].ToString())+1;
                tatable.Rows[rows][0] = k;
                //update new value to database
                String command = "insert assistant values ('" +k.ToString()+"','"+tatable.Rows[rows][1].ToString() + "','0')";
                MySqlCommand comm = new MySqlCommand(command, conn);
                comm.ExecuteNonQuery();
            }
        }

        private void barButtonItem1_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //add new row of ta
            DataRow x = tatable.NewRow();
            int lastid = int.Parse(tatable.Rows[tatable.Rows.Count - 1][0].ToString());
            x.SetField(tatable.Columns[0], lastid + 1);
            //add to tatable
            tatable.Rows.Add(x);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //exit
            System.Environment.Exit(0);
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
                sw = new StreamWriter(path + "/assginTA.xls", false, Encoding.GetEncoding("gb2312"));

                StringBuilder sb = new StringBuilder();
                for (int k = 0; k < coursetablex.Columns.Count-1; k++)
                {
                    //add the name of the column
                    sb.Append(coursetablex.Columns[k].ColumnName.ToString() + "\t");
                }
                sb.Append(Environment.NewLine);
                // add row data   
                for (int i = 0; i < coursetablex.Rows.Count; i++)
                {
                    DataRow row = coursetablex.Rows[i];
                    for (int j = 0; j < coursetablex.Columns.Count-1; j++)
                    {
                        // depends on the column to add the data 
                        sb.Append(row[j].ToString() + "\t");
                    }
                    sb.Append(Environment.NewLine);
                }
                sw.Write(sb.ToString());
                sw.Flush();
                sw = new StreamWriter(path + "/workloadTA.xls", false, Encoding.GetEncoding("gb2312"));               
                StringBuilder sd = new StringBuilder();
                for (int k = 0; k < tatable.Columns.Count; k++)
                {
                    //add the name of the column
                    sd.Append(tatable.Columns[k].ColumnName.ToString() + "\t");
                }
                sd.Append(Environment.NewLine);
                // add row data   
                for (int i = 0; i < tatable.Rows.Count; i++)
                {
                    DataRow row = tatable.Rows[i];
                    for (int j = 0; j < tatable.Columns.Count; j++)
                    {
                        // depends on the column to add the data 
                        sd.Append(row[j].ToString() + "\t");
                    }
                    sd.Append(Environment.NewLine);
                }
                sw.Write(sd.ToString());
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            
        }
        private void ribbon_Click(object sender, EventArgs e)
        {

        }
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //show chart
            chartControl1.Visible = true;
            //value: Workload
            _pieSeries.ValueDataMembers[0] = "Workload";
            //argument: Name
            _pieSeries.ArgumentDataMember = "Name";
            _pieSeries.DataSource = tatable;         
            _pieSeries.LegendPointOptions.PointView = PointView.ArgumentAndValues;
        }
        private void chartControl1_Click(object sender, EventArgs e)
        {
            //when you cilck chart it will disappear
            chartControl1.Visible = false;
        }

        private void TAinformation_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {   
            if(e.Column.Name=="TAname")
            tatable.Rows[e.RowHandle][1] = e.Value.ToString();
        }

        private void Error_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}