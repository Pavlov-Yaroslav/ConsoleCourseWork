using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class TreatmentRecord : ITreatmentRecord
    {
        public string ID { get; }
        public IPatient Patient { get; }
        public IMedInstitution MedInstitution { get; }
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; private set; }
        public List<IDiagnosis> Diagnoses { get; private set; }
        public List<IPrescription> Prescriptions { get; private set; }
        public List<IProcedure> Procedures { get; private set; }
        public List<IMedicalStaff> AttendingDoctors { get; private set; }
        public List<IAnalysis> Analyses { get; private set; }

        public TreatmentRecord(string id, IPatient patient,
                              IMedInstitution institution, DateTime startDate)
        {
            ID = id;
            Patient = patient;
            MedInstitution = institution;
            StartDate = startDate;
            Diagnoses = new List<IDiagnosis>();
            Prescriptions = new List<IPrescription>();
            Procedures = new List<IProcedure>();
            AttendingDoctors = new List<IMedicalStaff>();
            Analyses = new List<IAnalysis>();
        }

        // Методы для добавления данных
        public void AddDiagnosis(IDiagnosis diagnosis) => Diagnoses.Add(diagnosis);
        public void AddPrescription(IPrescription prescription) => Prescriptions.Add(prescription);
        public void AddProcedure(IProcedure procedure) => Procedures.Add(procedure);
        public void AddAttendingDoctor(IMedicalStaff doctor) => AttendingDoctors.Add(doctor);
        public void AddAnalysis(IAnalysis analysis) => Analyses.Add(analysis);

        public void CompleteRecord(DateTime endDate)
        {
            EndDate = endDate;
        }

        public override string ToString()
        {
            string endDateStr = EndDate.HasValue ? EndDate.Value.ToString("dd.MM.yyyy") : "не завершено";
            return $"История болезни #{ID}\n" +
                   $"Пациент: {Patient.Surname} {Patient.Name}\n" +
                   $"Учреждение: {MedInstitution.Name}\n" +
                   $"Период: {StartDate:dd.MM.yyyy} - {endDateStr}\n" +
                   $"Диагнозы: {Diagnoses.Count}\n" +
                   $"Рецепты: {Prescriptions.Count}\n" +
                   $"Процедуры: {Procedures.Count}\n" +
                   $"Анализы: {Analyses.Count}\n" +
                   $"Врачи: {AttendingDoctors.Count}";
        }
    }
}