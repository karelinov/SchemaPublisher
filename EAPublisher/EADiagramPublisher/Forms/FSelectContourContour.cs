using EADiagramPublisher.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EADiagramPublisher.Forms
{
    public partial class FSelectContourContour : Form
    {
        public FSelectContourContour()
        {
            InitializeComponent();
        }

        public ExecResult<List<NodeData>> Execute(List<NodeData> nodeDataList)
        {
            ExecResult<List<NodeData>> result = new ExecResult<List<NodeData>>(){ value = new List<NodeData>()};
            try
            {
                // Заливаем список 'элементов в список
                clbContours.Items.Clear();

                foreach (var nodeData in nodeDataList)
                {

                    // Строим элемент
                    clbContours.Items.Add(nodeData);
                }

                DialogResult res = this.ShowDialog();
                if (res == DialogResult.OK)
                {
                    result.code = 0;

                    foreach(var item in clbContours.CheckedItems)
                    {
                        result.value.Add((NodeData)item);
                    }

                }
                else
                {
                    result.code = (int)DialogResult.Cancel;
                }

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }


    }
}
