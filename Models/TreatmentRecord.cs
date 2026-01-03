using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class TreatmentRecord : Interfaces.ITreatmentRecord
    {
        public string ID { get; }
        public Interfaces.IPatient Patient { get; }
        public Interfaces.IMedInstitution Institution { get; }
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; private set; }
        public List<Interfaces.IDiagnosis> Diagnoses { get; private set; }

        public TreatmentRecord(string id, Interfaces.IPatient patient,
                              Interfaces.IMedInstitution institution, DateTime startDate)
        {
            ID = id;
            Patient = patient;
            Institution = institution;
            StartDate = startDate;
            Diagnoses = new List<Interfaces.IDiagnosis>();
        }

        // Добавляем недостающий метод
        public void AddDiagnosis(Interfaces.IDiagnosis diagnosis)
        {
            Diagnoses.Add(diagnosis);
        }

        public void CompleteRecord(DateTime endDate)
        {
            EndDate = endDate;
        }

        public override string ToString()
        {
            string endDateStr = EndDate.HasValue ? EndDate.Value.ToString("dd.MM.yyyy") : "не завершено";
            return $"История болезни #{ID}\n" +
                   $"Пациент: {Patient.Surname} {Patient.Name}\n" +
                   $"Больница: {Institution.Name}\n" +
                   $"Период: {StartDate:dd.MM.yyyy} - {endDateStr}\n" +
                   $"Диагнозы: {Diagnoses.Count}";
        }
    }
}