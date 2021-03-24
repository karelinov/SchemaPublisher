using EADiagramPublisher.Contracts;
using EADiagramPublisher.Enums;
using EADiagramPublisher.SQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EADiagramPublisher.Forms
{

    public partial class FSelectDiagramObjects : Form
    {
        private ListViewComparer listViewComparer;


        public FSelectDiagramObjects()
        {
            InitializeComponent();

            listViewComparer = new ListViewComparer();
            this.lvDiagramObjects.ListViewItemSorter = listViewComparer;
        }

        public static ExecResult<int[]> Execute(int[] alreadySelectedObjects)
        {
            var result = new ExecResult<int[]>();
            try
            {
                var form = new FSelectDiagramObjects();
                form.LoadDiagramObjects();
                if (alreadySelectedObjects != null)
                    form.SelectObjects(alreadySelectedObjects);


                DialogResult res = form.ShowDialog();
                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    List<int> selectedObjIDs = new List<int>();
                    foreach (ListViewItem item in form.lvDiagramObjects.SelectedItems)
                    {

                        selectedObjIDs.Add(((ElementData)item.Tag)._ElementID);
                    }

                    result.value = selectedObjIDs.ToArray();
                }
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        /// <summary>
        /// Загрузка на экран списка элементов на текущей диаграмме
        /// </summary>
        private void LoadDiagramObjects()
        {
            lvDiagramObjects.Items.Clear();

            // получаем список объектов ElementData для текущей диаграммы
            Dictionary<int, ElementData> elementDataList = EAHelper.GetCurDiagramElementData();

            foreach (ElementData elementData in elementDataList.Values)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = elementData;
                item.Text = elementData._ElementID.ToString();
                item.SubItems.Add(elementData.DisplayName );
                item.SubItems.Add(elementData.EAType);
                if (elementData.ComponentLevel != null)
                {
                    item.SubItems.Add(elementData.ComponentLevel.ToString());
                }
                else
                {
                    item.SubItems.Add("");
                }
                item.SubItems.Add(elementData.Note);

                lvDiagramObjects.Items.Add(item);
            }

        }

        /// <summary>
        /// Функция выделяет перечисленные в списке объекты
        /// </summary>
        /// <param name="alreadySelectedObjects"></param>
        private void SelectObjects(int[] alreadySelectedObjects)
        {
            foreach (ListViewItem item in lvDiagramObjects.Items)
            {
                int curElementID = ((ElementData)item.Tag)._ElementID;

                if(alreadySelectedObjects.Contains(curElementID))
                {
                    item.Selected = true;
                }

            }
        }

        private void lvDiagramObjects_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == listViewComparer.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (listViewComparer.Order == SortOrder.Ascending)
                {
                    listViewComparer.Order = SortOrder.Descending;
                }
                else
                {
                    listViewComparer.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                listViewComparer.SortColumn = e.Column;
                listViewComparer.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvDiagramObjects.Sort();
        }

        private void tsbSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void tsbClearSelection_Click(object sender, EventArgs e)
        {
            lvDiagramObjects.SelectedItems.Clear();
        }
    }
}
