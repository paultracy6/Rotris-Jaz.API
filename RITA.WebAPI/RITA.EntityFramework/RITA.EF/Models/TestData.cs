using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RITA.EF.Models
{

    [Microsoft.EntityFrameworkCore.Index(nameof(Suspended))]
    public class TestData : CommonFields
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public int StatusCode { get; set; }

        [Required]
        public bool Suspended { get; set; }

        [Timestamp]
        public DateTime SuspendedOn { get; set; }

        [MaxLength(50)]
        public string SuspendedBy { get; set; }

        [MaxLength(250)]
        public string Comment { get; set; }

        [Required]
        [Column("TestTypeId")]
        public int TestTypeId { get; set; }

        public virtual TestType TestType { get; set; }


        [Required]
        [Column("TestCaseId")]
        public int TestCaseId { get; set; }

        public virtual TestCase TestCase { get; set; }

        [MaxLength(8000)]
        public string RequestContent { get; set; }

        [Column("ContentTypeId")]
        public int RequestContentTypeId { get; set; }

        [Required]
        [MaxLength(8000)]
        public string ResponseContent { get; set; }

        [Required]
        [Column("ContentTypeId")]
        public int ResponseContentTypeId { get; set; }
    }
}
