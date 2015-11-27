using AutoMapper;
using DAL.Interface;
using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Manager;


namespace BAL.Manager
{
    public class AddressManager : BaseManager, IAddressManager
    {
		
        public AddressManager(IUnitOfWork uOW)
			:base(uOW)
        {

        }

		public IEnumerable<AddressDTO> GetAddressesEmulation()
		{
			List<AddressDTO> list = new List<AddressDTO>();

			for (int j = 1; j < 25; j++)
			{
				list.Add(new AddressDTO() { Number = "25", Street = "Halturina", City = "Chernivtsi", Comment = "", UserId = j });
				list.Add(new AddressDTO() { Number = "76", Street = "Olega Koshoovogo", City = "Chernivtsi", Comment = "", UserId = j });
				list.Add(new AddressDTO() { Number = "86", Street = "Golovna", City = "Chernivtsi", Comment = "", UserId = j });
			}


			return list;
		}

        public IEnumerable<AddressDTO> GetAddresses()
        {
			var addressDat = uOW.AddressRepo.Get();
			if (addressDat != null)
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
				return list;
			}
		return null;
	    }

        public IEnumerable<AddressDTO> GetAddressesForUser(int id)
        {
                var res = uOW.AddressRepo.All.Where(x => x.UserId == id).ToList();

                return res.Select(x=> Mapper.Map<AddressDTO>(x)); 
        }

		public AddressDTO AddAddress(AddressDTO address)
		{
			var temp = Mapper.Map<UserAddress>(address);
			temp.City = address.City.Trim();
			temp.Street = address.Street.Trim();
			temp.Number = address.Number.Trim();
			temp.Comment = address.Comment.Trim();

			uOW.AddressRepo.Insert(temp);
			uOW.Save();

			return Mapper.Map<AddressDTO>(temp);

		}

        public void AddFavoriteAddress(AddressDTO address)
        {
            var temp = Mapper.Map<UserAddress>(address);
            temp.City=address.City.Trim();
            temp.Street=address.Street.Trim();
            temp.Number=address.Number.Trim();
            temp.Comment = address.Comment.Trim();

			uOW.AddressRepo.Insert(temp);
            uOW.Save();
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

        public void UpdAddress(AddressDTO address)
        {
			var currentAddress = uOW.AddressRepo.Get().FirstOrDefault(u => u.AddressId == address.AddressId);

            uOW.AddressRepo.SetStateModified(currentAddress);

            currentAddress.City = address.City;
            currentAddress.Street = address.Street;
            currentAddress.Number = address.Number;
			currentAddress.Comment = address.Comment;
            uOW.Save();
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
