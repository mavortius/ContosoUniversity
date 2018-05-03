using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly SchoolDbContext _context;

        public InstructorsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Instructors
        public async Task<IActionResult> Index(int? id, int? courseId)
        {
            var viewModel = new InstructorIndexData
            {
                Instructors = await _context.Instructors
                    .Include(i => i.OfficeAssignment)
                    .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                    .ThenInclude(i => i.Enrollments)
                    .ThenInclude(i => i.Student)
                    .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                    .ThenInclude(i => i.Department)
                    .AsNoTracking()
                    .OrderBy(i => i.LastName)
                    .ToListAsync()
            };

            if (id != null)
            {
                ViewData["InstructorId"] = id.Value;
                var instructor = viewModel.Instructors.Single(i => i.Id == id.Value);
                viewModel.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }

            if (courseId != null)
            {
                ViewData["CourseId"] = courseId.Value;
                viewModel.Enrollments = viewModel.Courses.Single(x => x.Id == courseId).Enrollments;
            }

            return View(viewModel);
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .SingleOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        public IActionResult Create()
        {
            var instructor = new Instructor {CourseAssignments = new List<CourseAssignment>()};

            PopulateAssignedCourseData(instructor);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstMidName,HireDate,LastName,OfficeAssignment")]
            Instructor instructor, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.CourseAssignments = new List<CourseAssignment>();

                foreach (var course in selectedCourses)
                {
                    var courseToAdd = new CourseAssignment
                    {
                        InstructorId = instructor.Id,
                        CourseId = int.Parse(course)
                    };
                    instructor.CourseAssignments.Add(courseToAdd);
                }
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            PopulateAssignedCourseData(instructor);

            return View(instructor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments).ThenInclude(i => i.Course)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            PopulateAssignedCourseData(instructor);

            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorToUpdate = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments).ThenInclude(i => i.Course)
                .SingleOrDefaultAsync(s => s.Id == id);

            if (await TryUpdateModelAsync(instructorToUpdate, "",
                i => i.FirstMidName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment))
            {
                if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
                {
                    instructorToUpdate.OfficeAssignment = null;
                }

                UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                                                 "Try again, and if the problem persists, " +
                                                 "see your system administrator.");
                }

                return RedirectToAction(nameof(Index));
            }

            UpdateInstructorCourses(selectedCourses, instructorToUpdate);
            PopulateAssignedCourseData(instructorToUpdate);

            return View(instructorToUpdate);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .SingleOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.CourseAssignments)
                .SingleOrDefaultAsync(i => i.Id == id);
            var departments = await _context.Departments
                .Where(d => d.InstructorID == id)
                .ToListAsync();

            departments.ForEach(d => d.InstructorID = null);

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _context.Courses;
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseId));
            var viewModel = allCourses.Select(course => new AssignedCourseData
                {
                    CourseID = course.Id,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.Id)
                })
                .ToList();
            ViewData["Courses"] = viewModel;
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCoursesHs = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructorToUpdate.CourseAssignments.Select(c => c.Course.Id));

            foreach (var course in _context.Courses)
            {
                if (selectedCoursesHs.Contains(course.Id.ToString()))
                {
                    if (!instructorCourses.Contains(course.Id))
                    {
                        instructorToUpdate.CourseAssignments.Add(new CourseAssignment
                        {
                            InstructorId = instructorToUpdate.Id,
                            CourseId = course.Id
                        });
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.Id))
                    {
                        var courseToRemove =
                            instructorToUpdate.CourseAssignments.Single(i => i.CourseId == course.Id);
                        _context.Remove(courseToRemove);
                    }
                }
            }
        }
    }
}