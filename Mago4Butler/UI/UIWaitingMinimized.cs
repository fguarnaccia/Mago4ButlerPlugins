using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    public partial class UIWaitingMinimized : Form
    {
        SynchronizationContext syncCtx;
        MainForm mainForm;
        UIWaiting uiWaiting;

        public UIWaitingMinimized()
        {
            this.syncCtx = SynchronizationContext.Current;
            if (this.syncCtx == null)
            {
                this.syncCtx = new WindowsFormsSynchronizationContext();
            }
            InitializeComponent();
        }

        internal void AttachToMainUI(MainForm mainForm)
        {
            this.mainForm = mainForm;
            this.mainForm.LocationChanged += MainForm_LocationChanged;
        }

        internal void AttachToMainUiWaiting(UIWaiting uiWaiting)
        {
            this.uiWaiting = uiWaiting;
            this.uiWaiting.ProgressTextChanged += UiWaiting_ProgressTextChanged;
        }

        private void UiWaiting_ProgressTextChanged(object sender, EventArgs e)
        {
            this.syncCtx.Post(new SendOrPostCallback((obj) => this.SetProgressText(this.uiWaiting.GetProgressText())), null);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                MainForm_LocationChanged(this.mainForm, EventArgs.Empty);
            }
        }

        void SetProgressText(string message)
        {
            this.syncCtx.Post(new SendOrPostCallback((obj) => this.lblProgressText.Text = message), null);
        }

        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                var mainForm = sender as MainForm;
                var mainFormLocation = mainForm.Location;
                var newX = mainFormLocation.X + (mainForm.Width - this.Width);
                var newY = mainFormLocation.Y + (mainForm.Height - this.Height);
                this.Location = new Point(newX, newY);
            }
        }
    }
}
