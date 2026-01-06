using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;
using ConsoleCourceWork.Models;

namespace ConsoleCourceWork.Services
{
    public class StaffManagementService
    {
        private readonly Hospital _hospital;
        private readonly Dictionary<IStaff, StaffPlacement> _staffPlacements;

        public StaffManagementService(Hospital hospital)
        {
            _hospital = hospital ?? throw new ArgumentNullException(nameof(hospital));
            _staffPlacements = new Dictionary<IStaff, StaffPlacement>();
        }

        public StaffPlacement HireStaff(IStaff staff, IDepartment department = null)
        {
            if (staff == null)
                throw new ArgumentNullException(nameof(staff));

            // Проверяем, не работает ли уже сотрудник
            if (_staffPlacements.ContainsKey(staff) && _staffPlacements[staff].IsActive)
            {
                Console.WriteLine($"Сотрудник {staff.Surname} уже работает в больнице!");
                return _staffPlacements[staff];
            }

            var placement = new StaffPlacement(staff, department);
            _staffPlacements[staff] = placement;

            Console.WriteLine($"Принят на работу: {staff.Surname} {staff.Name}");

            if (department != null)
            {
                Console.WriteLine($"  Назначен в отделение: {department.Name}");
            }

            return placement;
        }

        public bool DismissStaff(IStaff staff)
        {
            if (!_staffPlacements.ContainsKey(staff))
            {
                Console.WriteLine($"Сотрудник {staff.Surname} не работает в этой больнице.");
                return false;
            }

            var placement = _staffPlacements[staff];

            if (!placement.IsActive)
            {
                Console.WriteLine($"Сотрудник {staff.Surname} уже уволен.");
                return false;
            }

            // Увольнение разрешено всегда (проверку на пациентов можно добавить при необходимости)
            placement.SetDismissal();
            Console.WriteLine($"{staff.Surname} {staff.Name} уволен.");
            return true;
        }

        public List<StaffPlacement> GetActiveStaff()
        {
            return _staffPlacements.Values
                .Where(p => p.IsActive)
                .ToList();
        }

        public List<StaffPlacement> GetStaffByDepartment(IDepartment department)
        {
            return _staffPlacements.Values
                .Where(p => p.IsActive && p.Department == department)
                .ToList();
        }

        public bool AssignStaffToDepartment(IStaff staff, IDepartment department)
        {
            if (!_staffPlacements.ContainsKey(staff))
            {
                Console.WriteLine($"Сотрудник {staff.Surname} не работает в этой больнице.");
                return false;
            }

            var placement = _staffPlacements[staff];

            if (!placement.IsActive)
            {
                Console.WriteLine($"Сотрудник {staff.Surname} уволен, нельзя назначить в отделение.");
                return false;
            }

            placement.UpdateDepartment(department);
            Console.WriteLine($"{staff.Surname} {staff.Name} назначен в отделение {department.Name}");
            return true;
        }

        public IMedicalStaff FindDoctorForPatient(IDepartment department, SpecializationDep requiredSpecialization,
                                                 PatientManagementService patientService)
        {
            // Сначала ищем в нужном отделении
            var doctorsInDepartment = _staffPlacements
                .Where(kvp => kvp.Value.IsActive &&
                              kvp.Value.Department == department &&
                              kvp.Key is IMedicalStaff medicalStaff &&
                              medicalStaff.IsActive &&
                              IsSpecializationMatch(medicalStaff.Specialization, requiredSpecialization))
                .Select(kvp => kvp.Key as IMedicalStaff)
                .ToList();

            if (doctorsInDepartment.Any())
            {
                return doctorsInDepartment
                    .OrderBy(doctor => CountPatientsByDoctor(doctor, patientService))
                    .First();
            }

            // Если в отделении нет подходящих врачей, ищем по всей больнице
            var allDoctors = _staffPlacements
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

            return allDoctors
                .OrderBy(doctor => CountPatientsByDoctor(doctor, patientService))
                .First();
        }

        private int CountPatientsByDoctor(IMedicalStaff doctor, PatientManagementService patientService)
        {
            return patientService.GetActivePlacements()
                .Count(p => p.AttendingDoctor == doctor);
        }

        private bool IsSpecializationMatch(MedicalSpecialization doctorSpecialization, SpecializationDep departmentSpecialization)
        {
            switch (departmentSpecialization)
            {
                case SpecializationDep.SurgicalDep:
                    return doctorSpecialization == MedicalSpecialization.Surgeon;
                case SpecializationDep.CardiologyDep:
                    return doctorSpecialization == MedicalSpecialization.Cardiologist;
                case SpecializationDep.GeneralMedicineDep:
                    return doctorSpecialization == MedicalSpecialization.Therapist;
                default:
                    return false;
            }
        }

        public void PrintStaffStatistics()
        {
            int activeStaff = GetActiveStaff().Count;
            int dismissedStaff = _staffPlacements.Values.Count(p => !p.IsActive);

            Console.WriteLine($"\n=== СТАТИСТИКА ПЕРСОНАЛА: {_hospital.Name} ===");
            Console.WriteLine($"Работает: {activeStaff} сотрудников");
            Console.WriteLine($"Уволено: {dismissedStaff} сотрудников");
            Console.WriteLine($"Всего в базе: {_staffPlacements.Count} записей");
        }

        public List<IMedicalStaff> GetAllMedicalStaff()
        {
            return _staffPlacements.Values
                .Where(p => p.IsActive && p.Staff is IMedicalStaff)
                .Select(p => p.Staff as IMedicalStaff)
                .ToList();
        }

        public List<ISupportStaff> GetAllSupportStaff()
        {
            return _staffPlacements.Values
                .Where(p => p.IsActive && p.Staff is ISupportStaff)
                .Select(p => p.Staff as ISupportStaff)
                .ToList();
        }

        public List<IHazardWorkStaff> GetAllHazardStaff()
        {
            return _staffPlacements.Values
                .Where(p => p.IsActive && p.Staff is IHazardWorkStaff)
                .Select(p => p.Staff as IHazardWorkStaff)
                .ToList();
        }
    }
}
