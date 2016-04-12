using System;
using System.Collections.Generic;
using Model.DTO;

namespace BAL.Manager
{
	public interface IAddressManager
	{
		AddressDTO AddAddress(AddressDTO address);
		void AddFavoriteAddress(AddressDTO address);
		void DeleteAddress(int AddressId);
		IEnumerable<AddressDTO> GetAddresses();
		IEnumerable<AddressDTO> GetAddressesEmulation();
		IEnumerable<AddressDTO> GetAddressesForUser(int id);
		AddressDTO GetById(int id);
		void UpdAddress(AddressDTO address);
		AddressDTO UpdateAddress(AddressDTO address);
	}
}
