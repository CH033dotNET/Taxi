using System;
namespace BAL.Manager
{
	public interface IAddressManager
	{
		Model.DTO.AddressDTO AddAddress(Model.DTO.AddressDTO address);
		void AddFavoriteAddress(Model.DTO.AddressDTO address);
		void DeleteAddress(int AddressId);
		System.Collections.Generic.IEnumerable<Model.DTO.AddressDTO> GetAddresses();
		System.Collections.Generic.IEnumerable<Model.DTO.AddressDTO> GetAddressesEmulation();
		System.Collections.Generic.IEnumerable<Model.DTO.AddressDTO> GetAddressesForUser(int id);
		Model.DTO.AddressDTO GetById(int id);
		void UpdAddress(Model.DTO.AddressDTO address);
		Model.DTO.AddressDTO UpdateAddress(Model.DTO.AddressDTO address);
	}
}
