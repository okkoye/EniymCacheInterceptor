using System;

namespace EniymCacheInterceptor.Demo.Web.Models
{
    public class Person
    {
        public Person(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}