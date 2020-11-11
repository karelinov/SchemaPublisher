using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EADiagramPublisher.Enums;


namespace EADiagramPublisher.Forms
{
    public partial class FSelectComponentLevels : Form
    {
        public FSelectComponentLevels()
        {
            InitializeComponent();

        }

        public static ExecResult<List<ComponentLevel>> Execute(List<ComponentLevel> defaultSelected = null)
        {
            ExecResult<List<ComponentLevel>> result = new ExecResult<List<ComponentLevel>>();
            try
            {
                var form = new FSelectComponentLevels();

                // заполняем и отмечаем список элементов
                form.clbHierarchyLevels.Items.Clear();
                foreach (ComponentLevel componentLevel in Enum.GetValues(typeof(ComponentLevel))) {
                    bool componentLevelChecked = defaultSelected !=null? defaultSelected.Contains(componentLevel): true;
                    form.clbHierarchyLevels.Items.Add(componentLevel, componentLevelChecked);
                }

                DialogResult res = form.ShowDialog();
                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    result.value = new List<ComponentLevel>();
                    foreach (object checkedListObject in form.clbHierarchyLevels.CheckedItems)
                    {
                        result.value.Add((ComponentLevel)checkedListObject);
                    }
                }

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }

        private void FSelectHierarcyLevels_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!new DialogResult[] { DialogResult.Cancel, DialogResult.OK }.Contains(this.DialogResult))
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void tsbCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbHierarchyLevels.Items.Count; i++)
            {
                clbHierarchyLevels.SetItemChecked(i, true);
            }
        }

        private void tsbClearSelection_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbHierarchyLevels.Items.Count; i++)
            {
                clbHierarchyLevels.SetItemChecked(i, false);
            }
        }
    }

}
