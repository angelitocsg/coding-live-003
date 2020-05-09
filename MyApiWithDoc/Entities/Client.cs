using System;
using static Bogus.DataSets.Name;

namespace MyApiWithDoc.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Enabled { get; set; } = false;

        public Client(int id, string name, string email, Gender gender, string phone)
        {
            Id = id;
            Name = name;
            Email = email;
            Gender = gender;
            Phone = phone;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public Client(int id)
        {
            Id = id;
        }
    }
}
