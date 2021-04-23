using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRegistry.Domain.Interfaces
{
    public interface IAccount
    {
        string Username { get;}
        bool Authorized { get;}
        string Password { get;}
    }
}
