using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class Ward : Interfaces.IWard
    {
        public int WardNumber { get; }
        public int TotalBeds { get; }
        public Dictionary<int, bool> BedOccupancy { get; }

        public Ward(int wardNumber, int totalBeds)
        {
            WardNumber = wardNumber;
            TotalBeds = totalBeds;
            BedOccupancy = new Dictionary<int, bool>();

            for (int i = 1; i <= totalBeds; i++)
            {
                BedOccupancy[i] = false;
            }
        }

        public int GetNextAvailableBed()
        {
            var availableBed = BedOccupancy.FirstOrDefault(bed => !bed.Value);
            return availableBed.Key != 0 ? availableBed.Key : -1;
        }

        public void OccupyBed(int bedNumber = -1)
        {
            if (bedNumber == -1)
            {
                bedNumber = GetNextAvailableBed();
            }

            if (bedNumber > 0 && bedNumber <= TotalBeds && !BedOccupancy[bedNumber])
            {
                BedOccupancy[bedNumber] = true;
            }
        }

        public void ReleaseBed(int bedNumber)
        {
            if (bedNumber > 0 && bedNumber <= TotalBeds)
            {
                BedOccupancy[bedNumber] = false;
            }
        }

        public override string ToString()
        {
            int occupiedBeds = BedOccupancy.Count(b => b.Value);
            return $"Палата {WardNumber}: {occupiedBeds}/{TotalBeds}";
        }
    }
}