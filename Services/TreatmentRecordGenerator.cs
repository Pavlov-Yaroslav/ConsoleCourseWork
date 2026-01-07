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
    public class TreatmentRecordGenerator
    {
        private static readonly Random _random = new Random();
        private static int _prescriptionIdCounter = 1;
        private static int _procedureIdCounter = 1;
        private static int _analysisIdCounter = 1;

        // ============ БАЗЫ ДАННЫХ ПО ДИАГНОЗАМ ============

        // Диагнозы -> типичные процедуры
        private static readonly Dictionary<SpecializationDep, List<string>> _diagnosisProcedures =
            new Dictionary<SpecializationDep, List<string>>
            {
                [SpecializationDep.SurgicalDep] = new List<string>
            {
                "Хирургическая операция",
                "Перевязка",
                "Дренирование",
                "Лапароскопия",
                "Наложение швов",
                "Ревизия раны"
            },

                [SpecializationDep.CardiologyDep] = new List<string>
            {
                "ЭКГ",
                "ЭхоКГ",
                "Суточное мониторирование ЭКГ",
                "Нагрузочный тест",
                "Коронарография",
                "Холтеровское мониторирование"
            },

                [SpecializationDep.GeneralMedicineDep] = new List<string>
            {
                "Инъекция",
                "Физиотерапия",
                "Ингаляция",
                "Массаж",
                "ЛФК",
                "Промывание"
            },

                [SpecializationDep.XrayDep] = new List<string>
            {
                "Рентгенография",
                "Компьютерная томография",
                "МРТ",
                "УЗИ",
                "Флюорография",
                "Маммография"
            }
            };

        // Диагнозы -> типичные лекарства
        private static readonly Dictionary<SpecializationDep, List<MedicationData>> _diagnosisMedications =
            new Dictionary<SpecializationDep, List<MedicationData>>
            {
                [SpecializationDep.SurgicalDep] = new List<MedicationData>
            {
                new MedicationData("Амоксициллин", new[] { "500 мг", "1 г" }),
                new MedicationData("Цефтриаксон", new[] { "1 г", "2 г" }),
                new MedicationData("Метронидазол", new[] { "500 мг" }),
                new MedicationData("Кеторолак", new[] { "10 мг", "30 мг" }),
                new MedicationData("Трамадол", new[] { "50 мг", "100 мг" }),
                new MedicationData("Лидокаин", new[] { "1%", "2%" })
            },

                [SpecializationDep.CardiologyDep] = new List<MedicationData>
            {
                new MedicationData("Аспирин", new[] { "75 мг", "100 мг" }),
                new MedicationData("Клопидогрел", new[] { "75 мг" }),
                new MedicationData("Метопролол", new[] { "25 мг", "50 мг", "100 мг" }),
                new MedicationData("Аторвастатин", new[] { "10 мг", "20 мг", "40 мг" }),
                new MedicationData("Эналаприл", new[] { "5 мг", "10 мг", "20 мг" }),
                new MedicationData("Нитроглицерин", new[] { "0.5 мг", "1 мг" })
            },

                [SpecializationDep.GeneralMedicineDep] = new List<MedicationData>
            {
                new MedicationData("Парацетамол", new[] { "500 мг", "1 г" }),
                new MedicationData("Ибупрофен", new[] { "200 мг", "400 мг" }),
                new MedicationData("Амоксициллин", new[] { "250 мг", "500 мг", "1 г" }),
                new MedicationData("Азитромицин", new[] { "250 мг", "500 мг" }),
                new MedicationData("Амброксол", new[] { "30 мг", "60 мг" }),
                new MedicationData("Лоратадин", new[] { "10 мг" })
            },

                [SpecializationDep.XrayDep] = new List<MedicationData>
            {
                new MedicationData("Контрастное вещество", new[] { "10 мл", "20 мл", "50 мл" }),
                new MedicationData("Спазмолитики", new[] { "по необходимости" })
            }
            };

        // Типичные анализы для диагнозов
        private static readonly Dictionary<SpecializationDep, List<AnalysisData>> _diagnosisAnalyses =
            new Dictionary<SpecializationDep, List<AnalysisData>>
            {
                [SpecializationDep.SurgicalDep] = new List<AnalysisData>
            {
                new AnalysisData("Общий анализ крови", LabProfileType.Clinical),
                new AnalysisData("Биохимический анализ крови", LabProfileType.Biochemical),
                new AnalysisData("Коагулограмма", LabProfileType.Hematological),
                new AnalysisData("Группа крови и резус-фактор", LabProfileType.Hematological),
                new AnalysisData("Общий анализ мочи", LabProfileType.Clinical),
                new AnalysisData("Электролиты крови", LabProfileType.Biochemical)
            },

                [SpecializationDep.CardiologyDep] = new List<AnalysisData>
            {
                new AnalysisData("Липидограмма", LabProfileType.Biochemical),
                new AnalysisData("Тропонин", LabProfileType.Biochemical),
                new AnalysisData("Электролиты крови", LabProfileType.Biochemical),
                new AnalysisData("Креатинкиназа-МВ", LabProfileType.Biochemical),
                new AnalysisData("D-димер", LabProfileType.Hematological),
                new AnalysisData("С-реактивный белок", LabProfileType.Biochemical)
            },

                [SpecializationDep.GeneralMedicineDep] = new List<AnalysisData>
            {
                new AnalysisData("Общий анализ крови", LabProfileType.Clinical),
                new AnalysisData("С-реактивный белок", LabProfileType.Biochemical),
                new AnalysisData("Анализ мокроты", LabProfileType.Microbiological),
                new AnalysisData("Иммуноглобулины", LabProfileType.Immunological),
                new AnalysisData("Креатинин и мочевина", LabProfileType.Biochemical),
                new AnalysisData("Глюкоза крови", LabProfileType.Biochemical)
            },

                [SpecializationDep.XrayDep] = new List<AnalysisData>
            {
                new AnalysisData("Онкомаркеры", LabProfileType.Immunological),
                new AnalysisData("Гормональный профиль", LabProfileType.Hematological),
                new AnalysisData("Генетический анализ", LabProfileType.Genetic)
            }
            };

        // Общие данные
        private static readonly string[] _instructions = new[]
        {
            "3 раза в день после еды",
            "2 раза в день утром и вечером",
            "1 раз в день утром натощак",
            "По необходимости при боли",
            "Перед сном",
            "Каждые 6-8 часов",
            "За 30 минут до еды"
        };

        private static readonly string[] _procedureResults = new[]
        {
            "Успешно выполнена",
            "Без осложнений",
            "Пациент перенес удовлетворительно",
            "Результат положительный",
            "Осложнений нет",
            "Требуется наблюдение",
            "Выполнена в полном объеме"
        };

        private static readonly string[] _analysisResults = new[]
        {
            "В пределах нормы",
            "Незначительные отклонения",
            "Требуется повторный анализ",
            "Повышенные показатели",
            "Пониженные показатели",
            "Норма",
            "Выявлены патологические изменения",
            "Критически повышен"
        };

        // Вспомогательные классы для хранения данных
        private class MedicationData
        {
            public string Name { get; }
            public string[] Dosages { get; }

            public MedicationData(string name, string[] dosages)
            {
                Name = name;
                Dosages = dosages;
            }
        }

        private class AnalysisData
        {
            public string Name { get; }
            public LabProfileType Type { get; }

            public AnalysisData(string name, LabProfileType type)
            {
                Name = name;
                Type = type;
            }
        }

        // ============ УМНЫЕ МЕТОДЫ ГЕНЕРАЦИИ ============

        public static IPrescription GeneratePrescription(IMedicalStaff doctor, SpecializationDep specialization)
        {
            string medication;
            string dosage;

            if (!_diagnosisMedications.ContainsKey(specialization))
            {
                // Резервный вариант для неизвестных специализаций
                var commonMeds = new[]
                {
                    ("Парацетамол", "500 мг"),
                    ("Ибупрофен", "400 мг"),
                    ("Аспирин", "100 мг")
                };
                var commonMed = commonMeds[_random.Next(commonMeds.Length)];
                medication = commonMed.Item1;
                dosage = commonMed.Item2;
            }
            else
            {
                var medications = _diagnosisMedications[specialization];
                var medData = medications[_random.Next(medications.Count)];
                medication = medData.Name;
                dosage = medData.Dosages[_random.Next(medData.Dosages.Length)];
            }

            var instruction = _instructions[_random.Next(_instructions.Length)];
            var prescriptionId = $"PR{_prescriptionIdCounter++:0000}";

            return new Prescription(prescriptionId, DateTime.Now, doctor, medication, dosage, instruction);
        }

        public static IProcedure GenerateProcedure(IMedicalStaff doctor, SpecializationDep specialization)
        {
            string description;

            if (!_diagnosisProcedures.ContainsKey(specialization))
            {
                // Резервный вариант
                var allProcedures = new List<string>();
                foreach (var procs in _diagnosisProcedures.Values)
                {
                    allProcedures.AddRange(procs);
                }
                description = allProcedures[_random.Next(allProcedures.Count)];
            }
            else
            {
                var procedures = _diagnosisProcedures[specialization];
                description = procedures[_random.Next(procedures.Count)];
            }

            var result = _procedureResults[_random.Next(_procedureResults.Length)];
            var procedureId = $"PROC{_procedureIdCounter++:0000}";

            return new Procedure(procedureId, DateTime.Now, doctor, description, result);
        }

        public static IAnalysis GenerateAnalysis(SpecializationDep specialization)
        {
            string analysisName;
            LabProfileType analysisType;

            if (!_diagnosisAnalyses.ContainsKey(specialization))
            {
                // Резервный вариант
                var allTypes = Enum.GetValues(typeof(LabProfileType)).Cast<LabProfileType>().ToArray();
                var randomType = allTypes[_random.Next(allTypes.Length)];
                analysisType = randomType;
                analysisName = GetAnalysisNameByType(randomType);
            }
            else
            {
                var analyses = _diagnosisAnalyses[specialization];
                var analysisData = analyses[_random.Next(analyses.Count)];
                analysisName = analysisData.Name;
                analysisType = analysisData.Type;
            }

            var result = _analysisResults[_random.Next(_analysisResults.Length)];
            var analysisId = $"AN{_analysisIdCounter++:0000}";

            return new Analysis(analysisId, analysisName, analysisType, DateTime.Now, result);
        }

        private static string GetAnalysisNameByType(LabProfileType type)
        {
            switch (type)
            {
                case LabProfileType.Clinical:
                    return "Общий анализ крови";
                case LabProfileType.Biochemical:
                    return "Биохимический анализ крови";
                case LabProfileType.Microbiological:
                    return "Микробиологический анализ";
                case LabProfileType.Genetic:
                    return "Генетический анализ";
                case LabProfileType.Hematological:
                    return "Гематологический анализ";
                case LabProfileType.Immunological:
                    return "Иммунологический анализ";
                default:
                    return "Лабораторное исследование";
            }
        }

        // ============ РУЧНОЕ СОЗДАНИЕ (для тестов) ============

        public static IPrescription CreatePrescription(IMedicalStaff doctor,
            string medication, string dosage, string instructions)
        {
            var prescriptionId = $"PR{_prescriptionIdCounter++:0000}";
            return new Prescription(prescriptionId, DateTime.Now, doctor, medication, dosage, instructions);
        }

        public static IProcedure CreateProcedure(IMedicalStaff doctor,
            string description, string result)
        {
            var procedureId = $"PROC{_procedureIdCounter++:0000}";
            return new Procedure(procedureId, DateTime.Now, doctor, description, result);
        }

        public static IAnalysis CreateAnalysis(string name, LabProfileType type, string results)
        {
            var analysisId = $"AN{_analysisIdCounter++:0000}";
            return new Analysis(analysisId, name, type, DateTime.Now, results);
        }

        // ============ УМНАЯ ГЕНЕРАЦИЯ ПОЛНОЙ ИСТОРИИ ============

        public static ITreatmentRecord GenerateTreatmentRecord(
            IPatient patient,
            IMedInstitution institution,
            IDiagnosis diagnosis,
            List<IMedicalStaff> doctors,
            int daysInTreatment = 7)
        {
            var recordId = IdGenerator.GenerateTreatmentRecordId();
            var startDate = DateTime.Now.AddDays(-daysInTreatment);
            var endDate = DateTime.Now;

            var record = new TreatmentRecord(recordId, patient, institution, startDate);

            // Добавляем диагноз
            record.AddDiagnosis(diagnosis);

            // Добавляем врачей
            foreach (var doctor in doctors)
            {
                record.AddAttendingDoctor(doctor);
            }

            var specialization = diagnosis.RequiredSpecialization;

            // УМНЫЕ рецепты (1-3 штуки) в зависимости от диагноза
            int prescriptionCount = _random.Next(1, 4);
            for (int i = 0; i < prescriptionCount; i++)
            {
                IMedicalStaff doctor = null;
                if (doctors.Count > 0)
                {
                    doctor = doctors[_random.Next(doctors.Count)];
                }
                var prescription = GeneratePrescription(doctor, specialization);
                record.AddPrescription(prescription);
            }

            // УМНЫЕ процедуры (1-2 штуки) в зависимости от диагноза
            int procedureCount = _random.Next(1, 3);
            for (int i = 0; i < procedureCount; i++)
            {
                IMedicalStaff doctor = null;
                if (doctors.Count > 0)
                {
                    doctor = doctors[_random.Next(doctors.Count)];
                }
                var procedure = GenerateProcedure(doctor, specialization);
                record.AddProcedure(procedure);
            }

            // УМНЫЕ анализы (1-3 штуки) в зависимости от диагноза
            int analysisCount = _random.Next(1, 4);
            for (int i = 0; i < analysisCount; i++)
            {
                var analysis = GenerateAnalysis(specialization);
                record.AddAnalysis(analysis);
            }

            record.CompleteRecord(endDate);
            return record;
        }
    }
}