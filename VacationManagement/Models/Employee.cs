using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationManagement.Models
{
    public class Employee : EntityBase
    {
        [StringLength(120)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Vacation Balance")]
        [Range(1,31)]
        public int VacationBalance { get; set; }

        [Display(Name ="Department")]
        public int Did { get; set; }
        [ForeignKey("Did")]
        public Department? Department { get; set; }

    }
}
