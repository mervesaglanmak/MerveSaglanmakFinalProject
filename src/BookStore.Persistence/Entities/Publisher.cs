using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Persistence.Entities
{
    public class Publisher : Entity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
