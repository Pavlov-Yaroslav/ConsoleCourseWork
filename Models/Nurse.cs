using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class Nurse : IStaff, ISupportStaff
    {
        // IStaff properties
        public string ID { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Patronymic { get; }
        public decimal Salary { get; private set; }
        public bool IsActive { get; set; }
        public int VacationDays { get; private set; }
        public List<IMedInstitution> Workplaces { get; }

        // ISupportStaff properties
        public SupportSpecialization Specialization { get; }
        public string Qualification { get; }

        public Nurse(string id, string surname, string name, string patronymic,
                    string qualification, decimal initialSalary)
        {
            ID = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Specialization = SupportSpecialization.Nurse;
            Qualification = qualification;
            Salary = initialSalary;
            IsActive = true;
            VacationDays = 28; // Базовый отпуск
            Workplaces = new List<IMedInstitution>();
        }

        // IStaff methods
        public void UpdateSalary(decimal newSalary)
        {
            if (newSalary < 0)
                throw new ArgumentException("Зарплата не может быть отрицательной");
            Salary = newSalary;
        }

        public void UpdateVacationDays(int newVacationDays)
        {
            if (newVacationDays < 0)
                throw new ArgumentException("Дни отпуска не могут быть отрицательными");
            VacationDays = newVacationDays;
        }

        public void AddWorkplace(IMedInstitution institution)
        {
            if (institution == null)
                throw new ArgumentNullException(nameof(institution));

            if (!Workplaces.Contains(institution))
            {
                Workplaces.Add(institution);
            }
        }

        // Дополнительные методы
        public void RemoveWorkplace(IMedInstitution institution)
        {
            Workplaces.Remove(institution);
        }

        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic} - {Specialization}, Квалификация: {Qualification}";
        }

        public override bool Equals(object obj)
        {
            return obj is Nurse nurse && ID == nurse.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
