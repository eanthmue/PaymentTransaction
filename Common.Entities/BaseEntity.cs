using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
