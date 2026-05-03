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
    public partial class MainHome : UserControl
    {
        public event EventHandler ConfirmClicked;
        public event EventHandler SettingsClicked;

        public MainHome()
        {
            InitializeComponent();
            btnConfirm.Click += (s, e) => ConfirmClicked?.Invoke(this, EventArgs.Empty);
            btnSettings.Click += (s, e) => SettingsClicked?.Invoke(this, EventArgs.Empty);
        }

        public int GetSelectedNumber()
        {
            return (int)numInputPeople.Value;
        }

        public string GetSelectedMethod()
        {
            return cboMethod.SelectedItem?.ToString() ?? "随机";
        }

    }
}
