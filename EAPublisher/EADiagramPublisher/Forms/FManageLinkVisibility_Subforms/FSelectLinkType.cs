using EADiagramPublisher.Enums;
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
    public partial class FSelectComponentLevel : Form
    {
        public FSelectComponentLevel()
        {
            InitializeComponent();
        }

        public static ExecResult<LinkType[]> Execute(LinkType[] alreadySelectedObjects)
        {
            var result = new ExecResult<LinkType[]>();
            try
            {
                var form = new FSelectComponentLevel();
                form.LoadLinkTypes();
                if (alreadySelectedObjects != null)
                    form.SelectLinkTypes(alreadySelectedObjects);


                DialogResult res = form.ShowDialog();
                if (res != DialogResult.OK)
                {
                    result.code = (int)res;
                }
                else
                {
                    List<LinkType> selectedObjects = new List<LinkType>();
                    foreach (LinkType linkType in form.clbLinkTypes.CheckedItems)
                    {
                        selectedObjects.Add(linkType);
                    }

                    result.value = selectedObjects.ToArray();
                }
            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            return result;
        }



        public void LoadLinkTypes()
        {
            clbLinkTypes.Items.Clear();

            foreach (LinkType linkType in Enum.GetValues(typeof(LinkType))) {
                clbLinkTypes.Items.Add(linkType);
            }
        }

        public void SelectLinkTypes(LinkType[] alreadySelectedObjects)
        {
            for (int i = 0; i < clbLinkTypes.Items.Count; i++)
            {

                LinkType linkType = (LinkType)clbLinkTypes.Items[i];
                if (alreadySelectedObjects.Contains(linkType))
                    clbLinkTypes.SetItemChecked(i, true);
            }
        }

        private void tsbCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbLinkTypes.Items.Count; i++)
            {
                clbLinkTypes.SetItemChecked(i, true);
            }
        }

        private void tsbClearSelection_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbLinkTypes.Items.Count; i++)
            {
                clbLinkTypes.SetItemChecked(i, false);
            }
        }

    }

}
