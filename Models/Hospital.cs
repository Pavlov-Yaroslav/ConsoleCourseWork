using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;

namespace ConsoleCourceWork.Models
{
    public class Hospital : Interfaces.IHospital, Interfaces.IMedInstitution
    {
        // Свойства IMedInstitution (только необходимые)
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string ID { get; private set; }
        public bool IsActive { get; set; }

        // Свойства IHospital
        public List<Building> Buildings { get; private set; } = new List<Building>();
        public Dictionary<Interfaces.IPatient, PatientPlacement> PatientPlacements { get; private set; } = new Dictionary<Interfaces.IPatient, PatientPlacement>();

        // Конструктор с параметрами
        public Hospital(string id, string name, string address)
        {
            ID = id;
            Name = name;
            Address = address;
            IsActive = true;
        }

        // Конструктор по умолчанию
        public Hospital() : this("H001", "Городская больница", "ул. Центральная, 1")
        {
        }

        // Методы IHospital
        public void AddBuilding(Building building)
        {
            Buildings.Add(building);
        }

        public void RemoveBuilding(Building building)
        {
            Buildings.Remove(building);
        }

        public PatientPlacement AdmitPatient(Interfaces.IPatient patient, Interfaces.IDiagnosis diagnosis)
        {
            Console.WriteLine($"\n[{Name}] Регистрация пациента: {patient.Surname} {patient.Name}");
            Console.WriteLine($"Диагноз: {diagnosis.Description}");

            if (PatientPlacements.ContainsKey(patient) && PatientPlacements[patient].IsActive)
            {
                Console.WriteLine($"Пациент уже находится в больнице!");
                return null;
            }

            var department = FindAvailableDepartment(diagnosis.RequiredSpecialization);
            if (department == null)
            {
                Console.WriteLine($"Нет свободных коек в отделении {diagnosis.RequiredSpecialization}");
                return null;
            }

            var availableBed = department.FindAvailableBed();
            if (availableBed == null)
            {
                Console.WriteLine("Не удалось найти свободную койку");
                return null;
            }

            var building = FindBuildingByDepartment(department);
            if (building == null) return null;

            var placement = new PatientPlacement(
                patient, diagnosis, building,
                department, availableBed.Value.ward,
                availableBed.Value.bedNumber
            );

            PatientPlacements[patient] = placement;
            Console.WriteLine("Пациент успешно зарегистрирован!");

            return placement;
        }

        public bool DischargePatient(Interfaces.IPatient patient)
        {
            if (!PatientPlacements.ContainsKey(patient))
            {
                Console.WriteLine($"Пациент {patient.Surname} {patient.Name} не найден в системе.");
                return false;
            }

            var placement = PatientPlacements[patient];

            if (!placement.IsActive)
            {
                Console.WriteLine($"Пациент {patient.Surname} {patient.Name} уже выписан.");
                return false;
            }

            // Используем генератор ID
            var recordId = Services.IdGenerator.GenerateTreatmentRecordId();

            // Создаем запись в истории болезней
            var treatmentRecord = new TreatmentRecord(
                recordId,
                patient,
                this as Interfaces.IMedInstitution,
                placement.AdmissionDate
            );

            // Добавляем диагноз
            treatmentRecord.AddDiagnosis(placement.Diagnosis);

            // Завершаем запись
            treatmentRecord.CompleteRecord(DateTime.Now);

            // Добавляем запись в историю пациента
            patient.TreatmentHistory.Add(treatmentRecord);

            // Выписываем пациента
            placement.Discharge();

            Console.WriteLine($"[{Name}] Пациент {patient.Surname} {patient.Name} выписан.");
            Console.WriteLine($"Создана запись в истории болезней: #{recordId}");

            return true;
        }

        public bool DeletePatient(Interfaces.IPatient patient)
        {
            if (!PatientPlacements.ContainsKey(patient))
            {
                Console.WriteLine($"Пациент {patient.Surname} {patient.Name} не найден в системе.");
                return false;
            }

            var placement = PatientPlacements[patient];

            if (placement.IsActive)
            {
                placement.Discharge();
            }

            PatientPlacements.Remove(patient);

            Console.WriteLine($"[{Name}] Пациент {patient.Surname} {patient.Name} полностью удален из системы.");

            return true;
        }

        public List<PatientPlacement> GetActivePlacements()
        {
            return PatientPlacements.Values
                .Where(p => p.IsActive)
                .ToList();
        }

        public List<PatientPlacement> GetDischargedPatients()
        {
            return PatientPlacements.Values
                .Where(p => !p.IsActive)
                .ToList();
        }

        private Interfaces.IDepartment FindAvailableDepartment(Enums.SpecializationDep specialization)
        {
            foreach (var building in Buildings)
            {
                var department = building.FindDepartmentBySpecialization(specialization);
                if (department != null) return department;
            }
            return null;
        }

        private Interfaces.IBuilding FindBuildingByDepartment(Interfaces.IDepartment department)
        {
            return Buildings.FirstOrDefault(b => b.Departments.Contains(department));
        }

        public override string ToString()
        {
            int activePatients = GetActivePlacements().Count;
            int dischargedPatients = GetDischargedPatients().Count;

            return $"{Name} (ID: {ID})\n" +
                   $"Адрес: {Address}\n" +
                   $"Статус: {(IsActive ? "Активна" : "Неактивна")}\n" +
                   $"Зданий: {Buildings.Count}\n" +
                   $"Пациентов: {activePatients} в больнице, {dischargedPatients} выписанных";
        }
    }
}