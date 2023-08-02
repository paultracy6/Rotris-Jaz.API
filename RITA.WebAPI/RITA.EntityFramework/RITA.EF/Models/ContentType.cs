using System.ComponentModel.DataAnnotations;

namespace RITA.EF.Models;

public class ContentType : CommonFields
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string MimeType { get; set; }
}