using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserLoginHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime LoginTime { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}
