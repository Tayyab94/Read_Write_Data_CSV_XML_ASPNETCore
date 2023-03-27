using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using task7.Models;
using CsvHelper;
using task7.Models.ViewModels;
using System.Text;

namespace task7.Controllers
{

    public class CsvRecord
    {
        public string SubjectAndCode { get; set; }
        public string Title { get; set; }
        public string CourseId { get; set; }
        public string Instructor { get; set; }

    }

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _csvFilePath;
        public HomeController(ILogger<HomeController> logger)
        {
            //var csvFileName = "Courses1.csv";
            var csvFileName = "Courses2.csv";


            // Get the path to the CSV file in the project directory
            _csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), csvFileName);
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Read the contents of the CSV file into a string
            var CourseCsvData = System.IO.File.ReadAllText(_csvFilePath, Encoding.GetEncoding("iso-8859-1"));

            // Parse the CSV data into an array of CsvRecord objects using CsvHelper
            CsvRecord[] recordsArray;
            using (var reader = new StringReader(CourseCsvData))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                // Converting into an Array
                recordsArray = csv.GetRecords<CsvRecord>().ToArray();
            }

            // Applying the query..  whos course code greater than 3000
            var data = recordsArray.Where(s => Convert.ToInt32(s.SubjectAndCode.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ElementAt(1)) >= 300).Select(s => new CsvRecordVM()
            {
                Instructor = s.Instructor,
                Title = s.Title,
            }).ToList();

            return View(data);
        }

        public IActionResult One2B()
        {
            // Read the contents of the CSV file into a string
            var CourseCsvData = System.IO.File.ReadAllText(_csvFilePath, Encoding.GetEncoding("iso-8859-1"));


            // Parse the CSV data into an array of CsvRecord objects using CsvHelper
            CsvRecord[] recordsArray;
            using (var reader = new StringReader(CourseCsvData))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                recordsArray = csv.GetRecords<CsvRecord>().ToArray();
            }


            var courese = new List<GSCV>();
            foreach (var d in recordsArray)
            {
                courese.Add(new GSCV()
                {
                    Code = d.SubjectAndCode.Split(" ")[1],
                    Subject = d.SubjectAndCode.Split(" ")[0],
                    CourseId = d.CourseId,
                    Instructor = d.Instructor,
                    Title = d.Title,

                });
            }

            //Retrieve and deliver the courses in groups and subgroups (in two levels):
            //Use the course subject, (e.g., CPI, CSE, and IEE), as the first level key, and use the course code (e.g., 240, 310, and 494)
            //as the second level key. Print the groups that have at least two courses in the second level group. 

            var courseGroups = courese.GroupBy(c => new { c.Subject });

            // Filter the groups with at least two courses
            var filteredGroups = courseGroups.Where(g => g.Count() >= 2).ToList();
            ViewBag.GrupBy = filteredGroups;
            return View();
        }


        public IActionResult Privacy()
        {
            // Read the contents of the CSV file into a string
            var CourseCsvData = System.IO.File.ReadAllText(_csvFilePath, Encoding.GetEncoding("iso-8859-1"));


            // Parse the CSV data into an array of CsvRecord objects using CsvHelper
            CsvRecord[] recordsArray;
            using (var reader = new StringReader(CourseCsvData))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                recordsArray = csv.GetRecords<CsvRecord>().ToArray();
            }


            var ddd = new List<GSCV>();
            foreach (var d in recordsArray)
            {
                ddd.Add(new GSCV()
                {
                    Code = d.SubjectAndCode.Split(" ")[1],
                    Subject = d.SubjectAndCode.Split(" ")[0],
                    CourseId = d.CourseId,
                    Instructor = d.Instructor,
                    Title = d.Title,

                });
            }

            //Retrieve and deliver the courses in groups and subgroups (in two levels):
            //Use the course subject, (e.g., CPI, CSE, and IEE), as the first level key, and use the course code (e.g., 240, 310, and 494)
            //as the second level key. Print the groups that have at least two courses in the second level group. 
           
            var courseGroups = ddd.GroupBy(c => new { c.Subject });

            // Filter the groups with at least two courses
            var filteredGroups = courseGroups.Where(g => g.Count() >= 2).ToList();
            ViewBag.GrupBy = filteredGroups;
            return View();
        }

        public IActionResult GetAllTeachers()
        {

            // Read the contents of the CSV file into a string
            var InstrcutroCsvData = System.IO.File.ReadAllText("Instructors.csv", Encoding.GetEncoding("iso-8859-1"));

            var InstructorsList = new List<Instructor>();
            using (var reader = new StringReader(InstrcutroCsvData))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                InstructorsList = csv.GetRecords<Instructor>().ToList();
            }

            return View(InstructorsList);
        }


        public IActionResult GetAllCoursesWithTeacher()
        {
            
            // Reading the Course From CSV fiel
            var CourseCsvData = System.IO.File.ReadAllText(_csvFilePath, Encoding.GetEncoding("iso-8859-1"));
            CsvRecord[] recordsArray;
            using (var reader = new StringReader(CourseCsvData))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                recordsArray = csv.GetRecords<CsvRecord>().ToArray();
            }   

            // Reading the Instructor Data from CSV file
            // Read the contents of the CSV file into a string
            var InstructorCsvData = System.IO.File.ReadAllText("Instructors.csv", Encoding.GetEncoding("iso-8859-1"));

            var csvRecords = new List<Instructor>();
            using (var reader = new StringReader(InstructorCsvData))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                csvRecords = csv.GetRecords<Instructor>().ToList();
            }

            // Applying the query.. Getting the Courses with instructor detail. if Name No math then Instator's email is emply..
            var query = from course in recordsArray
                        join instructor in csvRecords
                        on course.Instructor equals instructor.Name into instructorGroup
                        from instructor in instructorGroup.DefaultIfEmpty()
                        where (Convert.ToInt32(course.SubjectAndCode.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ElementAt(1)) >= 200 && Convert.ToInt32(course.SubjectAndCode.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ElementAt(1)) <= 299)

                        orderby course.SubjectAndCode.Split(" ")[1] ascending
                        select new CoursesWithTeachersViewModel
                        {
                            Subject = course.SubjectAndCode,
                            //Code = course.SubjectAndCode.Split(" ")[1],
                            InstructorEmail = instructor?.Email ?? ""
                        };

                        // This below query is an alterna of above query..

                        //        var query = recordsArray
                        //.Where(c => (Convert.ToInt32(c.SubjectAndCode.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ElementAt(1)) >= 200 && Convert.ToInt32(c.SubjectAndCode.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ElementAt(1)) <= 299))
                        //.OrderBy(c => c.SubjectAndCode)
                        //.Select(c => new
                        //{
                        //    Subject = c.SubjectAndCode.Split(' ')[0],
                        //    Code = c.SubjectAndCode.Split(' ')[1],
                        //    InstructorEmail = csvRecords.FirstOrDefault(i => i.Name == c.Instructor)?.Email ?? ""
                        //}).ToList();

            var result = query.ToList();
            return View(result);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        #region Retrieve and deliver the courses in groups and subgroups (in two levels): Use the course subject, (e.g., CPI, CSE, and IEE), as the first level key, and use the course code (e.g., 240, 310, and 494) as the second level key. Print the groups that have at least two courses in the second level group. 
        //    var ddd = new List<GSCV>();
        //        foreach (var d in recordsArray)
        //        {
        //            ddd.Add(new GSCV()
        //    {
        //        Code = d.SubjectAndCode.Split(" ")[1],
        //                Subject = d.SubjectAndCode.Split(" ")[0],
        //                CourseId = d.CourseId,
        //                Instructor = d.Instructor,
        //                Title = d.Title,

        //            });
        //        }


        //var groupedRecords = ddd.GroupBy(
        //                    r => r.Subject, // First level key
        //                    r => new { r.Code, r.Title }, // Second level key and value
        //                    (subject, courses) => new
        //                    {
        //                        Subject = subject,
        //                        Courses = courses.GroupBy(c => c.Code, c => c.Title).Where(c => c.Count() >= 2) // Group by code within each subject
        //                    }).ToList();

        //ViewBag.GrupBy = groupedRecords;
        #endregion

    }
}