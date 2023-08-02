using System.ComponentModel.DataAnnotations;

namespace RITA.WebAPI.Data.Models
{
	public class TestType : CommonFields
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }
	}
}
