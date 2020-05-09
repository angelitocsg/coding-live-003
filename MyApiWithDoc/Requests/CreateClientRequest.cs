using System.ComponentModel.DataAnnotations;
using static Bogus.DataSets.Name;

namespace MyApiWithDoc.Requests
{
    public class CreateClientRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}
