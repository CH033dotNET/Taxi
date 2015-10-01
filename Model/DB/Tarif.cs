using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
    public class Tarif
    {

        [Key]
        public int id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }

        [Required]
        public bool IsIntercity { get; set; }

        public int? DistrictId { get; set; }
        [Required]
        public decimal MinimalPrice { get; set; }
        [Required]
        public decimal StartPrice { get; set; }
        [Required]
        public decimal OneMinuteCost { get; set; }
        [Required]
        public decimal WaitingCost { get; set; }

    }
}
