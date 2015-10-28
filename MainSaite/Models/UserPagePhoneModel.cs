using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainSaite.Models
{
    public class UserPagePhoneModel
    {
        public List<AddressDTO> addresses { get; set; }
        public PersonDTO person { get; set; }
        public List<string> DroPlaces { get; set; }

        public UserPagePhoneModel()
        {
            addresses = new List<AddressDTO>();
            person = new PersonDTO();
            DroPlaces = new List<string>();
        }
    }
}