using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RITA.WebAPI.Data.Models
{
	//[Microsoft.EntityFrameworkCore.Index(nameof(TestData))]
	public class ResponseField : CommonFields
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(8000)]
		public string Name { get; set; }

		[Required]
		[MaxLength(8000)]
		public string Value { get; set; }

		[Required]
		[Column("TestDataId")]
		public TestData TestData { get; set; }
	}
}
