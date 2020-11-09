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

    public partial class FSelectObject : Form
    {
        private ListViewComparer listViewComparer;


        public FSelectObject()
        {
            InitializeComponent();

            listViewComparer = new ListViewComparer();
            this.lvObjects.ListViewItemSorter = listViewComparer;
        }

        public static ExecResult<ElementData> Execute()
        {
            var result = new ExecResult<ElementData>();
            try
            {
                var form = new FSelectObject();
                form.LoadObjects();

                DialogResult res = form.ShowDialog();
                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    if (form.lvObjects.SelectedItems.Count > 0)
                    {
                        result.value = (ElementData)(form.lvObjects.SelectedItems[0]).Tag;
                    }
                    else
                    {
                        result.code = -1;
                        result.message = "Не выбран объект";
                    }
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
        private void LoadObjects()
        {
            lvObjects.Items.Clear();

            foreach (ElementData elementData in Context.ElementData.Values)
            {
                if (!AllowByFilter(elementData)) continue;

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

                lvObjects.Items.Add(item);
            }

        }

        /// <summary>
        /// Функция выделяет перечисленные в списке объекты
        /// </summary>
        /// <param name="alreadySelectedObjects"></param>
        private void SelectObjects(int[] alreadySelectedObjects)
        {
            foreach (ListViewItem item in lvObjects.Items)
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
            this.lvObjects.Sort();
        }

        private void tsbSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void tsbClearSelection_Click(object sender, EventArgs e)
        {
            lvObjects.SelectedItems.Clear();
        }

        /// <summary>
        /// Функция проверяет, разрешён ли показ коннекторат текущим фильтром
        /// </summary>
        private bool AllowByFilter(ElementData elementData)
        {
            bool result = true;

            if (tstbTextFilter.Text != "")
            {
                if (!elementData.ID.ToString().Contains(tstbTextFilter.Text)
                    && !elementData.Name.Contains(tstbTextFilter.Text)
                    && !elementData.EAType.Contains(tstbTextFilter.Text)
                    && !elementData.ComponentLevel.ToString().Contains(tstbTextFilter.Text)
                    )
                    return false;
            }

            return result;
        }

        private void tstbTextFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                LoadObjects();
            }
        }

        private void FSelectObject_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
