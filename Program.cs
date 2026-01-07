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
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ ИСТОРИИ БОЛЕЗНИ ПРИ ГОСПИТАЛИЗАЦИИ ===\n");

            // Создаем больницу
            var hospital = new Hospital("H001", "Центральная больница", "ул. Центральная, 1");

            // 1. СОЗДАЕМ СТРУКТУРУ БОЛЬНИЦЫ
            Console.WriteLine("1. СОЗДАНИЕ СТРУКТУРЫ БОЛЬНИЦЫ:");
            Console.WriteLine("--------------------------------");

            var building = new Building("B001", "Главный корпус", "ул. Центральная, 1");
            hospital.AddBuilding(building);

            // Создаем несколько отделений
            var surgicalDept = new Department("D001", "Хирургическое отделение",
                SpecializationDep.SurgicalDep);
            var cardiologyDept = new Department("D002", "Кардиологическое отделение",
                SpecializationDep.CardiologyDep);
            var therapyDept = new Department("D003", "Терапевтическое отделение",
                SpecializationDep.GeneralMedicineDep);

            building.AddDepartment(surgicalDept);
            building.AddDepartment(cardiologyDept);
            building.AddDepartment(therapyDept);

            // Создаем палаты
            var ward1 = new Ward(101, 5);
            var ward2 = new Ward(102, 5);
            var ward3 = new Ward(103, 5);

            surgicalDept.AddWard(ward1);
            cardiologyDept.AddWard(ward2);
            therapyDept.AddWard(ward3);

            Console.WriteLine($"Создана больница: {hospital.Name}");
            Console.WriteLine($"Здание: {building.Name}");
            Console.WriteLine($"Отделения: {surgicalDept.Name}, {cardiologyDept.Name}, {therapyDept.Name}");
            Console.WriteLine($"Всего коек: {ward1.TotalBeds + ward2.TotalBeds + ward3.TotalBeds}\n");

            // 2. СОЗДАЕМ ВРАЧЕЙ
            Console.WriteLine("2. НАЙМ ВРАЧЕЙ:");
            Console.WriteLine("---------------");

            var surgeon = new Surgeon("D001", "Петров", "Иван", "Сергеевич",
                "SUR12345", 80000);
            var cardiologist = new Cardiologist("D002", "Сидорова", "Елена", "Александровна",
                "CAR12345", 75000);
            var therapist = new Therapist("D003", "Иванов", "Алексей", "Михайлович",
                "THER12345", 60000);

            hospital.StaffService.HireStaff(surgeon, surgicalDept);
            hospital.StaffService.HireStaff(cardiologist, cardiologyDept);
            hospital.StaffService.HireStaff(therapist, therapyDept);

            Console.WriteLine($"На работу приняты:");
            Console.WriteLine($"  - {surgeon.Surname} {surgeon.Name} ({surgeon.Specialization})");
            Console.WriteLine($"  - {cardiologist.Surname} {cardiologist.Name} ({cardiologist.Specialization})");
            Console.WriteLine($"  - {therapist.Surname} {therapist.Name} ({therapist.Specialization})\n");

            // 3. СОЗДАЕМ ПАЦИЕНТОВ И ДИАГНОЗЫ
            Console.WriteLine("3. СОЗДАНИЕ ПАЦИЕНТОВ И ДИАГНОЗОВ:");
            Console.WriteLine("-----------------------------------");

            var patient1 = new Patient("P001", "Кузнецов", "Дмитрий", "Викторович",
                new DateTime(1978, 4, 12), "111222333");
            var patient2 = new Patient("P002", "Орлова", "Мария", "Сергеевна",
                new DateTime(1985, 8, 25), "444555666");
            var patient3 = new Patient("P003", "Семенов", "Андрей", "Игоревич",
                new DateTime(1992, 11, 3), "777888999");

            // Диагнозы
            var appendicitis = new Diagnosis("K35", "Острый аппендицит",
                "Воспаление червеобразного отростка", SpecializationDep.SurgicalDep);
            var heartAttack = new Diagnosis("I21", "Острый инфаркт миокарда",
                "Некроз сердечной мышцы", SpecializationDep.CardiologyDep);
            var pneumonia = new Diagnosis("J18", "Пневмония",
                "Воспаление легких", SpecializationDep.GeneralMedicineDep);

            Console.WriteLine($"Пациенты:");
            Console.WriteLine($"  - {patient1.Surname} {patient1.Name}");
            Console.WriteLine($"  - {patient2.Surname} {patient2.Name}");
            Console.WriteLine($"  - {patient3.Surname} {patient3.Name}");
            Console.WriteLine();
            Console.WriteLine($"Диагнозы:");
            Console.WriteLine($"  - {appendicitis.Name} ({appendicitis.Code})");
            Console.WriteLine($"  - {heartAttack.Name} ({heartAttack.Code})");
            Console.WriteLine($"  - {pneumonia.Name} ({pneumonia.Code})\n");

            // 4. ГОСПИТАЛИЗАЦИЯ ПАЦИЕНТОВ
            Console.WriteLine("4. ГОСПИТАЛИЗАЦИЯ ПАЦИЕНТОВ:");
            Console.WriteLine("----------------------------");

            // Пациент 1 - Хирургическое отделение
            Console.WriteLine($"\n{patient1.Surname} {patient1.Name} - {appendicitis.Name}:");
            var placement1 = hospital.PatientService.AdmitPatient(patient1, appendicitis);
            if (placement1 != null)
            {
                placement1.AttendingDoctor = surgeon;
                Console.WriteLine($"  Госпитализирован в: {placement1.Department.Name}");
                Console.WriteLine($"  Палата: {placement1.Ward.WardNumber}, Койка: {placement1.BedNumber}");
                Console.WriteLine($"  Лечащий врач: {placement1.AttendingDoctor.Surname}");
            }

            // Пациент 2 - Кардиологическое отделение
            Console.WriteLine($"\n{patient2.Surname} {patient2.Name} - {heartAttack.Name}:");
            var placement2 = hospital.PatientService.AdmitPatient(patient2, heartAttack);
            if (placement2 != null)
            {
                placement2.AttendingDoctor = cardiologist;
                Console.WriteLine($"  Госпитализирован в: {placement2.Department.Name}");
                Console.WriteLine($"  Палата: {placement2.Ward.WardNumber}, Койка: {placement2.BedNumber}");
                Console.WriteLine($"  Лечащий врач: {placement2.AttendingDoctor.Surname}");
            }

            // Пациент 3 - Терапевтическое отделение
            Console.WriteLine($"\n{patient3.Surname} {patient3.Name} - {pneumonia.Name}:");
            var placement3 = hospital.PatientService.AdmitPatient(patient3, pneumonia);
            if (placement3 != null)
            {
                placement3.AttendingDoctor = therapist;
                Console.WriteLine($"  Госпитализирован в: {placement3.Department.Name}");
                Console.WriteLine($"  Палата: {placement3.Ward.WardNumber}, Койка: {placement3.BedNumber}");
                Console.WriteLine($"  Лечащий врач: {placement3.AttendingDoctor.Surname}");
            }

            // 5. ИМИТАЦИЯ ЛЕЧЕНИЯ (несколько дней)
            Console.WriteLine("\n5. ИМИТАЦИЯ ЛЕЧЕНИЯ (5 дней)...\n");

            // 6. ВЫПИСКА ПАЦИЕНТОВ И СОЗДАНИЕ ИСТОРИИ БОЛЕЗНИ
            Console.WriteLine("6. ВЫПИСКА ПАЦИЕНТОВ И СОЗДАНИЕ ИСТОРИИ БОЛЕЗНИ:");
            Console.WriteLine("------------------------------------------------");

            // Выписываем пациентов
            Console.WriteLine($"\nВыписка {patient1.Surname}:");
            hospital.PatientService.DischargePatient(patient1);

            Console.WriteLine($"\nВыписка {patient2.Surname}:");
            hospital.PatientService.DischargePatient(patient2);

            Console.WriteLine($"\nВыписка {patient3.Surname}:");
            hospital.PatientService.DischargePatient(patient3);

            // 7. ВЫВОД ДЕТАЛИЗИРОВАННОЙ ИСТОРИИ БОЛЕЗНИ
            Console.WriteLine("\n7. ДЕТАЛИЗИРОВАННАЯ ИСТОРИЯ БОЛЕЗНИ:");
            Console.WriteLine("------------------------------------");

            // Для каждого пациента выводим полную историю
            Console.WriteLine($"\n=== ИСТОРИИ БОЛЕЗНИ ПАЦИЕНТОВ ===");

            PrintPatientHistory(patient1, "Кузнецов Дмитрий Викторович");
            PrintPatientHistory(patient2, "Орлова Мария Сергеевна");
            PrintPatientHistory(patient3, "Семенов Андрей Игоревич");

            // 8. СТАТИСТИКА
            Console.WriteLine("\n8. СТАТИСТИКА ЛЕЧЕНИЯ:");
            Console.WriteLine("----------------------");

            var activePatients = hospital.PatientService.GetActivePlacements().Count;
            var dischargedPatients = hospital.PatientService.GetDischargedPatients().Count;

            Console.WriteLine($"Больница: {hospital.Name}");
            Console.WriteLine($"  Всего пациентов: {activePatients + dischargedPatients}");
            Console.WriteLine($"  В больнице: {activePatients}");
            Console.WriteLine($"  Выписано: {dischargedPatients}");
            Console.WriteLine($"  Создано историй болезней: {patient1.TreatmentHistory.Count + patient2.TreatmentHistory.Count + patient3.TreatmentHistory.Count}");

            // 9. ДОПОЛНИТЕЛЬНАЯ ДЕМОНСТРАЦИЯ ГЕНЕРАЦИИ
            Console.WriteLine("\n9. ДОПОЛНИТЕЛЬНАЯ ДЕМОНСТРАЦИЯ ГЕНЕРАЦИИ:");
            Console.WriteLine("----------------------------------------");

            // Создаем еще один диагноз и историю вручную
            var testDiagnosis = new Diagnosis("E11", "Сахарный диабет 2 типа",
                "Нарушение углеводного обмена", SpecializationDep.GeneralMedicineDep);

            Console.WriteLine($"\nРучное создание полной истории болезни:");
            var testRecord = TreatmentRecordGenerator.GenerateTreatmentRecord(
                patient: patient1,
                institution: hospital as IMedInstitution,
                diagnosis: testDiagnosis,
                doctors: new List<IMedicalStaff> { therapist, surgeon },
                daysInTreatment: 10
            );

            Console.WriteLine($"\nСоздана тестовая история:");
            PrintTreatmentRecordDetails(testRecord);

            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
            Console.ReadLine();
        }

        // Метод для вывода истории пациента
        private static void PrintPatientHistory(IPatient patient, string fullName)
        {
            Console.WriteLine($"\n──── {fullName} ────");

            if (patient.TreatmentHistory.Count == 0)
            {
                Console.WriteLine("  Историй болезней нет");
                return;
            }

            for (int i = 0; i < patient.TreatmentHistory.Count; i++)
            {
                var record = patient.TreatmentHistory[i];
                Console.WriteLine($"\n  История #{i + 1} ({record.ID}):");
                PrintTreatmentRecordDetails(record);
            }
        }

        // Метод для детализированного вывода истории
        private static void PrintTreatmentRecordDetails(ITreatmentRecord record)
        {
            Console.WriteLine($"    Учреждение: {record.MedInstitution.Name}");
            Console.WriteLine($"    Период: {record.StartDate:dd.MM.yyyy} - {(record.EndDate.HasValue ? record.EndDate.Value.ToString("dd.MM.yyyy") : "лечение продолжается")}");
            Console.WriteLine($"    Длительность: {(record.EndDate.HasValue ? (record.EndDate.Value - record.StartDate).Days : 0)} дней");

            if (record.Diagnoses.Count > 0)
            {
                Console.WriteLine($"    Диагнозы:");
                foreach (var diagnosis in record.Diagnoses)
                {
                    Console.WriteLine($"      - {diagnosis.Name} ({diagnosis.Code})");
                }
            }

            if (record.AttendingDoctors.Count > 0)
            {
                Console.WriteLine($"    Лечащие врачи:");
                foreach (var doctor in record.AttendingDoctors)
                {
                    if (doctor != null)
                        Console.WriteLine($"      - {doctor.Surname} {doctor.Name} ({doctor.Specialization})");
                }
            }

            if (record.Prescriptions.Count > 0)
            {
                Console.WriteLine($"    Назначенные препараты:");
                foreach (var prescription in record.Prescriptions)
                {
                    if (prescription.Doctor != null)
                        Console.WriteLine($"      - {prescription.Medication} {prescription.Dosage} ({prescription.Instructions}) - назначил: {prescription.Doctor.Surname}");
                    else
                        Console.WriteLine($"      - {prescription.Medication} {prescription.Dosage} ({prescription.Instructions})");
                }
            }

            if (record.Procedures.Count > 0)
            {
                Console.WriteLine($"    Выполненные процедуры:");
                foreach (var procedure in record.Procedures)
                {
                    if (procedure.PerformingDoctor != null)
                        Console.WriteLine($"      - {procedure.Description} - результат: {procedure.Result} (выполнил: {procedure.PerformingDoctor.Surname})");
                    else
                        Console.WriteLine($"      - {procedure.Description} - результат: {procedure.Result}");
                }
            }

            if (record.Analyses.Count > 0)
            {
                Console.WriteLine($"    Лабораторные анализы:");
                foreach (var analysis in record.Analyses)
                {
                    Console.WriteLine($"      - {analysis.Name} ({analysis.Type}) - результат: {analysis.Results}");
                }
            }

            Console.WriteLine($"    Всего записей:");
            Console.WriteLine($"      Диагнозов: {record.Diagnoses.Count}");
            Console.WriteLine($"      Рецептов: {record.Prescriptions.Count}");
            Console.WriteLine($"      Процедур: {record.Procedures.Count}");
            Console.WriteLine($"      Анализов: {record.Analyses.Count}");
        }
    }
}