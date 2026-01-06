using ConsoleCourceWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface IClinic : IMedInstitution
    {
        // Основные свойства
        List<IStaff> MedicalStaff { get; }
        List<IPatient> RegisteredPatients { get; }

        // Методы работы с персоналом
        void AddStaff(IStaff staff);
        void RemoveStaff(IStaff staff);

        // Методы работы с пациентами
        void AddPatient(IPatient patient);
        void RemovePatient(IPatient patient);

        // Вспомогательные методы
        int GetStaffCount();
        int GetPatientCount();
    }
}

