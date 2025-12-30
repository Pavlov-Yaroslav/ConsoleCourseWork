using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConsoleCourceWork.Enums;

namespace ConsoleCourceWork.Interfaces
{
    public interface IDiagnosis
    {
        string Code { get; }
        string Name { get; }
        string Description { get; }
        SpecializationDep RequiredSpecialization { get; }
    }
}