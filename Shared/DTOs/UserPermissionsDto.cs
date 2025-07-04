﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class UserPermissionsDto
    {
        public string Email { get; set; } = "";
        public string FullName { get; set; } = "";
        public List<string> Roles { get; set; } = new();
    }
}
