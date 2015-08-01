﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APIfy.Example.Models
{
	public class ModuleDto
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Url { get; set; }
	}
}