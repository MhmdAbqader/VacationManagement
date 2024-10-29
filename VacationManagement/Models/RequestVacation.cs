using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacationManagement.Models
{
    public class RequestVacation : EntityBase
    {
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public bool Approve { get; set; }
        public DateTime? ApproveDate { get; set; }

        public string? Comment { get; set; }

        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        [ForeignKey("EmpId")]
        public Employee? Employee { get; set; }

        [Display(Name = "Vacation Type")]
        public int VacationTypeId { get; set; }
        [ForeignKey("VacationTypeId")]
        public VacationType? VacationType { get; set; }

        public List<VacationPlan> VacationPlansList { get; set; } = new List<VacationPlan>();
    }
}
