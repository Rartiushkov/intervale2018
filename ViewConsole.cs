using System;
using static Intervale.Employee;

namespace Intervale
{
    class ViewConsole
    {
        public void StartMainMenu(Staff staff)
        {
            var option = -1;

            do
            {
                switch (option)
                {
                    case 1: AddEmployee(staff); break;
                    case 2: DeleteEmployee(staff); break;
                    case 3: ChangeTypeEmployee(staff); break;
                    case 4: ShowManager(staff); break;
                    case 5: staff.SortSurname(); break;
                    case 6: staff.SortDateEmployment(); break;
                    case 7: ReadWriteStaff.Save(staff); break;
                    case 8: ReadWriteStaff.Read(staff); break;
                }

                Console.Clear();
                Console.WriteLine(staff.ToView());
                Console.WriteLine("1) Добавить сотрудника");
                Console.WriteLine("2) Удалить сотрудника");
                Console.WriteLine("3) Изменить тип сотрудника");
                Console.WriteLine("4) Менеджеры");
                Console.WriteLine("Сортировать список");
                Console.WriteLine("5) По фамилиям");
                Console.WriteLine("6) По датам принятия на работу");
                Console.WriteLine("7) Сохранить в файл");
                Console.WriteLine("8) Считать с файла");
                Console.WriteLine("9) Выход");
                Console.WriteLine("Выберите вариант и нажмите enter: ");

                int.TryParse(Console.ReadLine(), out option);

            } while (option != 9);
        }

        private void ShowManager(Staff staff)
        {
            var option = -1;

            do
            {
                switch (option)
                {
                    case 1:  AddWorkerToManager(staff); break;
                    case 2: ShowManagerAndWorkers(staff); break;
                }
                Console.Clear();
                Console.WriteLine(staff.ToViewManager());
                Console.WriteLine("1) Привязать сотрудника к менежеру");
                Console.WriteLine("2) Показать менеджеров со сотрудниками");
                Console.WriteLine("3) Назад");
                int.TryParse(Console.ReadLine(), out option);

            } while (option != 3);
        }

        public void AddWorkerToManager(Staff staff)
        {
            try
            {
                var exception = new Exception("Error!");

                if (!staff.isManager())
                {
                    throw exception;
                }

                Employee manager = null;
                Employee worker = null;

                CheckEmployee(staff, staff.ToViewManager(), idT =>
                {
                    manager = staff.GetEmployeeManager(idT);

                    if(manager == null)
                    {
                        throw exception;
                    }
                });

                CheckEmployee(staff, staff.FreeWorkerNotManager(), idT =>
                {
                    worker = staff.GetFreeWorker(idT);

                    if(worker == null)
                    {
                        throw exception;
                    }
                });

                manager.AddWorkerIsManager(worker);

                Console.WriteLine("Success");
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        public void ShowManagerAndWorkers(Staff staff)
        {
            Console.Clear();
            Console.WriteLine(staff.ToStringManagerAndWorkers());
            Console.ReadKey();
        }

        private void ChangeTypeEmployee(Staff staff)
        {
            CheckEmployee(staff, staff.ToView(), id =>
            {
                var employee = staff.GetEmployee(id);

                if (employee != null)
                {
                    try
                    {
                        var type = ReadType();

                        if(employee.Type == TypeEmployee.Manager)
                        {
                            staff.ClearAllTied(employee.IdsEmployees);
                        }

                        employee.SetType = type;
                        Console.WriteLine("Успешно изменен!");
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Такого работника нет!");
                }
            });
        }

        private void DeleteEmployee(Staff staff)
        {
            CheckEmployee(staff, staff.ToView(), id =>
            {
                if (staff.Delete(id))
                {
                    Console.WriteLine("Успешно удален!");
                }
                else
                {
                    Console.WriteLine("Такого работника нет!");
                }
            });
        }

        private void CheckEmployee(Staff staff, string title, Action<int> action)
        {

            Console.Clear();
            Console.WriteLine(title);
            Console.Write("Введите id: ");

            var id = -1;

            if (int.TryParse(Console.ReadLine(), out id))
            {
                action?.Invoke(id);
            }
            else
            {
                Console.WriteLine("Вы ввели не корректные данные!");
            }
        }


        private string ReadLine(string title)
        {
            Console.Write(title);
            var name = Console.ReadLine();
            if (string.IsNullOrEmpty(name)) throw new Exception("Error! Попробуйте добвать заново!");
            return name;
        }

        private TypeEmployee ReadType()
        {
            var types = Enum.GetNames(typeof(TypeEmployee));
            var option = -1;

            for (var i = 0; i < types.Length; i++)
            {
                Console.WriteLine(string.Format("{0}) {1}", i + 1, types[i]));
            }

            Console.WriteLine("Выберите вариант: ");

            var exception = new Exception("Вы ввели не корректные данные!");

            if (!int.TryParse(Console.ReadLine(), out option))
            {
                throw exception;
            }

            option -= 1;

            if(option < 0 || option >= types.Length)
            {
                throw exception;
            }

            return (TypeEmployee)option;
        }

        private void AddEmployee(Staff staff)
        {
            Console.Clear();

            try
            {
                var name = ReadLine("Введите Имя: ");
                var surname = ReadLine("Введите Фамилию: ");
                var patronymic = ReadLine("Введите Отчество: ");
                var type = ReadType();
                var dateBirthValue = ReadLine("Введите дату рождения (dd.mm.yyyy): ");
                var dateBirth = DateTime.Parse(dateBirthValue);
                var dateEmploymentValue = ReadLine("Введите дату наема на работу (dd.mm.yyyy): ");
                var dateEmploymen = DateTime.Parse(dateEmploymentValue);
                var description = ReadLine("Введите описание: ");
                staff.Add(new Employee(staff.NewId(), name, surname, patronymic, dateBirth, dateEmploymen, description, type));
                Console.WriteLine("\nУспешно добавлен\n");
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
