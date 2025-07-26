namespace WebAPI.Features.User.Models
{
    public class ValidaCadastroDto
    {
        public List<int> profileIds { get; set; }
        public string name { get; set; }
        public bool isValid { get; set; }
        public string errorMessage { get; set; }
        public bool registrationCompleted { get; set; }
    }
} 