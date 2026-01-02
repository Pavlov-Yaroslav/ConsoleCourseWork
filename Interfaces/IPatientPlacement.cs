using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface IPatientPlacement
    {
        IPatient Patient { get; }
        IDiagnosis Diagnosis { get; }
        IBuilding Building { get; }
        IDepartment Department { get; }
        IWard Ward { get; }
        int BedNumber { get; }
        DateTime AdmissionDate { get; }
        DateTime? DischargeDate { get; } // Добавляем
        bool IsActive { get; }
    }
}
