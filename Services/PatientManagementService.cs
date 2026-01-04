using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Interfaces;
using ConsoleCourceWork.Models;
using ConsoleCourceWork.Enums;

namespace ConsoleCourceWork.Services
{
    public class PatientManagementService
    {
        private readonly Hospital _hospital;
        private readonly Dictionary<IPatient, PatientPlacement> _patientPlacements;

        public PatientManagementService(Hospital hospital)
        {
            _hospital = hospital ?? throw new ArgumentNullException(nameof(hospital));
            _patientPlacements = new Dictionary<IPatient, PatientPlacement>();
        }

        public PatientPlacement AdmitPatient(IPatient patient, IDiagnosis diagnosis)
        {
            Console.WriteLine($"\n[{_hospital.Name}] Регистрация пациента: {patient.Surname} {patient.Name}");
            Console.WriteLine($"Диагноз: {diagnosis.Description}");

            if (_patientPlacements.ContainsKey(patient) && _patientPlacements[patient].IsActive)
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

            // Врача будем назначать через другой сервис
            var placement = new PatientPlacement(
                patient, diagnosis, building,
                department, availableBed.Value.ward,
                availableBed.Value.bedNumber
            );

            _patientPlacements[patient] = placement;
            Console.WriteLine("Пациент успешно зарегистрирован!");

            return placement;
        }

        public bool DischargePatient(IPatient patient)
        {
            if (!_patientPlacements.ContainsKey(patient))
            {
                Console.WriteLine($"Пациент {patient.Surname} {patient.Name} не найден в системе.");
                return false;
            }

            var placement = _patientPlacements[patient];

            if (!placement.IsActive)
            {
                Console.WriteLine($"Пациент {patient.Surname} {patient.Name} уже выписан.");
                return false;
            }

            // Создаем запись в истории болезней
            var recordId = IdGenerator.GenerateTreatmentRecordId();
            var treatmentRecord = new TreatmentRecord(
                recordId,
                patient,
                _hospital as IMedInstitution,
                placement.AdmissionDate
            );

            treatmentRecord.AddDiagnosis(placement.Diagnosis);
            treatmentRecord.CompleteRecord(DateTime.Now);
            patient.TreatmentHistory.Add(treatmentRecord);

            // Выписываем пациента
            placement.Discharge();

            Console.WriteLine($"[{_hospital.Name}] Пациент {patient.Surname} {patient.Name} выписан.");
            Console.WriteLine($"Создана запись в истории болезней: #{recordId}");

            return true;
        }

        public List<PatientPlacement> GetActivePlacements()
        {
            return _patientPlacements.Values
                .Where(p => p.IsActive)
                .ToList();
        }

        public List<PatientPlacement> GetDischargedPatients()
        {
            return _patientPlacements.Values
                .Where(p => !p.IsActive)
                .ToList();
        }

        public bool HasPatient(IPatient patient)
        {
            return _patientPlacements.ContainsKey(patient);
        }

        public PatientPlacement GetPatientPlacement(IPatient patient)
        {
            _patientPlacements.TryGetValue(patient, out var placement);
            return placement;
        }

        private IDepartment FindAvailableDepartment(SpecializationDep specialization)
        {
            foreach (var building in _hospital.Buildings)
            {
                var department = building.FindDepartmentBySpecialization(specialization);
                if (department != null) return department;
            }
            return null;
        }

        private IBuilding FindBuildingByDepartment(IDepartment department)
        {
            return _hospital.Buildings.FirstOrDefault(b => b.Departments.Contains(department));
        }

        public void PrintPatientStatistics()
        {
            int activePatients = GetActivePlacements().Count;
            int dischargedPatients = GetDischargedPatients().Count;

            Console.WriteLine($"\n=== СТАТИСТИКА ПАЦИЕНТОВ: {_hospital.Name} ===");
            Console.WriteLine($"В больнице: {activePatients} пациентов");
            Console.WriteLine($"Выписано: {dischargedPatients} пациентов");
            Console.WriteLine($"Всего: {_patientPlacements.Count} записей");
        }
    }
}
