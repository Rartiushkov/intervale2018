using System.IO;

namespace Intervale
{
    class ReadWriteStaff
    {
        private static string path = "file.txt";

        public static bool Save(Staff staff) //сохранение сотрудников в файл
        {
            var output = staff.ToString();

            if (!string.IsNullOrEmpty(output)) //если хоть один сотрудник есть, тогда сохраняем
            {
                File.WriteAllText(path, output);
                return true;
            }

            return false;
        }

        public static void Read(Staff staff)
        {
            staff.DeleteAll();

            if (File.Exists(path))
            {
                var input = File.ReadAllText(path);

                if (!string.IsNullOrEmpty(input)) //проверяем, что там хоть что-то есть
                {
                    if (input.Contains("|")) //проверяем, у нас один или больше сотрудников
                    {
                        var split = input.Split('|');

                        foreach (var value in split) //парсим сотрудников
                        {
                            staff.AddEmployee(value); //парсим сотрудника
                        }
                    }
                    else
                    {
                        staff.AddEmployee(input); //парсим сотрудника
                    } 
                }
            }
        }
    }
}
