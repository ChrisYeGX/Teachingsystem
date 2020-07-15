using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace 主界面
{
    public partial class Logintest : DevExpress.XtraEditors.XtraForm
    {
        public Logintest()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            PD x = new PD();
            x.Visible = true;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            TA y = new TA();
            y.Visible = true;
        }
    }
}