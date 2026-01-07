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
        private static int _idCounter = 1;

        // Данные для генерации
        private static readonly string[] _medications = new[]
        {
            "Аспирин", "Парацетамол", "Ибупрофен", "Амоксициллин", "Цефтриаксон"
        };

        private static readonly string[] _dosages = new[]
        {
            "100 мг", "250 мг", "500 мг", "1 г"
        };

        private static readonly string[] _instructions = new[]
        {
            "3 раза в день после еды", "2 раза в день утром и вечером",
            "1 раз в день утром натощак"
        };

        private static readonly string[] _analysisResults = new[]
        {
            "В пределах нормы", "Незначительные отклонения", "Норма"
        };

        private static readonly string[] _procedureDescriptions = new[]
        {
            "Хирургическая операция", "Перевязка", "Физиотерапия",
            "Инъекция", "ЭКГ", "Рентген", "УЗИ", "Массаж"
        };

        private static readonly string[] _procedureResults = new[]
        {
            "Успешно", "Без осложнений", "Положительный результат"
        };

        // ============ РУЧНОЕ СОЗДАНИЕ ============

        public static IPrescription CreatePrescription(IMedicalStaff doctor,
            string medication, string dosage, string instructions)
        {
            var id = $"PR{_idCounter++:0000}";
            return new Prescription(id, DateTime.Now, doctor, medication, dosage, instructions);
        }

        public static IProcedure CreateProcedure(IMedicalStaff doctor,
            string description, string result)
        {
            var id = $"PROC{_idCounter++:0000}";
            return new Procedure(id, DateTime.Now, doctor, description, result);
        }

        public static IAnalysis CreateAnalysis(string name, LabProfileType type, string results)
        {
            var id = $"AN{_idCounter++:0000}";
            return new Analysis(id, name, type, DateTime.Now, results);
        }

        // ============ АВТОМАТИЧЕСКАЯ ГЕНЕРАЦИЯ ============

        public static IPrescription GenerateRandomPrescription(IMedicalStaff doctor)
        {
            var medication = _medications[_random.Next(_medications.Length)];
            var dosage = _dosages[_random.Next(_dosages.Length)];
            var instruction = _instructions[_random.Next(_instructions.Length)];

            return CreatePrescription(doctor, medication, dosage, instruction);
        }

        public static IProcedure GenerateRandomProcedure(IMedicalStaff doctor)
        {
            var description = _procedureDescriptions[_random.Next(_procedureDescriptions.Length)];
            var result = _procedureResults[_random.Next(_procedureResults.Length)];

            return CreateProcedure(doctor, description, result);
        }

        public static IAnalysis GenerateRandomAnalysis()
        {
            var analysisTypes = Enum.GetValues(typeof(LabProfileType));
            var type = (LabProfileType)analysisTypes.GetValue(_random.Next(analysisTypes.Length));

            // Заменяем switch expression на обычный switch
            string name;
            switch (type)
            {
                case LabProfileType.Clinical:
                    name = "Клинический анализ";
                    break;
                case LabProfileType.Biochemical:
                    name = "Биохимический анализ";
                    break;
                case LabProfileType.Microbiological:
                    name = "Микробиологический анализ";
                    break;
                case LabProfileType.Genetic:
                    name = "Генетический анализ";
                    break;
                case LabProfileType.Hematological:
                    name = "Гематологический анализ";
                    break;
                case LabProfileType.Immunological:
                    name = "Иммунологический анализ";
                    break;
                default:
                    name = "Лабораторное исследование";
                    break;
            }

            var result = _analysisResults[_random.Next(_analysisResults.Length)];
            return CreateAnalysis(name, type, result);
        }

        // ============ СОЗДАНИЕ ПОЛНОЙ ИСТОРИИ ============

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

            // Диагноз
            record.AddDiagnosis(diagnosis);

            // Врачи
            foreach (var doctor in doctors)
            {
                record.AddAttendingDoctor(doctor);
            }

            // Рецепты (1-2)
            int prescriptionCount = _random.Next(1, 3);
            for (int i = 0; i < prescriptionCount; i++)
            {
                var doctor = doctors.Count > 0 ? doctors[_random.Next(doctors.Count)] : null;
                var prescription = GenerateRandomPrescription(doctor);
                record.AddPrescription(prescription);
            }

            // Процедуры (1-2)
            int procedureCount = _random.Next(1, 3);
            for (int i = 0; i < procedureCount; i++)
            {
                var doctor = doctors.Count > 0 ? doctors[_random.Next(doctors.Count)] : null;
                var procedure = GenerateRandomProcedure(doctor);
                record.AddProcedure(procedure);
            }

            // Анализы (1-2)
            int analysisCount = _random.Next(1, 3);
            for (int i = 0; i < analysisCount; i++)
            {
                var analysis = GenerateRandomAnalysis();
                record.AddAnalysis(analysis);
            }

            record.CompleteRecord(endDate);
            return record;
        }
    }
}