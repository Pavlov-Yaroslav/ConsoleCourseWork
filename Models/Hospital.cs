using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;

namespace ConsoleCourceWork.Models
{
    public class Hospital : IHospital, IMedInstitution
    {
        // Свойства IMedInstitution
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string ID { get; private set; }
        public bool IsActive { get; set; }

        // Свойства IHospital
        public List<Building> Buildings { get; private set; } = new List<Building>();
        public Dictionary<IPatient, PatientPlacement> PatientPlacements { get; private set; } = new Dictionary<IPatient, PatientPlacement>();

        // НОВОЕ: Словарь для персонала
        public Dictionary<IStaff, StaffPlacement> StaffPlacements { get; private set; } = new Dictionary<IStaff, StaffPlacement>();

        // Конструктор
        public Hospital(string id = "H001", string name = "Городская больница", string address = "ул. Центральная, 1")
        {
            ID = id;
            Name = name;
            Address = address;
            IsActive = true;
        }

        // === МЕТОДЫ ДЛЯ ПАЦИЕНТОВ ===

        public void AddBuilding(Building building)
        {
            Buildings.Add(building);
        }

        public void RemoveBuilding(Building building)
        {
            Buildings.Remove(building);
        }

        public PatientPlacement AdmitPatient(IPatient patient, IDiagnosis diagnosis)
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

            // Находим и назначаем врача
            var doctor = FindDoctorForPatient(department, diagnosis.RequiredSpecialization);

            var placement = new PatientPlacement(
                patient, diagnosis, building,
                department, availableBed.Value.ward,
                availableBed.Value.bedNumber
            );

            // Назначаем врача пациенту
            if (doctor != null)
            {
                placement.AttendingDoctor = doctor;
                Console.WriteLine($"Назначен врач: {doctor.Surname} {doctor.Name}");
            }
            else
            {
                Console.WriteLine($"Внимание: Не найден подходящий врач для пациента!");
            }

            PatientPlacements[patient] = placement;
            Console.WriteLine("Пациент успешно зарегистрирован!");

            return placement;
        }

        public bool DischargePatient(IPatient patient)
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
                this as IMedInstitution,
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

        public bool DeletePatient(IPatient patient)
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

        // === МЕТОДЫ ДЛЯ ПЕРСОНАЛА ===

        public StaffPlacement HireStaff(IStaff staff, IDepartment department = null)
        {
            if (staff == null)
                throw new ArgumentNullException(nameof(staff));

            // Проверяем, не работает ли уже сотрудник
            if (StaffPlacements.ContainsKey(staff) && StaffPlacements[staff].IsActive)
            {
                Console.WriteLine($"Сотрудник {staff.Surname} уже работает в больнице!");
                return StaffPlacements[staff];
            }

            var placement = new StaffPlacement(staff, department);
            StaffPlacements[staff] = placement;

            Console.WriteLine($"Принят на работу: {staff.Surname} {staff.Name}");

            if (department != null)
            {
                Console.WriteLine($"  Назначен в отделение: {department.Name}");
            }

            return placement;
        }

        public bool AssignStaffToDepartment(IStaff staff, IDepartment department)
        {
            if (!StaffPlacements.ContainsKey(staff))
            {
                Console.WriteLine($"Сотрудник {staff.Surname} не работает в этой больнице.");
                return false;
            }

            var placement = StaffPlacements[staff];

            if (!placement.IsActive)
            {
                Console.WriteLine($"Сотрудник {staff.Surname} уволен, нельзя назначить в отделение.");
                return false;
            }

            placement.UpdateDepartment(department);
            Console.WriteLine($"{staff.Surname} {staff.Name} назначен в отделение {department.Name}");
            return true;
        }

        public bool DismissStaff(IStaff staff)
        {
            if (!StaffPlacements.ContainsKey(staff))
            {
                Console.WriteLine($"Сотрудник {staff.Surname} не работает в этой больнице.");
                return false;
            }

            var placement = StaffPlacements[staff];

            if (!placement.IsActive)
            {
                Console.WriteLine($"Сотрудник {staff.Surname} уже уволен.");
                return false;
            }

            // Проверка, есть ли у врача пациенты
            if (staff is IMedicalStaff medicalStaff)
            {
                var patientCount = GetPatientsByDoctor(medicalStaff).Count;
                if (patientCount > 0)
                {
                    Console.WriteLine($"Нельзя уволить {staff.Surname}! У врача {patientCount} пациентов.");
                    return false;
                }
            }

            placement.SetDismissal();
            Console.WriteLine($"{staff.Surname} {staff.Name} уволен.");
            return true;
        }

        public List<StaffPlacement> GetActiveStaff()
        {
            return StaffPlacements.Values
                .Where(p => p.IsActive)
                .ToList();
        }

        public List<StaffPlacement> GetDismissedStaff()
        {
            return StaffPlacements.Values
                .Where(p => !p.IsActive)
                .ToList();
        }

        public IMedicalStaff FindDoctorForPatient(IDepartment department, SpecializationDep requiredSpecialization)
        {
            // Сначала ищем в нужном отделении
            var doctorsInDepartment = StaffPlacements
                .Where(kvp => kvp.Value.IsActive &&
                              kvp.Value.Department == department &&
                              kvp.Key is IMedicalStaff medicalStaff &&
                              medicalStaff.IsActive &&
                              IsSpecializationMatch(medicalStaff.Specialization, requiredSpecialization))
                .Select(kvp => kvp.Key as IMedicalStaff)
                .ToList();

            if (doctorsInDepartment.Any())
            {
                // Выбираем врача с наименьшим количеством пациентов
                return doctorsInDepartment
                    .OrderBy(doctor => CountPatientsByDoctor(doctor))
                    .First();
            }

            // Если в отделении нет подходящих врачей, ищем по всей больнице
            var allDoctors = StaffPlacements
                .Where(kvp => kvp.Value.IsActive &&
                              kvp.Key is IMedicalStaff medicalStaff &&
                              medicalStaff.IsActive &&
                              IsSpecializationMatch(medicalStaff.Specialization, requiredSpecialization))
                .Select(kvp => kvp.Key as IMedicalStaff)
                .ToList();

            if (!allDoctors.Any())
            {
                Console.WriteLine($"  Предупреждение: В больнице нет врачей специализации {requiredSpecialization}");
                return null;
            }

            // Выбираем врача с наименьшим количеством пациентов
            return allDoctors
                .OrderBy(doctor => CountPatientsByDoctor(doctor))
                .First();
        }

        private int CountPatientsByDoctor(IMedicalStaff doctor)
        {
            return PatientPlacements.Values
                .Count(p => p.IsActive && p.AttendingDoctor == doctor);
        }

        public List<PatientPlacement> GetPatientsByDoctor(IMedicalStaff doctor)
        {
            return PatientPlacements.Values
                .Where(p => p.IsActive && p.AttendingDoctor == doctor)
                .ToList();
        }

        public List<StaffPlacement> GetStaffByDepartment(IDepartment department)
        {
            return StaffPlacements.Values
                .Where(p => p.IsActive && p.Department == department)
                .ToList();
        }

        public void PrintDepartmentStaff(IDepartment department)
        {
            var staffList = GetStaffByDepartment(department);

            Console.WriteLine($"\nПерсонал отделения {department.Name}:");

            if (!staffList.Any())
            {
                Console.WriteLine("  В отделении нет персонала");
                return;
            }

            foreach (var placement in staffList)
            {
                var staff = placement.Staff;
                if (staff is IMedicalStaff medicalStaff)
                {
                    var patientCount = CountPatientsByDoctor(medicalStaff);
                    Console.WriteLine($"  - {medicalStaff.Specialization}: {staff.Surname} {staff.Name} ({patientCount} пациентов)");
                }
                else
                {
                    Console.WriteLine($"  - {staff.Surname} {staff.Name}");
                }
            }
        }

        // === ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ===

        private IDepartment FindAvailableDepartment(SpecializationDep specialization)
        {
            foreach (var building in Buildings)
            {
                var department = building.FindDepartmentBySpecialization(specialization);
                if (department != null) return department;
            }
            return null;
        }

        private IBuilding FindBuildingByDepartment(IDepartment department)
        {
            return Buildings.FirstOrDefault(b => b.Departments.Contains(department));
        }

        private bool IsSpecializationMatch(MedicalSpecialization doctorSpecialization, SpecializationDep departmentSpecialization)
        {
            // Более точное соответствие специализаций
            switch (departmentSpecialization)
            {
                case SpecializationDep.SurgicalDep:
                    return doctorSpecialization == MedicalSpecialization.Surgeon;

                case SpecializationDep.CardiologyDep:
                    return doctorSpecialization == MedicalSpecialization.Cardiologist;

                case SpecializationDep.NeurologyDep:
                    return doctorSpecialization == MedicalSpecialization.Neurologist;

                case SpecializationDep.PediatricsDep:
                    return doctorSpecialization == MedicalSpecialization.Pediatrician;

                case SpecializationDep.GeneralMedicineDep:
                    // Терапевт или общая медицина
                    return doctorSpecialization == MedicalSpecialization.Therapist ||
                           doctorSpecialization == MedicalSpecialization.GeneralMedicine;

                case SpecializationDep.XrayDep:
                    return doctorSpecialization == MedicalSpecialization.Radiologist;

                default:
                    return false;
            }
        }

        // === ИНФОРМАЦИОННЫЕ МЕТОДЫ ===

        public void PrintHospitalStats()
        {
            Console.WriteLine($"\n=== СТАТИСТИКА БОЛЬНИЦЫ: {Name} ===");

            int activePatients = GetActivePlacements().Count;
            int dischargedPatients = GetDischargedPatients().Count;
            int activeStaff = GetActiveStaff().Count;
            int dismissedStaff = GetDismissedStaff().Count;

            Console.WriteLine($"Пациенты: {activePatients} в больнице, {dischargedPatients} выписанных");
            Console.WriteLine($"Персонал: {activeStaff} работает, {dismissedStaff} уволено");
            Console.WriteLine($"Здания: {Buildings.Count}");

            // Статистика по отделениям
            foreach (var building in Buildings)
            {
                Console.WriteLine($"\nЗдание: {building.Name}");
                foreach (var department in building.Departments)
                {
                    var staffCount = GetStaffByDepartment(department).Count;
                    var patientCount = PatientPlacements.Values
                        .Count(p => p.IsActive && p.Department == department);

                    Console.WriteLine($"  {department.Name}: {staffCount} сотрудников, {patientCount} пациентов");
                }
            }
        }

        public override string ToString()
        {
            int activePatients = GetActivePlacements().Count;
            int activeStaff = GetActiveStaff().Count;

            return $"{Name} (ID: {ID})\n" +
                   $"Адрес: {Address}\n" +
                   $"Статус: {(IsActive ? "Активна" : "Неактивна")}\n" +
                   $"Зданий: {Buildings.Count}\n" +
                   $"Персонал: {activeStaff} человек\n" +
                   $"Пациентов: {activePatients} в больнице";
        }
    }
}