using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class Patient : Interfaces.IPatient
    {
        public string ID { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Patronymic { get; }
        public DateTime BirthDate { get; }
        public string InsuranceNumber { get; }
        public List<Interfaces.ITreatmentRecord> TreatmentHistory { get; private set; }

        public Patient(string id, string surname, string name, string patronymic,
                      DateTime birthDate, string insuranceNumber)
        {
            ID = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            BirthDate = birthDate;
            InsuranceNumber = insuranceNumber;
            TreatmentHistory = new List<Interfaces.ITreatmentRecord>();
        }

        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic} (ID: {ID})";
        }
    }
}
