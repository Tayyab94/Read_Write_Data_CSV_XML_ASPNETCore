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
            //// Read the contents of the CSV file into a string
            //var csvData = System.IO.File.ReadAllText(_csvFilePath);

            //// Return the CSV data as a string in the HTTP response
            //return Ok(csvData);

            // Read the contents of the CSV file into a string
            var csvData = System.IO.File.ReadAllText(_csvFilePath, Encoding.GetEncoding("iso-8859-1"));

            //// Parse the CSV data into a list of CsvRecord objects using CsvHelper
            //var csvRecords = new List<CsvRecord>();


            //using (var reader = new StringReader(csvData))
            //using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            //{
            //    csvRecords = csv.GetRecords<CsvRecord>().ToList();
            //}
            //CsvRecord[] recordsArray = new CsvRecord[csvRecords.Count];

            //for (int i = 0; i < csvRecords.Count; i++)
            //{
            //    recordsArray[i] = new CsvRecord
            //    {
            //        Title = csvRecords[i].Title,
            //        CourseId = csvRecords[i].CourseId,
            //        Instructor = csvRecords[i].Instructor,
            //    };
            //}            //// Parse the CSV data into a list of CsvRecord objects using CsvHelper
            //var csvRecords = new List<CsvRecord>();


            //using (var reader = new StringReader(csvData))
            //using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            //{
            //    csvRecords = csv.GetRecords<CsvRecord>().ToList();
            //}
            //CsvRecord[] recordsArray = new CsvRecord[csvRecords.Count];

            //for (int i = 0; i < csvRecords.Count; i++)
            //{
            //    recordsArray[i] = new CsvRecord
            //    {
            //        Title = csvRecords[i].Title,
            //        CourseId = csvRecords[i].CourseId,
            //        Instructor = csvRecords[i].Instructor,
            //    };
            //}


            // Parse the CSV data into an array of CsvRecord objects using CsvHelper
            CsvRecord[] recordsArray;
            using (var reader = new StringReader(csvData))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                recordsArray = csv.GetRecords<CsvRecord>().ToArray();
            }

            var data = recordsArray.Where(s => Convert.ToInt32(s.SubjectAndCode.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ElementAt(1)) >= 300).Select(s => new CsvRecordVM()
            {
                Instructor = s.Instructor,
                Title = s.Title,
            }).ToList();

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

            //var ned = ddd.GroupBy(s => s.Subject).Select(s=> new GSCV()
            //{
            //    Code=s.Count
            //}).ToList();

            var groupedRecords = ddd.GroupBy(
                                r => r.Subject, // First level key
                                r => new { r.Code, r.Title }, // Second level key and value
                                (subject, courses) => new
                                {
                                    Subject = subject,
                                    Courses = courses.GroupBy(c => c.Code, c => c.Title).Where(c => c.Count() >= 2) // Group by code within each subject
                                }).ToList();

            ViewBag.GrupBy = groupedRecords;
            return View(data);
        }

        public IActionResult One2B()
        {
            // Read the contents of the CSV file into a string
            var csvData = System.IO.File.ReadAllText(_csvFilePath, Encoding.GetEncoding("iso-8859-1"));


            // Parse the CSV data into an array of CsvRecord objects using CsvHelper
            CsvRecord[] recordsArray;
            using (var reader = new StringReader(csvData))
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

            var courseGroups = ddd.GroupBy(c => new { c.Subject });

            // Filter the groups with at least two courses
            var filteredGroups = courseGroups.Where(g => g.Count() >= 2).ToList();
            ViewBag.GrupBy = filteredGroups;
            return View();
        }
        public IActionResult Privacy()
        {
            // Read the contents of the CSV file into a string
            var csvData = System.IO.File.ReadAllText(_csvFilePath, Encoding.GetEncoding("iso-8859-1"));


            // Parse the CSV data into an array of CsvRecord objects using CsvHelper
            CsvRecord[] recordsArray;
            using (var reader = new StringReader(csvData))
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



            //        var groupedRecords = ddd.GroupBy(
            //r => r.Subject, // First level key
            //r => new { r.Code, r.Title }, // Second level key and value
            //(subject, courses) => new
            //{
            //    Subject = subject,
            //    /*  Courses = courses.GroupBy(c => c.Code, c => c.Title)*/ // Group by code within each subject
            //    Courses = courses
            //}).ToList();

            //        var groupedRecords = ddd
            //.GroupBy(r => r.Subject).Where(s=>s.Key.Count() >= 2)
            //.Select(g => new
            //{
            //    subject=g.Key
            //    //Subject = g.Key,
            //    //Courses = g
            //    //           .Where(g2 => g2.() >= 2)
            //    //           .Select(g2 => new
            //    //           {
            //    //               Code = g2.Key,
            //    //               Titles = g2.Select(r => r.Title)
            //    //           })
            //}).ToList();

            //        ViewBag.GrupBy = groupedRecords;

            var courseGroups = ddd.GroupBy(c => new { c.Subject });

            // Filter the groups with at least two courses
            var filteredGroups = courseGroups.Where(g => g.Count() >= 2).ToList();
            ViewBag.GrupBy = filteredGroups;
            return View();
        }

        public IActionResult GetAllTeachers()
        {

            // Read the contents of the CSV file into a string
            var csvData = System.IO.File.ReadAllText("Instructors.csv", Encoding.GetEncoding("iso-8859-1"));

            var csvRecords = new List<Instructor>();
            using (var reader = new StringReader(csvData))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                csvRecords = csv.GetRecords<Instructor>().ToList();
            }


            return View(csvRecords);
        }


        public IActionResult GetAllCoursesWithTeacher()
        {
            var csvData1 = System.IO.File.ReadAllText(_csvFilePath, Encoding.GetEncoding("iso-8859-1"));

            // Parse the CSV data into an array of CsvRecord objects using CsvHelper
            CsvRecord[] recordsArray;
            using (var reader = new StringReader(csvData1))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                recordsArray = csv.GetRecords<CsvRecord>().ToArray();
            }     // Read the contents of the CSV file into a string
            var csvData = System.IO.File.ReadAllText("Instructors.csv", Encoding.GetEncoding("iso-8859-1"));

            var csvRecords = new List<Instructor>();
            using (var reader = new StringReader(csvData))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                csvRecords = csv.GetRecords<Instructor>().ToList();
            }

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
    }
}