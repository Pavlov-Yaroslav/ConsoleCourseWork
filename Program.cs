using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;
using ConsoleCourceWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork
{
    internal class Program
    {
        static void Main()
        {

            var hospital = new Hospital();
            SetupHospital(hospital);
            RegisterPatients(hospital);
            ShowInfo(hospital);

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void SetupHospital(Hospital hospital)
        {
            var building = new Building("B001", "Главный корпус", "ул. Медицинская, 1");

            var surgery = new Department("D001", "Хирургическое", SpecializationDep.SurgicalDep);
            surgery.AddWard(new Ward(101, 5));
            surgery.AddWard(new Ward(102, 4));

            var xray = new Department("D002", "Рентгенологическое", SpecializationDep.XrayDep);
            xray.AddWard(new Ward(201, 3));

            var cardio = new Department("D003", "Кардиологическое", SpecializationDep.CardiologyDep);
            cardio.AddWard(new Ward(301, 6));

            building.AddDepartment(surgery);
            building.AddDepartment(xray);
            building.AddDepartment(cardio);

            hospital.AddBuilding(building);
        }

        static void RegisterPatients(Hospital hospital)
        {
            var patient1 = new Patient("P001", "Иванов", "Иван", "Иванович",
                new DateTime(1985, 5, 15), "123456789012");

            var diagnosis1 = new Diagnosis("K40", "Паховая грыжа",
                "Паховая грыба без осложнений", SpecializationDep.SurgicalDep);

            hospital.AdmitPatient(patient1, diagnosis1);

            var patient2 = new Patient("P002", "Петрова", "Мария", "Сергеевна",
                new DateTime(1990, 8, 22), "987654321098");

            var diagnosis2 = new Diagnosis("I20", "Стенокардия",
                "Стенокардия напряжения", SpecializationDep.CardiologyDep);

            hospital.AdmitPatient(patient2, diagnosis2);
        }

        static void ShowInfo(Hospital hospital)
        {
            Console.WriteLine("\n=== Информация о больнице ===");
            Console.WriteLine(hospital);

            Console.WriteLine("\n=== Размещения пациентов ===");
            foreach (var placement in hospital.PatientPlacements.Values)
            {
                Console.WriteLine(placement);
                Console.WriteLine("---");
            }

            Console.WriteLine("\n=== Инфраструктура ===");
            foreach (var building in hospital.Buildings)
            {
                Console.WriteLine(building);
                foreach (var department in building.Departments)
                {
                    Console.WriteLine($"  {department}");
                }
            }
        }
    }
}