﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Models.Interfaces
{
    public interface ITeacherRepository : IPersonRepository<Teacher>
    {
        List<Classs> GetClasses(int teacherId);
        List<Subject> GetSubjects(int teacherId);
    }
}