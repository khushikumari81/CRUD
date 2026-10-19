using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuetStudentPortal.Data;
using MuetStudentPortal.Models;

namespace MuetStudentPortal.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchString, string departmentFilter)
        {
            var studentsQuery = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                studentsQuery = studentsQuery.Where(s =>
                    s.FullName.Contains(searchString) ||
                    s.RollNumber.Contains(searchString) ||
                    s.Email.Contains(searchString));
            }

            if (!string.IsNullOrWhiteSpace(departmentFilter))
            {
                studentsQuery = studentsQuery.Where(s => s.Department == departmentFilter);
            }

            ViewBag.SearchString = searchString;
            ViewBag.DepartmentFilter = departmentFilter;
            ViewBag.Departments = await _context.Students
                .Select(s => s.Department)
                .Distinct()
                .ToListAsync();

            var students = await studentsQuery.OrderByDescending(s => s.Id).ToListAsync();
            return View(students);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
            if (student == null) return NotFound();

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RollNumber,FullName,Department,Batch,Email,PhoneNumber,Cgpa,EnrollmentDate")] Student student)
        {
            // Check for duplicate Roll Number
            if (await _context.Students.AnyAsync(s => s.RollNumber == student.RollNumber))
            {
                ModelState.AddModelError("RollNumber", "This Roll Number is already registered.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student registered successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RollNumber,FullName,Department,Batch,Email,PhoneNumber,Cgpa,EnrollmentDate")] Student student)
        {
            if (id != student.Id) return NotFound();

            if (await _context.Students.AnyAsync(s => s.RollNumber == student.RollNumber && s.Id != student.Id))
            {
                ModelState.AddModelError("RollNumber", "Another student already has this Roll Number.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Student profile updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Students.AnyAsync(e => e.Id == student.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}