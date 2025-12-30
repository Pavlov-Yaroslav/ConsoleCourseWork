using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface IPatient
    {
        string ID { get; }
        string Name { get; }
        string Surname { get; }
        string Patronymic { get; }
        DateTime BirthDate { get; }
        string InsuranceNumber { get; }
    }
}
