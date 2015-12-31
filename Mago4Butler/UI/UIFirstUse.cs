using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microarea.Mago4Butler.BL;

namespace Microarea.Mago4Butler
{
    public partial class UIFirstUse : UserControl
    {
        public event EventHandler<InstallInstanceEventArgs> SelectMsiToInstall;
        protected virtual void OnSelectMsiToInstall(InstallInstanceEventArgs e)
        {
            var handler = SelectMsiToInstall;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public UIFirstUse()
        {
            InitializeComponent();
        }

        private void lnkSelectMsi_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OnSelectMsiToInstall(new InstallInstanceEventArgs());
        }
    }
}
