using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface IPrescription
    {
        string ID { get; }
        DateTime Date { get; }
        IMedicalStaff Doctor { get; }
        string Medication { get; }
        string Dosage { get; }
        string Instructions { get; }
    }
}
