using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EADiagramPublisher
{
    class Context
    {
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
            }
        }

    }



}
