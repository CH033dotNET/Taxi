using AutoMapper;
using DAL.Interface;
using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BAL.Manager
{
    public class AddressManager : BaseManager
    {
        public AddressManager(IUnitOfWork uOW)
            : base(uOW)
        {
        }

        public IEnumerable<AddressDTO> GetAddresses()
        {
            var list = from address in uOW.AddressRepo.Get()
                       select new AddressDTO
                       {
                           AddressId = address.AddressId,
                           City = address.City,
                           Street = address.Street,
                           Number = address.Number,
						   Comment = address.Comment,
                           UserId = address.UserId
                       };

            return list.ToList();
        }

        public AddressDTO AddAddress(AddressDTO address)
        {

            var temp = Mapper.Map<UserAddress>(address);
            temp.City=temp.City.Trim();
            temp.Street=temp.Street.Trim();
            temp.Number=temp.Number.Trim();
            temp.Comment = temp.Comment.Trim();
            
            uOW.AddressRepo.Insert(temp);
            uOW.Save();


            return Mapper.Map<AddressDTO>(temp);
        }

        public void DeleteAddress(int AddressId)
        {

            uOW.AddressRepo.Delete(uOW.AddressRepo.GetByID(AddressId));

            uOW.Save();

            return;
        }

        public AddressDTO UpdateAddress(AddressDTO address)
        {
            var temp = uOW.AddressRepo.Get(u => u.AddressId == address.AddressId).First();
            if (temp == null)
            {
                return null;
            }

            uOW.AddressRepo.SetStateModified(temp);

            temp.City = address.City;
            temp.Street = address.Street;
            temp.Number = address.Number;
			temp.Comment = address.Comment;
            uOW.Save();
            return Mapper.Map<AddressDTO>(temp);
        }
        public AddressDTO GetById(int id)
        {
            var item = uOW.AddressRepo.Get().Where(s => s.AddressId == id)
                .FirstOrDefault();

            if (item != null)
            {
                return Mapper.Map<AddressDTO>(item);
            }
            return null;
        }
    }
}
