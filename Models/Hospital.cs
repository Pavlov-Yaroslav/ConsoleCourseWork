using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;

namespace ConsoleCourceWork.Models
{
    public class Hospital : Interfaces.IHospital
    {
        public List<Building> Buildings { get; private set; } = new List<Building>();
        public Dictionary<Interfaces.IPatient, PatientPlacement> PatientPlacements { get; private set; } = new Dictionary<Interfaces.IPatient, PatientPlacement>();

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
            Console.WriteLine($"\nРегистрация пациента: {patient.Surname} {patient.Name}");
            Console.WriteLine($"Диагноз: {diagnosis.Description}");

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
            return $"Больница: {Buildings.Count} зданий, {PatientPlacements.Count} пациентов";
        }
    }
}
