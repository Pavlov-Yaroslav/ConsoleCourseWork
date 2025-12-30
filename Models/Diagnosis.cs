using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;

namespace ConsoleCourceWork.Models
{
    public class Diagnosis : Interfaces.IDiagnosis
    {
        public string Code { get; }
        public string Name { get; }
        public string Description { get; }
        public Enums.SpecializationDep RequiredSpecialization { get; }

        public Diagnosis(string code, string name, string description, Enums.SpecializationDep requiredSpecialization)
        {
            Code = code;
            Name = name;
            Description = description;
            RequiredSpecialization = requiredSpecialization;
        }

        public override string ToString()
        {
            return $"{Code} - {Name}: {Description}";
        }
    }
}
