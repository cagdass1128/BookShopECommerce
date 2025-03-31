using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KitapETicaret18Mart.Models
{
	public class Category
	{
		[Key]
        public int Id { get; set; }
		[Required]
		[MaxLength(30, ErrorMessage = "Kategori Adı en fazla 30 karakter olabilir")]
		[DisplayName("Kategori Adı")]
		public string? Name { get; set; }

		[DisplayName("Gösterim Sırası")]
		[Range(1,100,ErrorMessage = "Gösterim Sırası 1~100 arasında olabilir")]
		public int DisplayOrder { get; set; }
    }
}
