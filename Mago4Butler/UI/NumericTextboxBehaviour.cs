using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    class NumericTextboxBehaviour
    {
        const char backspace = (char)8;
        TextBox textbox;

        public NumericTextboxBehaviour(TextBox textbox)
        {
            this.textbox = textbox;
            this.textbox.Disposed += Textbox_Disposed;
            this.textbox.KeyPress += Textbox_KeyPress;
        }

        private void Textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != backspace && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Textbox_Disposed(object sender, EventArgs e)
        {
            this.textbox.Disposed -= Textbox_Disposed;
            this.textbox.KeyPress -= Textbox_KeyPress;
        }
    }
}
