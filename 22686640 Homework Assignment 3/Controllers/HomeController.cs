using _22686640_Homework_Assignment_3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PagedList;

namespace _22686640_Homework_Assignment_3.Controllers
{

    public class HomeController : Controller
    {
        LibraryEntities db = new LibraryEntities();

        public ActionResult CombinedIndex(int studentPage = 1, int bookPage = 1)
        {
            int studentPageSize = 10;  // Number of students per page
            int bookPageSize = 10;     // Number of books per page

            // Fetch and paginate students
            var studentsPagedList = db.students
                .OrderBy(s => s.surname)
                .ToPagedList(studentPage, studentPageSize);

            // Fetch books with related author and type
            var booksPagedList = db.books
                .Include(b => b.author) // Include related author data
                .Include(b => b.type)   // Include related type data
                .OrderBy(b => b.name)
                .ToPagedList(bookPage, bookPageSize);

            // Create ViewModel
            var viewModel = new CombinedViewModel
            {
                students = studentsPagedList,
                books = booksPagedList
            };

            // Return the View with the ViewModel
            return View(viewModel);
        }


        public async Task<ActionResult> MaintainView(int authorPage = 1, int typesPage = 1, int borrowsPage = 1)
        {
            int authorPageSize = 5;   // Number of authors per page
            int typesPageSize = 5;    // Number of types per page
            int borrowsPageSize = 5;  // Number of borrows per page

            // Fetch the authors, types, and borrows asynchronously
            var authorsList = await db.authors
                .Include(a => a.books)  // Eager load related books if needed
                .OrderBy(a => a.surname)
                .ToListAsync();
            var typesList = await db.types.OrderBy(t => t.name).ToListAsync();
            var borrowsList = await db.borrows.OrderBy(b => b.takenDate).ToListAsync();

            // Convert lists to PagedList objects
            var authorPagedList = authorsList.ToPagedList(authorPage, authorPageSize);
            var typesPagedList = typesList.ToPagedList(typesPage, typesPageSize);
            var borrowsPagedList = borrowsList.ToPagedList(borrowsPage, borrowsPageSize);

            // Create ViewModel with paginated lists
            var viewModel = new CombinedViewModel
            {
                authors = authorPagedList,
                types = typesPagedList,
                borrows = borrowsPagedList
            };

            // Return the View with the ViewModel
            return View(viewModel);
        }

        public ActionResult ReportView()
        {
            
                // Fetch top 15 most borrowed books
                var popularBooks = db.borrows
                    .GroupBy(b => b.bookId)
                    .Select(g => new PopularBookViewModel
                    {
                        BookName = db.books.FirstOrDefault(bk => bk.bookId == g.Key).name,
                        BorrowCount = g.Count()
                    })
                    .OrderByDescending(b => b.BorrowCount)
                    .Take(15) // Limit to the top 15 books
                    .ToList();

            // Pass the data to the view
            var model = new CombinedViewModel
            {
                popularBooks = popularBooks,
                reports = new List<report>() 
            };

                return View(model);
            }



            public async Task<ActionResult> StudentsIndex()
        {
            var students = db.students;
            return View(await students.ToListAsync());
        }

        public async Task<ActionResult> StudentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = await db.students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public ActionResult StudentsCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] student student)
        {
            if (ModelState.IsValid)
            {
                db.students.Add(student);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: students/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = await db.students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: students/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = await db.students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            student student = await db.students.FindAsync(id);
            db.students.Remove(student);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }

}