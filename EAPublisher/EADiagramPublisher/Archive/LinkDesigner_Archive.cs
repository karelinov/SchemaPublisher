using EADiagramPublisher.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EADiagramPublisher.Archive
{
    class LinkDesigner_Archive
    {
        public ExecResult<Boolean> SetConnectorVisibility()
        {
            ExecResult<Boolean> result = new ExecResult<bool>();

            Logger.Out("");

            try
            {

                if (!Context.CheckCurrentDiagram())
                    throw new Exception("Не установлена или не открыта текущая диаграмма");

                // запускаем форму
                ExecResult<LinkVisibilityData> selectLVResult = FSetLinkVisibility.Execute();
                if (selectLVResult.code != 0) return result;

                // Обрабатываем результаты
                foreach (var curLinkType in selectLVResult.value.showLinkType)
                {
                   Context.LinkDesigner.SetConnectorVisibility(curLinkType, true);
                }

                foreach (var curLinkType in selectLVResult.value.hideLinkType)
                {
                    Context.LinkDesigner.SetConnectorVisibility(curLinkType, false);
                }

                Context.LinkDesigner.SetConnectorVisibility_Untyped(selectLVResult.value.showNotLibElements);

            }
            catch (Exception ex)
            {
                result.setException(ex);
            }

            Context.CurrentDiagram.DiagramLinks.Refresh();
            Context.EARepository.ReloadDiagram(Context.CurrentDiagram.DiagramID);

            return result;
        }
    }
}
