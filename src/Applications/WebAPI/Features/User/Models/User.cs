using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Features.User.Models
{
    [Table("tb_user")]
    public class User
    {
        [Key]
        public Guid id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string documentNumber { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string? photo { get; set; }

        public int attempts { get; set; } = 0;
        public bool blocked { get; set; } = false;
        public string? confirmationCode { get; set; }
        public DateTime? codeExpirationDate { get; set; }

        public ICollection<UserProfile> UserProfiles { get; set; }
    }
} 