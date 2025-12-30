using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface IWard
    {
        int WardNumber { get; }
        int TotalBeds { get; }
        Dictionary<int, bool> BedOccupancy { get; }
        int GetNextAvailableBed();
        void OccupyBed(int bedNumber = -1);
        void ReleaseBed(int bedNumber);
    }
}