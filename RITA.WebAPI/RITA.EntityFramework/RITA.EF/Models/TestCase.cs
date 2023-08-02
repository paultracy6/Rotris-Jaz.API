using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RITA.EF.Models
{
    [Index(nameof(SuiteId))]
    [Index(nameof(CreatedBy),nameof(CreatedOn), nameof(UpdatedOn), nameof(UpdatedBy), IsDescending = new[] { false,true,true,false },Name = "IX_Created_Updated")]
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
        [MaxLength(10)]
        public string RequestMethod { get; set; }

        [Required]
        public int SuiteId { get; set; }

        public virtual Suite Suite { get; set; }

        public virtual IEnumerable<TestData> Data { get; set; }
    }
}
