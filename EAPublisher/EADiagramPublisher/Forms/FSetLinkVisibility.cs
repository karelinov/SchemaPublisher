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
    public partial class FSetLinkVisibility : Form
    {
        public FSetLinkVisibility()
        {
            InitializeComponent();

            this.clbShowLinkType.Items.Clear();
            foreach (LinkType linkType in (LinkType[])Enum.GetValues(typeof(LinkType)))
            {
                this.clbShowLinkType.Items.Add(linkType, false);
            }


            this.clbHideLinkType.Items.Clear();
            foreach (LinkType linkType in (LinkType[])Enum.GetValues(typeof(LinkType)))
            {
                this.clbHideLinkType.Items.Add(linkType, false);
            }


        }

        public static ExecResult<LinkVisibilityData> Execute()
        {
            ExecResult<LinkVisibilityData> result = new ExecResult<LinkVisibilityData>() { value = new LinkVisibilityData() };
            try
            {
                var form = new FSetLinkVisibility();

                DialogResult res = form.ShowDialog();
                if (res == DialogResult.OK)
                {
                    result.code = 0;

                    foreach (var checkedItem in form.clbShowLinkType.CheckedItems)
                    {
                        result.value.showLinkType.Add((LinkType)checkedItem);
                    }
                    result.value.showNotLibElements = form.cbShowNotLibConnectors.Checked;

                    foreach (var checkedItem in form.clbHideLinkType.CheckedItems)
                    {
                        result.value.hideLinkType.Add((LinkType)checkedItem);
                    }
                    result.value.hideTempDiagramLinks = form.cbHideTempDiagramLinks.Checked;
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

    /// <summary>
    /// Класс с выбранными опциями показа линков
    /// </summary>
    public class LinkVisibilityData
    {
        public List<LinkType> showLinkType = new List<LinkType>(); // список типов линков , которые надо показать
        public bool showNotLibElements = false; // показывать небиблиотечные линки
        public bool showOnlyForSelected = false; // показывать только для выделенных элементов
        public List<String> showComponents = new List<string>(); // показывать соединения указанных компонентов 

        public List<LinkType> hideLinkType = new List<LinkType>(); // список типов линков , которые надо скрыть
        public bool hideTempDiagramLinks = false; // скрывать временые линки других диаграмм
    }


}
