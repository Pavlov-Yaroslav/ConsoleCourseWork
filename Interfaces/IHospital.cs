using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Interfaces;

namespace ConsoleCourceWork.Interfaces
{
    public interface IHospital
    {
        List<Building> Buildings { get; }
        Dictionary<IPatient, PatientPlacement> PatientPlacements { get; }

        void AddBuilding(Building building);
        void RemoveBuilding(Building building);
        PatientPlacement AdmitPatient(IPatient patient, IDiagnosis diagnosis);

        bool DischargePatient(IPatient patient);
        bool DeletePatient(IPatient patient);

        // Возвращаем вспомогательные методы
        List<PatientPlacement> GetActivePlacements();
        List<PatientPlacement> GetDischargedPatients();
    }
}
