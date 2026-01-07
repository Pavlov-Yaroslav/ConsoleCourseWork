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
        // Пациенты теперь получаются через методы
        int GetPatientCount();
        List<IPatient> GetRegisteredPatients();

        List<IStaff> MedicalStaff { get; }
        void AddStaff(IStaff staff);
        void RemoveStaff(IStaff staff);

        // Методы для услуг
        bool CanProvideService(IPatient patient);
        void ProvideSimpleService(IPatient patient, string serviceName);
    }
}

