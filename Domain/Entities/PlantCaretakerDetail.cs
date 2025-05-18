using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PlantCaretakerDetail
    {
        public int Id { get; set; }
        public int RoleRequestId { get; set; }
        public string Specialization { get; set; }
        public string Certifications { get; set; }
        public int YearsOfExperience { get; set; }

        public RoleRequest RoleRequest { get; set; }
    }

}
