using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class Prescription : IPrescription
    {
        public string ID { get; }
        public DateTime Date { get; }
        public IMedicalStaff Doctor { get; }
        public string Medication { get; }
        public string Dosage { get; }
        public string Instructions { get; }

        public Prescription(string id, DateTime date, IMedicalStaff doctor,
                           string medication, string dosage, string instructions)
        {
            ID = id;
            Date = date;
            Doctor = doctor;
            Medication = medication;
            Dosage = dosage;
            Instructions = instructions;
        }
    }
}
