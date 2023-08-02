using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RITA.WebAPI.Data.Models
{

	[Microsoft.EntityFrameworkCore.Index(nameof(Suspended))]
	public class TestData : CommonFields
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public bool IsDefault { get; set; }

		[Required]
		public bool Suspended { get; set; }

		[Timestamp]
		public byte[] SuspendedOn { get; set; }

		[MaxLength(50)]
		public string SuspendedBy { get; set; }

		[MaxLength(250)]
		public string Comment { get; set; }

		[Required]
		[Column("TestTypeId")]
		public TestType TestType { get; set; }


		[Required]
		[Column("TestCaseId")]
		public TestCase TestCase { get; set; }
	}
}
