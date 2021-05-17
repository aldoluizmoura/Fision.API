using System;
using System.Collections.Generic;
using System.Text;

namespace FIsionAPI.Business.Models
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
