using Application.DTOs;

namespace Application.DTOs
{
    public class CreateRoleRequestDTO
    {
        public Guid UserId { get; set; }
        public string RequestedRole { get; set; }
        public int? PlanId { get; set; }
        public object Metadata { get; set; } // Deserialize based on role
    }



    public class DeliveryPersonMetadataDTO
    {
        public string VehicleType { get; set; }
        public string LicenseNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class PlantCaretakerMetadataDTO
    {
        public string Specialization { get; set; }
        public string Certifications { get; set; }
        public int YearsOfExperience { get; set; }
    }


}
