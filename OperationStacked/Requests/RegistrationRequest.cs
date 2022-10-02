using System.ComponentModel.DataAnnotations;

namespace OperationStacked.Requests
{
    public class RegistrationRequest
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
