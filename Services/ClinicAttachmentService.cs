using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Interfaces;
using ConsoleCourceWork.Models;

namespace ConsoleCourceWork.Services
{
    public class ClinicAttachmentService
    {
        private readonly Dictionary<Clinic, Hospital> _attachments;

        public ClinicAttachmentService()
        {
            _attachments = new Dictionary<Clinic, Hospital>();
        }

        public void AttachClinicToHospital(Clinic clinic, Hospital hospital)
        {
            if (clinic == null || hospital == null)
                throw new ArgumentNullException();

            if (_attachments.ContainsKey(clinic))
            {
                var currentHospital = _attachments[clinic];
                throw new InvalidOperationException(
                    $"Поликлиника '{clinic.Name}' уже прикреплена к больнице '{currentHospital.Name}'");
            }

            // Используем метод клиники
            clinic.AttachToHospital(hospital);

            // Сохраняем в словаре
            _attachments[clinic] = hospital;

            Console.WriteLine($"Сервис: связь сохранена ({clinic.Name} → {hospital.Name})");
        }

        public void DetachClinic(Clinic clinic)
        {
            if (clinic == null)
                throw new ArgumentNullException(nameof(clinic));

            if (!_attachments.ContainsKey(clinic))
                throw new InvalidOperationException($"Поликлиника '{clinic.Name}' не найдена в сервисе");

            clinic.DetachFromHospital();
            _attachments.Remove(clinic);

            Console.WriteLine($"Сервис: связь удалена для {clinic.Name}");
        }

        public Hospital GetHospitalForClinic(Clinic clinic)
        {
            _attachments.TryGetValue(clinic, out var hospital);
            return hospital;
        }

        public List<Clinic> GetClinicsForHospital(Hospital hospital)
        {
            return _attachments
                .Where(kvp => kvp.Value == hospital)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        public bool IsClinicAttached(Clinic clinic)
        {
            return _attachments.ContainsKey(clinic);
        }

        public void PrintAllAttachments()
        {
            Console.WriteLine("\n=== ТЕКУЩИЕ ПРИКРЕПЛЕНИЯ ПОЛИКЛИНИК ===");

            if (!_attachments.Any())
            {
                Console.WriteLine("Нет прикреплений");
                return;
            }

            foreach (var kvp in _attachments)
            {
                Console.WriteLine($"• {kvp.Key.Name} (ID: {kvp.Key.ID}) → {kvp.Value.Name}");
            }
        }

        // Дополнительные полезные методы
        public int GetClinicCountForHospital(Hospital hospital)
        {
            return _attachments.Count(kvp => kvp.Value == hospital);
        }

        public bool HospitalHasClinics(Hospital hospital)
        {
            return _attachments.Values.Any(h => h == hospital);
        }

        public void DetachAllClinicsFromHospital(Hospital hospital)
        {
            var clinicsToDetach = GetClinicsForHospital(hospital).ToList();

            foreach (var clinic in clinicsToDetach)
            {
                DetachClinic(clinic);
            }

            Console.WriteLine($"Откреплены все поликлиники от больницы '{hospital.Name}' ({clinicsToDetach.Count} шт.)");
        }
    }
}
