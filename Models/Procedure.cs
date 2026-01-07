using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class Procedure : IProcedure
    {
        public string ID { get; }
        public DateTime Date { get; }
        public IMedicalStaff PerformingDoctor { get; }
        public string Description { get; }
        public string Result { get; }

        public Procedure(string id, DateTime date, IMedicalStaff performingDoctor,
                        string description, string result)
        {
            ID = id;
            Date = date;
            PerformingDoctor = performingDoctor;
            Description = description;
            Result = result;
        }
    }
}
