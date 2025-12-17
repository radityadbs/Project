using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;


namespace FinalProject.Services
{
    public class UserLogService
    {
         private readonly AppDbContext _db;

        public UserLogService(AppDbContext db)
        {
            _db = db;
        }

        public async Task LogAsync(string email, string action)
        {
            _db.UserLogs.Add(new UserLog
            {
                Email = email,
                Action = action
            });

            await _db.SaveChangesAsync();
        }
    }
}