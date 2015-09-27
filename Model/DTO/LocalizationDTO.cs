using Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class LocalizationDTO
    {
        [Key]
        public int LocalizationId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int UserId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }
        public int DistrictId { get; set; }


    }
}
