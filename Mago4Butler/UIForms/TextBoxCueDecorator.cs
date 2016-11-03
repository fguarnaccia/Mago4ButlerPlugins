using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    class TextBoxCueDecorator
    {
        TextBox textbox;
        public string CueMessage { get; set; }

        public TextBoxCueDecorator(TextBox textbox)
        {
            this.textbox = textbox;
            this.textbox.HandleCreated += Textbox_HandleCreated;
            this.textbox.Disposed += Textbox_Disposed;
        }

        private void Textbox_Disposed(object sender, EventArgs e)
        {
            this.textbox.HandleCreated -= Textbox_HandleCreated;
            this.textbox.Disposed -= Textbox_Disposed;
        }

        private void Textbox_HandleCreated(object sender, EventArgs e)
        {
            if (this.textbox.IsHandleCreated && CueMessage != null)
            {
                SafeNativeMethods.SendMessage(this.textbox.Handle, SafeNativeMethods.EM_SETCUEBANNER, (IntPtr)1, CueMessage);
            }
        }
    }
}
