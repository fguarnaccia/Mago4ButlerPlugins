using System;
using System.Windows.Forms;
using System.IO;

namespace MsiClassicModePlugin
{
    public partial class MSISelector  : UserControl
    {

        CCNet.UpdatesService updateservice = new CCNet.UpdatesService();
        CCNet.NightlyBuildsResult[] nightlybuilds;
       

        public MSISelector ()
        {
            InitializeComponent();
            this.PopulateListBoxWithMsiFiles(true);

        }

        static public string LocalFolderDestination { get; set; }

        public event EventHandler MsiSelected;


        internal void PopulateListBoxWithMsiFiles(bool FromWebService)
        {

            nightlybuilds = updateservice.GetNightlyBuilds();

            foreach (var nightly in nightlybuilds)

            {

                lstboxMsi.Items.Add(Path.GetFileName(nightly.FilePath));

            }


            if (lstboxMsi.Items.Count == 1)
                lstboxMsi.SelectedIndex = 0;

        }

        static public string ProvideInstanceName(string MsiFile, bool FromCCNet)
        {

            FileInfo fi = new FileInfo(MsiFile);

            string instancename = "{0}_{1}";
            string root = "";
            string buildnumb = string.Empty;

            try
            {

                if (MsiFile.IndexOf("build") < 0)
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

                plgnex.ParamName = "MsiFile";
                plgnex.ParamValue = MsiFile;
                plgnex.ToString();
                throw plgnex;


            }

            if (MsiFile.ToLowerInvariant().Contains("mago4"))
            {
                root = "M4";
            }
            if (MsiFile.ToLowerInvariant().Contains("magonet"))
            {
                root = "MN";
            }

            if (FromCCNet)
            {
                instancename = "Session-" + instancename; // 0 - build ; 1 -  root}
                return string.Format(instancename, buildnumb, root);
            }


            return string.Format(instancename, root, buildnumb);

        }

        private void lstboxMsi_DoubleClick(object sender, EventArgs e)
        {

            if (this.MsiSelected != null)
            {
                this.MsiSelected(this, e);

                this.CopyMsiFromCCnet(MsiSourcePath());
            }

        }


        public string MsiName()
        {

            return lstboxMsi.SelectedItem.ToString();
        }


        public string MsiSourcePath()
        {

            int idx = lstboxMsi.SelectedIndex;
            return nightlybuilds[idx].FilePath;

        }

        public string MsiSetupPath()
        {
            return Path.Combine(LocalFolderDestination, MsiName());
        }

        void CopyMsiFromCCnet(string CCNetSource)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (!File.Exists(Path.Combine(LocalFolderDestination, MsiName())))
            {
                File.Copy(CCNetSource, Path.Combine(LocalFolderDestination, MsiName()), true);
            }
        }

        private void lstboxMsi_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                lstboxMsi_DoubleClick(sender, null);
            }
        }
    }




}
