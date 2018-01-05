using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entity;
using TaskManager.Repository;

namespace TaskManager.Service
{
    public static class AuthenticationService
    {
        public static User LoggedUser { get; internal set; }

        public static void AuthenticateUser(string username, string password)
        {
            UsersRepository userRepo = new UsersRepository("users.txt");
            AuthenticationService.LoggedUser = userRepo.GetUserByUsernameAndPassword(username, password);
        }
    }
}
