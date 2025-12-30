using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Enums;

namespace ConsoleCourceWork.Interfaces
{
    public interface IDepartment
    {
        string Name { get; }
        SpecializationDep Specialization { get; }
        string ID { get; }
        bool IsActive { get; }
        List<IWard> GetWards();
        bool HasAvailableBeds();
        (IWard ward, int bedNumber)? FindAvailableBed();
        void AddWard(IWard ward);
    }
}
