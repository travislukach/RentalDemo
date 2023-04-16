using System.ComponentModel.DataAnnotations;

namespace RentalDemo.Models
{
    public class Property
    {
        [Editable(false)]
        public int PropertyId { get; set; }
        [MaxLength(50)]
        public string PropertyName { get; set; }
        public Decimal DailyRate { get; set; }
        public int MaximumOccupants { get; set; }
        [MaxLength(50)]
        public string Location { get; set; }
        public string NonStandardFeatures { get; set; }

    }
}
