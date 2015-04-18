﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Models.Interfaces;
using Models.Models;

namespace Repositories.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        EDziennikContext db = new EDziennikContext();
        public List<Subject> GetAll()
        {
            return db.Subjects.ToList();
        }

        public Subject FindById(int id)
        {
            return db.Subjects.Single(a => a.Id == id);
        }

        public void Insert(Subject item)
        {
            db.Subjects.Add(item);
        }

        public void Update(Subject item)
        {
            var subject = db.Subjects.Single(a => a.Id == item.Id);
            subject.ClassId = item.Id;
            subject.ClassroomId = item.ClassroomId;
            subject.Id = item.Id;
            subject.Name = item.Name;
            subject.TeacherId = item.TeacherId;
        }

        public void Delete(int id)
        {
            var subject = db.Subjects.Single(a => a.Id == id);
            db.Subjects.Remove(subject);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}