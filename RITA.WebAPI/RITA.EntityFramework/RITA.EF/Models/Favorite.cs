using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RITA.EF.Models
{
    [Index(nameof(UserId))]
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(80)]
        public string FavoriteType { get; set; }

        [Required]
        public int ReferenceId { get; set; }
    }
}
