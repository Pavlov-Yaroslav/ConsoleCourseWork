using ConsoleCourceWork.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface IOperatingStaff : IMedicalStaff
    {
        int CompletedOperations { get; }
        int FatalOperations { get; }
        decimal SuccessRate { get; }

        void UpdateOpStats(int completed, int fatal);
        bool CanCureDiagnosis(SpecializationDep specialization);
    }
}
