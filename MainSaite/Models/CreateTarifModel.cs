using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainSaite.Models
{
    public class CreateTarifModel
    {
        public TarifDTO Tarif { get; set; }
        public List<District> Districts { get; set; }

    }
}