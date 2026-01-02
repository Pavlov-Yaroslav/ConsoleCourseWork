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
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Система управления больницей ===\n");

            var hospital = new Hospital();
            SetupHospital(hospital);

            var (patient1, patient2, patient3) = RegisterPatients(hospital);

            ShowInfo(hospital);

            Console.WriteLine("\n=== Управление пациентами ===");

            // Показываем активных пациентов
            Console.WriteLine("\n1. Активные пациенты в больнице:");
            var activePatients = hospital.GetActivePlacements();
            if (activePatients.Count > 0)
            {
                foreach (var placement in activePatients)
                {
                    Console.WriteLine($"- {placement.Patient.Surname} {placement.Patient.Name} " +
                                     $"({placement.Department.Name})");
                }
            }
            else
            {
                Console.WriteLine("Нет активных пациентов.");
            }

            // Выписываем пациента 1
            Console.WriteLine("\n2. Выписываем пациента 1:");
            hospital.DischargePatient(patient1);

            // Показываем активных после выписки
            Console.WriteLine("\n3. Активные пациенты после выписки:");
            ShowActivePatients(hospital);

            // Показываем выписанных
            Console.WriteLine("\n4. Выписанные пациенты:");
            ShowDischargedPatients(hospital);

            // Удаляем пациента 2
            Console.WriteLine("\n5. Удаляем пациента 2 из системы:");
            hospital.DeletePatient(patient2);

            // Показываем итоговую информацию
            Console.WriteLine("\n6. Финальное состояние больницы:");
            Console.WriteLine(hospital);

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void SetupHospital(Hospital hospital)
        {
            var building = new Building("B001", "Главный корпус", "ул. Медицинская, 1");

            var surgery = new Department("D001", "Хирургическое", SpecializationDep.SurgicalDep);
            surgery.AddWard(new Ward(101, 5));
            surgery.AddWard(new Ward(102, 4));

            var cardio = new Department("D002", "Кардиологическое", SpecializationDep.CardiologyDep);
            cardio.AddWard(new Ward(201, 6));

            var neurology = new Department("D003", "Неврологическое", SpecializationDep.NeurologyDep);
            neurology.AddWard(new Ward(301, 4));

            building.AddDepartment(surgery);
            building.AddDepartment(cardio);
            building.AddDepartment(neurology);

            hospital.AddBuilding(building);
        }

        static (Patient patient1, Patient patient2, Patient patient3) RegisterPatients(Hospital hospital)
        {
            // Пациент 1 - Хирургия
            var patient1 = new Patient("P001", "Иванов", "Иван", "Иванович",
                new DateTime(1985, 5, 15), "123456789012");

            var diagnosis1 = new Diagnosis("K40", "Паховая грыжа",
                "Паховая грыжа без осложнений", SpecializationDep.SurgicalDep);

            hospital.AdmitPatient(patient1, diagnosis1);

            // Пациент 2 - Кардиология
            var patient2 = new Patient("P002", "Петрова", "Мария", "Сергеевна",
                new DateTime(1990, 8, 22), "987654321098");

            var diagnosis2 = new Diagnosis("I20", "Стенокардия",
                "Стенокардия напряжения", SpecializationDep.CardiologyDep);

            hospital.AdmitPatient(patient2, diagnosis2);

            // Пациент 3 - Неврология
            var patient3 = new Patient("P003", "Сидоров", "Алексей", "Петрович",
                new DateTime(1978, 3, 10), "456789123456");

            var diagnosis3 = new Diagnosis("G40", "Эпилепсия",
                "Эпилептические припадки", SpecializationDep.NeurologyDep);

            hospital.AdmitPatient(patient3, diagnosis3);

            return (patient1, patient2, patient3);
        }

        static void ShowInfo(Hospital hospital)
        {
            Console.WriteLine("\n=== Информация о больнице ===");
            Console.WriteLine(hospital);

            Console.WriteLine("\n=== Все размещения пациентов ===");
            foreach (var placement in hospital.PatientPlacements.Values)
            {
                Console.WriteLine(placement);
                Console.WriteLine("---");
            }
        }

        static void ShowActivePatients(Hospital hospital)
        {
            var active = hospital.GetActivePlacements();
            if (active.Count == 0)
            {
                Console.WriteLine("Нет активных пациентов.");
                return;
            }

            Console.WriteLine($"Всего активных пациентов: {active.Count}");
            foreach (var placement in active)
            {
                Console.WriteLine($"- {placement.Patient.Surname} {placement.Patient.Name} " +
                                 $"(палата {placement.Ward.WardNumber}, койка {placement.BedNumber})");
            }
        }

        static void ShowDischargedPatients(Hospital hospital)
        {
            var discharged = hospital.GetDischargedPatients();
            if (discharged.Count == 0)
            {
                Console.WriteLine("Нет выписанных пациентов.");
                return;
            }

            Console.WriteLine($"Всего выписанных пациентов: {discharged.Count}");
            foreach (var placement in discharged)
            {
                Console.WriteLine($"- {placement.Patient.Surname} {placement.Patient.Name} " +
                                 $"(выписан: {placement.DischargeDate:dd.MM.yyyy})");
            }
        }
    }
}