using System;
using System.Linq;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            if (dbContext.Students.Any())
            {
                return;
            }

            var students = new[]
            {
                new Student
                {
                    FirstMidName = "Carson",
                    LastName = "Alexander",
                    EnrollmentDate = DateTime.Parse("2005-09-01")
                },
                new Student
                {
                    FirstMidName = "Meredith",
                    LastName = "Alonso",
                    EnrollmentDate = DateTime.Parse("2002-09-01")
                },
                new Student
                {
                    FirstMidName = "Arturo",
                    LastName = "Anand",
                    EnrollmentDate = DateTime.Parse("2003-09-01")
                },
                new Student
                {
                    FirstMidName = "Gytis",
                    LastName = "Barzdukas",
                    EnrollmentDate = DateTime.Parse("2002-09-01")
                },
                new Student {FirstMidName = "Yan", LastName = "Li", EnrollmentDate = DateTime.Parse("2002-09-01")},
                new Student
                {
                    FirstMidName = "Peggy",
                    LastName = "Justice",
                    EnrollmentDate = DateTime.Parse("2001-09-01")
                },
                new Student
                {
                    FirstMidName = "Laura",
                    LastName = "Norman",
                    EnrollmentDate = DateTime.Parse("2003-09-01")
                },
                new Student
                {
                    FirstMidName = "Nino",
                    LastName = "Olivetto",
                    EnrollmentDate = DateTime.Parse("2005-09-01")
                }
            };
            foreach (var s in students)
            {
                dbContext.Students.Add(s);
            }

            dbContext.SaveChanges();

            var instructors = new[]
            {
                new Instructor
                {
                    FirstMidName = "Kim",
                    LastName = "Abercrombie",
                    HireDate = DateTime.Parse("1995-03-11")
                },
                new Instructor
                {
                    FirstMidName = "Fadi",
                    LastName = "Fakhouri",
                    HireDate = DateTime.Parse("2002-07-06")
                },
                new Instructor
                {
                    FirstMidName = "Roger",
                    LastName = "Harui",
                    HireDate = DateTime.Parse("1998-07-01")
                },
                new Instructor
                {
                    FirstMidName = "Candace",
                    LastName = "Kapoor",
                    HireDate = DateTime.Parse("2001-01-15")
                },
                new Instructor
                {
                    FirstMidName = "Roger",
                    LastName = "Zheng",
                    HireDate = DateTime.Parse("2004-02-12")
                }
            };

            foreach (var i in instructors)
            {
                dbContext.Instructors.Add(i);
            }

            dbContext.SaveChanges();

            var departments = new[]
            {
                new Department
                {
                    Name = "English",
                    Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID = instructors.Single(i => i.LastName == "Abercrombie").Id
                },
                new Department
                {
                    Name = "Mathematics",
                    Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID = instructors.Single(i => i.LastName == "Fakhouri").Id
                },
                new Department
                {
                    Name = "Engineering",
                    Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID = instructors.Single(i => i.LastName == "Harui").Id
                },
                new Department
                {
                    Name = "Economics",
                    Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID = instructors.Single(i => i.LastName == "Kapoor").Id
                }
            };

            foreach (var d in departments)
            {
                dbContext.Departments.Add(d);
            }

            dbContext.SaveChanges();

            var courses = new[]
            {
                new Course
                {
                    Id = 1050,
                    Title = "Chemistry",
                    Credits = 3,
                    DepartmentId = departments.Single(s => s.Name == "Engineering").Id
                },
                new Course
                {
                    Id = 4022,
                    Title = "Microeconomics",
                    Credits = 3,
                    DepartmentId = departments.Single(s => s.Name == "Economics").Id
                },
                new Course
                {
                    Id = 4041,
                    Title = "Macroeconomics",
                    Credits = 3,
                    DepartmentId = departments.Single(s => s.Name == "Economics").Id
                },
                new Course
                {
                    Id = 1045,
                    Title = "Calculus",
                    Credits = 4,
                    DepartmentId = departments.Single(s => s.Name == "Mathematics").Id
                },
                new Course
                {
                    Id = 3141,
                    Title = "Trigonometry",
                    Credits = 4,
                    DepartmentId = departments.Single(s => s.Name == "Mathematics").Id
                },
                new Course
                {
                    Id = 2021,
                    Title = "Composition",
                    Credits = 3,
                    DepartmentId = departments.Single(s => s.Name == "English").Id
                },
                new Course
                {
                    Id = 2042,
                    Title = "Literature",
                    Credits = 4,
                    DepartmentId = departments.Single(s => s.Name == "English").Id
                },
            };
            foreach (var c in courses)
            {
                dbContext.Courses.Add(c);
            }

            dbContext.SaveChanges();

            var officeAssignments = new[]
            {
                new OfficeAssignment
                {
                    InstructorId = instructors.Single(i => i.LastName == "Fakhouri").Id,
                    Location = "Smith 17"
                },
                new OfficeAssignment
                {
                    InstructorId = instructors.Single(i => i.LastName == "Harui").Id,
                    Location = "Gowan 27"
                },
                new OfficeAssignment
                {
                    InstructorId = instructors.Single(i => i.LastName == "Kapoor").Id,
                    Location = "Thompson 304"
                },
            };

            foreach (var o in officeAssignments)
            {
                dbContext.OfficeAssignments.Add(o);
            }

            dbContext.SaveChanges();

            var courseInstructors = new[]
            {
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "Chemistry").Id,
                    InstructorId = instructors.Single(i => i.LastName == "Kapoor").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "Chemistry").Id,
                    InstructorId = instructors.Single(i => i.LastName == "Harui").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "Microeconomics").Id,
                    InstructorId = instructors.Single(i => i.LastName == "Zheng").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "Macroeconomics").Id,
                    InstructorId = instructors.Single(i => i.LastName == "Zheng").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "Calculus").Id,
                    InstructorId = instructors.Single(i => i.LastName == "Fakhouri").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "Trigonometry").Id,
                    InstructorId = instructors.Single(i => i.LastName == "Harui").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "Composition").Id,
                    InstructorId = instructors.Single(i => i.LastName == "Abercrombie").Id
                },
                new CourseAssignment
                {
                    CourseId = courses.Single(c => c.Title == "Literature").Id,
                    InstructorId = instructors.Single(i => i.LastName == "Abercrombie").Id
                },
            };

            foreach (var ci in courseInstructors)
            {
                dbContext.CourseAssignments.Add(ci);
            }

            dbContext.SaveChanges();

            var enrollments = new[]
            {
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Alexander").Id,
                    CourseId = courses.Single(c => c.Title == "Chemistry").Id,
                    Grade = Grade.A
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Alexander").Id,
                    CourseId = courses.Single(c => c.Title == "Microeconomics").Id,
                    Grade = Grade.C
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Alexander").Id,
                    CourseId = courses.Single(c => c.Title == "Macroeconomics").Id,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Alonso").Id,
                    CourseId = courses.Single(c => c.Title == "Calculus").Id,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Alonso").Id,
                    CourseId = courses.Single(c => c.Title == "Trigonometry").Id,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Alonso").Id,
                    CourseId = courses.Single(c => c.Title == "Composition").Id,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Anand").Id,
                    CourseId = courses.Single(c => c.Title == "Chemistry").Id
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Anand").Id,
                    CourseId = courses.Single(c => c.Title == "Microeconomics").Id,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Barzdukas").Id,
                    CourseId = courses.Single(c => c.Title == "Chemistry").Id,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Li").Id,
                    CourseId = courses.Single(c => c.Title == "Composition").Id,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentId = students.Single(s => s.LastName == "Justice").Id,
                    CourseId = courses.Single(c => c.Title == "Literature").Id,
                    Grade = Grade.B
                }
            };

            foreach (var e in enrollments)
            {
                var enrollmentInDataBase = dbContext.Enrollments.SingleOrDefault(s => s.Student.Id == e.StudentId &&
                                                                                      s.Course.Id == e.CourseId);
                if (enrollmentInDataBase == null)
                {
                    dbContext.Enrollments.Add(e);
                }
            }

            dbContext.SaveChanges();
        }
    }
}