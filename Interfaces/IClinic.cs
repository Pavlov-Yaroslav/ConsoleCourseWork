using ConsoleCourceWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface IClinic
    {
        // Базовые свойства
        string Name { get; }
        string ID { get; }

        // Связь с больницей
        Hospital AttachedHospital { get; }

        // Коллекции
        List<IStaff> MedicalStaff { get; }
        List<IPatient> RegisteredPatients { get; }

        // Методы прикрепления/открепления
        void AttachToHospital(Hospital hospital);
        void DetachFromHospital();

        // Базовые методы для персонала
        void AddStaff(IStaff staff);
        void RemoveStaff(IStaff staff);

        // Базовые методы для пациентов
        void AddPatient(IPatient patient);
        void RemovePatient(IPatient patient);
    }
}
