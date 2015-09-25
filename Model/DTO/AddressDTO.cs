using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class AddressDTO
    {
        public int AddressId { get; set; }

        
        public string City { get; set; }

        public string Street { get; set; }


        public string Number { get; set; }

        public string Comment { get; set; }
        
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
