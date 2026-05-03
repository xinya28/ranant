using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ranant.Pages
{
    public partial class MainSettings : UserControl
    {
        public MainSettings()
        {
            InitializeComponent();
            btnBack.Click += (s, e) =>
            {
                var mainForm = this.FindForm() as MainForm;
                mainForm?.ShowMainHome();
            };
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
