using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface IMedInstitution
    {
        string Name { get; }
        string Address { get; }
        string ID { get; }
        bool IsActive { get; }
    }
}