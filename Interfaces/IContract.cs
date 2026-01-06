using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Models;

namespace ConsoleCourceWork.Interfaces
{
    public interface IContract
    {
        string ID { get; }
        Laboratory Laboratory { get; }
        IMedInstitutClient Client { get; }  // Не MedicalInstitution, а просто Client
        DateTime StartDate { get; }
        DateTime? EndDate { get; }
        bool IsActive { get; }
    }
}
