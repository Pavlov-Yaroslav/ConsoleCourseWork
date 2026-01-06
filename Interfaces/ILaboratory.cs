using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Enums;

namespace ConsoleCourceWork.Interfaces
{
    public interface ILaboratory
    {
        LabProfileType ProfileType { get; }
        List<IStaff> Staff { get; }  // Просто IStaff, а не ILaboratoryStaff
    }
}
