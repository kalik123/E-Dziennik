﻿using edziennik.Resources;
using Microsoft.AspNet.Identity;
using Models.Models;
using Repositories.Repositories;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using edziennik.Models.ViewModels;
using PagedList;
using System;

namespace edziennik.Controllers
{
    [Authorize(Roles = "Admins")]
    public class TeachersController : PersonController
    {
        private readonly TeacherRepository teacherRepo;

        public TeachersController(ApplicationUserManager userManager,TeacherRepository _repo)
            :base(userManager)
        {
            teacherRepo = _repo;
        }

        // GET: Teachers
        public ActionResult Index(int? page, string sortOrder)
        {
            int currentPage = page ?? 1;
            var items = teacherRepo.GetAll();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSort = String.IsNullOrEmpty(sortOrder) ? "IdAsc" : "";
            ViewBag.PeselSort = sortOrder == "PeselAsc" ? "Pesel" : "PeselAsc";
            ViewBag.FirstNameSort = sortOrder == "FirstNameAsc" ? "FirstName" : "FirstNameAsc";
            ViewBag.SecondNameSort = sortOrder == "SecondNameAsc" ? "SecondName" : "SecondNameAsc";
            ViewBag.SurnameSort = sortOrder == "SurnameAsc" ? "Surname" : "SurnameAsc";

            switch (sortOrder)
            {
                case "Pesel":
                    items = items.OrderByDescending(s => s.Pesel);
                    break;
                case "PeselAsc":
                    items = items.OrderBy(s => s.Pesel);
                    break;
                case "FirstName":
                    items = items.OrderByDescending(s => s.FirstName);
                    break;
                case "FirstNameAsc":
                    items = items.OrderBy(s => s.FirstName);
                    break;
                case "SecondName":
                    items = items.OrderByDescending(s => s.SecondName);
                    break;
                case "SecondNameAsc":
                    items = items.OrderBy(s => s.SecondName);
                    break;
                case "Surname":
                    items = items.OrderByDescending(s => s.Surname);
                    break;
                case "SurnameAsc":
                    items = items.OrderBy(s => s.Surname);
                    break;
                case "IdAsc":
                    items = items.OrderBy(s => s.Id);
                    break;
                default:    // id descending
                    items = items.OrderByDescending(s => s.Id);
                    break;
            }

            return View(items.ToList().ToPagedList<Teacher>(currentPage, 10));
         
        }

        // GET: Teachers/Details/5ff
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = teacherRepo.FindById(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            var teacherVm = new TeacherDetailsViewModel
            {
                EmailConfirmed = userManager.FindById(teacher.Id).EmailConfirmed,
                FirstName = teacher.FirstName,
                Id = teacher.Id,
                Pesel = teacher.Pesel,
                SecondName = teacher.SecondName,
                Surname =  teacher.Surname

            };
            return View(teacherVm);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TeacherRegisterViewModel teacherVm)
        {
            if (ModelState.IsValid)
            {
                var userid = await CreateUser(teacherVm, "Teachers");
                
                if (userid == "Error") return View(teacherVm);
                
                var teacher = new Teacher()
                {
                    Id = userid,
                    FirstName = teacherVm.FirstName,
                    SecondName = teacherVm.SecondName,
                    Surname = teacherVm.Surname,
                    Pesel = teacherVm.Login
                };
                teacherRepo.Insert(teacher);
                teacherRepo.Save();
                Logs.SaveLog("Create", User.Identity.GetUserId(), 
                             "Teacher", teacher.Id, Request.UserHostAddress);
                return RedirectToAction("Index");
            }

            return View(teacherVm);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = teacherRepo.FindById(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            var user = userManager.FindById(teacher.Id);
            var teacherEditVm = new TeacherEditViewModel
            {
                FirstName = teacher.FirstName,
                Email = user.Email,
                Id = teacher.Id,
                Login = teacher.Pesel,
                SecondName = teacher.SecondName,
                Surname = teacher.Surname,
                EmailConfirmed = user.EmailConfirmed,
                AvatarUrl = user.AvatarUrl
            };

            return View(teacherEditVm);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TeacherEditViewModel teacherVm)
        {
            if (ModelState.IsValid)
            {
                var teacher = new Teacher
                {
                    Id = teacherVm.Id,
                    FirstName = teacherVm.FirstName,
                    Pesel = teacherVm.Login,
                    SecondName = teacherVm.SecondName,
                    Surname = teacherVm.Surname
                };

                teacherRepo.Update(teacher);
                teacherRepo.Save();
                
                var user = await userManager.FindByIdAsync(teacherVm.Id);
                await UpdateUser(user, teacher, teacherVm.Email,teacherVm.EmailConfirmed);
                Logs.SaveLog("Edit", User.Identity.GetUserId(), 
                             "Teacher", teacher.Id, Request.UserHostAddress);

                return RedirectToAction("Index");
            }
            return View(teacherVm);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = teacherRepo.FindById(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            teacherRepo.Delete(id);
            teacherRepo.Save();
            DeleteUser(id);
            Logs.SaveLog("Delete", User.Identity.GetUserId(), 
                         "Teacher", id, Request.UserHostAddress);
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            teacherRepo.Dispose();
            base.Dispose(disposing);
        }

    }
}
