using ConsoleCourceWork.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface IAnalysis
    {
        string ID { get; }
        string Name { get; }
        LabProfileType Type { get; }
        DateTime OrderDate { get; }
        DateTime? ResultDate { get; }
        string Results { get; }
    }
}
