using System;
using System.Collections;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    public class InstanceComparer : IComparer
    {
        SortOrder sorting = SortOrder.Ascending;
        CompareOn compareOn = CompareOn.Name;

        public SortOrder Sorting
        {
            get
            {
                return sorting;
            }

            set
            {
                sorting = value;
            }
        }

        public CompareOn CompareOn
        {
            get
            {
                return compareOn;
            }

            set
            {
                compareOn = value;
            }
        }

        public int Compare(object x, object y)
        {
            if (this.sorting == SortOrder.Descending)
            {
                return CompareInternal(y, x);
            }
            return CompareInternal(x, y);
        }

        private int CompareInternal(object x, object y)
        {
            var lviX = x as ListViewItem;
            var lviY = y as ListViewItem;

            var instX = lviX.Tag as BL.Instance;
            var instY = lviY.Tag as BL.Instance;

            if (instX == null && instY == null)
            {
                return 0;
            }
            if (instX == null)
            {
                return -1;
            }
            if (instY == null)
            {
                return 1;
            }

            switch (this.compareOn)
            {
                case CompareOn.Version:
                    return instX.Version.CompareTo(instY.Version);
                case CompareOn.InstallationDate:
                    return instX.InstalledOn.CompareTo(instY.InstalledOn);
                default:
                    return string.Compare(instX.Name, instY.Name, StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }

    public enum CompareOn
    {
        None,
        Name,
        Version,
        InstallationDate
    }
}