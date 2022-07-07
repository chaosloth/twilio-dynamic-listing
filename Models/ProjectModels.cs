using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;

namespace Twilio.Example.Models
{

    public enum Region
    {
        [Display(Name = "New South Wales")]
        NSW = 1,

        [Display(Name = "Victoria")]
        VIC = 2,

        [Display(Name = "Western Australia")]
        WA = 3,

        [Display(Name = "Northern Territory")]
        NT = 4,

        [Display(Name = "Tasmania")]
        TAS = 5,

        [Display(Name = "Queensland")]
        QLD = 6,

        [Display(Name = "ACT")]
        ACT = 7
    }

    public class Listing
    {
        public int ListingId { get; set; }
        public String? Description { get; set; }
    }

    public class Dealer
    {
        public int DealerId { get; set; }
        public String? Name { get; set; }
        public String? PrivateNumber { get; set; }
        public Region? LocalRegion { get; set; }
    }

    public enum Status
    {
        [Display(Name = "Unknown")]
        Unknown = 0,

        [Display(Name = "Assigned")]
        Assigned = 1,

        [Display(Name = "Unassigned")]
        Unassigned = 2,

        [Display(Name = "Available")]
        Available = 3
    }

    public class DynamicNumber
    {
        public int DynamicNumberId { get; set; }

        public int DealerId { get; set; }
        public virtual Dealer? Dealer { get; set; }
        public string? PhoneNumber { get; set; }

        [EnumDataType(typeof(Status))]
        public Status NumberStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LastUsedDate { get; set; }

        public int? ListingId { get; set; }
        public virtual Listing? Listing { get; set; }
    }
}