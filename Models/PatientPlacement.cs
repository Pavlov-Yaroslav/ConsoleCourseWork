using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class PatientPlacement : Interfaces.IPatientPlacement
    {
        public Interfaces.IPatient Patient { get; }
        public Interfaces.IDiagnosis Diagnosis { get; }
        public Interfaces.IBuilding Building { get; }
        public Interfaces.IDepartment Department { get; }
        public Interfaces.IWard Ward { get; }
        public int BedNumber { get; }
        public DateTime AdmissionDate { get; }
        public DateTime? DischargeDate { get; private set; }
        public bool IsActive { get; private set; }

        public PatientPlacement(Interfaces.IPatient patient, Interfaces.IDiagnosis diagnosis,
                               Interfaces.IBuilding building, Interfaces.IDepartment department,
                               Interfaces.IWard ward, int bedNumber)
        {
            Patient = patient;
            Diagnosis = diagnosis;
            Building = building;
            Department = department;
            Ward = ward;
            BedNumber = bedNumber;
            AdmissionDate = DateTime.Now;
            IsActive = true;

            ward.OccupyBed(bedNumber);
        }

        public void Discharge()
        {
            if (IsActive)
            {
                IsActive = false;
                DischargeDate = DateTime.Now;

                // Освобождаем койку
                if (Ward != null)
                {
                    Ward.ReleaseBed(BedNumber);
                }

                // Обнуляем указатели для предотвращения утечек памяти
                // (в C# это не обязательно, но хорошо для ясности)
                // В реальности объекты не удаляются, просто освобождаем ссылки

                // Примечание: В C# мы не можем "обнулить" свойства только для чтения (get-only)
                // но мы можем отметить объект как выписанный
            }
        }

        public override string ToString()
        {
            string status = IsActive ? "В больнице" : $"Выписан {DischargeDate:dd.MM.yyyy}";

            return $"Пациент: {Patient.Name} {Patient.Surname}\n" +
                   $"Диагноз: {Diagnosis.Description}\n" +
                   $"Здание: {Building.Name}, Отделение: {Department.Name}\n" +
                   $"Палата: {Ward.WardNumber}, Койка: {BedNumber}\n" +
                   $"Поступил: {AdmissionDate:dd.MM.yyyy}\n" +
                   $"Статус: {status}";
        }
    }
}
