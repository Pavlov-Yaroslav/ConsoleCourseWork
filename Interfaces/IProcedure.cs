using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface IProcedure
    {
        string ID { get; }
        DateTime Date { get; }
        IMedicalStaff PerformingDoctor { get; }
        string Description { get; }  // Вместо ProcedureType
        string Result { get; }
    }
}
