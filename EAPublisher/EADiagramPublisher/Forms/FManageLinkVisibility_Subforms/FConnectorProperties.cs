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
    public partial class FConnectorProperties : Form
    {
        public FConnectorProperties()
        {
            InitializeComponent();
        }

        public static void Execute(ConnectorData connectorData )
        {
            var form = new FConnectorProperties();

            EA.Connector connector = Context.EARepository.GetConnectorByID(connectorData.ConnectorID);

            form.tbID.Text = connector.ConnectorID.ToString();
            form.tbGUID.Text = connector.ConnectorGUID;
            form.tbName.Text = connector.Name;
            form.tbType.Text = connector.Type;
            form.tbClientID.Text = connector.ClientID.ToString();
            form.tbSupplierID.Text = connector.SupplierID.ToString();
            form.tbNotes.Text = connector.Notes;

            form.lvTaggedValues.Items.Clear();
            foreach(EA.ConnectorTag connectorTag in connector.TaggedValues)
            {
                ListViewItem item = new ListViewItem(connectorTag.Name);
                item.SubItems.Add(connectorTag.Value);
                form.lvTaggedValues.Items.Add(item);
            }

            form.ShowDialog();
        }


    }
}
