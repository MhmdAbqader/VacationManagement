using System.ComponentModel.DataAnnotations;

namespace VacationManagement.Models
{
    public class Department: EntityBase
    {
        
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Description { get; set; } 
    }
}
