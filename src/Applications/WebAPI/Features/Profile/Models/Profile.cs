using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Features.Profile.Models
{
    [Table("tb_profile")]
    public class Profile
    {
        [Key]
        public int id { get; set; }
        public string description { get; set; }
        public string status { get; set; }

        // Relationship with User
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
} 