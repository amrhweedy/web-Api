﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2.DAL
{
    public interface IDepartmentRepo
    {
        Department? GetwithticketsandDevelopersByid(int id);
    }
}