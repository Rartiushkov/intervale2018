using System;
using System.Collections.Generic;
using System.Text;

namespace Intervale
{
    class Employee
    {
        public int Id { get; private set; } //Id нужен для связки менеджер - работники
        public string Name { get; private set; } 
        public string Surname { get; private set; }
        public string Patronymic { get; private set; }
        public DateTime DateBirth { get; private set; }
        public DateTime DateEmployment { get; private set; }
        public string Description { get; private set; }
        public TypeEmployee Type { get; private set; } //Тип работника
        public readonly List<int> IdsEmployees = new List<int>();
        public bool IsTied { get; set; }

        public enum TypeEmployee
        {
            Employee = 0,
            Manager,
            Secretary,
            Principal,
            Cleaner
        }

        public Employee(int id, string name, string surname, string patronymic, DateTime dateBirth,
            DateTime dateEmployment, string description, TypeEmployee typeEmployee, bool isTied = false,
            List<int> list = null)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            DateBirth = dateBirth;
            DateEmployment = dateEmployment;
            Description = description;
            Type = typeEmployee;
            IsTied = isTied;
            
            if(list != null)
            {
                IdsEmployees.AddRange(list);
            }
        }

        public void AddWorkerIsManager(Employee employee)
        {
            IdsEmployees.Add(employee.Id);
            employee.IsTied = true;
        }

        public TypeEmployee SetType
        {
            set
            {
                Type = value;
            }
        }

        public override string ToString() //переопределяем, на выходе строковое представление работника
        {
            var stringBuilder = new StringBuilder();

            foreach(var id in IdsEmployees)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append('&');
                }
                stringBuilder.Append(id);
            }

            if(stringBuilder.Length == 0)
            {
                stringBuilder.Append('#');
            }

            return string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", Id, Name, Surname, Patronymic,
                DateBirth.ToShortDateString(), DateEmployment.ToShortDateString(), Description, Type, IsTied,
                stringBuilder.ToString());
        }
    }
}
