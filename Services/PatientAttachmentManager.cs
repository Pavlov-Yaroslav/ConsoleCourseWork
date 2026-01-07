using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Interfaces;
using ConsoleCourceWork.Models;

namespace ConsoleCourceWork.Services
{
    public class PatientAttachmentManager
    {
        // Единственный источник данных о прикреплениях
        private readonly Dictionary<IPatient, Clinic> _patientToClinic = new Dictionary<IPatient, Clinic>();
        private readonly Dictionary<Clinic, List<IPatient>> _clinicToPatients = new Dictionary<Clinic, List<IPatient>>();

        // Singleton
        private static PatientAttachmentManager _instance;
        private static readonly object _lock = new object();

        public static PatientAttachmentManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new PatientAttachmentManager();
                    }
                    return _instance;
                }
            }
        }

        private PatientAttachmentManager() { }

        // ============ ОСНОВНЫЕ МЕТОДЫ ============

        // 1. Прикрепить пациента к клинике
        public bool Attach(IPatient patient, Clinic clinic)
        {
            if (patient == null || clinic == null)
            {
                Console.WriteLine("Ошибка: пациент или клиника не могут быть null");
                return false;
            }

            // Проверка: уже прикреплен к этой клинике?
            if (_patientToClinic.ContainsKey(patient) && _patientToClinic[patient] == clinic)
            {
                Console.WriteLine($"Пациент {patient.Surname} уже прикреплен к '{clinic.Name}'");
                return false;
            }

            // Проверка: прикреплен к другой клинике?
            if (_patientToClinic.ContainsKey(patient))
            {
                var currentClinic = _patientToClinic[patient];
                Console.WriteLine($"Внимание: {patient.Surname} уже прикреплен к '{currentClinic.Name}'");
                Console.WriteLine("Сначала открепите пациента!");
                return false;
            }

            // СОЗДАЕМ СВЯЗЬ
            _patientToClinic[patient] = clinic;

            if (!_clinicToPatients.ContainsKey(clinic))
                _clinicToPatients[clinic] = new List<IPatient>();

            _clinicToPatients[clinic].Add(patient);

            Console.WriteLine($"+ {patient.Surname} прикреплен к '{clinic.Name}'");
            return true;
        }

        // 2. Открепить пациента от клиники
        public bool Detach(IPatient patient)
        {
            if (!_patientToClinic.ContainsKey(patient))
            {
                Console.WriteLine($"Пациент {patient.Surname} не прикреплен ни к одной клинике");
                return false;
            }

            var clinic = _patientToClinic[patient];

            // УДАЛЯЕМ СВЯЗЬ
            _patientToClinic.Remove(patient);
            _clinicToPatients[clinic].Remove(patient);

            // Если у клиники больше нет пациентов, удаляем запись
            if (!_clinicToPatients[clinic].Any())
                _clinicToPatients.Remove(clinic);

            Console.WriteLine($"- {patient.Surname} откреплен от '{clinic.Name}'");
            return true;
        }

        // 3. Получить клинику пациента
        public Clinic GetClinicForPatient(IPatient patient)
        {
            _patientToClinic.TryGetValue(patient, out var clinic);
            return clinic;
        }

        // 4. Получить пациентов клиники
        public List<IPatient> GetPatientsForClinic(Clinic clinic)
        {
            _clinicToPatients.TryGetValue(clinic, out var patients);
            return patients ?? new List<IPatient>();
        }

        // 5. Проверить прикрепление
        public bool IsAttached(IPatient patient) => _patientToClinic.ContainsKey(patient);

        // 6. Сколько пациентов у клиники
        public int GetPatientCount(Clinic clinic)
        {
            return GetPatientsForClinic(clinic).Count;
        }

        // 7. Перенести пациента в другую клинику
        public bool TransferPatient(IPatient patient, Clinic newClinic)
        {
            if (!IsAttached(patient))
            {
                return Attach(patient, newClinic);
            }

            var oldClinic = GetClinicForPatient(patient);
            Detach(patient);
            Attach(patient, newClinic);

            Console.WriteLine($"~ Пациент {patient.Surname}: {oldClinic.Name} → {newClinic.Name}");
            return true;
        }

        // ============ УТИЛИТЫ ============

        public void PrintClinicStats(Clinic clinic)
        {
            var patients = GetPatientsForClinic(clinic);
            Console.WriteLine($"\nКлиника '{clinic.Name}': {patients.Count} пациентов");
        }

        public void PrintPatientInfo(IPatient patient)
        {
            var clinic = GetClinicForPatient(patient);

            if (clinic != null)
            {
                Console.WriteLine($"Пациент {patient.Surname} прикреплен к: {clinic.Name}");
            }
            else
            {
                Console.WriteLine($"Пациент {patient.Surname} не прикреплен к клинике");
            }
        }
    }
}
