using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class StaffPlacement : IStaffPlacement
    {
        public IStaff Staff { get; }
        public IDepartment Department { get; private set; }
        public DateTime HireDate { get; }
        public DateTime? DismissalDate { get; private set; }
        public bool IsActive { get; private set; }

        public StaffPlacement(IStaff staff, IDepartment department = null)
        {
            Staff = staff ?? throw new ArgumentNullException(nameof(staff));
            Department = department;
            HireDate = DateTime.Now;
            IsActive = true;
        }

        public void UpdateDepartment(IDepartment department)
        {
            Department = department;
        }

        public void SetDismissal()
        {
            if (IsActive)
            {
                IsActive = false;
                DismissalDate = DateTime.Now;
            }
        }

        public override string ToString()
        {
            string status = IsActive ? "Работает" : $"Уволен {DismissalDate:dd.MM.yyyy}";
            string deptInfo = Department != null ? $"Отделение: {Department.Name}" : "Не назначен";

            return $"{Staff.Surname} {Staff.Name}\n" +
                   $"{deptInfo}\n" +
                   $"Принят: {HireDate:dd.MM.yyyy}\n" +
                   $"Статус: {status}";
        }
    }
}
