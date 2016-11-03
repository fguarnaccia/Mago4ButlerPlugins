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
    public partial class UIError : UserControl
    {
        SynchronizationContext syncCtx;
        public event EventHandler<EventArgs> Back;
        protected virtual void OnBack(EventArgs e)
        {
            var handler = Back;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public UIError()
        {
            this.syncCtx = SynchronizationContext.Current;
            if (this.syncCtx == null)
            {
                this.syncCtx = new WindowsFormsSynchronizationContext();
            }
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.OnBack(e);
        }

        public void SetErrorMessage(string errorMessage)
        {
            this.syncCtx.Post(new SendOrPostCallback((obj) => this.txtError.Text = errorMessage), null);
        }
    }
}
