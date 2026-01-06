using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Enums;

namespace ConsoleCourceWork.Interfaces
{
    public interface ISupportStaff : IStaff
    {
        SupportSpecialization Specialization { get; }
        string Qualification { get; }
    }
}
