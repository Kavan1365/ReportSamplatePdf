using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class DataType: BaseEntity
    {
        public int ID { get; set; }
        public string FAName { get; set; }
        public string Name { get; set; }
    }
}
