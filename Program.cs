using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;
using ConsoleCourceWork.Models;
using ConsoleCourceWork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork
{
    class Program
    {
        // Пример использования в реальной программе
        static void Main()
        {
            var manager = ClinicAttachmentManager.Instance;

            // Создаем объекты
            var hospital = new Hospital("H1", "Больница", "ул. 1");
            var clinic = new Clinic("C1", "Поликлиника", "ул. 2");

            // Реальная работа
            manager.Attach(clinic, hospital);

            // Получение данных для бизнес-логики
            var attachedClinics = manager.GetClinicsForHospital(hospital);
            var hospitalForClinic = manager.GetHospitalForClinic(clinic);

            // Массовые операции
            manager.Transfer(clinic, anotherHospital);

            // Отображение информации
            manager.PrintHospitalStats(hospital);
        }
    }
}