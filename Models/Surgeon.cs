using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class Surgeon : IStaff, IMedicalStaff, IOperatingStaff
    {
        // IStaff properties
        public string Name { get; }
        public string Surname { get; }
        public string Patronymic { get; }
        public decimal Salary { get; private set; }
        public bool IsActive { get; set; }
        public List<IMedInstitution> Workplaces { get; }
        public string ID { get; }
        public int VacationDays { get; private set; }

        // IMedicalStaff properties
        public MedicalSpecialization Specialization { get; }
        public AcademicDegree AcademicDegree { get; private set; }
        public AcademicTitle AcademicTitle { get; private set; }
        public string LicenseNumber { get; }

        // IOperatingStaff properties
        public int CompletedOperations { get; private set; }
        public int FatalOperations { get; private set; }
        public decimal SuccessRate
        {
            get
            {
                if (CompletedOperations == 0) return 0;
                return Math.Round(((decimal)(CompletedOperations - FatalOperations) / CompletedOperations) * 100, 2);
            }
        }

        public Surgeon(string id, string surname, string name, string patronymic,
                      string licenseNumber, decimal initialSalary)
        {
            ID = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Specialization = MedicalSpecialization.Surgeon;
            LicenseNumber = licenseNumber;
            Salary = initialSalary;
            IsActive = true;
            Workplaces = new List<IMedInstitution>();
            VacationDays = 28;
            AcademicDegree = AcademicDegree.None;
            AcademicTitle = AcademicTitle.None;
            CompletedOperations = 0;
            FatalOperations = 0;
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

        // IOperatingStaff methods
        public void UpdateOpStats(int completed, int fatal)
        {
            if (completed < 0 || fatal < 0 || fatal > completed)
                throw new ArgumentException("Некорректные данные операций");

            CompletedOperations += completed;
            FatalOperations += fatal;
        }

        public bool CanCureDiagnosis(SpecializationDep specialization)
        {
            // Хирург может лечить только хирургические диагнозы
            return specialization == SpecializationDep.SurgicalDep;
        }

        // Дополнительные методы
        public void SetAcademicDegree(AcademicDegree degree)
        {
            AcademicDegree = degree;
        }

        public void SetAcademicTitle(AcademicTitle title)
        {
            AcademicTitle = title;
        }

        public void RemoveWorkplace(IMedInstitution institution)
        {
            Workplaces.Remove(institution);
        }

        public string GetOperationStats()
        {
            return $"Операций: {CompletedOperations}, Успешных: {SuccessRate}%";
        }

        public override string ToString()
        {
            string degree = AcademicDegree != AcademicDegree.None ? $"{AcademicDegree} " : "";
            string title = AcademicTitle != AcademicTitle.None ? $"{AcademicTitle} " : "";
            return $"{degree}{title}{Surname} {Name} {Patronymic} - {Specialization} (ID: {ID})";
        }

        public override bool Equals(object obj)
        {
            return obj is Surgeon surgeon && ID == surgeon.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}