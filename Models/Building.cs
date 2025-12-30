using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class Building : Interfaces.IBuilding
    {
        public string Name { get; }
        public string Address { get; }
        public string ID { get; }
        public bool IsActive { get; set; }
        public List<Interfaces.IDepartment> Departments { get; }

        private List<Interfaces.IDepartment> _departments = new List<Interfaces.IDepartment>();

        public Building(string id, string name, string address)
        {
            ID = id;
            Name = name;
            Address = address;
            IsActive = true;
            Departments = _departments;
        }

        public void AddDepartment(Interfaces.IDepartment department)
        {
            _departments.Add(department);
        }

        public Interfaces.IDepartment FindDepartmentBySpecialization(Enums.SpecializationDep specialization)
        {
            return _departments.FirstOrDefault(d => d.Specialization == specialization && d.IsActive && d.HasAvailableBeds());
        }

        public override string ToString()
        {
            return $"{Name} ({Address})";
        }
    }
}
