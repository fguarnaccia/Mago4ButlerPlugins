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

        public event EventHandler<EventArgs> ProgressTextChanged;
        public event EventHandler<EventArgs> Back;
        protected virtual void OnBack(EventArgs e)
        {
            var handler = Back;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnProgressTextChanged(EventArgs e)
        {
            var handler = ProgressTextChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal void ClearDetails()
        {
            this.txtDetails.Clear();
        }

        public void SetProgressText(string message)
        {
            this.syncCtx.Post(new SendOrPostCallback(
                (obj)
                =>
                {
                    this.lblProgressText.Text = message;
                    this.OnProgressTextChanged(EventArgs.Empty);
                }
                ), null);
        }
        public string GetProgressText()
        {
            string progressText = null;
            this.syncCtx.Send(new SendOrPostCallback((obj) => progressText = this.lblProgressText.Text), null);
            return progressText;
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.OnBack(e);
        }
    }
}
