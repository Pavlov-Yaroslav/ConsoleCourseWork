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
                return false;

            var placement = _patientPlacements[patient];
            if (!placement.IsActive)
                return false;

            // Собираем врачей - ДОБАВЛЯЕМ лечащего врача
            var doctors = new List<IMedicalStaff>();
            if (placement.AttendingDoctor != null)
                doctors.Add(placement.AttendingDoctor);

            // Генерируем историю
            var record = TreatmentRecordGenerator.GenerateTreatmentRecord(
                patient: patient,
                institution: _hospital as IMedInstitution,
                diagnosis: placement.Diagnosis,
                doctors: doctors, // Теперь врачи передаются
                daysInTreatment: (int)(DateTime.Now - placement.AdmissionDate).TotalDays
            );

            patient.TreatmentHistory.Add(record);
            placement.Discharge();

            Console.WriteLine($"\nПациент {patient.Surname} выписан.");
            Console.WriteLine($"Создана история болезни #{record.ID}");

            // Выводим детали
            Console.WriteLine($"  Диагноз: {record.Diagnoses[0].Name}");
            Console.WriteLine($"  Врачей: {record.AttendingDoctors.Count}");
            Console.WriteLine($"  Рецептов: {record.Prescriptions.Count}");

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

        public bool DischargePatient(IPatient patient, IDiagnosis finalDiagnosis = null)
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

            // Если не указан финальный диагноз, используем из размещения
            var diagnosis = finalDiagnosis ?? placement.Diagnosis;

            // Получаем врачей (если есть лечащий врач - добавляем его)
            var doctors = new List<IMedicalStaff>();
            if (placement.AttendingDoctor != null)
                doctors.Add(placement.AttendingDoctor);

            // Если в больнице есть другие врачи, добавляем их
            var hospitalStaff = _hospital.StaffService.GetAllMedicalStaff();
            foreach (var doctor in hospitalStaff)
            {
                if (doctor != placement.AttendingDoctor && doctors.Count < 3) // не более 3 врачей
                    doctors.Add(doctor);
            }

            // ГЕНЕРИРУЕМ ИСТОРИЮ БОЛЕЗНИ
            var treatmentRecord = TreatmentRecordGenerator.GenerateTreatmentRecord(
                patient: patient,
                institution: _hospital as IMedInstitution,
                diagnosis: diagnosis,
                doctors: doctors,
                daysInTreatment: (int)(DateTime.Now - placement.AdmissionDate).TotalDays
            );

            // Добавляем в историю пациента
            patient.TreatmentHistory.Add(treatmentRecord);

            // Выписываем пациента
            placement.Discharge();

            Console.WriteLine($"[{_hospital.Name}] Пациент {patient.Surname} {patient.Name} выписан.");
            Console.WriteLine($"Создана запись в истории болезней: #{treatmentRecord.ID}");

            // Выводим детали истории
            PrintTreatmentRecordDetails(treatmentRecord);

            return true;
        }

        private void PrintTreatmentRecordDetails(ITreatmentRecord record)
        {
            Console.WriteLine("\n=== ДЕТАЛИ ИСТОРИИ БОЛЕЗНИ ===");
            Console.WriteLine($"Диагноз: {record.Diagnoses.FirstOrDefault()?.Name}");
            Console.WriteLine($"Рецептов: {record.Prescriptions.Count}");
            Console.WriteLine($"Процедур: {record.Procedures.Count}");
            Console.WriteLine($"Анализов: {record.Analyses.Count}");

            if (record.Prescriptions.Any())
            {
                Console.WriteLine("\nРецепты:");
                foreach (var prescription in record.Prescriptions)
                {
                    Console.WriteLine($"  - {prescription.Medication} ({prescription.Dosage})");
                }
            }
        }
    }
}
