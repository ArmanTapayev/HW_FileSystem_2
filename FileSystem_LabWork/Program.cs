using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileSystem_LabWork
{
    class Program
    {
        //Получить расширения всех файлов
        public static Dictionary<string, int> GetFiles(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (FileInfo item in dir.GetFiles())
            {
                if (!dict.ContainsKey(item.Extension))
                {
                    dict.Add(item.Extension, 0);
                }
            }
            return dict;
        }

        // Получить массив выбранных файлов по заданному расширению
        public static List<FileInfo> ExtentionChoice(string path, string ext)
        {
            //получить информацию о папке
            //получить список файлов
            //пробежаться по файлам 
            //найти файлы с расширением ext
            //записать информацию о файле в массив FileInfo
            //вернуть массив

            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fi = dir.GetFiles();
            List<FileInfo> temp = new List<FileInfo>();
            foreach (FileInfo item in fi)
            {
                if (item.Extension == ext)
                {
                    temp.Add(item);
                }
            }
            return temp;
        }

        // Вывод на печать
        public static void PrintFiles(List<FileInfo> fileInfos, string ext)
        {
            foreach (FileInfo item in fileInfos)
            {
                if (item.Extension == ext)
                {
                    Console.WriteLine(item.Name);
                }
            }

        }

        // Выбираем наименование которые не есть буквы
        public static Dictionary<char, string> GetNameOfFiles(List<FileInfo> collection)
        {
            Dictionary<char, string> chars = new Dictionary<char, string>();

            foreach (FileInfo item in collection)
            {
                foreach (char ch in item.Name)
                {
                    if (!char.IsLetter(ch) && !chars.ContainsKey(ch))// && ch != '.')
                    {
                        chars.Add(ch, "");
                    }
                }
            }

            return chars;
        }

        static void Main(string[] args)
        {
            string path;

            Console.WriteLine(new string('-', 80));
            Console.WriteLine("\t\t\t\tЛабораторная работа");
            Console.WriteLine(new string('-', 80));

            Console.WriteLine("Введите путь к файлу: ");
            Console.Write("> ");
            path = Console.ReadLine();
            Console.WriteLine(new string('-', 80));

            // Получить расширения всех файлов
            var t = GetFiles(path);

            Console.WriteLine("Список расширений: ");
            foreach (var item in t)
            {
                Console.WriteLine(item.Key);
            }
            Console.WriteLine(new string('-', 80));

            // Предложить пользователю выбрать расширение
            Console.WriteLine("Выберите расширение: ");
            Console.Write("> ");
            string value = Console.ReadLine();
            Console.WriteLine(new string('-', 80));

            // Получить массив выбранных файлов по заданному расширению
            List<FileInfo> returnExtention = ExtentionChoice(path, value);
            PrintFiles(returnExtention, value);
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("Количество выбранных файлов: {0}", returnExtention.Count);
            Console.WriteLine(new string('-', 80));

            // Запрос на редактирование
            Console.Write("Редактировать выбранные файлы? [y|n]: ");
            char edit = Console.ReadLine()[0];
            Dictionary<char, string> chgSymbols = new Dictionary<char, string>();  // хранит значения value для замены символов
            Dictionary<char, string> getSymbols = GetNameOfFiles(returnExtention); // хранит значения key полученных символов

            if (edit == 'y')
            {
                Console.WriteLine("Введите путь для сохранения измененных файлов: ");
                Console.Write("> ");
                string newPath = Console.ReadLine();

                // Директория для измененных файлов
                DirectoryInfo newDirectiory = new DirectoryInfo(newPath);
                if (!newDirectiory.Exists)
                {
                    newDirectiory.Create();
                    Console.WriteLine(new string('-', 80));
                    Console.WriteLine("Создана новая директория по адресу: {0}", newDirectiory.FullName);
                    Console.WriteLine(new string('-', 80));
                }

                Console.WriteLine(new string('-', 80));
                Console.WriteLine("Введите значения для замены выбранных символов: ");
                Console.WriteLine(new string('-', 80));

                foreach (var item in getSymbols)
                {
                    Console.Write("{0} - ", item.Key);
                    string changeItem = Console.ReadLine();

                    chgSymbols.Add(item.Key, changeItem);
                }
                Console.WriteLine(new string('-', 80));
                Console.WriteLine("Изменения сохранены в отдельной коллекции: ");
                foreach (var item in chgSymbols)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine(new string('-', 80));

                bool flag = false;      // показатель замены символа
                bool rusSymbol = false; // показатель наличия кириллицы

                foreach (FileInfo item in returnExtention)
                {
                    string extention = item.Extension;
                    int extCount = extention.Length;
                    string newName = "";

                    // проверка на наличие кириллицы
                    for (int i = 0; i < item.Name.Length - extCount; i++)
                    {
                        if ((item.Name[i] >= 'а' && item.Name[i] <= 'я') || (item.Name[i] >= 'А' && item.Name[i] <= 'Я'))
                        {
                            rusSymbol = true;
                        }
                    }

                    for (int i = 0; i < item.Name.Length-extCount; i++)
                    {                        
                        foreach (var chg in chgSymbols)
                        {
                            if (item.Name[i] == chg.Key)
                            {
                                if (rusSymbol == true && chg.Key==' ')
                                {
                                    break;
                                }
                                newName += chg.Value; 
                                flag = true;
                                break;
                            }
                            
                        }

                        if (!flag)
                        {
                            newName += item.Name[i];
                        }
                        flag = false;
                        
                    }
                    rusSymbol = false;

                    Console.WriteLine(newName+extention);
                    
                    FileInfo newFileInfo = new FileInfo(newPath + @"\" + newName + extention);
                    item.MoveTo(newFileInfo.FullName);
                    newFileInfo.Refresh();
                }
                Console.WriteLine(new string('-', 80));
                Console.WriteLine("Измененные файлы сохренены в директории по адресу {0}", newPath);
                Console.WriteLine(new string('-', 80));
            }
            Console.WriteLine();
        }
    }
}
