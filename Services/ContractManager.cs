using ConsoleCourceWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCourceWork.Models;

namespace ConsoleCourceWork.Services
{
    public class ContractManager
    {
        private readonly List<Contract> _contracts = new List<Contract>();
        public IReadOnlyList<Contract> AllContracts => _contracts.AsReadOnly();

        private static ContractManager _instance;
        private static readonly object _lock = new object();

        public static ContractManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ContractManager();
                    }
                    return _instance;
                }
            }
        }

        private ContractManager() { }

        // ============ ОСНОВНЫЕ МЕТОДЫ (всего 4!) ============

        public Contract CreateContract(Laboratory laboratory, IMedInstitutClient client,
                                      DateTime startDate, DateTime? endDate = null)
        {
            if (laboratory == null || client == null)
                throw new ArgumentNullException("Лаборатория и клиент обязательны");

            // Проверяем активный договор
            var existing = GetActiveContract(laboratory, client);
            if (existing != null)
                throw new InvalidOperationException($"Уже есть активный договор: {existing.ID}");

            var contract = new Contract(laboratory, client, startDate, endDate);
            _contracts.Add(contract);

            Console.WriteLine($"Договор создан: {contract.ID}");
            return contract;
        }

        public void TerminateContract(string contractId)
        {
            var contract = GetContract(contractId);
            if (contract == null)
                throw new ArgumentException($"Договор не найден: {contractId}");

            if (contract.IsActive)
            {
                contract.IsActive = false;
                contract.EndDate = DateTime.Now;
                Console.WriteLine($"Договор расторгнут: {contractId}");
            }
        }

        public Contract GetContract(string contractId) =>
            _contracts.FirstOrDefault(c => c.ID == contractId);

        public Contract GetActiveContract(ILaboratory laboratory, IMedInstitutClient client) =>
            _contracts.FirstOrDefault(c =>
                c.Laboratory == laboratory &&
                c.Client == client &&
                c.IsActive &&
                (!c.EndDate.HasValue || c.EndDate.Value > DateTime.Now));

        // ============ ПОИСК ============

        public List<Contract> GetContractsByLaboratory(ILaboratory laboratory) =>
            _contracts.Where(c => c.Laboratory == laboratory).ToList();

        public List<Contract> GetContractsByClient(IMedInstitutClient client) =>
            _contracts.Where(c => c.Client == client).ToList();

        public List<Contract> GetActiveContracts() =>
            _contracts.Where(c => c.IsActive && (!c.EndDate.HasValue || c.EndDate.Value > DateTime.Now)).ToList();

        // ============ УТИЛИТА ============

        public void PrintAllContracts()
        {
            Console.WriteLine("\n=== ВСЕ ДОГОВОРЫ ===");

            if (!_contracts.Any())
            {
                Console.WriteLine("Договоров нет");
                return;
            }

            foreach (var contract in _contracts.OrderBy(c => c.StartDate))
            {
                string status = contract.IsActive && (!contract.EndDate.HasValue || contract.EndDate.Value > DateTime.Now)
                    ? "[АКТИВЕН]" : "[ЗАВЕРШЕН]";
                Console.WriteLine($"{status} {contract}");
            }
        }
    }
}
