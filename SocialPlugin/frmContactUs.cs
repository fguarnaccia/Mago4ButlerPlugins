using System;
using System.DirectoryServices;
using System.Linq;
using System.Windows.Forms;


namespace SocialPlugin
{
    public partial class frmContactUs : Form
    {

        public event EventHandler ContactUsOK;
        string Domain = Environment.UserDomainName;
        string User = Environment.UserName;
        //public string Ticket { get; set; }

    //    public frmContactUs()
    //    {
    //        InitializeComponent();
    //        txtFrom.Text = RetrieveEmailAddress(Domain, User);
            
      
    //}
        public frmContactUs(string Ticket) 
        {

            InitializeComponent();
            txtFrom.Text = RetrieveEmailAddress(Domain, User);
            txtSubject.Text = Ticket;
           

        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            this.ContactUsOK(this, e);
            MessageBox.Show("Message has been sent successfully!", "Social",MessageBoxButtons.OK,MessageBoxIcon.Information);
            this.Close();
            this.Dispose();
               
        }


        public string RetrieveEmailAddress(string domain, string userName)
        {
            if (
                domain == null || domain.Trim().Length == 0 ||
                userName == null || userName.Trim().Length == 0
                )
                return String.Empty;

            string rootQuery = String.Format(
                @"LDAP://{0}.it/DC={0},DC=it",
                domain
                );
            string searchFilter = String.Format(
                @"(&(samAccountName={0})(objectCategory=person)(objectClass=user))",
                userName
                );

            SearchResult result = null;
            using (DirectoryEntry root = new DirectoryEntry(rootQuery))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(root))
                {
                    searcher.Filter = searchFilter;
                    SearchResultCollection results = searcher.FindAll();

                    result = (results.Count != 0) ? results[0] : null;
                }
            }
            if (result == null)
                return String.Empty;

            string email = String.Empty;
            try
            {
                foreach (string proxyAddr in result.Properties["proxyaddresses"])
                {
                    if (proxyAddr.ToLowerInvariant().StartsWith("smtp:"))
                    {
                        email = proxyAddr.Substring(5);
                        break;
                    }
                }
            }
            //Do nothing for any errors, make the collection null
            catch
            { }

            return email;
        }
    }
}
