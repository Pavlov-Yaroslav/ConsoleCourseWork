using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Interfaces
{
    public interface IStaff
    {
        string ID { get; }
        string Name { get; }
        string Surname { get; }
        string Patronymic { get; }
        decimal Salary { get; }
        bool IsActive { get; set; }
        int VacationDays { get; }
        List<IMedInstitution> Workplaces { get; }

        void UpdateSalary(decimal newSalary);
        void UpdateVacationDays(int newVacationDays);
        void AddWorkplace(IMedInstitution institution); // ДОБАВЛЯЕМ
        void RemoveWorkplace(IMedInstitution institution); // ДОБАВЛЯЕМ
    }
}
