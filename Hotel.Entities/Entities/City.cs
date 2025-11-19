using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Entities.Entities
{

        public class City : BaseEntity
        {
            public string Name { get; set; }
            public Guid CountryId { get; set; }
            public Country Country { get; set; }

            public ICollection<Hotel> Hotels { get; set; }
        }
    
}
