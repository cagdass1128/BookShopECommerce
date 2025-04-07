using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace KitapETicaret18Mart.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[Display(Name = "Başlık")]
		public string? Title { get; set; }
		[Required]
		[Display(Name = "Açıklama")]
		public string? Description { get; set; }
		[Required]
		public string? ISBN { get; set; }
		[Required]
		[Display(Name = "Yazar")]
		public string? Author { get; set; }
		[Required]
		[Display(Name = "Liste Fiyatı")]
		[Range(1, 1000)]
		public double ListPrice { get; set; }

		[Required]
		[Display(Name = "1~50 için Liste Fiyatı")]
		[Range(1, 1000)]
		public double Price { get; set; }

		[Required]
		[Display(Name = "50+ Adet İçin Fiyat")]
		[Range(1, 1000)]
		public double Price50 { get; set; }

		[Required]
		[Display(Name = "100+ Adet İçin Fiyat")]
		[Range(1, 1000)]
		public double Price100 { get; set; }

		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		
		public Category? Category { get; set; }
      
        public string? ImageUrl { get; set; }

	}
}
