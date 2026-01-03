using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Services
{
    public static class IdGenerator
    {
        private static int _treatmentRecordCounter = 0;
        private static readonly Random _random = new Random();
        private static readonly object _lock = new object();

        // Генерация ID для записей истории болезней
        public static string GenerateTreatmentRecordId()
        {
            lock (_lock) // Для потокобезопасности
            {
                _treatmentRecordCounter++;
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var randomNum = _random.Next(1000, 9999);
                return $"TR{timestamp}{_treatmentRecordCounter:0000}{randomNum}";
            }
        }

        // Сброс счетчика (для тестов)
        public static void Reset()
        {
            lock (_lock)
            {
                _treatmentRecordCounter = 0;
            }
        }
    }
}
