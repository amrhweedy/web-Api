﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2.DAL;

public class Developer
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<Ticket> Tickets { get; set; }=new HashSet<Ticket>();
}
