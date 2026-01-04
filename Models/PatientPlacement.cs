using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCourceWork.Models
{
    public class PatientPlacement : IPatientPlacement
    {
        public IPatient Patient { get; }
        public IDiagnosis Diagnosis { get; }
        public IBuilding Building { get; }
        public IDepartment Department { get; }
        public IWard Ward { get; }
        public int BedNumber { get; }
        public DateTime AdmissionDate { get; }
        public DateTime? DischargeDate { get; private set; }
        public IMedicalStaff AttendingDoctor { get; set; }
        public bool IsActive { get; private set; }

        public PatientPlacement(IPatient patient, IDiagnosis diagnosis,
                               IBuilding building, IDepartment department,
                               IWard ward, int bedNumber)
        {
            Patient = patient;
            Diagnosis = diagnosis;
            Building = building;
            Department = department;
            Ward = ward;
            BedNumber = bedNumber;
            AdmissionDate = DateTime.Now;
            IsActive = true;
            AttendingDoctor = null;

            ward.OccupyBed(bedNumber);
        }

        public void Discharge()
        {
            if (IsActive)
            {
                IsActive = false;
                DischargeDate = DateTime.Now;

                if (Ward != null)
                {
                    Ward.ReleaseBed(BedNumber);
                }
            }
        }

        public override string ToString()
        {
            string status = IsActive ? "В больнице" : $"Выписан {DischargeDate:dd.MM.yyyy}";
            string doctorInfo = AttendingDoctor != null
                ? $"{AttendingDoctor.Surname} {AttendingDoctor.Name[0]}."
                : "Не назначен";

            return $"Пациент: {Patient.Name} {Patient.Surname}\n" +
                   $"Диагноз: {Diagnosis.Description}\n" +
                   $"Здание: {Building.Name}, Отделение: {Department.Name}\n" +
                   $"Палата: {Ward.WardNumber}, Койка: {BedNumber}\n" +
                   $"Лечащий врач: {doctorInfo}\n" +
                   $"Поступил: {AdmissionDate:dd.MM.yyyy}\n" +
                   $"Статус: {status}";
        }
    }
}
