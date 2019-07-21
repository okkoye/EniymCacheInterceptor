using System;

namespace EniymCacheInterceptor.Demo.Web.Models
{
    public class Person
    {
        public Person()
        {

        }
        public Person(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public Address Address { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}