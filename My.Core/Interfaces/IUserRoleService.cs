using System.Collections.Generic;
using System.Threading.Tasks;
using My.Core.Models;

namespace My.Core.Interfaces
{
    public interface IUserRoleService
    {
        ConnectionParams ConnectionParams { get; set; }
        string AdminIP { get; set; }
        ConnectionParams DetermineUserRole(string userIP);
    }
}