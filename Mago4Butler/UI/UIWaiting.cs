using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Microarea.Mago4Butler
{
    public partial class UIWaiting : UserControl
    {
        SynchronizationContext syncCtx;

        public UIWaiting()
        {
            this.syncCtx = SynchronizationContext.Current;
            if (this.syncCtx == null)
            {
                this.syncCtx = new WindowsFormsSynchronizationContext();
            }
            InitializeComponent();
        }

        public void SetProgressText(string message)
        {
            this.syncCtx.Post(new SendOrPostCallback((obj) => this.lblProgressText.Text = message), null);
        }

        public void AddDetailsText(string message)
        {
            this.syncCtx.Post(new SendOrPostCallback((obj) =>
            {
                this.txtDetails.AppendText(message);
                this.txtDetails.AppendText(Environment.NewLine);
                this.txtDetails.ScrollToCaret();
            }),
            null);
        }
    }
}
