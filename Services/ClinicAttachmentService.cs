using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Interfaces;
using ConsoleCourceWork.Models;

namespace ConsoleCourceWork.Services
{
    public class ClinicAttachmentManager
    {
        // Центральное хранилище всех связей
        private readonly Dictionary<Clinic, Hospital> _clinicToHospital = new Dictionary<Clinic, Hospital>();
        private readonly Dictionary<Hospital, List<Clinic>> _hospitalToClinics = new Dictionary<Hospital, List<Clinic>>();

        // Singleton для глобального доступа
        private static ClinicAttachmentManager _instance;
        private static readonly object _lock = new object();

        public static ClinicAttachmentManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ClinicAttachmentManager();
                    }
                    return _instance;
                }
            }
        }

        private ClinicAttachmentManager() { }

        // ============ ОСНОВНЫЕ РАБОЧИЕ МЕТОДЫ ============

        public bool Attach(Clinic clinic, Hospital hospital)
        {
            if (clinic == null || hospital == null)
            {
                Console.WriteLine("Ошибка: клиника или больница не могут быть null");
                return false;
            }

            // Проверка 1: уже прикреплена к этой больнице?
            if (_clinicToHospital.ContainsKey(clinic) && _clinicToHospital[clinic] == hospital)
            {
                Console.WriteLine($"Клиника '{clinic.Name}' уже прикреплена к '{hospital.Name}'");
                return false;
            }

            // Проверка 2: прикреплена к другой больнице?
            if (_clinicToHospital.ContainsKey(clinic))
            {
                var currentHospital = _clinicToHospital[clinic];
                throw new InvalidOperationException(
                    $"Клиника '{clinic.Name}' уже прикреплена к другой больнице '{currentHospital.Name}'");
            }

            // СОЗДАЕМ СВЯЗЬ
            _clinicToHospital[clinic] = hospital;

            if (!_hospitalToClinics.ContainsKey(hospital))
                _hospitalToClinics[hospital] = new List<Clinic>();

            if (!_hospitalToClinics[hospital].Contains(clinic))
                _hospitalToClinics[hospital].Add(clinic);

            Console.WriteLine($"+ {clinic.Name} -> {hospital.Name}");
            return true;
        }

        public bool Detach(Clinic clinic)
        {
            if (!_clinicToHospital.ContainsKey(clinic))
            {
                Console.WriteLine($"Клиника '{clinic.Name}' не прикреплена ни к одной больнице");
                return false;
            }

            var hospital = _clinicToHospital[clinic];

            // УДАЛЯЕМ СВЯЗЬ
            _clinicToHospital.Remove(clinic);

            if (_hospitalToClinics.ContainsKey(hospital))
            {
                _hospitalToClinics[hospital].Remove(clinic);

                // Если у больницы больше нет клиник, удаляем запись
                if (!_hospitalToClinics[hospital].Any())
                    _hospitalToClinics.Remove(hospital);
            }

            Console.WriteLine($"- {clinic.Name} откреплена от {hospital.Name}");
            return true;
        }

        // ============ МЕТОДЫ ДЛЯ ПОЛУЧЕНИЯ ДАННЫХ (рабочие) ============

        public Hospital GetHospitalForClinic(Clinic clinic)
        {
            if (_clinicToHospital.TryGetValue(clinic, out var hospital))
                return hospital;
            return null;
        }

        public List<Clinic> GetClinicsForHospital(Hospital hospital)
        {
            if (_hospitalToClinics.TryGetValue(hospital, out var clinics))
                return clinics;
            return new List<Clinic>();
        }

        public bool IsAttached(Clinic clinic) => _clinicToHospital.ContainsKey(clinic);

        public bool HasClinics(Hospital hospital) =>
            _hospitalToClinics.ContainsKey(hospital) && _hospitalToClinics[hospital].Any();

        public int GetClinicCount(Hospital hospital)
        {
            if (_hospitalToClinics.TryGetValue(hospital, out var clinics))
                return clinics.Count;
            return 0;
        }

        // ============ ВСПОМОГАТЕЛЬНЫЕ РАБОЧИЕ МЕТОДЫ ============

        public void PrintHospitalStats(Hospital hospital)
        {
            var clinics = GetClinicsForHospital(hospital);
            Console.WriteLine($"\n{hospital.Name}: {clinics.Count} клиник");

            if (clinics.Any())
            {
                foreach (var clinic in clinics)
                {
                    Console.WriteLine($"  - {clinic.Name}");
                }
            }
        }

        // ============ МЕТОДЫ МАССОВЫХ ОПЕРАЦИЙ (рабочие) ============

        public void DetachAllFromHospital(Hospital hospital)
        {
            var clinics = GetClinicsForHospital(hospital).ToList();
            int count = clinics.Count;

            foreach (var clinic in clinics)
            {
                Detach(clinic);
            }

            if (count > 0)
                Console.WriteLine($"Все {count} клиник откреплены от {hospital.Name}");
        }

        public void Transfer(Clinic clinic, Hospital newHospital)
        {
            if (!IsAttached(clinic))
            {
                Attach(clinic, newHospital);
                return;
            }

            var oldHospital = GetHospitalForClinic(clinic);
            Detach(clinic);
            Attach(clinic, newHospital);

            Console.WriteLine($"~ {clinic.Name}: {oldHospital.Name} -> {newHospital.Name}");
        }
    }
}
