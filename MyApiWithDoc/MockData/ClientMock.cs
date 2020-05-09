using System.Collections.Generic;
using System.Linq;
using Bogus;
using MyApiWithDoc.Entities;
using static Bogus.DataSets.Name;

namespace MyApiWithDoc.MockData
{
    public class ClientMock
    {
        private List<Client> _data;

        public IEnumerable<Client> Data
        {
            get { return _data; }
        }

        public ClientMock()
        {
            _data = new List<Client>();

            var userId = 0;

            for (int i = 0; i < 100; i++)
            {
                _data.Add(
                    new Faker<Client>("en")
                        .CustomInstantiator(f => new Client(++userId))
                        .RuleFor(u => u.Gender, (f, u) => f.PickRandom<Gender>())
                        .RuleFor(u => u.Name, (f, u) => f.Name.FullName(u.Gender))
                        .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name, null, "api.doc").ToLower())
                        .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber("(##) ####-####"))
                        .RuleFor(u => u.CreatedAt, (f, u) => f.Date.Past(2))
                        .RuleFor(u => u.UpdatedAt, (f, u) => f.Date.Past(1, u.CreatedAt))
                );
            }
        }

        public void Add(Client client)
        {
            _data.Add(client);
        }

        public Client GetById(int id)
        {
            return (from c in _data
                    where c.Id.Equals(id)
                    select c).FirstOrDefault();
        }

        public void Remove(int id)
        {
            _data = (from c in _data
                     where !c.Id.Equals(id)
                     select c).ToList();
        }

        public void Update(Client client)
        {
            _data = (from c in _data
                     select c.Id == client.Id ? client : c).ToList();
        }
    }
}
