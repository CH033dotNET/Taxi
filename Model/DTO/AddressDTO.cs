﻿using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class AddressDTO
	{
		[Key]
		public int AddressId { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredError")]
		[StringLength(50)]
		public string City { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredError")]
		[StringLength(100)]
		public string Street { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredError")]
		[StringLength(10)]
		public string Number { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredError")]
		[StringLength(500)]
		public string Comment { get; set; }

		
		public int UserId { get; set; }
	}
}
