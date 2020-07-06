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
    public partial class FSelectHierarcyLevels : Form
    {
        public FSelectHierarcyLevels()
        {
            InitializeComponent();

            this.clbHierarchyLevels.Items.Clear();
            this.clbHierarchyLevels.Items.Add(ComponentLevel.SystemContour, true);
            this.clbHierarchyLevels.Items.Add(ComponentLevel.SystemComponent, true);
            this.clbHierarchyLevels.Items.Add(ComponentLevel.ContourContour, true);
            this.clbHierarchyLevels.Items.Add(ComponentLevel.ContourComponent, true);
            this.clbHierarchyLevels.Items.Add(ComponentLevel.Node, true);
            this.clbHierarchyLevels.Items.Add(ComponentLevel.Device, true);
            this.clbHierarchyLevels.Items.Add(ComponentLevel.ExecutionEnv, true);
            this.clbHierarchyLevels.Items.Add(ComponentLevel.Component, true);
        }

        public ExecResult<List<ComponentLevel>> Execute()
        {
            ExecResult<List<ComponentLevel>> result = new ExecResult<List<ComponentLevel>>();
            try
            {
                DialogResult res = this.ShowDialog();
                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    result.value = new List<ComponentLevel>();
                    foreach (object checkedListObject in clbHierarchyLevels.CheckedItems)
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
            if (! new DialogResult[]{ DialogResult.Cancel, DialogResult.OK}.Contains(this.DialogResult))
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }

}
