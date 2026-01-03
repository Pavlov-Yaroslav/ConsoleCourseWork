using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;
using ConsoleCourceWork.Models;
using ConsoleCourceWork.Services;
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
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Тест генерации уникальных ID ===\n");

            // Тестируем генератор
            Console.WriteLine("Генерация 5 ID подряд:");
            for (int i = 0; i < 5; i++)
            {
                var id = IdGenerator.GenerateTreatmentRecordId();
                Console.WriteLine($"  {i + 1}. {id}");
                System.Threading.Thread.Sleep(10); // Минимальная задержка
            }

            Console.WriteLine("\n=== Основная демонстрация ===\n");

            var hospital = new Hospital("H001", "Городская больница", "ул. Центральная, 1");
            SetupHospital(hospital);

            var patient = new Patient("P001", "Иванов", "Иван", "Иванович",
                new DateTime(1985, 5, 15), "123456789012");

            // Первая госпитализация
            Console.WriteLine("\n=== Первая госпитализация ===");
            var diagnosis1 = new Diagnosis("K40", "Паховая грыжа",
                "Паховая грыжа без осложнений", SpecializationDep.SurgicalDep);

            hospital.AdmitPatient(patient, diagnosis1);
            hospital.DischargePatient(patient);

            // Вторая госпитализация (быстро, чтобы проверить уникальность)
            Console.WriteLine("\n=== Вторая госпитализация (сразу после первой) ===");
            var diagnosis2 = new Diagnosis("J18", "Пневмония",
                "Воспаление легких", SpecializationDep.GeneralMedicineDep);

            hospital.AdmitPatient(patient, diagnosis2);
            hospital.DischargePatient(patient);

            // Третья госпитализация
            Console.WriteLine("\n=== Третья госпитализация ===");
            var diagnosis3 = new Diagnosis("I20", "Стенокардия",
                "Стенокардия напряжения", SpecializationDep.CardiologyDep);

            hospital.AdmitPatient(patient, diagnosis3);
            hospital.DischargePatient(patient);

            // Проверяем уникальность
            Console.WriteLine("\n=== Проверка уникальности ID ===");
            var uniqueIds = new System.Collections.Generic.HashSet<string>();

            foreach (var record in patient.TreatmentHistory)
            {
                Console.WriteLine($"  ID: {record.ID}");
                uniqueIds.Add(record.ID);
            }

            Console.WriteLine($"\nВсего записей: {patient.TreatmentHistory.Count}");
            Console.WriteLine($"Уникальных ID: {uniqueIds.Count}");

            if (uniqueIds.Count == patient.TreatmentHistory.Count)
            {
                Console.WriteLine("✓ Все ID уникальны!");
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void SetupHospital(Hospital hospital)
        {
            var building = new Building("B001", "Главный корпус", "ул. Центральная, 1");

            var surgery = new Department("D001", "Хирургия", SpecializationDep.SurgicalDep);
            surgery.AddWard(new Ward(101, 5));

            var therapy = new Department("D002", "Терапия", SpecializationDep.GeneralMedicineDep);
            therapy.AddWard(new Ward(201, 6));

            building.AddDepartment(surgery);
            building.AddDepartment(therapy);

            hospital.AddBuilding(building);
        }

        static void SetupHospitalInfrastructure(Hospital hospital)
        {
            var building1 = new Building("B001", "Главный корпус", "ул. Медицинская, 15");
            var building2 = new Building("B002", "Поликлиника", "ул. Медицинская, 15А");

            var surgery = new Department("D001", "Хирургическое отделение", SpecializationDep.SurgicalDep);
            surgery.AddWard(new Ward(101, 5));
            surgery.AddWard(new Ward(102, 4));

            var cardio = new Department("D002", "Кардиологическое отделение", SpecializationDep.CardiologyDep);
            cardio.AddWard(new Ward(201, 6));

            var xray = new Department("D003", "Рентгенологическое отделение", SpecializationDep.XrayDep);
            xray.AddWard(new Ward(301, 3));

            building1.AddDepartment(surgery);
            building1.AddDepartment(cardio);
            building2.AddDepartment(xray);

            hospital.AddBuilding(building1);
            hospital.AddBuilding(building2);
        }

        static (Patient patient1, Patient patient2, Patient patient3) RegisterPatients(Hospital hospital)
        {
            var patient1 = new Patient("P001", "Иванов", "Иван", "Иванович",
                new DateTime(1985, 5, 15), "123456789012");

            var diagnosis1 = new Diagnosis("K40", "Паховая грыжа",
                "Паховая грыжа без осложнений", SpecializationDep.SurgicalDep);

            hospital.AdmitPatient(patient1, diagnosis1);

            var patient2 = new Patient("P002", "Петрова", "Мария", "Сергеевна",
                new DateTime(1990, 8, 22), "987654321098");

            var diagnosis2 = new Diagnosis("I20", "Стенокардия",
                "Стенокардия напряжения", SpecializationDep.CardiologyDep);

            hospital.AdmitPatient(patient2, diagnosis2);

            var patient3 = new Patient("P003", "Сидоров", "Алексей", "Петрович",
                new DateTime(1978, 3, 10), "456123789045");

            var diagnosis3 = new Diagnosis("R91", "Патология легких",
                "Изменения на рентгене", SpecializationDep.XrayDep);

            hospital.AdmitPatient(patient3, diagnosis3);

            return (patient1, patient2, patient3);
        }

        static void ShowHospitalInfo(Hospital hospital)
        {
            Console.WriteLine("\n=== Информация о больнице ===");
            Console.WriteLine(hospital);

            Console.WriteLine("\n=== Подробная информация о пациентах ===");
            var activePatients = hospital.GetActivePlacements();
            if (activePatients.Count == 0)
            {
                Console.WriteLine("Нет пациентов в больнице.");
            }
            else
            {
                foreach (var placement in activePatients)
                {
                    Console.WriteLine($"- {placement.Patient.Surname} {placement.Patient.Name} " +
                                     $"(палата {placement.Ward.WardNumber}, койка {placement.BedNumber})");
                }
            }

            Console.WriteLine("\n=== Здания и отделения ===");
            foreach (var building in hospital.Buildings)
            {
                Console.WriteLine($"Здание: {building.Name}");
                foreach (var department in building.Departments)
                {
                    Console.WriteLine($"  - {department.Name} ({department.Specialization})");
                }
            }
        }
    }
}