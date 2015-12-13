using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankService.Interfaces;
using BankService.Model;

namespace BankService.Repositories
{
    class TransferRepository : ITransferRepository
    {
        private static readonly List<Transfer> Transfers = new List<Transfer>();

        public void Add(Transfer transfer)
        {
            Transfers.Add(transfer);
        }

        public IEnumerable<Transfer> Get()
        {
            return Transfers;
        }

        public Transfer Get(string token)
        {
            return Transfers.FirstOrDefault(t => t.ConfirmationCode == token);
        }
    }
}