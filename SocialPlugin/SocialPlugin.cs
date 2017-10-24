using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microarea.Mago4Butler.Plugins;
using System.Windows.Forms;

namespace SocialPlugin
{
    public class SocialPlugin : Mago4ButlerPlugin
    {
        frmContactUs contactus ;

        Slack.PostSlackRequest payload = new Slack.PostSlackRequest();
        //Slack.SlackServiceSoapClient slack = new Slack.SlackServiceSoapClient(); necessario se il WS viene referenziato come Service e non come web 
        Slack.SlackService slack = new Slack.SlackService();
      
        public void CmdContactUS()
        {
            contactus = new frmContactUs(slack.TichetNumber());
            contactus.ContactUsOK += Contactus_ContactUsOK;
            contactus.ShowDialog();
            
        }

        private void Contactus_ContactUsOK(object sender, EventArgs e)
        {     

            payload.Username = contactus.txtFrom.Text;
            payload.Subject = contactus.txtSubject.Text;
            payload.Text  = contactus.rchtxtBody.Text;

            slack.SendSlackRequest(payload);
        }

        public override IEnumerable<ToolstripMenuItem> GetToolstripMenuItems()
        {
            ToolstripMenuItem menu = new ToolstripMenuItem();

            menu.Text = "&Contact us";
            menu.Name = "mnuSocialContactUs";
            menu.Command = () => CmdContactUS();

            return new List<ToolstripMenuItem>() { menu };
        }
    }
}
