using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EADiagramPublisher.SQL
{
    /// <summary>
    /// Хэлпер для запуска SQL
    /// </summary>
    public class SQLHelper
    {
        /// <summary>
        /// Shortcut до глобальной переменной с EA.Repository
        /// </summary>
        private static EA.Repository EARepository
        {
            get
            {
                return Context.EARepository;
            }
        }

        /// <summary>
        /// Функция запускает указанный скрипт, возвращая XML - resultset
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public static XDocument RunSQL(string scriptName, string[] args = null)
        {
            XDocument result;

            // Получаем полное имя файла
            string  fullScriptName= Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "SQL", scriptName);
            if (!File.Exists(fullScriptName)) throw new Exception("файл скрипта " + fullScriptName + "не найден");

            // Загружаем текст скрипта
            string sqlString = File.ReadAllText(fullScriptName);

            // заменяем параметры
            if (args != null && args.Length > 0)
            {
                for(int i=0; i< args.Length; i++)
                {
                    string paramName = "#PARAM" + i.ToString() + "#";
                    sqlString = sqlString.Replace(paramName, args[i]);
                }
            }

            // запускаем
            string sqlResult = EARepository.SQLQuery(sqlString);


            // парсим результат запроса
            result = XDocument.Parse(sqlResult);

            return result;
        }

    }
}
