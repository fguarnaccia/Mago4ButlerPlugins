using System;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Microarea.Mago4Butler.Plugins;
using System.Drawing;

namespace MsiClassicModePlugin
{
    public partial class MSISelector : UserControl
    {

        internal MABuilds.UpdatesService updateservice = new MABuilds.UpdatesService();


        public MSISelector()
        {
            InitializeComponent();

        }

        internal string SelectedMsiFileName { get; set; }
        public string SelectedMsiFilePath { get; set; }
        public string SelectedInstanceName { get; set; }
        internal bool SessionTest { get; set; }

        static internal string LocalFolderDestination { get; set; }

        public event EventHandler MsiSelected;

        internal  string ProvideInstanceName()
        {

            FileInfo fi = new FileInfo(SelectedMsiFileName);

            string instancename = "{0}_{1}";
            string root = "";
            string buildnumb = string.Empty;

            try
            {

                if (SelectedMsiFileName.IndexOf("build") < 0)
                {
                    buildnumb = (fi.Name).Substring((fi.Name).IndexOf("x.") + 2, 4);
                }
                else
                {

                    buildnumb = (fi.Name).Substring((fi.Name).IndexOf("build") + 5, 4);
                }

            }
            catch (ArgumentOutOfRangeException e)
            {

                PluginException plgnex = new PluginException("", e);

                plgnex.ParamName = "SelectedMsiFileName";
                plgnex.ParamValue = SelectedMsiFileName;
                plgnex.ToString();
                throw plgnex;

            }

            if (SelectedMsiFileName.ToLowerInvariant().Contains("mago4"))
            {
                root = "M4";
            }
            if (SelectedMsiFileName.ToLowerInvariant().Contains("magonet"))
            {
                root = "MN";
            }

            if (SessionTest)
                
            {
                instancename = "Session-" + instancename; // 0 - build ; 1 -  root}
                return string.Format(instancename, buildnumb, root);
            }


            return string.Format(instancename, root, buildnumb);

        }


        public virtual void DrawSelector()
        {
            BackColor = System.Drawing.Color.White;
            Location = new System.Drawing.Point(50, 60);
            Size = new System.Drawing.Size(365, 218);
            TabIndex = 3;
        }

        internal static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private void lstbox_DoubleClick(object sender, EventArgs e)
        {

            SelectedMsiFileName = ((ListBox)(sender)).SelectedItem.ToString();
            
            if (this.MsiSelected != null)
            {
                this.MsiSelected(this, e);
            }
        }
        private void lstbox_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode != Keys.Return) return;
            var selecteditem = ((ListBox)(sender)).SelectedItem;
            if (selecteditem == null) return;
         
                SelectedMsiFileName = selecteditem.ToString();
            if (this.MsiSelected != null)
            {
                this.MsiSelected(this, e);
            }
        }


        internal static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        private void lstbox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Graphics g = e.Graphics;
            SolidBrush color;

            //g.FillRectangle(new SolidBrush(Color.Beige), e.Bounds);

            ListBox lb = (ListBox)sender;

            if (lb.Items[e.Index].ToString().ToLower().Contains("hf"))
            {
                color = new SolidBrush(Color.DarkOrange);
            }
            else
            {
                color = new SolidBrush(Color.OliveDrab);
            }


            g.DrawString(lb.Items[e.Index].ToString(), e.Font, color, new PointF(e.Bounds.X, e.Bounds.Y));

            //e.DrawFocusRectangle();
        }
    }
    // ---------------- ****** --------------------------------
    public class SelectorFromCCNet : MSISelector
    {
        internal MABuilds.NightlyBuildsResult[] nightlybuilds;
        public SelectorFromCCNet()
        {
            SessionTest = true;

            base.MsiSelected += SelectorFromCCNet_MsiSelected;
            this.PopulateListBoxWithNightlytMsi(true);
        }


        private void SelectorFromCCNet_MsiSelected(object sender, EventArgs e)
        {

            SelectedMsiFilePath = Path.Combine(LocalFolderDestination, SelectedMsiFileName);
            SelectedInstanceName = ProvideInstanceName();
            this.CopyMsiFromCCnet(MsiSourcePath());
        }

   

        internal void PopulateListBoxWithNightlytMsi(bool FromWebService)
        {

            nightlybuilds = updateservice.GetNightlyBuilds();

            foreach (var nightly in nightlybuilds)

            {

                lstboxMain.Items.Add(UppercaseFirst((Path.GetFileName(nightly.FilePath).ToLower())));

            }


            if (lstboxMain.Items.Count == 1)
                lstboxMain.SelectedIndex = 0;

        }




        string MsiSourcePath()
        {
            int idx = lstboxMain.SelectedIndex;
            return nightlybuilds[idx].FilePath;
        }


        void CopyMsiFromCCnet(string CCNetSource)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (!File.Exists(Path.Combine(LocalFolderDestination, SelectedMsiFileName)))
            {
                File.Copy(CCNetSource, Path.Combine(LocalFolderDestination, SelectedMsiFileName), true);
            }
        }
        public override void DrawSelector()

        {
            base.DrawSelector();
            tabMain.Text = "";
            
            tabControlMsi.Controls.Remove(tabAux);
        }

    }

    // ---------------- ****** --------------------------------
    public class SelectorFromSite : MSISelector
    {
        MABuilds.GetUpdatesResponse[] officialmago4builds;
        MABuilds.GetUpdatesResponse[] officialmagonetbuilds;
        MABuilds.GetUpdatesRequest updaterequest = new MABuilds.GetUpdatesRequest();

        public SelectorFromSite()
        {
  

        SessionTest = false;
            base.MsiSelected += SelectorFromSiteM4_MsiSelected;
            //base.KeyUp += SelectorFromSiteM4_KeyUp;

            base.MsiSelected += SelectorFromSiteMN_MsiSelected;
            base.KeyUp += SelectorFromSiteMN_KeyUp;

            PopulateListBoxWithOfficialMago4();
            PopulateListBoxWithOfficialMagonet();
        }

        private void SelectorFromSiteMN_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.SelectorFromSiteMN_MsiSelected(sender, null);
            }

        }

        private void SelectorFromSiteMN_MsiSelected(object sender, EventArgs e)
        {
  
            SelectedMsiFilePath = Path.Combine(LocalFolderDestination, SelectedMsiFileName);
            SelectedInstanceName = ProvideInstanceName();

            DownloadMsiFromSite(MsiURINet());

        }

        private void SelectorFromSiteM4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.SelectorFromSiteM4_MsiSelected(sender, null);
            }
            
        }

        private void SelectorFromSiteM4_MsiSelected(object sender, EventArgs e)
        {
            SelectedMsiFilePath = Path.Combine(LocalFolderDestination, SelectedMsiFileName);
            SelectedInstanceName = ProvideInstanceName();
            var tmp = MsiURI4();
            if (tmp != null)
            DownloadMsiFromSite(tmp);
           
        }

        void PopulateListBoxWithOfficialMago4()
        {

            updaterequest.ProductSignature = MsiClassicMode.ProductSignature.M4GO.ToString();

            officialmago4builds = updateservice.GetOfficialBuilds(updaterequest);

            foreach (var official in officialmago4builds)

            {
                lstboxMain.Items.Add(UppercaseFirst(official.MsiFileName.ToLower()));

            }

            if (lstboxMain.Items.Count == 1)
                lstboxMain.SelectedIndex = 0;
        }

        void PopulateListBoxWithOfficialMagonet()
        {

            updaterequest.ProductSignature = GetEnumDescription(MsiClassicMode.ProductSignature.MagoNetPro);

            officialmagonetbuilds = updateservice.GetOfficialBuilds(updaterequest);

            foreach (var official in officialmagonetbuilds)

            {

                lstboxAux.Items.Add(UppercaseFirst(official.MsiFileName.ToLower()));

                lstboxAux.ForeColor = System.Drawing.Color.Magenta;
            }

            if (lstboxAux.Items.Count == 1)
                lstboxAux.SelectedIndex = 0;

        }

        public string MsiURI4()
        {
            int idx = lstboxMain.SelectedIndex;
            if (idx < 0 || idx > officialmago4builds.Length) return null;

            return officialmago4builds[idx].DownloadUri;

        }
        public string MsiURINet()
        {
            int idx = lstboxAux.SelectedIndex;
            if (idx < 0 || idx > officialmagonetbuilds.Length) return null;
            return officialmagonetbuilds[idx].DownloadUri;
        }

 
        void DownloadMsiFromSite(string MsiUri)
        {
            Cursor.Current = Cursors.WaitCursor;
                    
            if (!File.Exists(SelectedMsiFilePath))
            {
                App.Instance.DownloadMsi(MsiUri, SelectedMsiFilePath);

            }
        }



    }



}