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

                        selectedObjIDs.Add((int)item.Tag);
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

            string[] args = new string[] { Context.CurrentDiagram.DiagramGUID };
            XDocument sqlResultSet = SQLHelper.RunSQL("CurDiagramObjects.sql", args);

            Dictionary<int, ElementData> elementDataList = new Dictionary<int, ElementData>();
            IEnumerable<XElement> rowNodes = sqlResultSet.Root.XPathSelectElements("/EADATA/Dataset_0/Data/Row");
            foreach (XElement rowNode in rowNodes)
            {
                ElementData elementData;
                
                int object_id = int.Parse(rowNode.Descendants("object_id").First().Value);

                if(elementDataList.ContainsKey(object_id))
                {
                    elementData = elementDataList[object_id];
                }
                else
                {
                    elementData = new ElementData();
                    elementData._ElementID = object_id;
                    elementData.Name = rowNode.Descendants("name").First().Value;
                    elementData.EAType = rowNode.Descendants("object_type").First().Value;
                    elementData.Note = rowNode.Descendants("note").First().Value;
                    string sClassifierID = rowNode.Descendants("classifier_id").First().Value;
                    if (sClassifierID !="")
                    {
                        elementData.ClassifierID = int.Parse(rowNode.Descendants("classifier_id").First().Value);
                       elementData.ClassifierName = rowNode.Descendants("classifier_name").First().Value;
                       elementData.ClassifierEAType = rowNode.Descendants("classifier_type").First().Value;
                    }

                    elementDataList.Add(object_id,elementData);
                }

                string tagName = rowNode.Descendants("property").First().Value;
                string tagValue = rowNode.Descendants("value").First().Value;

                if(tagName == DAConst.DP_LibraryTag)
                    elementData.IsLibrary = true;
                if (tagName == DAConst.DP_ComponentLevelTag)
                    elementData.ComponentLevel = Enum.Parse(typeof(ComponentLevel),tagValue) as ComponentLevel?;
                if (tagName == DAConst.DP_NodeGroupsTag)
                    elementData.NodeGroups = tagValue.Split(',');


            }

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
                int curElementID = (int)item.Tag;

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
    }
}
