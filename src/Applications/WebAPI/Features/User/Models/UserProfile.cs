using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Features.Profile.Models;
using WebAPI.Features.User.Models;

[Table("user_profile")]
public class UserProfile
{
    [Key]
    [Column("id", TypeName = "uuid")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("user_id", TypeName = "uuid")]
    public Guid UserId { get; set; }

    [Required]
    [Column("profile_id", TypeName = "int")]
    public int ProfileId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [ForeignKey("ProfileId")]
    public Profile Profile { get; set; }
} 