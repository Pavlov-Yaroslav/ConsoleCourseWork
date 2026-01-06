using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;

namespace ConsoleCourceWork.Models
{
    public class LaboratoryAssistant : IStaff, IMedicalStaff
    {
        // IStaff
        public string ID { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Patronymic { get; }
        public decimal Salary { get; private set; }
        public bool IsActive { get; set; }
        public int VacationDays { get; private set; }
        public List<IMedInstitution> Workplaces { get; }

        // IMedicalStaff
        public MedicalSpecialization Specialization => MedicalSpecialization.LaboratoryAssistant;
        public AcademicDegree AcademicDegree { get; private set; }
        public AcademicTitle AcademicTitle { get; private set; }
        public string LicenseNumber { get; }

        public LaboratoryAssistant(string id, string surname, string name, string patronymic,
                                  string licenseNumber, decimal initialSalary)
        {
            ID = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            LicenseNumber = licenseNumber;
            Salary = initialSalary;
            IsActive = true;
            Workplaces = new List<IMedInstitution>();
            VacationDays = 28;
            AcademicDegree = AcademicDegree.None;
            AcademicTitle = AcademicTitle.None;
        }

        // IStaff
        public void UpdateSalary(decimal newSalary)
        {
            if (newSalary < 0) throw new ArgumentException("Зарплата не может быть отрицательной");
            Salary = newSalary;
        }

        public void UpdateVacationDays(int newVacationDays)
        {
            if (newVacationDays < 0) throw new ArgumentException("Дни отпуска не могут быть отрицательными");
            VacationDays = newVacationDays;
        }

        public void AddWorkplace(IMedInstitution institution)
        {
            if (!Workplaces.Contains(institution))
                Workplaces.Add(institution);
        }

        public void RemoveWorkplace(IMedInstitution institution)
        {
            Workplaces.Remove(institution);
        }

        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic} - Лаборант (ID: {ID})";
        }
    }
}
