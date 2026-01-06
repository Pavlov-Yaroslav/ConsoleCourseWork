using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;
using ConsoleCourceWork.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCourceWork.Models
{
    public class Hospital : IMedInstitution
    {
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string ID { get; private set; }
        public bool IsActive { get; set; }

        // ВООБЩЕ УБИРАЕМ AttachedClinics - сервис всё хранит
        public List<Building> Buildings { get; private set; }
        public PatientManagementService PatientService { get; private set; }
        public StaffManagementService StaffService { get; private set; }

        public Hospital(string id, string name, string address)
        {
            ID = id;
            Name = name;
            Address = address;
            IsActive = true;
            Buildings = new List<Building>();
            PatientService = new PatientManagementService(this);
            StaffService = new StaffManagementService(this);
        }

        public void AddBuilding(Building building) => Buildings.Add(building);
        public void RemoveBuilding(Building building) => Buildings.Remove(building);

        public override string ToString() => $"{Name} (ID: {ID})\nАдрес: {Address}\nЗданий: {Buildings.Count}";
    }
}
