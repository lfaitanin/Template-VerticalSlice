namespace WebAPI.Features.Login.Models
{
    public class WhoAmIResponse
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DocumentNumber { get; set; }
        public string Phone { get; set; }
        public string? Photo { get; set; }
        public IEnumerable<int> Profiles { get; set; }
        public bool Blocked { get; set; }
    }
}
