using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class Radiologist : IStaff, IMedicalStaff, IOperatingStaff, IHazardWorkStaff
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

        // IHazardWorkStaff properties
        public decimal HazardCoefficient { get; }
        public HazardType HazardType { get; }

        public Radiologist(string id, string surname, string name, string patronymic,
                          string licenseNumber, decimal initialSalary)
        {
            ID = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Specialization = MedicalSpecialization.Radiologist;
            LicenseNumber = licenseNumber;
            Salary = initialSalary;
            IsActive = true;
            Workplaces = new List<IMedInstitution>();
            VacationDays = 28; // Базовое значение
            AcademicDegree = AcademicDegree.None;
            AcademicTitle = AcademicTitle.None;
            CompletedOperations = 0;
            FatalOperations = 0;
            HazardCoefficient = 1.4m; // Коэффициент для радиологов
            HazardType = HazardType.RadiationExposure;
        }

        // IStaff methods (с учетом коэффициента опасности)
        public void UpdateSalary(decimal newSalary)
        {
            if (newSalary < 0)
                throw new ArgumentException("Зарплата не может быть отрицательной");

            // Применяем коэффициент опасности
            Salary = newSalary * HazardCoefficient;
        }

        public void UpdateVacationDays(int newVacationDays)
        {
            if (newVacationDays < 0)
                throw new ArgumentException("Дни отпуска не могут быть отрицательными");

            // Применяем коэффициент опасности
            VacationDays = (int)(newVacationDays * HazardCoefficient);
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
            return specialization == SpecializationDep.XrayDep;
        }

        // Дополнительные методы
        public void SetAcademicDegree(AcademicDegree degree)
        {
            AcademicDegree = degree;

            // Автоматическое присвоение звания в зависимости от степени
            if (degree == AcademicDegree.DoctorOfMedicalSciences)
            {
                AcademicTitle = AcademicTitle.Professor;
            }
            else if (degree == AcademicDegree.CandidateOfMedicalSciences)
            {
                AcademicTitle = AcademicTitle.AssistantProfessor; // Доцент
            }
            else
            {
                AcademicTitle = AcademicTitle.None;
            }
        }

        // Добавляем проверку при установке звания
        public void SetAcademicTitle(AcademicTitle title)
        {
            // Проверяем соответствие степени и звания
            if (title == AcademicTitle.Professor &&
                AcademicDegree != AcademicDegree.DoctorOfMedicalSciences)
            {
                Console.WriteLine($"Предупреждение: {Surname} {Name} не имеет степени доктора медицинских наук, " +
                                 "звание профессора не присвоено. Используется 'None'.");
                AcademicTitle = AcademicTitle.None;
                return;
            }

            if (title == AcademicTitle.AssistantProfessor &&
                AcademicDegree != AcademicDegree.CandidateOfMedicalSciences)
            {
                Console.WriteLine($"Предупреждение: {Surname} {Name} не имеет степени кандидата медицинских наук, " +
                                 "звание доцента не присвоено. Используется 'None'.");
                AcademicTitle = AcademicTitle.None;
                return;
            }

            AcademicTitle = title;
        }

        public void RemoveWorkplace(IMedInstitution institution)
        {
            Workplaces.Remove(institution);
        }

        public string GetOperationStats()
        {
            return $"Рентгеновских исследований: {CompletedOperations}, Успешных: {SuccessRate}%";
        }

        public override string ToString()
        {
            string degree = AcademicDegree != AcademicDegree.None ? $"{AcademicDegree} " : "";
            string title = AcademicTitle != AcademicTitle.None ? $"{AcademicTitle} " : "";
            return $"{degree}{title}{Surname} {Name} {Patronymic} - {Specialization} (Опасная работа, коэффициент: {HazardCoefficient})";
        }

        public override bool Equals(object obj)
        {
            return obj is Radiologist radiologist && ID == radiologist.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}