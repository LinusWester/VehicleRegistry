using System.Collections.Generic;
using VehicleRegistry.Domain.Interfaces;

namespace VehicleRegistry.Repository.Interfaces
{
    public interface IAccountRepository
    {
        void RegisterAccount(IAccount account);
        List<IAccount> GetAllAccounts();
        IAccount GetAccount(string username);
    }
}
