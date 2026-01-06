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
            Console.WriteLine("=== ТЕСТИРОВАНИЕ СИСТЕМЫ ДОГОВОРОВ ===\n");

            var contractManager = ContractManager.Instance;

            // 1. СОЗДАЕМ ОБЪЕКТЫ ДЛЯ ТЕСТОВ
            Console.WriteLine("1. СОЗДАНИЕ ТЕСТОВЫХ ОБЪЕКТОВ:");

            var laboratory1 = new Laboratory("LAB1", "Центральная диагностическая лаборатория",
                                           "ул. Лабораторная, 1", LabProfileType.Clinical);
            var laboratory2 = new Laboratory("LAB2", "Биохимическая лаборатория",
                                           "ул. Аналитическая, 5", LabProfileType.Biochemical);

            var hospital = new Hospital("H1", "Городская больница №1", "ул. Больничная, 10");
            var clinic = new Clinic("C1", "Поликлиника №1", "ул. Поликлиническая, 15");

            Console.WriteLine($"• {laboratory1.Name} ({laboratory1.ProfileType})");
            Console.WriteLine($"• {laboratory2.Name} ({laboratory2.ProfileType})");
            Console.WriteLine($"• {hospital.Name}");
            Console.WriteLine($"• {clinic.Name}");

            // ============================================
            // ТЕСТ 1: СОЗДАНИЕ ДОГОВОРОВ
            // ============================================
            Console.WriteLine("\n\n2. ТЕСТ 1: СОЗДАНИЕ ДОГОВОРОВ:");

            Console.WriteLine("\n2.1 Создаем договор лаборатории 1 с больницей:");
            Contract contract1 = null;
            try
            {
                contract1 = contractManager.CreateContract(
                    laboratory: laboratory1,
                    client: hospital,
                    startDate: DateTime.Now.AddMonths(-3), // договор начался 3 месяца назад
                    endDate: DateTime.Now.AddMonths(9)     // закончится через 9 месяцев
                );
                Console.WriteLine($"✓ Создан: {contract1}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
            }

            Console.WriteLine("\n2.2 Создаем договор лаборатории 1 с поликлиникой:");
            Contract contract2 = null;
            try
            {
                contract2 = contractManager.CreateContract(
                    laboratory: laboratory1,
                    client: clinic,
                    startDate: DateTime.Now, // начинается сегодня
                    endDate: null            // бессрочный договор
                );
                Console.WriteLine($"✓ Создан: {contract2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
            }

            Console.WriteLine("\n2.3 Создаем договор лаборатории 2 с больницей:");
            Contract contract3 = null;
            try
            {
                contract3 = contractManager.CreateContract(
                    laboratory: laboratory2,
                    client: hospital,
                    startDate: DateTime.Now.AddDays(-10),
                    endDate: DateTime.Now.AddMonths(6)
                );
                Console.WriteLine($"✓ Создан: {contract3}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка: {ex.Message}");
            }

            // ============================================
            // ТЕСТ 2: ПРОВЕРКА ДУБЛИРОВАНИЯ
            // ============================================
            Console.WriteLine("\n\n3. ТЕСТ 2: ЗАЩИТА ОТ ДУБЛИРОВАНИЯ:");

            Console.WriteLine("\n3.1 Пытаемся создать второй договор между теми же участниками:");
            try
            {
                var duplicateContract = contractManager.CreateContract(
                    laboratory: laboratory1,
                    client: hospital,
                    startDate: DateTime.Now,
                    endDate: DateTime.Now.AddYears(1)
                );
                Console.WriteLine($"✗ ОШИБКА: Второй договор создан (это не должно было произойти)");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"✓ Корректная ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Неожиданная ошибка: {ex.GetType().Name}: {ex.Message}");
            }

            // ============================================
            // ТЕСТ 3: ПОИСК И ФИЛЬТРАЦИЯ
            // ============================================
            Console.WriteLine("\n\n4. ТЕСТ 3: ПОИСК И ФИЛЬТРАЦИЯ:");

            Console.WriteLine("\n4.1 Все договоры лаборатории 1:");
            var lab1Contracts = contractManager.GetContractsByLaboratory(laboratory1);
            Console.WriteLine($"Найдено {lab1Contracts.Count} договоров:");
            foreach (var contract in lab1Contracts)
            {
                Console.WriteLine($"  - {contract}");
            }

            Console.WriteLine("\n4.2 Все договоры больницы:");
            var hospitalContracts = contractManager.GetContractsByClient(hospital);
            Console.WriteLine($"Найдено {hospitalContracts.Count} договоров:");
            foreach (var contract in hospitalContracts)
            {
                Console.WriteLine($"  - {contract}");
            }

            Console.WriteLine("\n4.3 Активные договоры:");
            var activeContracts = contractManager.GetActiveContracts();
            Console.WriteLine($"Активных договоров: {activeContracts.Count}");

            // ============================================
            // ТЕСТ 4: ПОЛУЧЕНИЕ ДОГОВОРА ПО ID
            // ============================================
            Console.WriteLine("\n\n5. ТЕСТ 4: РАБОТА С ID:");

            if (contract1 != null)
            {
                Console.WriteLine($"\n5.1 Ищем договор по ID '{contract1.ID}':");
                var foundContract = contractManager.GetContract(contract1.ID);
                if (foundContract != null)
                {
                    Console.WriteLine($"✓ Найден: {foundContract}");
                }
                else
                {
                    Console.WriteLine($"✗ Не найден (ошибка)");
                }
            }

            Console.WriteLine("\n5.2 Пытаемся найти несуществующий договор:");
            var nonExistent = contractManager.GetContract("NONEXISTENT");
            Console.WriteLine($"Результат: {(nonExistent == null ? "null (корректно)" : "найден (ошибка)")}");

            // ============================================
            // ТЕСТ 5: РАСТОРЖЕНИЕ ДОГОВОРА
            // ============================================
            Console.WriteLine("\n\n6. ТЕСТ 5: РАСТОРЖЕНИЕ ДОГОВОРА:");

            if (contract1 != null)
            {
                Console.WriteLine($"\n6.1 Расторгаем договор {contract1.ID}:");
                contractManager.TerminateContract(contract1.ID);

                Console.WriteLine("\n6.2 Проверяем активные договоры после расторжения:");
                var activeAfterTermination = contractManager.GetActiveContracts();
                Console.WriteLine($"Активных договоров: {activeAfterTermination.Count}");

                Console.WriteLine("\n6.3 Пытаемся создать новый договор с теми же участниками:");
                try
                {
                    var newContract = contractManager.CreateContract(laboratory1, hospital, DateTime.Now);
                    Console.WriteLine($"✓ Новый договор создан после расторжения старого");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Ошибка: {ex.Message}");
                }
            }

            // ============================================
            // ТЕСТ 6: ПРОВЕРКА ДАТ
            // ============================================
            Console.WriteLine("\n\n7. ТЕСТ 6: ВАЛИДАЦИЯ ДАТ:");

            Console.WriteLine("\n7.1 Пытаемся создать договор с датой начала в будущем:");
            try
            {
                // Используем лабораторию 2 и поликлинику (у них нет договора)
                var futureContract = contractManager.CreateContract(
                    laboratory: laboratory2,  // Используем другую лабораторию!
                    client: clinic,
                    startDate: DateTime.Now.AddDays(7), // Дата в будущем
                    endDate: DateTime.Now.AddYears(1)
                );
                Console.WriteLine($"✗ Договор создан (это ошибка!)");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✓ Корректная ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Неожиданная ошибка: {ex.GetType().Name}: {ex.Message}");
            }

            Console.WriteLine("\n7.2 Пытаемся создать договор с некорректными датами:");
            try
            {
                // Нужно использовать участников БЕЗ существующего договора
                // Например, лабораторию 2 и поликлинику (у них нет договора)
                var invalidContract = contractManager.CreateContract(
                    laboratory: laboratory2,  // Биохимическая лаборатория
                    client: clinic,           // Поликлиника №1
                    startDate: DateTime.Now,
                    endDate: DateTime.Now.AddDays(-1) // Дата окончания в прошлом (ошибка!)
                );
                Console.WriteLine($"[ERROR] Договор создан (это ошибка!)");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[OK] Корректная ошибка валидации дат: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"[OK] Корректная ошибка дублирования: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Неожиданная ошибка: {ex.GetType().Name}: {ex.Message}");
            }

            // ============================================
            // ТЕСТ 7: РАБОТА С ЛАБОРАНТАМИ
            // ============================================
            Console.WriteLine("\n\n8. ТЕСТ 7: ПЕРСОНАЛ ЛАБОРАТОРИИ:");

            Console.WriteLine("\n8.1 Создаем лаборантов:");
            var labAssistant1 = new LaboratoryAssistant(
                id: "EMP001",
                surname: "Иванова",
                name: "Мария",
                patronymic: "Петровна",
                licenseNumber: "LAB-LIC-001",
                initialSalary: 35000
            );

            var labAssistant2 = new LaboratoryAssistant(
                id: "EMP002",
                surname: "Петров",
                name: "Алексей",
                patronymic: "Сергеевич",
                licenseNumber: "LAB-LIC-002",
                initialSalary: 38000
            );

            Console.WriteLine($"Созданы лаборанты: {labAssistant1.Surname} и {labAssistant2.Surname}");

            Console.WriteLine("\n8.2 Добавляем лаборантов в лабораторию:");
            laboratory1.AddStaff(labAssistant1);
            laboratory1.AddStaff(labAssistant2);

            Console.WriteLine($"\n8.3 Персонал лаборатории '{laboratory1.Name}': {laboratory1.Staff.Count} человек");
            foreach (var staff in laboratory1.Staff)
            {
                Console.WriteLine($"  - {staff.Surname} {staff.Name}");
            }

            // ============================================
            // ИТОГОВАЯ СТАТИСТИКА
            // ============================================
            Console.WriteLine("\n\n9. ИТОГОВАЯ СТАТИСТИКА:");

            Console.WriteLine("\n9.1 Все договоры в системе:");
            contractManager.PrintAllContracts();

            Console.WriteLine("\n9.2 Сводка по объектам:");
            Console.WriteLine($"Лабораторий создано: 2");
            Console.WriteLine($"Учреждений создано: 2 (больница + поликлиника)");
            Console.WriteLine($"Лаборантов создано: 2");
            Console.WriteLine($"Договоров создано: {contractManager.AllContracts.Count}");

            Console.WriteLine("\n9.3 Проверяем доступ к свойствам через интерфейсы:");
            Console.WriteLine($"Лаборатория как IMedInstitution: {laboratory1.Name}");
            Console.WriteLine($"Лаборатория как ILaboratory: профиль = {laboratory1.ProfileType}");
            Console.WriteLine($"Лаборатория как IMedInstitutClient: ID = {laboratory1.ID}");

            Console.WriteLine($"\nБольница как IMedInstitution: {hospital.Name}");
            Console.WriteLine($"Больница как IMedInstitutClient: ID = {hospital.ID}");

            // ============================================
            // ТЕСТ 8: ПРОВЕРКА AllContracts
            // ============================================
            Console.WriteLine("\n\n10. ТЕСТ 8: ПРОВЕРКА AllContracts:");

            Console.WriteLine("\n10.1 Все ID договоров через AllContracts:");
            foreach (var contract in contractManager.AllContracts)
            {
                Console.WriteLine($"  ID: {contract.ID}, Лаборатория: {contract.Laboratory.Name}, Клиент: {contract.Client.Name}");
            }

            Console.WriteLine("\n10.2 Доступ к свойствам через AllContracts:");
            var firstContract = contractManager.AllContracts.FirstOrDefault();
            if (firstContract != null)
            {
                Console.WriteLine($"Первый договор: {firstContract.ID}");
                Console.WriteLine($"  Лаборатория.Name: {firstContract.Laboratory.Name} (доступ через конкретный класс)");
                Console.WriteLine($"  Client.Name: {firstContract.Client.Name} (доступ через интерфейс)");
            }

            Console.WriteLine("\n\n=== ТЕСТИРОВАНИЕ ЗАВЕРШЕНО ===");
            Console.WriteLine("Проверено:");
            Console.WriteLine("✓ Создание договоров");
            Console.WriteLine("✓ Защита от дублирования");
            Console.WriteLine("✓ Поиск и фильтрация");
            Console.WriteLine("✓ Работа с ID");
            Console.WriteLine("✓ Расторжение договоров");
            Console.WriteLine("✓ Валидация дат");
            Console.WriteLine("✓ Работа с персоналом");
            Console.WriteLine("✓ Доступ к свойствам через интерфейсы");

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}