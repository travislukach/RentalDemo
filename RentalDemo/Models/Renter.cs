namespace RentalDemo.Models
{
    public class Renter
    {
        public int RenterId { get; set; }
        public int PropertyId { get; set; }
        public int NumberOfOccupants { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
