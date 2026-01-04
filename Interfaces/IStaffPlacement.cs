using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface IStaffPlacement
    {
        IStaff Staff { get; }
        IDepartment Department { get; }
        DateTime HireDate { get; }
        DateTime? DismissalDate { get; }
        bool IsActive { get; }
    }
}

