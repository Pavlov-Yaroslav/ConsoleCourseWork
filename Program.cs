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
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ МЕДИЦИНСКОЙ СИСТЕМЫ ===\n");

            // Инициализируем менеджеры
            var clinicManager = ClinicAttachmentManager.Instance;
            var patientManager = PatientAttachmentManager.Instance;

            // 1. СОЗДАЕМ УЧРЕЖДЕНИЯ
            Console.WriteLine("1. СОЗДАНИЕ УЧРЕЖДЕНИЙ:");
            Console.WriteLine("-----------------------");

            var hospital = new Hospital("H001", "Городская больница №1", "ул. Ленина, 1");
            var clinic1 = new Clinic("C001", "Поликлиника №3", "ул. Мира, 15");
            var clinic2 = new Clinic("C002", "Детская поликлиника", "ул. Пушкина, 10");

            Console.WriteLine($"Создана больница: {hospital.Name}");
            Console.WriteLine($"Созданы клиники: {clinic1.Name}, {clinic2.Name}");

            // 2. ПРИКРЕПЛЯЕМ КЛИНИКИ К БОЛЬНИЦЕ
            Console.WriteLine("\n2. ПРИКРЕПЛЕНИЕ КЛИНИК К БОЛЬНИЦЕ:");
            Console.WriteLine("----------------------------------");

            clinicManager.Attach(clinic1, hospital);
            clinicManager.Attach(clinic2, hospital);

            // Проверяем связи
            var attachedClinics = clinicManager.GetClinicsForHospital(hospital);
            Console.WriteLine($"\nК больнице '{hospital.Name}' прикреплено: {attachedClinics.Count} клиник");

            // 3. СОЗДАЕМ ПАЦИЕНТОВ
            Console.WriteLine("\n3. СОЗДАНИЕ ПАЦИЕНТОВ:");
            Console.WriteLine("----------------------");

            var patient1 = new Patient("P001", "Иванов", "Иван", "Иванович",
                                      new DateTime(1980, 5, 15), "1234567890");
            var patient2 = new Patient("P002", "Петрова", "Мария", "Сергеевна",
                                      new DateTime(1992, 3, 22), "9876543210");
            var patient3 = new Patient("P003", "Сидоров", "Алексей", "Петрович",
                                      new DateTime(1975, 11, 8), "5555555555");

            Console.WriteLine($"Созданы пациенты: {patient1.Surname}, {patient2.Surname}, {patient3.Surname}");

            // 4. ПРИКРЕПЛЯЕМ ПАЦИЕНТОВ К КЛИНИКАМ
            Console.WriteLine("\n4. ПРИКРЕПЛЕНИЕ ПАЦИЕНТОВ К КЛИНИКАМ:");
            Console.WriteLine("-------------------------------------");

            patientManager.Attach(patient1, clinic1);
            patientManager.Attach(patient2, clinic2);
            patientManager.Attach(patient3, clinic1);

            // Пробуем прикрепить повторно (должна быть ошибка)
            Console.WriteLine("\nПопытка повторного прикрепления:");
            patientManager.Attach(patient1, clinic1);

            // 5. ПРОВЕРЯЕМ ПРИКРЕПЛЕНИЯ
            Console.WriteLine("\n5. ПРОВЕРКА ПРИКРЕПЛЕНИЙ:");
            Console.WriteLine("------------------------");

            Console.WriteLine($"Клиника пациента Иванова: {patientManager.GetClinicForPatient(patient1)?.Name}");
            Console.WriteLine($"Клиника пациента Петровой: {patientManager.GetClinicForPatient(patient2)?.Name}");
            Console.WriteLine($"Прикреплен ли Сидоров? {patientManager.IsAttached(patient3)}");

            // 6. ПРОВЕРЯЕМ КЛИНИКИ
            Console.WriteLine("\n6. СТАТИСТИКА КЛИНИК:");
            Console.WriteLine("--------------------");

            Console.WriteLine($"Пациентов в '{clinic1.Name}': {patientManager.GetPatientCount(clinic1)}");
            Console.WriteLine($"Пациентов в '{clinic2.Name}': {patientManager.GetPatientCount(clinic2)}");

            // 7. ТЕСТИРУЕМ УСЛУГИ КЛИНИК
            Console.WriteLine("\n7. ТЕСТИРОВАНИЕ УСЛУГ:");
            Console.WriteLine("----------------------");

            Console.WriteLine("\nПопытка получить услуги:");
            clinic1.ProvideSimpleService(patient1, "Консультация терапевта");
            clinic1.ProvideSimpleService(patient2, "Консультация терапевта");
            clinic2.ProvideSimpleService(patient2, "Прививка");

            // 8. ОТКРЕПЛЕНИЕ И ПЕРЕНОС
            Console.WriteLine("\n8. ОТКРЕПЛЕНИЕ И ПЕРЕНОС:");
            Console.WriteLine("-------------------------");

            // Открепляем пациента
            patientManager.Detach(patient3);
            Console.WriteLine($"Пациентов в '{clinic1.Name}': {patientManager.GetPatientCount(clinic1)}");

            // Прикрепляем к другой клинике
            patientManager.Attach(patient3, clinic2);
            Console.WriteLine($"Пациентов в '{clinic2.Name}': {patientManager.GetPatientCount(clinic2)}");

            // 9. ТЕСТ: ПРИКРЕПЛЕНИЕ К НЕСКОЛЬКИМ КЛИНИКАМ
            Console.WriteLine("\n9. ТЕСТ: ПРИКРЕПЛЕНИЕ К НЕСКОЛЬКИМ КЛИНИКАМ:");
            Console.WriteLine("-------------------------------------------");

            var testPatient = new Patient("P004", "Васильев", "Дмитрий", "Андреевич",
                                        new DateTime(1990, 7, 20), "999888777");

            Console.WriteLine("Попытка прикрепить Васильева к Поликлинике №3:");
            patientManager.Attach(testPatient, clinic1);

            Console.WriteLine("\nПопытка прикрепить того же Васильева к Детской поликлинике:");
            patientManager.Attach(testPatient, clinic2);

            // Проверяем
            var clinic = patientManager.GetClinicForPatient(testPatient);
            Console.WriteLine($"\nК какой клинике прикреплен Васильев? {clinic?.Name}");

            // 10. ФИНАЛЬНАЯ СТАТИСТИКА
            Console.WriteLine("\n10. ФИНАЛЬНАЯ СТАТИСТИКА:");
            Console.WriteLine("------------------------");

            Console.WriteLine($"Больница: {hospital.Name}");
            Console.WriteLine($"  Прикреплено клиник: {clinicManager.GetClinicCount(hospital)}");

            Console.WriteLine($"\nКлиника '{clinic1.Name}':");
            var patients1 = patientManager.GetPatientsForClinic(clinic1);
            Console.WriteLine($"  Пациентов: {patients1.Count}");
            foreach (var p in patients1)
                Console.WriteLine($"    - {p.Surname}");

            Console.WriteLine($"\nКлиника '{clinic2.Name}':");
            var patients2 = patientManager.GetPatientsForClinic(clinic2);
            Console.WriteLine($"  Пациентов: {patients2.Count}");
            foreach (var p in patients2)
                Console.WriteLine($"    - {p.Surname}");

            // 11. ДЕМОНСТРАЦИЯ ИСТОРИИ БОЛЕЗНИ
            Console.WriteLine("\n11. ДЕМОНСТРАЦИЯ ИСТОРИИ БОЛЕЗНИ:");
            Console.WriteLine("--------------------------------");

            // Создаем врача
            var cardiologist = new Cardiologist("D001", "Смирнов", "Александр", "Иванович",
                "LIC12345", 50000);

            // Создаем диагноз
            var diagnosis = new Diagnosis("I10", "Гипертония",
                "Повышенное давление", SpecializationDep.CardiologyDep);

            // 11.1 Ручное создание
            Console.WriteLine("\n11.1 РУЧНОЕ СОЗДАНИЕ:");
            var manualPrescription = TreatmentRecordGenerator.CreatePrescription(
                cardiologist, "Аспирин", "500 мг", "Принимать 2 раза в день");
            Console.WriteLine($"Ручной рецепт: {manualPrescription.Medication}");

            var manualProcedure = TreatmentRecordGenerator.CreateProcedure(
                cardiologist, "Измерение давления", "Давление 140/90");
            Console.WriteLine($"Ручная процедура: {manualProcedure.Description}");

            var manualAnalysis = TreatmentRecordGenerator.CreateAnalysis(
                "Анализ крови", LabProfileType.Clinical, "Гемоглобин 145 г/л");
            Console.WriteLine($"Ручной анализ: {manualAnalysis.Name}");

            // 11.2 Автоматическая генерация
            Console.WriteLine("\n11.2 АВТОМАТИЧЕСКАЯ ГЕНЕРАЦИЯ:");
            var autoPrescription = TreatmentRecordGenerator.GenerateRandomPrescription(cardiologist);
            Console.WriteLine($"Авто-рецепт: {autoPrescription.Medication} {autoPrescription.Dosage}");

            var autoProcedure = TreatmentRecordGenerator.GenerateRandomProcedure(cardiologist);
            Console.WriteLine($"Авто-процедура: {autoProcedure.Description}");

            var autoAnalysis = TreatmentRecordGenerator.GenerateRandomAnalysis();
            Console.WriteLine($"Авто-анализ: {autoAnalysis.Name}");

            // 11.3 Полная история болезни
            Console.WriteLine("\n11.3 ПОЛНАЯ ИСТОРИЯ БОЛЕЗНИ:");
            var doctors = new List<IMedicalStaff> { cardiologist };

            var fullRecord = TreatmentRecordGenerator.GenerateTreatmentRecord(
                patient: patient1,
                institution: hospital as IMedInstitution,
                diagnosis: diagnosis,
                doctors: doctors,
                daysInTreatment: 5
            );

            Console.WriteLine($"Создана история болезни #{fullRecord.ID}");
            Console.WriteLine($"Диагноз: {fullRecord.Diagnoses[0].Name}");
            Console.WriteLine($"Рецептов: {fullRecord.Prescriptions.Count}");
            Console.WriteLine($"Процедур: {fullRecord.Procedures.Count}");
            Console.WriteLine($"Анализов: {fullRecord.Analyses.Count}");

            // 11.4 Интеграция с выпиской
            Console.WriteLine("\n11.4 ИНТЕГРАЦИЯ С ВЫПИСКОЙ:");

            // Создаем структуру больницы для госпитализации
            Console.WriteLine("\nСоздание структуры больницы:");
            var building = new Building("B001", "Главный корпус", "ул. Ленина, 1");
            hospital.AddBuilding(building);

            var cardiologyDept = new Department("D001", "Кардиологическое отделение",
                SpecializationDep.CardiologyDep);
            building.AddDepartment(cardiologyDept);

            var ward = new Ward(101, 10);
            cardiologyDept.AddWard(ward);

            Console.WriteLine($"Создано: здание '{building.Name}', отделение '{cardiologyDept.Name}', палата на {ward.TotalBeds} коек");

            // Нанимаем врача
            hospital.StaffService.HireStaff(cardiologist, cardiologyDept);
            Console.WriteLine($"Принят на работу: {cardiologist.Surname} {cardiologist.Name}");

            // Госпитализируем пациента
            Console.WriteLine("\nГоспитализация пациента:");
            var placement = hospital.PatientService.AdmitPatient(patient1, diagnosis);

            if (placement != null)
            {
                placement.AttendingDoctor = cardiologist;

                Console.WriteLine($"Пациент {patient1.Surname} госпитализирован:");
                Console.WriteLine($"  Отделение: {placement.Department.Name}");
                Console.WriteLine($"  Палата: {placement.Ward.WardNumber}, Койка: {placement.BedNumber}");
                Console.WriteLine($"  Врач: {placement.AttendingDoctor.Surname}");

                // Выписываем пациента (автоматически генерируется история)
                Console.WriteLine("\nВыписка пациента:");
                hospital.PatientService.DischargePatient(patient1);

                // Проверяем
                Console.WriteLine($"\nИсторий болезней у {patient1.Surname}: {patient1.TreatmentHistory.Count}");

                if (patient1.TreatmentHistory.Count > 0)
                {
                    var lastRecord = patient1.TreatmentHistory[patient1.TreatmentHistory.Count - 1];
                    Console.WriteLine($"\nДетали последней записи:");
                    Console.WriteLine($"  ID: {lastRecord.ID}");
                    Console.WriteLine($"  Учреждение: {lastRecord.MedInstitution.Name}");
                    Console.WriteLine($"  Диагноз: {lastRecord.Diagnoses[0].Name}");
                    Console.WriteLine($"  Период: {lastRecord.StartDate:dd.MM.yyyy} - {lastRecord.EndDate:dd.MM.yyyy}");
                    Console.WriteLine($"  Врачей: {lastRecord.AttendingDoctors.Count}");

                    if (lastRecord.Prescriptions.Count > 0)
                    {
                        Console.WriteLine("\n  Рецепты:");
                        foreach (var p in lastRecord.Prescriptions)
                            Console.WriteLine($"    - {p.Medication} ({p.Dosage})");
                    }
                }
            }

            // 12. ТЕСТ СИНХРОНИЗАЦИИ СПИСКОВ
            Console.WriteLine("\n12. ТЕСТ СИНХРОНИЗАЦИИ СПИСКОВ КЛИНИК:");
            Console.WriteLine("-------------------------------------");

            var testPatient2 = new Patient("P005", "Кузнецов", "Сергей", "Викторович",
                                         new DateTime(1985, 9, 12), "111222333");

            Console.WriteLine($"Список пациентов Поликлиники №3 до прикрепления: {clinic1.GetPatientCount()}");

            // Прикрепляем
            patientManager.Attach(testPatient2, clinic1);
            Console.WriteLine($"Список пациентов Поликлиники №3 после прикрепления: {clinic1.GetPatientCount()}");

            // Сравниваем через менеджер и через клинику
            var patientsFromManager = patientManager.GetPatientsForClinic(clinic1);
            var patientsFromClinic = clinic1.GetRegisteredPatients();

            Console.WriteLine($"\nСравнение списков:");
            Console.WriteLine($"Из менеджера: {patientsFromManager.Count} пациентов");
            Console.WriteLine($"Из клиники: {patientsFromClinic.Count} пациентов");
            Console.WriteLine($"Совпадают? {patientsFromManager.Count == patientsFromClinic.Count}");

            // Открепляем
            Console.WriteLine($"\nОткрепляем пациента...");
            patientManager.Detach(testPatient2);

            Console.WriteLine($"Список пациентов Поликлиники №3 после открепления: {clinic1.GetPatientCount()}");

            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===");
            Console.ReadLine();
        }
    }
}