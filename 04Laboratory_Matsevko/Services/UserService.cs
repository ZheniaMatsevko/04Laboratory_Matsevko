using _04Laboratory_Matsevko.Models;
using _04Laboratory_Matsevko.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04Laboratory_Matsevko.Services
{
    class UserService
    {
        private static FileRepository Repository = new FileRepository();
        public List<Person> GetAllUsers()
        {
            return Repository.GetAll();
        }
        public async void Serialize(List<Person> people)
        {
            foreach(Person p in people)
            {
                await Repository.AddOrUpdateAsync(p);
            }
        }
     
        public async void Edit(Person person, String oldEmail)
        {
            if(person.Email!=oldEmail)
                Repository.Delete(oldEmail);
            await Repository.AddOrUpdateAsync(person);
        }
        public void RemoveOne(Person person)
        {
            Repository.Delete(person.Email);
        }
        public async void SerializeOne(Person person)
        {
            await Repository.AddOrUpdateAsync(person);
        }
    }
}
