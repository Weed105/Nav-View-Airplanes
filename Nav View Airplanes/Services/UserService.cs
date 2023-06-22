using Microsoft.EntityFrameworkCore;
using Nav_View_Airplanes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav_View_Airplanes.Services
{
    public class UserService
    {
        public readonly FlightContext _context;
        public UserService(FlightContext context)
        {
            _context = context;
        }
        public async Task<bool> AuthorizationAsync(string login, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == login);
            if (user == null)
                return false;
            if (user.Password.Equals(password))
            {
                Global.CurrentUser = user;
                return true;
            }
            return false;
        }
    }
}
