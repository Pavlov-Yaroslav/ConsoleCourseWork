using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class Department : Interfaces.IDepartment
    {
        public string Name { get; }
        public Enums.SpecializationDep Specialization { get; }
        public string ID { get; }
        public bool IsActive { get; set; }

        private List<Interfaces.IWard> _wards = new List<Interfaces.IWard>();

        public Department(string id, string name, Enums.SpecializationDep specialization)
        {
            ID = id;
            Name = name;
            Specialization = specialization;
            IsActive = true;
        }

        public List<Interfaces.IWard> GetWards() => _wards;

        public bool HasAvailableBeds()
        {
            return _wards.Any(ward => ward.BedOccupancy.Any(bed => !bed.Value));
        }

        public (Interfaces.IWard ward, int bedNumber)? FindAvailableBed()
        {
            foreach (var ward in _wards)
            {
                int availableBed = ward.GetNextAvailableBed();
                if (availableBed != -1)
                {
                    return (ward, availableBed);
                }
            }
            return null;
        }

        public void AddWard(Interfaces.IWard ward)
        {
            _wards.Add(ward);
        }

        public override string ToString()
        {
            int totalBeds = _wards.Sum(w => w.TotalBeds);
            int occupiedBeds = _wards.Sum(w => w.BedOccupancy.Count(b => b.Value));

            return $"{Name} - {Specialization} ({totalBeds - occupiedBeds}/{totalBeds} свободно)";
        }
    }
}