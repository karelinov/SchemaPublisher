using System;

using EADiagramPublisher.Enums;

namespace EADiagramPublisher
{
    public class EATVHelper
    {
        /// <summary>
        /// Shortcut для глобальной переменной EA.Repository
        /// </summary>
        private static EA.Repository EARepository
        {
            get
            {
                return Context.EARepository;
            }
        }

        public static EA.Collection GetTaggedValues(EA.Element eaElement)
        {
            // Для обычных элементов возвращаем их TaggedValuesEx
            if (eaElement.Type != "Boundary")
                return eaElement.TaggedValuesEx;

            // Для рамочек ищем связанный с рамочкой объект DeploymentSpecification и возвращаем его TaggedValuesEx
            EA.Collection result = null;
            foreach (EA.Connector connector in eaElement.Connectors)
            {
                if (connector.Type == "Dependency" && connector.Stereotype == "")
                {
                    EA.Element dependedElement = EARepository.GetElementByID(connector.ClientID);
                    if (dependedElement.Type == "DeploymentSpecification")
                    {
                        return dependedElement.TaggedValuesEx;
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает в виде строки значение запрошенного TaggedValue из объекта
        /// Если такого TaggedValue нет - возвращается пустая строка
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="taggedValueName"></param>
        /// <returns></returns>
        public static string GetTaggedValue(EA.Connector connector, string taggedValueName)
        {
            EA.ConnectorTag taggedValue = null;
            try
            {
                taggedValue = connector.TaggedValues.GetByName(taggedValueName);
            }
            catch { };

            if (taggedValue == null)
                return "";
            else
                return taggedValue.Value;
        }
        public static string GetTaggedValue(EA.Element element, string taggedValueName, bool useEx = true)
        {
            EA.Collection elementTags;
            if (useEx)
                elementTags = element.TaggedValuesEx;
            else
                elementTags = element.TaggedValues;

            EA.TaggedValue taggedValue = elementTags.GetByName(taggedValueName);
            if (taggedValue == null)
                return "";
            else
                return taggedValue.Value;
        }

        public static void TaggedValueSet(EA.Element element, string tagName, string tagValue)
        {
            EA.TaggedValue tag = element.TaggedValues.GetByName(tagName);
            if (tag == null)
            {
                tag = element.TaggedValues.AddNew(tagName, tagValue);
            }
            else
            {
                tag.Value = tagValue;
            }

            tag.Update();
            //element.Update(); // is it needed?

        }

        public static void TaggedValueSet(EA.Connector connector, string tagName, string tagValue)
        {
            EA.ConnectorTag tag = null;
            try
            {
                tag = connector.TaggedValues.GetByName(tagName);
            }
            catch (Exception ex)
            {
                if (ex.Message != "Index out of bounds") throw ex;
            }

            if (tag == null)
                tag = connector.TaggedValues.AddNew(tagName, tagValue);

            tag.Value = tagValue;
            bool res = tag.Update();
            //connector.Update(); // is it needed?

        }

        /// <summary>
        /// Удаляет TaggedValue из элемента
        /// </summary>
        /// <param name="element"></param>
        /// <param name="tagName"></param>
        public static void TaggedValueRemove(EA.Element element, string tagName)
        {
            for (short i = 0; i < element.TaggedValues.Count; i++)
            {
                EA.TaggedValue tag = element.TaggedValues.GetAt(i);
                if (tag.Name == tagName)
                {
                    element.TaggedValues.DeleteAt(i, true);
                }
            }
        }
        /// <summary>
        /// Удаляет TaggedValue из элемента
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="tagName"></param>
        public static void TaggedValueRemove(EA.Connector connector, string tagName)
        {
            for (short i = 0; i < connector.TaggedValues.Count; i++)
            {
                EA.ConnectorTag tag = connector.TaggedValues.GetAt(i);
                if (tag.Name == tagName)
                {
                    connector.TaggedValues.DeleteAt(i, true);
                }
            }
        }

        /// <summary>
        /// Функция записывает в выделенные элементы признак библиотечного
        /// </summary>
        /// <returns></returns>
        public static ExecResult<Boolean> SetDPLibratyTag(string location)
        {
            ExecResult<Boolean> result = new ExecResult<bool>();

            try
            {
                switch (location)
                {
                    case "Diagram":
                        if (Context.CurrentDiagram != null)
                        {
                            if (Context.CurrentDiagram.SelectedObjects.Count > 0)
                            {
                                foreach (EA.DiagramObject curDA in Context.CurrentDiagram.SelectedObjects)
                                {
                                    EA.Element curElement = EARepository.GetElementByID(curDA.ElementID);
                                    TaggedValueSet(curElement, DAConst.DP_LibraryTag, "");
                                }
                            }
                            else if (Context.CurrentDiagram.SelectedConnector != null)
                            {
                                TaggedValueSet(Context.CurrentDiagram.SelectedConnector, DAConst.DP_LibraryTag, "");
                            }
                        }
                        break;
                    case "TreeView":
                        foreach (EA.Element curElement in EARepository.GetTreeSelectedElements())
                        {
                            TaggedValueSet(curElement, DAConst.DP_LibraryTag, "");
                        }
                        break;

                    case "MainMenu":
                        if (Context.CurrentDiagram != null)
                        {
                            if (Context.CurrentDiagram.SelectedObjects.Count > 0)
                            {

                                foreach (EA.DiagramObject curDA in Context.CurrentDiagram.SelectedObjects)
                                {
                                    EA.Element curElement = EARepository.GetElementByID(curDA.ElementID);
                                    TaggedValueSet(curElement, DAConst.DP_LibraryTag, "");
                                }
                            }
                            else if (Context.CurrentDiagram.SelectedConnector != null)
                            {
                                TaggedValueSet(Context.CurrentDiagram.SelectedConnector, DAConst.DP_LibraryTag, "");
                            }
                        }
                        else
                        {
                            foreach (EA.Element curElement in EARepository.GetTreeSelectedElements())
                            {
                                TaggedValueSet(curElement, DAConst.DP_LibraryTag, "");
                            }
                            break;
                        }



                        break;
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
