using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Interfaces;

namespace ConsoleCourceWork.Models
{
    public class Clinic : IClinic, IMedInstitutClient
    {
        // IClinic properties
        public string Name { get; }
        public string ID { get; }
        public string Address { get; }
        public bool IsActive { get; set; }
        public List<IStaff> MedicalStaff { get; private set; }
        public List<IPatient> RegisteredPatients { get; private set; }

        public Clinic(string id, string name, string address)
        {
            ID = id;
            Name = name;
            Address = address;
            IsActive = true;
            MedicalStaff = new List<IStaff>();
            RegisteredPatients = new List<IPatient>();
        }

        // IClinic methods
        public void AddStaff(IStaff staff)
        {
            if (staff == null) throw new ArgumentNullException(nameof(staff));
            if (!MedicalStaff.Contains(staff))
            {
                MedicalStaff.Add(staff);
                Console.WriteLine($"+ {staff.Surname} {staff.Name} добавлен в персонал поликлиники");
            }
        }

        public void RemoveStaff(IStaff staff)
        {
            if (MedicalStaff.Remove(staff))
            {
                Console.WriteLine($"- {staff.Surname} удален из персонала поликлиники");
            }
        }

        public void AddPatient(IPatient patient)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (!RegisteredPatients.Contains(patient))
            {
                RegisteredPatients.Add(patient);
                Console.WriteLine($"+ Пациент {patient.Surname} {patient.Name} зарегистрирован");
            }
        }

        public void RemovePatient(IPatient patient)
        {
            if (RegisteredPatients.Remove(patient))
            {
                Console.WriteLine($"- Пациент {patient.Surname} удален из регистрации");
            }
        }

        public int GetStaffCount() => MedicalStaff.Count;
        public int GetPatientCount() => RegisteredPatients.Count;

        public override string ToString()
        {
            return $"{Name} (ID: {ID})\n" +
                   $"Адрес: {Address}\n" +
                   $"Персонал: {MedicalStaff.Count} чел.\n" +
                   $"Пациентов: {RegisteredPatients.Count} чел.";
        }
    }
}
