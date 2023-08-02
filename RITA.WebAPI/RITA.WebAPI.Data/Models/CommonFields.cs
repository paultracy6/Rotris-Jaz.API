using System.ComponentModel.DataAnnotations;

namespace RITA.WebAPI.Data.Models
{
	public class CommonFields
	{
		[Required]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

		[MaxLength(50)]
		[Required]
		public string CreatedBy { get; set; }

		public DateTime? UpdatedOn { get; set; } = null;

		[MaxLength(50)]
		public string UpdatedBy { get; set; } = null;
	}
}
