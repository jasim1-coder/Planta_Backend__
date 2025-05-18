using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreatePlantCaretakerRoleRequestDTO
    {
        public Guid UserId { get; set; }
        public int PlanId { get; set; }
        public string Specialization { get; set; }
        public string Certifications { get; set; }
        public int YearsOfExperience { get; set; }
    }

}
