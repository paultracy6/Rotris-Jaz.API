using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RITA.WebAPI.Data.Models
{
	//[Microsoft.EntityFrameworkCore.Index(nameof(Suite))]
	public class TestCase : CommonFields
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(2048)]
		public string Url { get; set; }

		[Required]
		[MaxLength(8000)]
		public string Name { get; set; }

		[Required]
		[Column("SuiteId")]
		public Suite Suite { get; set; }

	}
}
