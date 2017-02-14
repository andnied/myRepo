using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Model.Models
{
    public class Child
    {
        public int Id { get; set; }

        public string ChildName { get; set; }

        public string OtherProperty { get; set; }

        public virtual Value Value { get; set; }

        public int ValueId { get; set; }
    }
}
