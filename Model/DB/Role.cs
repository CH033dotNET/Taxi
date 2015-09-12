using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Role() { }

        public Role (Role role)
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
            
        }

    }
}
