using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RITA.EF.Models
{
	[Index(nameof(AppId))]
	public class Suite : CommonFields
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int AppId { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		public virtual IEnumerable<TestCase> TestCases { get; set; }
	}
}
