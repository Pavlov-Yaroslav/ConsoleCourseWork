using ConsoleCourceWork.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleCourceWork.Interfaces
{
    public interface IMedicalStaff : IStaff
    {
        MedicalSpecialization Specialization { get; }
        AcademicDegree AcademicDegree { get; }
        AcademicTitle AcademicTitle { get; }
        string LicenseNumber { get; }
    }
}
