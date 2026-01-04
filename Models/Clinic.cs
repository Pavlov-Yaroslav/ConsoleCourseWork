using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Interfaces;

namespace ConsoleCourceWork.Models
{
    public class Clinic : IClinic
    {
        // IClinic properties
        public string Name { get; }
        public string ID { get; }
        public Hospital AttachedHospital { get; private set; }
        public List<IStaff> MedicalStaff { get; private set; }
        public List<IPatient> RegisteredPatients { get; private set; }

        // Дополнительные свойства
        public string Address { get; }
        public bool IsActive { get; set; }

        public Clinic(string id, string name, string address)
        {
            ID = id;
            Name = name;
            Address = address;
            IsActive = true;

            AttachedHospital = null;
            MedicalStaff = new List<IStaff>();
            RegisteredPatients = new List<IPatient>();
        }

        // === Методы прикрепления/открепления ===
        public void AttachToHospital(Hospital hospital)
        {
            if (hospital == null)
                throw new ArgumentNullException(nameof(hospital));

            if (AttachedHospital != null)
            {
                throw new InvalidOperationException(
                    $"Поликлиника '{Name}' уже прикреплена к больнице '{AttachedHospital.Name}'");
            }

            AttachedHospital = hospital;
            Console.WriteLine($"✓ Поликлиника '{Name}' прикреплена к больнице '{hospital.Name}'");
        }

        public void DetachFromHospital()
        {
            if (AttachedHospital == null)
            {
                throw new InvalidOperationException(
                    $"Поликлиника '{Name}' не прикреплена ни к одной больнице");
            }

            var hospitalName = AttachedHospital.Name;
            AttachedHospital = null;
            Console.WriteLine($"✓ Поликлиника '{Name}' откреплена от больницы '{hospitalName}'");
        }

        // === Методы работы с персоналом ===
        public void AddStaff(IStaff staff)
        {
            if (staff == null)
                throw new ArgumentNullException(nameof(staff));

            if (!MedicalStaff.Contains(staff))
            {
                MedicalStaff.Add(staff);
                Console.WriteLine($"✓ {staff.Surname} {staff.Name} добавлен в персонал поликлиники");
            }
        }

        public void RemoveStaff(IStaff staff)
        {
            if (MedicalStaff.Remove(staff))
            {
                Console.WriteLine($"✓ {staff.Surname} удален из персонала поликлиники");
            }
        }

        // === Методы работы с пациентами ===
        public void AddPatient(IPatient patient)
        {
            if (patient == null)
                throw new ArgumentNullException(nameof(patient));

            if (!RegisteredPatients.Contains(patient))
            {
                RegisteredPatients.Add(patient);
                Console.WriteLine($"✓ Пациент {patient.Surname} {patient.Name} зарегистрирован");
            }
        }

        public void RemovePatient(IPatient patient)
        {
            if (RegisteredPatients.Remove(patient))
            {
                Console.WriteLine($"✓ Пациент {patient.Surname} удален из регистрации");
            }
        }

        // === Вспомогательные методы ===
        public int GetStaffCount()
        {
            return MedicalStaff.Count;
        }

        public int GetPatientCount()
        {
            return RegisteredPatients.Count;
        }

        public override string ToString()
        {
            string hospitalInfo = AttachedHospital != null
                ? $"Прикреплена к: {AttachedHospital.Name}"
                : "Не прикреплена";

            return $"{Name} (ID: {ID})\n" +
                   $"Адрес: {Address}\n" +
                   $"{hospitalInfo}\n" +
                   $"Персонал: {MedicalStaff.Count} чел.\n" +
                   $"Пациентов: {RegisteredPatients.Count} чел.";
        }
    }
}
