﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2.DAL
{
    public class Ticket
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Title { get; set; }

        [ForeignKey("Department")]
        public int Dept_id { get; set; }

        public virtual Department Department { get; set; } = new Department();

        public ICollection<Developer> Developers { get; set; }= new HashSet<Developer>();

      
    }
}
