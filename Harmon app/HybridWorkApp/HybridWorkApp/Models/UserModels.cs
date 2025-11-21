using System.Collections.Generic;

namespace HybridWorkApp.Models
{
    // Base class User - demonstrates inheritance
    public class User
    {
        public string Name { get; set; } = "Usuário";
        public string Email { get; set; } = string.Empty;

        public virtual string GetDisplayName()
        {
            return Name;
        }
    }

    // Employee derives from User and overrides behavior - polymorphism example
    public class Employee : User
    {
        public string Role { get; set; } = "Colaborador";
        public List<ScheduleItem> PreferredSchedule { get; set; } = new();

        public override string GetDisplayName()
        {
            return $"{Name} ({Role})";
        }
    }

    public class UserData
    {
        public string UserName { get; set; } = "Usuário";
        public System.Collections.Generic.List<ScheduleItem> Schedule { get; set; } = new();
    }
}
