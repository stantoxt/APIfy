using System.ComponentModel.DataAnnotations;

namespace APIfy.Database.Models
{
    public class Module
    {
        [Key]
        public int ModuleId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string URL { get; set; }

        public bool? IsActive { get; set; }
    }
}
