using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class Contract : IContract
    {
        private static int _counter = 0;

        public string ID { get; }
        public Laboratory Laboratory { get; }
        public IMedInstitutClient Client { get; }  // Исправлено с MedicalInstitution на Client
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; internal set; }  // internal для менеджера
        public bool IsActive { get; internal set; }      // internal для менеджера

        // НЕТ IsValid - проверка через менеджер или логику

        internal Contract(Laboratory laboratory, IMedInstitutClient client,
                         DateTime startDate, DateTime? endDate)
        {
            // Валидация
            if (startDate.Date > DateTime.Today)
                throw new ArgumentException("Дата начала не может быть в будущем");

            if (endDate.HasValue && endDate.Value <= startDate)
                throw new ArgumentException("Дата окончания должна быть после даты начала");

            // Генерация ID
            _counter++;
            ID = $"CTR{_counter:0000}";

            Laboratory = laboratory ?? throw new ArgumentNullException(nameof(laboratory));
            Client = client ?? throw new ArgumentNullException(nameof(client));
            StartDate = startDate;
            EndDate = endDate;
            IsActive = true;
        }

        public override string ToString()
        {
            string period = EndDate.HasValue
                ? $"{StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}"
                : $"{StartDate:dd.MM.yyyy} - бессрочно";

            return $"{ID}: {Laboratory.Name} ↔ {Client.Name} ({period})";
        }
    }
}
