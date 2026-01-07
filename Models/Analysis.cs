using ConsoleCourceWork.Enums;
using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class Analysis : IAnalysis
    {
        public string ID { get; }
        public string Name { get; }
        public LabProfileType Type { get; }
        public DateTime OrderDate { get; }
        public DateTime? ResultDate { get; }
        public string Results { get; }

        public Analysis(string id, string name, LabProfileType type,
                       DateTime orderDate, string results)
        {
            ID = id;
            Name = name;
            Type = type;
            OrderDate = orderDate;
            ResultDate = orderDate.AddDays(2); // Результат через 2 дня
            Results = results;
        }
    }
}