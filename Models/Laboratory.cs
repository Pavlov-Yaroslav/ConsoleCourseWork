using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;

namespace ConsoleCourceWork.Models
{
    public class Laboratory : IMedInstitution, ILaboratory, IMedInstitutClient
    {
        // IMedInstitution
        public string Name { get; }
        public string Address { get; }
        public string ID { get; }
        public bool IsActive { get; set; }

        // ILaboratory
        public LabProfileType ProfileType { get; }
        public List<IStaff> Staff { get; private set; }

        public Laboratory(string id, string name, string address, LabProfileType profileType)
        {
            ID = id;
            Name = name;
            Address = address;
            ProfileType = profileType;
            IsActive = true;
            Staff = new List<IStaff>();
        }

        public void AddStaff(IStaff staff)
        {
            if (!Staff.Contains(staff))
            {
                Staff.Add(staff);
                staff.AddWorkplace(this);
            }
        }

        public void RemoveStaff(IStaff staff)
        {
            Staff.Remove(staff);
        }

        public override string ToString()
        {
            return $"{Name} ({ProfileType})\n" +
                   $"Адрес: {Address}\n" +
                   $"Сотрудников: {Staff.Count}";
        }
    }
}
