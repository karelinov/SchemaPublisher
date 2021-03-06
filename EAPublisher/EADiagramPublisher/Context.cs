﻿using EADiagramPublisher.Contracts;
using EADiagramPublisher.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EADiagramPublisher
{
    /// <summary>
    /// Хранит общие/кэшированные данные  для облегчения работы
    /// </summary>
    public class Context
    {
        public static ElementDesigner Designer { get; set; }
        public static LinkDesigner LinkDesigner { get; set; }


        /// <summary>
        /// Открытый плагином EA.Repository
        /// </summary>
        public static EA.Repository EARepository { get; set; }

        /// <summary>
        /// Установленная текущая рабочая диаграмма
        /// </summary>
        /// 
        private static int CurrentDiagramID = 0;
        public static EA.Diagram CurrentDiagram
        {
            get
            {
                if (CurrentDiagramID != 0)
                    return EARepository.GetDiagramByID(CurrentDiagramID);
                else
                {
                    EA.Diagram curDiagram = EARepository.GetCurrentDiagram();
                    if (curDiagram != null)
                    {
                        CurrentDiagramID = curDiagram.DiagramID;
                        return curDiagram;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
                
            set
            {
                CurrentDiagramID = value.DiagramID;
            }
        }


        /// <summary>
        /// Установленная текущая библиотека
        /// </summary>
        /// 
        private static int CurrentLibraryID = 0; // корневой пакет библиотеки
        public static EA.Package CurrentLibrary
        {
            get
            {
                if (CurrentLibraryID != 0)
                    return EARepository.GetPackageByID(CurrentLibraryID);
                else
                {
                    // если библиотеки нет, пытаемся её вычислить несколькими способами
                    EA.Package libPackage = LibraryHelper.GetLibraryRoot();
                    if (libPackage != null)
                    {
                        CurrentLibraryID = libPackage.PackageID;
                        return libPackage;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            set
            {
                CurrentLibraryID = value.PackageID;

                // Зависимые от библиотеки списки сбрасываются
                ConnectorData = null;
                ElementData = null;
                SoftwareClassification = null;
                CurLibPackageIDs = null;
            }
        }


        /// <summary>
        /// Данные коннекторов текущей библиотеки
        /// </summary>
        private static Dictionary<int, ConnectorData> _ConnectorData = null;
        public static Dictionary<int, ConnectorData> ConnectorData
        {
            get
            {
                if (_ConnectorData == null)
                {
                    _ConnectorData = ConnectorHelper.LoadConnectorData2();
                    if (_ElementData != null)
                    {
                        SyncElementConnectorData();
                    }
                }

                return _ConnectorData;
            }
            set
            {
                _ConnectorData = value;
            }
        }

        /// <summary>
        /// Данные элементов текущей библиотеки
        /// </summary>
        private static Dictionary<int, ElementData> _ElementData = null;
        public static Dictionary<int, ElementData> ElementData
        {
            get
            {
                if (_ElementData == null)
                {
                    _ElementData = EAHelper.GetCurLibElementData();
                    if (_ConnectorData != null)
                    {
                        SyncElementConnectorData();
                    }
                }


                return _ElementData;
            }
            set
            {
                _ElementData = value;
            }
        }

        /// <summary>
        /// Функция записывает в эанные ElementData идентификаторы коннекторов (на основе обработки ConnectorData)
        /// </summary>
        private static void SyncElementConnectorData()
        {
            foreach (ElementData elementData in ElementData.Values)
                elementData._ConnectorDataIDs = null;

            foreach (ConnectorData connectorData  in ConnectorData.Values)
            {
                ElementData sourceElementData;
                if (ElementData.ContainsKey(connectorData.SourceElementID))
                {
                    sourceElementData = ElementData[connectorData.SourceElementID];
                }
                else // небиблиотечный элемент
                {
                    EA.Element element = EARepository.GetElementByID(connectorData.SourceElementID);
                    sourceElementData = new ElementData(element);
                }
                if (sourceElementData._ConnectorDataIDs == null)
                    sourceElementData._ConnectorDataIDs = new List<int>();
                sourceElementData._ConnectorDataIDs.Add(connectorData.ConnectorID);


                ElementData targetElementData;
                if(ElementData.ContainsKey(connectorData.TargetElementID)) {
                    targetElementData = ElementData[connectorData.TargetElementID];
                }
                else
                {
                    EA.Element element = EARepository.GetElementByID(connectorData.TargetElementID);
                    targetElementData = new ElementData(element);
                }
                if (targetElementData._ConnectorDataIDs == null)
                    targetElementData._ConnectorDataIDs = new List<int>();
                targetElementData._ConnectorDataIDs.Add(connectorData.ConnectorID);
            }
        }



        /// <summary>
        /// Функция проверки, что текущая диаграмма установлена
        /// Могут быть установлены параметры, для авто открытия текущей или устанавливающий открытую на экране как текующую
        /// </summary>
        /// <param name="autoOpenLibDiagram"></param>/// 
        /// <param name="showUI"></param>
        /// <returns></returns>
        public static bool CheckCurrentDiagram(bool autoOpenLibDiagram = false, bool showUI = true)
        {
            bool result = false;

            EA.Diagram currentOpenedDiagram = EARepository.GetCurrentDiagram();
            EA.Diagram currentLibDiagram = Context.CurrentDiagram;

            if (currentOpenedDiagram == null && currentLibDiagram != null)
            {
                if (autoOpenLibDiagram)
                {
                    EARepository.ActivateDiagram(currentLibDiagram.DiagramID);
                    result = true;
                }
                else if (showUI)
                {
                    if (MessageBox.Show("Текущая библиотечная диаграмма не открыта. Открыть?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        EARepository.ActivateDiagram(currentLibDiagram.DiagramID);
                        result = true;
                    }
                }
            }
            else if (currentOpenedDiagram != null && currentLibDiagram != null && currentOpenedDiagram.DiagramID != currentLibDiagram.DiagramID)
            {
                if (autoOpenLibDiagram)
                {
                    Context.CurrentDiagram = currentOpenedDiagram;
                    result = true;
                }
                else if (showUI)
                {
                    DialogResult dr = MessageBox.Show("Текущая Открытая диаграмма не библиотечная библиотечная диаграмма не открыта. Назначить текущей открытую (Да) /Открыть библиотечную (Нет)? ", "", MessageBoxButtons.YesNoCancel);
                    if (dr == DialogResult.Yes)
                    {
                        Context.CurrentDiagram = currentOpenedDiagram;
                        result = true;
                    }
                }
            }
            else if (currentOpenedDiagram != null && currentLibDiagram != null && currentOpenedDiagram.DiagramID == currentLibDiagram.DiagramID)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Данные классификатора ПО текущей библиотеки
        /// </summary>
        private static DPTreeNode<ElementData> _SoftwareClassification = null;
        public static DPTreeNode<ElementData> SoftwareClassification
        {
            get
            {
                if (_SoftwareClassification == null)
                    _SoftwareClassification = SoftwareClassificationHelper.GetSoftwareClassification();

                return _SoftwareClassification;
            }
            set
            {
                _SoftwareClassification = value;
            }
        }

        /// <summary>
        /// Список ID пакетов текущей библиотеки
        /// </summary>
        private static int[] _CurLibPackageIDs = null;
        public static int[] CurLibPackageIDs
        {
            get
            {
                if (_CurLibPackageIDs == null)
                    _CurLibPackageIDs = LibraryHelper.GetCurrentLibPackageIDs();

                return _CurLibPackageIDs;
            }
            set
            {
                _CurLibPackageIDs = value;
            }
        }


    }
}
