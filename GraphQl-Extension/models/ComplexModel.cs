using System.Collections.Generic;

namespace GraphQL_Extension.models
{
    public class ComplexModelParameters
    {
        public ComplexModelParameters(string name, int age)
        {
            Name = name;
            Age = age;
            Address = null;
        }

        public string Name { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }

    }

    public class Address
    {
        public Address(string street, string city, string country)
        {
            Street = street;
            City = city;
            Country = country;
        }

        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
    
}