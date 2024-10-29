using System.ComponentModel.DataAnnotations;

namespace VacationManagement.Models
{
    public class VacationType : EntityBase
    {
        [StringLength(200)]
        public string VacationName { get; set; } = string.Empty;
        [StringLength(7)] // color Code length is 7 Letters

        [Display(Name = "Vacation Color ")]
        public string BackgroundVacationColor { get; set; } = string.Empty;
        [Display(Name ="Number Days")]
        public int NoOfdays { get; set; }

    }
}
