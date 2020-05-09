using System.ComponentModel.DataAnnotations;
using static Bogus.DataSets.Name;

namespace MyApiWithDoc.Responses
{
    public class ClientResponse
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public bool Enabled { get; set; }

        public ClientResponse(int id, string name, string email, Gender gender, string phone, bool enabled)
        {
            Id = id;
            Name = name;
            Email = email;
            Gender = gender;
            Phone = phone;
            Enabled = enabled;
        }
    }
}
