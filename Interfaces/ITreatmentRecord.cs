using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface ITreatmentRecord
    {
        string ID { get; }
        IPatient Patient { get; }
        IMedInstitution Institution { get; }
        DateTime StartDate { get; }
        DateTime? EndDate { get; }
        List<IDiagnosis> Diagnoses { get; }
    }
}