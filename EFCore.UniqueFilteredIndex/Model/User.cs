using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFCore.UniqueFilteredIndex.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Login { get; set; }

        [Required]
        [MaxLength(16)]
        public string Email { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
