namespace Intervale
{
    class Program
    {
        static void Main(string[] args)
        {
            var staff = new Staff(); //создаем класс сотрудников
            ReadWriteStaff.Read(staff); //пробуем загрузить из файла сотрудников

            var viewConsole = new ViewConsole(); //создаем консольное отображение
            viewConsole.StartMainMenu(staff); // передаем сотрудников туда, для управления
        }
    }
}
