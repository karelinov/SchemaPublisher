using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EADiagramPublisher.Contracts;

namespace EADiagramPublisher
{

    /// <summary>
    /// Хэлпер для работы с библиотекой
    /// </summary>
    public class LibraryHelper
    {
        /// <summary>
        /// Shortcut до глобальной переменной с EA.Diagram
        /// </summary>
        public static EA.Diagram CurrentDiagram
        {
            get
            {
                return Context.CurrentDiagram;
            }
        }
        /// <summary>
        /// Shortcut до глобальной переменной с EA.Repository
        /// </summary>
        private static  EA.Repository EARepository
        {
            get
            {
                return Context.EARepository;
            }
        }

        /// <summary>
        /// Возвращает корневой пакет библиотеки для первого найденного на текущей диаграмме библиотечного элемента
        /// </summary>
        /// <returns></returns>
        public static EA.Package GetLibraryRootFromDiagram()
        {
            EA.Element libElement = null;

            if (CurrentDiagram == null) return null;

            // Пробегаемся по диаграмме в поисках библиотечного элемента
            foreach(EA.Element curElement in CurrentDiagram.DiagramObjects)
            {
                if(EAHelper.IsLibrary(curElement))
                {
                    libElement = curElement;
                    break;
                }
            }
            if (libElement == null) return null; // облом, нет на диаграмме ничего библиотечного

            // Получаем пакет библиотечного элемента
            EA.Package curpackage = EARepository.GetPackageByID(libElement.PackageID);

            return EAHelper.GetRootLibPackage(curpackage);
        }

        /// <summary>
        /// Возвращает корневой пакет библиотеки для выделенного в дереве библиотечного элемента
        /// </summary>
        /// <returns></returns>
        public static EA.Package GetLibraryRootFromTreeSelection()
        {
            EA.Element libElement = null;

            EA.Collection curSelection = EARepository.GetTreeSelectedElements();
            foreach(EA.Element curSelectedElement in curSelection)
            {
                if(EAHelper.IsLibrary(curSelectedElement))
                {
                    libElement = curSelectedElement;
                    break;
                }
            }
            if (libElement == null) return null;

            // Пробегаемся по диаграмме в поисках библиотечного элемента
            foreach (EA.Element curElement in CurrentDiagram.DiagramObjects)
            {
                if (EAHelper.IsLibrary(curElement))
                {
                    libElement = curElement;
                    break;
                }
            }
            if (libElement == null) return null; // облом, нет среди выделенного ничего библиотечного
            EA.Package curpackage = EARepository.GetPackageByID(libElement.PackageID);

            return EAHelper.GetRootLibPackage(curpackage);
        }


        /// <summary>
        /// Возвращает корневой пакет библиотеки по данным выделеанного в дереве объекта или элементам текущей диаграммы
        /// </summary>
        /// <returns></returns>
        public static EA.Package GetLibraryRoot()
        {
            EA.Package result = null;

            result = GetLibraryRootFromTreeSelection();

            if (result == null)
                result = GetLibraryRootFromDiagram();

            return result;
        }


        public static TreeNode<String> GetLibComponentTree()
        {
            TreeNode<String> result = new TreeNode<String>(null);

            // Проходимся по дереву библиотеки и ищем компоненты.

            EA.Package libRootPackage = GetLibraryRoot();
            if (libRootPackage == null)
                throw new Exception("Не обнаружена библиотека компоннетов");

            List<EA.Package> curLevelPackages = new List<EA.Package>();
            curLevelPackages.Add(libRootPackage);

            do
            {
                foreach(EA.Package curPackage in curLevelPackages)
                {
                    foreach(EA.Element curElement in curPackage.Elements)
                    {
                        if(EAHelper.IsLibrary(curElement))
                        {

                        }
                    }
                }




            } while (curLevelPackages.Count == 0); // пока не кончатся пакеты "уровнем ниже"





            return result;
        }


    }


}
