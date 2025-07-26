using Shared.Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Features.PreRegistration.Models
{
    public class UserPreRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Required]
        public ProfileEnum profileId { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        [StringLength(11)]
        public string documentNumber { get; set; }

        [Required]
        public bool registrationCompleted { get; set; }
    }
} 