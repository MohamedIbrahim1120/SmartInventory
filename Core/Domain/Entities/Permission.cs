﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}
