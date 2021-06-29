using My.Core.Constants;
using My.Core.Interfaces;
using My.Core.Models;

namespace My.Core.Services
{
    public class UserRoleService : IUserRoleService
    {
        public ConnectionParams ConnectionParams { get; set; }
        public string AdminIP { get; set; }

        public ConnectionParams DetermineUserRole(string userIP)
        {
            ConnectionParams = new ConnectionParams();
            ConnectionParams.ConnectionIP = userIP;
            if (ConnectionParams.ConnectionIP == AdminIP) ConnectionParams.UserRole = Role.Admin;
            else ConnectionParams.UserRole = Role.Default;
            return ConnectionParams;
        }
    }
}