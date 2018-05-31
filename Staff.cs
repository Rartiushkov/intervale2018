using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Intervale.Employee;

namespace Intervale
{
    class Staff //сотрудники
    {
        private List<Employee> listStaff = new List<Employee>();

        public Staff() { }

        private int _directionSortSurname = 1; //служит, чтобы сортировать в противоположном порядке
        private int _directionSortDateEmployment = 1; //^

        public void DeleteAll() //удаляем всех работников
        {
            listStaff.Clear();
        }

        public void AddEmployee(string employee) //парсим строку и создаем работника
        {
            try
            {
                if (!string.IsNullOrEmpty(employee))
                {
                    var split = employee.Split(';');
                    var id = int.Parse(split[0]);

                    if(listStaff.Find(emp => emp.Id == id) != null)
                    {
                        throw new Exception("Сотрудник с таким id уже есть!");
                    }

                    var dateBirth = DateTime.Parse(split[4]);
                    var dateEmployment = DateTime.Parse(split[5]);
                    var type = (TypeEmployee) Enum.Parse(typeof(TypeEmployee), split[7]);
                    var isTied = bool.Parse(split[8]);

                    var listIds = new List<int>();
                    var ids = split[9];

                    if (!ids.Contains("#"))
                    {
                        if (ids.Contains("&"))
                        {
                            var splitIds = ids.Split('&');

                            foreach(var idT in splitIds)
                            {
                                listIds.Add(int.Parse(idT));
                            }
                        }
                        else
                        {
                            listIds.Add(int.Parse(ids));
                        }
                    }

                    listStaff.Add(new Employee(id, split[1], split[2], split[3], dateBirth, dateEmployment,
                        split[6], type, isTied, listIds));
                }
            }

            catch (Exception)
            {
                //ignore
            }
        }

        public Employee GetEmployee(int id) //возвращаем работника по id
        {
            return listStaff.Find(employee => employee.Id == id);
        }

        public Employee GetEmployeeManager(int id) //возвращаем менеджера
        {
            return listStaff.Find(employee => employee.Id == id && employee.Type == TypeEmployee.Manager);
        }

        public Employee GetFreeWorker(int id) //возвращаем свободного работника
        {
            return listStaff.Find(employee => employee.Id == id && employee.Type != TypeEmployee.Manager && !employee.IsTied);
        }

        public void ClearAllTied(List<int> ids) //очищаем состояния, если менеджера больше нет
        {
            foreach(var id in ids)
            {
                var find = listStaff.Find(emp => emp.Id == id);
                if(find != null)
                {
                    find.IsTied = false;
                }
            }

            ids.Clear();
        }

        public void SortSurname() //сортируем
        {
            listStaff.Sort((employeeOne, employeeTwo) =>
            {
                return employeeOne.Surname.CompareTo(employeeTwo.Surname) * _directionSortSurname;
            });

            _directionSortSurname *= -1;
        }

        public void SortDateEmployment() //сортируем
        {
            listStaff.Sort((employeeOne, employeeTwo) =>
            {
                var value = 1;
                if(employeeOne.DateEmployment < employeeTwo.DateEmployment) value = -1;
                else if(employeeOne.DateEmployment < employeeTwo.DateEmployment) value = 0;
                return value * _directionSortDateEmployment;
            });

            _directionSortDateEmployment *= -1;
        }

        public string ToStringManagerAndWorkers() //показываем managers и их работников
        {
            var stringBuilder = new StringBuilder();

            var findAllManagers = listStaff.FindAll(emp => emp.Type == TypeEmployee.Manager);

            if(findAllManagers != null)
            {
                foreach(var manager in findAllManagers)
                {
                    stringBuilder.Append(string.Format("{0,-5}|{1,-15}|{2, -10}\n", manager.Name, manager.Surname, manager.DateEmployment.ToShortDateString()));

                    if(manager.IdsEmployees.Count == 0)
                    {
                        stringBuilder.Append("---->Нет подчиненных");
                    }
                    else
                    {
                        foreach(var id in manager.IdsEmployees)
                        {
                            var find = listStaff.Find(emp => emp.Id == id);

                            if (find != null)
                            {
                                stringBuilder.Append(string.Format("{0}|{1,-5}|{2,-15}|{3, -10}\n", "---->", find.Name, find.Surname, find.DateEmployment.ToShortDateString()));
                            }
                        }
                    }
                   
                }
            }

            if (stringBuilder.Length == 0)
            {
                stringBuilder.Append("Пусто\n");
            }

            return stringBuilder.ToString();
        }

        private string ToStringView(List<Employee> list) //собираем строку для отрисовки
        {
            var separator = "-----------------------------------------------------------------------------------------------------------------------\n";
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(separator);
            var format = "|{0,-5}|{1,-15}|{2,-15}|{3,-15}|{4,-10}|{5,-10}|{6,-30}|{7,-10}|\n";
            stringBuilder.Append(string.Format(format, "Id", "Surname", "Name", "Patronymic", "DateBirth", "DateEmploy", "Description", "Type"));

            stringBuilder.Append(separator);

            foreach (var employee in list)
            {
                stringBuilder.Append(string.Format(format,
                    employee.Id, employee.Surname, employee.Name, employee.Patronymic,
                    employee.DateBirth.ToShortDateString(), employee.DateEmployment.ToShortDateString(),
                    employee.Description, employee.Type));
            }

            if (list.Count == 0)
            {
                stringBuilder.Append("Пусто\n");
            }

            stringBuilder.Append(separator);

            return stringBuilder.ToString();
        }

        public bool isManager() //проверяем, есть ли manager
        {
            return listStaff.Find(emp => emp.Type == TypeEmployee.Manager) != null;
        }

        public Employee FreeWorkerNotManager(int id) //получаем работника без manager
        {
            return listStaff.Find(emp => emp.Type != TypeEmployee.Manager && !emp.IsTied && emp.Id == id);
        }

        public string FreeWorkerNotManager() //для вывода рабочих у которых нет manager
        {
            return ToStringView(listStaff.FindAll(emp => emp.Type != TypeEmployee.Manager && !emp.IsTied));
        }

        public string ToViewManager() //для вывода manager
        {
            return ToStringView(listStaff.FindAll(emp => emp.Type == TypeEmployee.Manager));
        }

        public string ToView() //для вывода всех работников
        {
            return ToStringView(listStaff);
        }

        public bool Delete(int id) //удаляем по id
        {
            var find = listStaff.Find(e => e.Id == id);

            if(find != null)
            {
                if(find.Type == TypeEmployee.Manager)
                {
                    ClearAllTied(find.IdsEmployees);
                }

                listStaff.Remove(find);
                return true;
            }

            return false;
        }

        public void Add(Employee employee) //добавляем работника
        {
            listStaff.Add(employee);
        }

        public override string ToString() //для сохранения в файл
        {
            var stringBuilder = new StringBuilder();

            foreach (var employee in listStaff)
            {
                if(stringBuilder.Length > 0) stringBuilder.Append('|');
                stringBuilder.Append(employee.ToString());
            }

            return stringBuilder.ToString();
        }

        public int NewId() //получаем уникальный id
        {
            if(listStaff.Count > 0)
            {
                var max = 0;

                foreach(var employee in listStaff)
                {
                    if (max < employee.Id) max = employee.Id;
                }

                return max + 1;
            }

            return 0;
        }
    }
}
