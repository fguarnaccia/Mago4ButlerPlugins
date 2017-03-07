using Microarea.Mago4Butler.Model;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    public class ListViewSortManager : IDisposable
    {
        ListView listView;
        readonly InstanceComparer instanceComparer = new InstanceComparer();
        ISettings settings;

        public ListViewSortManager(ListView listView, ISettings settings)
        {
            this.settings = settings;

            this.listView = listView;
            listView.ColumnClick += ListView_ColumnClick;
            listView.ListViewItemSorter = instanceComparer;
        }

        public void SortByColumn(int columnId)
        {
            ListView_ColumnClick(
                this,
                new ColumnClickEventArgs(columnId)
                );
            this.instanceComparer.Sorting = (SortOrder)this.settings.ListViewSortOrder;
        }

        private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.instanceComparer.Sorting =
                (this.instanceComparer.Sorting == SortOrder.Ascending)
                ? SortOrder.Descending
                : SortOrder.Ascending;

            this.settings.ListViewSortOrder = (int)this.instanceComparer.Sorting;

            switch (e.Column)
            {
                case 1:
                    this.instanceComparer.CompareOn = CompareOn.Version;
                    this.settings.LastColumnSortedIndex = 1;
                    break;
                case 2:
                    this.instanceComparer.CompareOn = CompareOn.InstallationDate;
                    this.settings.LastColumnSortedIndex = 2;
                    break;
                default:
                    this.instanceComparer.CompareOn = CompareOn.Name;
                    this.settings.LastColumnSortedIndex = 0;
                    break;
            }
            this.settings.Save();

            this.listView.SetSortIcon(e.Column, this.instanceComparer.Sorting);
            this.listView.Sort();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool managed)
        {
            if (managed)
            {
                if (this.listView != null)
                {
                    listView.ColumnClick -= ListView_ColumnClick;
                    listView.ListViewItemSorter = null;
                }
            }
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class ListViewExtensions
    {
        public static void SetSortIcon(this ListView listViewControl, int columnIndex, SortOrder order)
        {
            IntPtr columnHeader = SafeNativeMethods.SendMessage(listViewControl.Handle, SafeNativeMethods.LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
            for (int columnNumber = 0; columnNumber <= listViewControl.Columns.Count - 1; columnNumber++)
            {
                var columnPtr = new IntPtr(columnNumber);
                var item = new SafeNativeMethods.HDITEM
                {
                    mask = SafeNativeMethods.HDITEM.Mask.Format
                };

                if (SafeNativeMethods.SendMessage(columnHeader, SafeNativeMethods.HDM_GETITEM, columnPtr, ref item) == IntPtr.Zero)
                {
                    return;
                }

                if (order != SortOrder.None && columnNumber == columnIndex)
                {
                    switch (order)
                    {
                        case SortOrder.Ascending:
                            item.fmt &= ~SafeNativeMethods.HDITEM.Format.SortDown;
                            item.fmt |= SafeNativeMethods.HDITEM.Format.SortUp;
                            break;
                        case SortOrder.Descending:
                            item.fmt &= ~SafeNativeMethods.HDITEM.Format.SortUp;
                            item.fmt |= SafeNativeMethods.HDITEM.Format.SortDown;
                            break;
                    }
                }
                else
                {
                    item.fmt &= ~SafeNativeMethods.HDITEM.Format.SortDown & ~SafeNativeMethods.HDITEM.Format.SortUp;
                }

                SafeNativeMethods.SendMessage(columnHeader, SafeNativeMethods.HDM_SETITEM, columnPtr, ref item);
            }
        }
    }
}