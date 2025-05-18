namespace Domain.Entities
{
    public class DeliveryPersonDetail
    {
        public int Id { get; set; }
        public int RoleRequestId { get; set; }
        public string VehicleType { get; set; }
        public string LicenseNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public string PhoneNumber { get; set; }

        public RoleRequest RoleRequest { get; set; }
    }

}
