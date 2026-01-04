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
            Console.WriteLine("=== СИСТЕМА РАСПРЕДЕЛЕНИЯ МЕДИЦИНСКИХ СОТРУДНИКОВ ===\n");

            // Создаем больницу
            var hospital = new Hospital("H001", "Городская клиническая больница №1", "ул. Медицинская, 1");

            // Настраиваем инфраструктуру
            SetupHospitalInfrastructure(hospital);

            // Нанимаем медицинский персонал
            HireMedicalStaff(hospital);

            // Регистрируем пациентов с назначением врачей
            Console.WriteLine("\n=== РЕГИСТРАЦИЯ ПАЦИЕНТОВ И НАЗНАЧЕНИЕ ВРАЧЕЙ ===\n");
            var patients = RegisterPatients(hospital);

            // Показываем информацию о больнице
            ShowHospitalInfo(hospital);

            // Демонстрация работы с персоналом
            DemonstrateStaffOperations(hospital);

            // Демонстрация поиска и назначения врачей
            DemonstrateDoctorAssignment(hospital, patients);

            // Дополнительные демонстрации
            DemonstrateAdditionalFeatures(hospital, patients);

            // ================================================
            // НОВАЯ ЧАСТЬ: ДЕМОНСТРАЦИЯ РАБОТЫ С ПОЛИКЛИНИКАМИ
            // ================================================

            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("ДЕМОНСТРАЦИЯ РАБОТЫ С ПОЛИКЛИНИКАМИ И БОЛЬНИЦАМИ");
            Console.WriteLine(new string('=', 70));

            // 1. Демонстрация прикреплений поликлиник к больницам (множественное)
            Console.WriteLine("\n[Часть 1: Прикрепление нескольких поликлиник к разным больницам]");
            DemonstrateClinicAttachment(hospital);

            // 2. Демонстрация операций внутри одной поликлиники
            Console.WriteLine("\n[Часть 2: Операции внутри одной поликлиники]");
            DemonstrateClinicOperations();

            // 3. Демонстрация оптимизированной архитектуры больницы
            Console.WriteLine("\n[Часть 3: Оптимизированная архитектура больницы]");
            DemonstrateRefactoredHospital();

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        // СУЩЕСТВУЮЩИЕ МЕТОДЫ (остаются без изменений)
        static void SetupHospitalInfrastructure(Hospital hospital)
        {
            Console.WriteLine("Настройка инфраструктуры больницы...");

            var mainBuilding = new Building("B001", "Главный корпус", "ул. Медицинская, 1");
            var diagnosticBuilding = new Building("B002", "Диагностический центр", "ул. Медицинская, 1А");

            // Хирургическое отделение
            var surgery = new Department("D001", "Хирургическое отделение", SpecializationDep.SurgicalDep);
            surgery.AddWard(new Ward(101, 4));
            surgery.AddWard(new Ward(102, 4));

            // Кардиологическое отделение
            var cardio = new Department("D002", "Кардиологическое отделение", SpecializationDep.CardiologyDep);
            cardio.AddWard(new Ward(201, 3));
            cardio.AddWard(new Ward(202, 3));

            // Терапевтическое отделение
            var therapy = new Department("D003", "Терапевтическое отделение", SpecializationDep.GeneralMedicineDep);
            therapy.AddWard(new Ward(301, 5));

            // Рентгенологическое отделение
            var xray = new Department("D004", "Рентгенологическое отделение", SpecializationDep.XrayDep);
            xray.AddWard(new Ward(401, 2));

            mainBuilding.AddDepartment(surgery);
            mainBuilding.AddDepartment(cardio);
            mainBuilding.AddDepartment(therapy);
            diagnosticBuilding.AddDepartment(xray);

            hospital.AddBuilding(mainBuilding);
            hospital.AddBuilding(diagnosticBuilding);

            Console.WriteLine("✓ Инфраструктура настроена: 2 здания, 4 отделения");
        }

        static void HireMedicalStaff(Hospital hospital)
        {
            Console.WriteLine("\n=== ПРИЕМ НА РАБОТУ МЕДИЦИНСКОГО ПЕРСОНАЛА ===\n");

            // Получаем отделения
            var surgeryDept = hospital.Buildings[0].Departments[0]; // Хирургическое
            var cardioDept = hospital.Buildings[0].Departments[1];  // Кардиологическое
            var therapyDept = hospital.Buildings[0].Departments[2]; // Терапевтическое

            // Создаем и нанимаем хирургов
            var surgeon1 = new Surgeon("S001", "Иванов", "Петр", "Сергеевич",
                "LIC-SURG-001", 150000);
            surgeon1.SetAcademicTitle(AcademicTitle.Professor);
            surgeon1.SetAcademicDegree(AcademicDegree.DoctorOfMedicalSciences);
            surgeon1.UpdateOpStats(200, 2);

            var surgeon2 = new Surgeon("S002", "Смирнов", "Алексей", "Владимирович",
                "LIC-SURG-002", 120000);
            surgeon2.SetAcademicTitle(AcademicTitle.AssistantProfessor);
            surgeon2.UpdateOpStats(80, 1);

            // Создаем и нанимаем терапевтов
            var therapist1 = new Therapist("T001", "Петрова", "Мария", "Ивановна",
                "LIC-THER-001", 80000);
            therapist1.SetAcademicDegree(AcademicDegree.CandidateOfMedicalSciences);

            var therapist2 = new Therapist("T002", "Сидорова", "Ольга", "Александровна",
                "LIC-THER-002", 75000);

            // Создаем и нанимаем кардиолога
            var cardiologist = new Cardiologist("C001", "Кузнецов", "Андрей", "Михайлович",
                "LIC-CARD-001", 110000);
            cardiologist.SetAcademicDegree(AcademicDegree.CandidateOfMedicalSciences);

            // Принимаем на работу и распределяем по отделениям
            hospital.StaffService.HireStaff(surgeon1, surgeryDept);
            hospital.StaffService.HireStaff(surgeon2, surgeryDept);
            hospital.StaffService.HireStaff(therapist1, therapyDept);
            hospital.StaffService.HireStaff(therapist2, therapyDept);
            hospital.StaffService.HireStaff(cardiologist, cardioDept);

            Console.WriteLine($"\n✓ Персонал нанят: {hospital.StaffService.GetActiveStaff().Count} сотрудников");
            Console.WriteLine("  Распределение:");
            Console.WriteLine($"    Хирурги: {hospital.StaffService.GetStaffByDepartment(surgeryDept).Count}");
            Console.WriteLine($"    Кардиологи: {hospital.StaffService.GetStaffByDepartment(cardioDept).Count}");
            Console.WriteLine($"    Терапевты: {hospital.StaffService.GetStaffByDepartment(therapyDept).Count}");
        }

        static (Patient patient1, Patient patient2, Patient patient3, Patient patient4) RegisterPatients(Hospital hospital)
        {
            Console.WriteLine("\n=== РЕГИСТРАЦИЯ ПАЦИЕНТОВ ===\n");

            // Пациент 1 - хирургический
            var patient1 = new Patient("P001", "Сидоров", "Алексей", "Владимирович",
                new DateTime(1975, 3, 12), "INS00123456789");
            var diagnosis1 = new Diagnosis("K40", "Паховая грыжа",
                "Паховая грыжа без осложнений", SpecializationDep.SurgicalDep);

            Console.WriteLine($"Пациент: {patient1.Surname} {patient1.Name}");
            Console.WriteLine($"Диагноз: {diagnosis1.Name}");
            var placement1 = hospital.PatientService.AdmitPatient(patient1, diagnosis1);

            // Пациент 2 - кардиологический
            var patient2 = new Patient("P002", "Васильева", "Елена", "Петровна",
                new DateTime(1968, 7, 25), "INS00234567890");
            var diagnosis2 = new Diagnosis("I20", "Стенокардия напряжения",
                "Ишемическая болезнь сердца", SpecializationDep.CardiologyDep);

            Console.WriteLine($"\nПациент: {patient2.Surname} {patient2.Name}");
            Console.WriteLine($"Диагноз: {diagnosis2.Name}");
            var placement2 = hospital.PatientService.AdmitPatient(patient2, diagnosis2);

            // Пациент 3 - терапевтический
            var patient3 = new Patient("P003", "Козлов", "Дмитрий", "Сергеевич",
                new DateTime(1982, 11, 8), "INS00345678901");
            var diagnosis3 = new Diagnosis("J18", "Пневмония",
                "Внебольничная пневмония", SpecializationDep.GeneralMedicineDep);

            Console.WriteLine($"\nПациент: {patient3.Surname} {patient3.Name}");
            Console.WriteLine($"Диагноз: {diagnosis3.Name}");
            var placement3 = hospital.PatientService.AdmitPatient(patient3, diagnosis3);

            // Пациент 4 - еще один хирургический
            var patient4 = new Patient("P004", "Николаев", "Сергей", "Андреевич",
                new DateTime(1990, 4, 18), "INS00456789012");
            var diagnosis4 = new Diagnosis("K35", "Острый аппендицит",
                "Воспаление червеобразного отростка", SpecializationDep.SurgicalDep);

            Console.WriteLine($"\nПациент: {patient4.Surname} {patient4.Name}");
            Console.WriteLine($"Диагноз: {diagnosis4.Name}");
            var placement4 = hospital.PatientService.AdmitPatient(patient4, diagnosis4);

            return (patient1, patient2, patient3, patient4);
        }

        static void ShowHospitalInfo(Hospital hospital)
        {
            Console.WriteLine("\n=== ИНФОРМАЦИЯ О БОЛЬНИЦЕ ===\n");

            Console.WriteLine(hospital);

            Console.WriteLine("\n--- Статистика по отделениям ---");
            foreach (var building in hospital.Buildings)
            {
                Console.WriteLine($"\nЗдание: {building.Name}");
                foreach (var department in building.Departments)
                {
                    var staffCount = hospital.StaffService.GetStaffByDepartment(department).Count;
                    var patientCount = hospital.PatientService.GetActivePlacements()
                        .Count(p => p.Department == department);
                    var availableBeds = GetAvailableBeds(department);

                    Console.WriteLine($"  {department.Name}:");
                    Console.WriteLine($"    Сотрудников: {staffCount}");
                    Console.WriteLine($"    Пациентов: {patientCount}");
                    Console.WriteLine($"    Свободных коек: {availableBeds}");
                }
            }

            Console.WriteLine("\n--- Активные пациенты ---");
            var activePatients = hospital.PatientService.GetActivePlacements();
            if (activePatients.Count == 0)
            {
                Console.WriteLine("Нет пациентов в больнице.");
            }
            else
            {
                foreach (var placement in activePatients)
                {
                    var doctorInfo = placement.AttendingDoctor != null
                        ? $"{placement.AttendingDoctor.Surname} {placement.AttendingDoctor.Name[0]}."
                        : "Врач не назначен";

                    Console.WriteLine($"- {placement.Patient.Surname} {placement.Patient.Name}");
                    Console.WriteLine($"  Отделение: {placement.Department.Name}");
                    Console.WriteLine($"  Лечащий врач: {doctorInfo}");
                    Console.WriteLine($"  Палата: {placement.Ward.WardNumber}, Койка: {placement.BedNumber}");
                }
            }
        }

        static int GetAvailableBeds(IDepartment department)
        {
            int totalBeds = department.GetWards().Sum(w => w.TotalBeds);
            int occupiedBeds = department.GetWards().Sum(w => w.BedOccupancy.Count(b => b.Value));
            return totalBeds - occupiedBeds;
        }

        static void DemonstrateStaffOperations(Hospital hospital)
        {
            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ РАБОТЫ С ПЕРСОНАЛОМ ===\n");

            // Показываем персонал по отделениям
            Console.WriteLine("Персонал по отделениям:");
            foreach (var building in hospital.Buildings)
            {
                foreach (var department in building.Departments)
                {
                    var staffList = hospital.StaffService.GetStaffByDepartment(department);
                    Console.WriteLine($"\nОтделение: {department.Name}");

                    if (!staffList.Any())
                    {
                        Console.WriteLine("  В отделении нет персонала");
                        continue;
                    }

                    foreach (var placement in staffList)
                    {
                        var staff = placement.Staff;
                        if (staff is IMedicalStaff medicalStaff)
                        {
                            var patientCount = hospital.PatientService.GetActivePlacements()
                                .Count(p => p.AttendingDoctor == medicalStaff);
                            Console.WriteLine($"  - {medicalStaff.Specialization}: {staff.Surname} {staff.Name} ({patientCount} пациентов)");
                        }
                        else
                        {
                            Console.WriteLine($"  - {staff.Surname} {staff.Name}");
                        }
                    }
                }
            }

            // Создаем и нанимаем нового врача
            Console.WriteLine("\n--- Прием на работу нового врача ---");
            var newSurgeon = new Surgeon("S003", "Волков", "Михаил", "Анатольевич",
                "LIC-SURG-003", 100000);
            newSurgeon.UpdateOpStats(50, 0);

            var surgeryDept = hospital.Buildings[0].Departments[0];
            hospital.StaffService.HireStaff(newSurgeon, surgeryDept);

            // Увольнение сотрудника без пациентов
            Console.WriteLine("\n--- Увольнение сотрудника без пациентов ---");
            var staffWithoutPatients = hospital.StaffService.GetActiveStaff()
                .FirstOrDefault(sp => sp.Staff is IMedicalStaff medicalStaff &&
                                     hospital.PatientService.GetActivePlacements()
                                         .Count(p => p.AttendingDoctor == medicalStaff) == 0);

            if (staffWithoutPatients != null)
            {
                Console.WriteLine($"Увольняем {staffWithoutPatients.Staff.Surname} (нет пациентов)");
                hospital.StaffService.DismissStaff(staffWithoutPatients.Staff);
            }

            Console.WriteLine("\n--- Итоговая статистика персонала ---");
            Console.WriteLine($"Работает: {hospital.StaffService.GetActiveStaff().Count} человек");
        }

        static void DemonstrateDoctorAssignment(Hospital hospital,
            (Patient p1, Patient p2, Patient p3, Patient p4) patients)
        {
            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ НАЗНАЧЕНИЯ ВРАЧЕЙ ===\n");

            // Показываем врачей и их пациентов
            Console.WriteLine("Врачи и их пациенты:");
            foreach (var staffPlacement in hospital.StaffService.GetActiveStaff())
            {
                if (staffPlacement.Staff is IMedicalStaff doctor)
                {
                    var doctorPatients = hospital.PatientService.GetActivePlacements()
                        .Where(p => p.AttendingDoctor == doctor)
                        .ToList();

                    if (doctor is Surgeon surgeon)
                    {
                        Console.WriteLine($"\n{doctor.Specialization}: {doctor.Surname} {doctor.Name}");
                        Console.WriteLine($"  Статистика операций: {surgeon.GetOperationStats()}");
                    }
                    else
                    {
                        Console.WriteLine($"\n{doctor.Specialization}: {doctor.Surname} {doctor.Name}");
                    }

                    Console.WriteLine($"  Пациентов: {doctorPatients.Count}");

                    if (doctorPatients.Any())
                    {
                        foreach (var placement in doctorPatients)
                        {
                            Console.WriteLine($"  - {placement.Patient.Surname} {placement.Patient.Name} " +
                                            $"(диагноз: {placement.Diagnosis.Name})");
                        }
                    }
                }
            }

            // Выписываем пациента и проверяем историю болезней
            Console.WriteLine("\n--- Выписка пациента и создание истории болезней ---");
            hospital.PatientService.DischargePatient(patients.p1);

            Console.WriteLine($"\nИстория болезней пациента {patients.p1.Surname}:");
            foreach (var record in patients.p1.TreatmentHistory)
            {
                Console.WriteLine($"- #{record.ID}: {record.Diagnoses.First().Name} " +
                                $"({record.StartDate:dd.MM.yyyy} - {record.EndDate:dd.MM.yyyy})");
            }
        }

        static void DemonstrateAdditionalFeatures(Hospital hospital,
            (Patient p1, Patient p2, Patient p3, Patient p4) patients)
        {
            Console.WriteLine("\n=== ДОПОЛНИТЕЛЬНЫЕ ВОЗМОЖНОСТИ СИСТЕМЫ ===\n");

            // 1. Переназначение врача существующему пациенту
            Console.WriteLine("1. Переназначение врача пациенту:");

            // Проверяем, что пациент p3 еще в больнице
            var p3Placement = hospital.PatientService.GetPatientPlacement(patients.p3);

            if (p3Placement != null && p3Placement.IsActive)
            {
                if (p3Placement.AttendingDoctor != null)
                {
                    Console.WriteLine($"Пациент: {patients.p3.Surname}");
                    Console.WriteLine($"Текущий врач: {p3Placement.AttendingDoctor.Specialization} {p3Placement.AttendingDoctor.Surname}");
                    Console.WriteLine($"Ищем другого врача той же специализации...");

                    // Ищем другого врача той же специализации в том же отделении
                    var otherDoctors = hospital.StaffService.GetStaffByDepartment(p3Placement.Department)
                        .Where(sp => sp.Staff is IMedicalStaff medicalStaff &&
                                    medicalStaff.IsActive &&
                                    medicalStaff != p3Placement.AttendingDoctor &&
                                    medicalStaff.Specialization == p3Placement.AttendingDoctor.Specialization)
                        .Select(sp => sp.Staff as IMedicalStaff)
                        .ToList();

                    if (otherDoctors.Any())
                    {
                        var newDoctor = otherDoctors.First();
                        Console.WriteLine($"Найден врач: {newDoctor.Specialization} {newDoctor.Surname}");

                        // Сохраняем старого врача для истории
                        var oldDoctor = p3Placement.AttendingDoctor;
                        p3Placement.AttendingDoctor = newDoctor;

                        Console.WriteLine($"✓ Врач переназначен: {oldDoctor.Surname} → {newDoctor.Surname}");

                        // Показываем текущее распределение
                        var oldDoctorPatients = hospital.PatientService.GetActivePlacements()
                            .Count(p => p.AttendingDoctor == oldDoctor);
                        var newDoctorPatients = hospital.PatientService.GetActivePlacements()
                            .Count(p => p.AttendingDoctor == newDoctor);

                        Console.WriteLine($"  Теперь у {oldDoctor.Surname}: {oldDoctorPatients} пациентов");
                        Console.WriteLine($"  Теперь у {newDoctor.Surname}: {newDoctorPatients} пациентов");
                    }
                    else
                    {
                        Console.WriteLine($"  Не найден другой {p3Placement.AttendingDoctor.Specialization} в отделении {p3Placement.Department.Name}");
                    }
                }
                else
                {
                    Console.WriteLine($"  У пациента {patients.p3.Surname} не назначен врач");
                }
            }
            else
            {
                Console.WriteLine($"  Пациент {patients.p3.Surname} уже выписан или не в больнице");
            }

            // 2. Поиск врача для нового пациента
            Console.WriteLine("\n2. Поиск врача для нового пациента:");
            var testPatient = new Patient("P999", "Тестовый", "Пациент", "Иванович",
                new DateTime(2000, 1, 1), "INS9999999999");
            var testDiagnosis = new Diagnosis("I50", "Сердечная недостаточность",
                "Хроническая сердечная недостаточность", SpecializationDep.CardiologyDep);

            Console.WriteLine($"Диагноз: {testDiagnosis.Name} ({testDiagnosis.RequiredSpecialization})");

            // Ищем в кардиологическом отделении
            var cardioDept = hospital.Buildings[0].Departments
                .First(d => d.Specialization == SpecializationDep.CardiologyDep);

            var foundDoctor = hospital.StaffService.FindDoctorForPatient(
                cardioDept,
                testDiagnosis.RequiredSpecialization,
                hospital.PatientService);

            if (foundDoctor != null)
            {
                Console.WriteLine($"Найден врач: {foundDoctor.Specialization} {foundDoctor.Surname}");
                var doctorPatients = hospital.PatientService.GetActivePlacements()
                    .Count(p => p.AttendingDoctor == foundDoctor);
                Console.WriteLine($"У него уже {doctorPatients} пациентов");

                if (foundDoctor is Cardiologist cardiologist)
                {
                    Console.WriteLine($"  Это кардиолог (должно быть верно)");
                }
                else
                {
                    Console.WriteLine($"  Внимание: Это {foundDoctor.Specialization}, а не Cardiologist!");
                }
            }
            else
            {
                Console.WriteLine("Врач не найден");
            }

            // 3. Статистика хирургов (только активных!)
            Console.WriteLine("\n3. Статистика активных хирургов:");
            var activeSurgeons = hospital.StaffService.GetActiveStaff()
                .Where(sp => sp.Staff is Surgeon)
                .Select(sp => sp.Staff as Surgeon)
                .ToList();

            if (activeSurgeons.Any())
            {
                Console.WriteLine($"Активных хирургов: {activeSurgeons.Count}");
                foreach (var surgeon in activeSurgeons)
                {
                    var patientsCount = hospital.PatientService.GetActivePlacements()
                        .Count(p => p.AttendingDoctor == surgeon);
                    Console.WriteLine($"  {surgeon.Surname}: {surgeon.GetOperationStats()}, пациентов: {patientsCount}");
                }
            }
            else
            {
                Console.WriteLine("Нет активных хирургов");
            }

            // 4. Выписка всех пациентов
            Console.WriteLine("\n4. Массовая выписка пациентов:");
            var activePatients = hospital.PatientService.GetActivePlacements().ToList();

            if (activePatients.Any())
            {
                Console.WriteLine($"Всего активных пациентов: {activePatients.Count}");
                var patientsToDischarge = activePatients.Take(2).ToList();

                foreach (var patientPlacement in patientsToDischarge)
                {
                    Console.WriteLine($"  Выписываем: {patientPlacement.Patient.Surname}");
                    hospital.PatientService.DischargePatient(patientPlacement.Patient);
                }

                Console.WriteLine($"Выписано: {patientsToDischarge.Count} пациентов");
                Console.WriteLine($"Осталось в больнице: {hospital.PatientService.GetActivePlacements().Count}");
            }
            else
            {
                Console.WriteLine("Нет пациентов для выписки");
            }

            // 5. Дополнительная демонстрация: поиск врача по специализации
            Console.WriteLine("\n5. Поиск врачей по специализациям:");

            var specializations = new[]
            {
        MedicalSpecialization.Surgeon,
        MedicalSpecialization.Cardiologist,
        MedicalSpecialization.Therapist
    };

            foreach (var spec in specializations)
            {
                var doctorsOfSpec = hospital.StaffService.GetActiveStaff()
                    .Where(sp => sp.Staff is IMedicalStaff medicalStaff &&
                                medicalStaff.Specialization == spec)
                    .Select(sp => sp.Staff as IMedicalStaff)
                    .ToList();

                if (doctorsOfSpec.Any())
                {
                    Console.WriteLine($"  {spec}: {doctorsOfSpec.Count} врачей");
                    foreach (var doctor in doctorsOfSpec)
                    {
                        var patientCount = hospital.PatientService.GetActivePlacements()
                            .Count(p => p.AttendingDoctor == doctor);
                        Console.WriteLine($"    - {doctor.Surname} ({patientCount} пациентов)");
                    }
                }
            }

            // 6. Демонстрация работы с сервисами
            Console.WriteLine("\n6. Сводная статистика через сервисы:");

            hospital.PatientService.PrintPatientStatistics();
            hospital.StaffService.PrintStaffStatistics();

            // 7. Проверка уникальности ID в истории болезней
            Console.WriteLine("\n7. Проверка уникальности ID записей в истории болезней:");
            var allPatients = hospital.PatientService.GetActivePlacements()
                .Select(p => p.Patient)
                .Concat(hospital.PatientService.GetDischargedPatients()
                        .Select(p => p.Patient))
                .Distinct()
                .ToList();

            var allRecords = allPatients.SelectMany(p => p.TreatmentHistory).ToList();
            var uniqueIds = allRecords.Select(r => r.ID).Distinct().Count();

            Console.WriteLine($"Всего записей в истории болезней: {allRecords.Count}");
            Console.WriteLine($"Уникальных ID: {uniqueIds}");

            if (uniqueIds == allRecords.Count)
            {
                Console.WriteLine("✓ Все ID уникальны!");
            }

            Console.WriteLine("\n✓ Дополнительные возможности системы продемонстрированы!");
        }

        // НОВЫЕ МЕТОДЫ ДЛЯ ДЕМОНСТРАЦИИ ПОЛИКЛИНИК

        static void DemonstrateClinicAttachment(Hospital existingHospital)
        {
            Console.WriteLine("\n=== СОЗДАНИЕ И ПРИКРЕПЛЕНИЕ ПОЛИКЛИНИК ===");

            // Используем сервис
            var attachmentService = new ClinicAttachmentService();

            // Создаем новую больницу специально для поликлиник
            var polyclinicHospital = new Hospital(
                "H002",
                "Городская поликлиническая больница",
                "ул. Поликлиническая, 5");

            // Создаем поликлиники
            var clinics = new[]
            {
                new Clinic("C001", "Поликлиника №1", "ул. Центральная, 10"),
                new Clinic("C002", "Детская поликлиника", "ул. Школьная, 15"),
                new Clinic("C003", "Стоматологическая поликлиника", "ул. Зубная, 3"),
                new Clinic("C004", "Женская консультация", "ул. Гинекологическая, 7")
            };

            Console.WriteLine("\n1. Прикрепляем поликлиники к больницам:");

            // Поликлиника 1 и 2 к новой больнице
            attachmentService.AttachClinicToHospital(clinics[0], polyclinicHospital);
            attachmentService.AttachClinicToHospital(clinics[1], polyclinicHospital);

            // Поликлиника 3 к существующей больнице
            attachmentService.AttachClinicToHospital(clinics[2], existingHospital);

            Console.WriteLine("\n2. Выводим все прикрепления:");
            attachmentService.PrintAllAttachments();

            Console.WriteLine("\n3. Информация о поликлиниках:");
            foreach (var clinic in clinics)
            {
                Console.WriteLine(clinic);
                Console.WriteLine();
            }

            Console.WriteLine("\n4. Получаем статистику:");
            Console.WriteLine($"Всего прикреплений: {attachmentService.GetClinicsForHospital(polyclinicHospital).Count + attachmentService.GetClinicsForHospital(existingHospital).Count}");
            Console.WriteLine($"К {polyclinicHospital.Name} прикреплено: {attachmentService.GetClinicCountForHospital(polyclinicHospital)}");
            Console.WriteLine($"К {existingHospital.Name} прикреплено: {attachmentService.GetClinicCountForHospital(existingHospital)}");

            Console.WriteLine("\n5. Открепляем одну поликлинику и проверяем:");
            attachmentService.DetachClinic(clinics[1]);

            Console.WriteLine("\n6. Финальный статус прикреплений:");
            attachmentService.PrintAllAttachments();
        }

        static void DemonstrateClinicOperations()
        {
            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ РАБОТЫ ПОЛИКЛИНИКИ ===\n");

            // Создаем поликлинику
            var clinic = new Clinic("C010", "Районная поликлиника", "ул. Районная, 20");

            Console.WriteLine("1. Создана поликлиника:");
            Console.WriteLine(clinic);

            // Создаем тестового персонал
            Console.WriteLine("\n2. Добавляем персонал:");

            var therapist = new Therapist(
                "T010", "Петров", "Иван", "Сергеевич",
                "LIC-THER-010", 85000);

            var cardiologist = new Cardiologist(
                "C010", "Сидоров", "Алексей", "Михайлович",
                "LIC-CARD-010", 95000);

            clinic.AddStaff(therapist);
            clinic.AddStaff(cardiologist);

            // Создаем тестовых пациентов
            Console.WriteLine("\n3. Регистрируем пациентов:");

            var patient1 = new Patient(
                "P010", "Иванова", "Мария", "Петровна",
                new DateTime(1975, 3, 15), "INS010101010");

            var patient2 = new Patient(
                "P011", "Кузнецов", "Сергей", "Андреевич",
                new DateTime(1982, 7, 22), "INS010101011");

            clinic.AddPatient(patient1);
            clinic.AddPatient(patient2);

            // Показываем обновленную информацию
            Console.WriteLine("\n4. Текущее состояние поликлиники:");
            Console.WriteLine(clinic);

            // Создаем сервис прикреплений ДО прикрепления
            Console.WriteLine("\n5. Прикрепляем к больнице через сервис:");
            var hospital = new Hospital("H010", "Районная больница", "ул. Больничная, 1");

            var attachmentService = new ClinicAttachmentService();

            try
            {
                // Используем только сервис, а не прямой метод
                attachmentService.AttachClinicToHospital(clinic, hospital);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            // Показываем связи через сервис
            Console.WriteLine("\n6. Текущие прикрепления в сервисе:");
            attachmentService.PrintAllAttachments();

            // Удаляем одного пациента и одного сотрудника
            Console.WriteLine("\n7. Удаляем пациента и сотрудника:");
            clinic.RemovePatient(patient1);
            clinic.RemoveStaff(cardiologist);

            Console.WriteLine("\n8. Финальное состояние поликлиники:");
            Console.WriteLine(clinic);

            // Открепляем через сервис
            Console.WriteLine("\n9. Открепляем поликлинику:");
            try
            {
                attachmentService.DetachClinic(clinic);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("\n10. Финальный статус прикреплений:");
            attachmentService.PrintAllAttachments();

            Console.WriteLine("\n✓ Демонстрация работы поликлиники завершена!");
        }

        static void DemonstrateRefactoredHospital()
        {
            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ОПТИМИЗИРОВАННОЙ БОЛЬНИЦЫ ===\n");

            // Создаем больницу
            var hospital = new Hospital("H100", "Оптимизированная больница", "ул. Оптимизационная, 1");

            // Добавляем инфраструктуру
            var building = new Building("B100", "Главный корпус", "ул. Оптимизационная, 1");
            var department = new Department("D100", "Терапевтическое", SpecializationDep.GeneralMedicineDep);
            department.AddWard(new Ward(101, 5));
            building.AddDepartment(department);
            hospital.AddBuilding(building);

            Console.WriteLine("1. Создана больница с сервисами:");
            Console.WriteLine(hospital);

            Console.WriteLine("\n2. Нанимаем персонал через StaffService:");
            var therapist = new Therapist("T100", "Сергеев", "Андрей", "Владимирович",
                                         "LIC-THER-100", 80000);
            hospital.StaffService.HireStaff(therapist, department);

            Console.WriteLine("\n3. Принимаем пациентов через PatientService:");
            var patient = new Patient("P100", "Ковалев", "Дмитрий", "Игоревич",
                                     new DateTime(1985, 5, 10), "INS100100100");
            var diagnosis = new Diagnosis("J06", "ОРВИ", "Острая респираторная вирусная инфекция",
                                         SpecializationDep.GeneralMedicineDep);

            var placement = hospital.PatientService.AdmitPatient(patient, diagnosis);

            // Назначаем врача
            if (placement != null)
            {
                var doctor = hospital.StaffService.FindDoctorForPatient(department,
                    diagnosis.RequiredSpecialization, hospital.PatientService);

                if (doctor != null)
                {
                    placement.AttendingDoctor = doctor;
                    Console.WriteLine($"Назначен врач: {doctor.Surname} {doctor.Name}");
                }
            }

            Console.WriteLine("\n4. Статистика через сервисы:");
            hospital.PatientService.PrintPatientStatistics();
            hospital.StaffService.PrintStaffStatistics();

            Console.WriteLine("\n5. Выписываем пациента:");
            hospital.PatientService.DischargePatient(patient);

            Console.WriteLine("\n6. Финальная статистика:");
            hospital.PatientService.PrintPatientStatistics();

            Console.WriteLine("\n✓ Демонстрация оптимизированной архитектуры завершена!");
        }
    }
}