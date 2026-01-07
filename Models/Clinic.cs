using ConsoleCourceWork.Interfaces;
using ConsoleCourceWork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Пациенты теперь хранятся ТОЛЬКО в PatientAttachmentManager
        private PatientAttachmentManager PatientManager => PatientAttachmentManager.Instance;

        public Clinic(string id, string name, string address)
        {
            ID = id;
            Name = name;
            Address = address;
            IsActive = true;
            MedicalStaff = new List<IStaff>();
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

        // Новые методы для работы с пациентами через менеджер
        public int GetPatientCount() => PatientManager.GetPatientCount(this);

        public List<IPatient> GetRegisteredPatients() => PatientManager.GetPatientsForClinic(this);

        public bool IsPatientRegistered(IPatient patient) => PatientManager.IsAttached(patient);

        // Метод для проверки возможности оказания услуги
        public bool CanProvideService(IPatient patient)
        {
            var attachedClinic = PatientManager.GetClinicForPatient(patient);

            if (attachedClinic == null)
            {
                Console.WriteLine($"{patient.Surname} не прикреплен ни к одной клинике");
                return false;
            }

            if (attachedClinic != this)
            {
                Console.WriteLine($"{patient.Surname} прикреплен к другой клинике: {attachedClinic.Name}");
                return false;
            }

            return true;
        }

        // Метод для оказания услуги
        public void ProvideSimpleService(IPatient patient, string serviceName)
        {
            if (!CanProvideService(patient))
            {
                Console.WriteLine($"❌ Услуга '{serviceName}' НЕ оказана!");
                return;
            }

            Console.WriteLine($"✓ Оказана услуга '{serviceName}' для {patient.Surname}");
        }

        public override string ToString()
        {
            return $"{Name} (ID: {ID})\n" +
                   $"Адрес: {Address}\n" +
                   $"Персонал: {MedicalStaff.Count} чел.\n" +
                   $"Пациентов: {GetPatientCount()} чел.";
        }
    }
}