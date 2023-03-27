using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using task7.Models;
using task7.Models.ViewModels;

namespace task7.Controllers
{
    public class XMLClassController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public XMLClassController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Index()
        {
            // Course-XML file Path
            string filePath = _environment.WebRootPath + "/Courses.xml";
            //Read the COurse XMl Noe..
            // Load the XML file into an XmlDocument object
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // Extract the student data using XPath expressions
            XmlNodeList studentNodes = xmlDoc.SelectNodes("//Course");
            List<Course> Courses = new List<Course>();

            // Extracting the XML data and bind with Course Class
            foreach (XmlNode studentNode in studentNodes)
            {
                string SubjectAndCode = studentNode.SelectSingleNode("SubjectAndCode").InnerText;
                string Title = studentNode.SelectSingleNode("Title").InnerText;
                string CourseId = studentNode.SelectSingleNode("CourseId").InnerText;
                string Instructor = studentNode.SelectSingleNode("Instructor").InnerText;
                string Days = studentNode.SelectSingleNode("Days").InnerText;
                string Start = studentNode.SelectSingleNode("Start").InnerText;
                string End = studentNode.SelectSingleNode("End").InnerText;
                string Location = studentNode.SelectSingleNode("Location").InnerText;
                string Dates = studentNode.SelectSingleNode("Dates").InnerText;
                string Units = studentNode.SelectSingleNode("Units").InnerText;
                string Enrollment = studentNode.SelectSingleNode("Enrollment").InnerText;

                var item = new Course
                {
                    SubjectAndCode = SubjectAndCode,
                    CourseId = CourseId,
                    Dates = Dates,
                    Days = Days,
                    End = End,
                    Enrollment = Enrollment,
                    Instructor = Instructor,
                    Location = Location,
                    Start = Start,
                    Title = Title,
                    Units = Units
                };
                Courses.Add(item);
            }

            // Applying the query here
            IEnumerable<task7.Models.ViewModels.XElement> data = Courses.Where(s => Convert.ToInt32(s.SubjectAndCode.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ElementAt(1)) >200
                               && s.SubjectAndCode.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ElementAt(0) == "CPI").Select(s => new task7.Models.ViewModels.XElement()
                               {
                                   Instructor = s.Instructor,
                                   Title = s.Title,
                               }).ToList();
            
            //-------------------- End Read Coursexml----------------

            return View(data);
        }


        public IActionResult TwoPoint2()
        {
            string filePath = _environment.WebRootPath + "/Courses.xml";
            //Read the COurse XMl Noe..
            // Load the XML file into an XmlDocument object
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // Extract the student data using XPath expressions
            XmlNodeList studentNodes = xmlDoc.SelectNodes("//Course");
            
            var CoursesList = new List<GSCV>();
          
            foreach (XmlNode studentNode in studentNodes)
            {

                string SubjectAndCode = studentNode.SelectSingleNode("SubjectAndCode").InnerText;
                string Title = studentNode.SelectSingleNode("Title").InnerText;
                string CourseId = studentNode.SelectSingleNode("CourseId").InnerText;
                string Instructor = studentNode.SelectSingleNode("Instructor").InnerText;
                string Days = studentNode.SelectSingleNode("Days").InnerText;
                string Start = studentNode.SelectSingleNode("Start").InnerText;
                string End = studentNode.SelectSingleNode("End").InnerText;
                string Location = studentNode.SelectSingleNode("Location").InnerText;
                string Dates = studentNode.SelectSingleNode("Dates").InnerText;
                string Units = studentNode.SelectSingleNode("Units").InnerText;
                string Enrollment = studentNode.SelectSingleNode("Enrollment").InnerText;


                CoursesList.Add(new GSCV()
                {
                    Code = SubjectAndCode.Split(" ")[1],
                    Subject = SubjectAndCode.Split(" ")[0],
                    CourseId = CourseId,
                    Instructor = Instructor,
                    Title = Title,

                });

            }

            var courseGroups = CoursesList.GroupBy(c => new { c.Subject });
            // Filter the groups with at least two courses
            var filteredGroups = courseGroups.Where(g => g.Count() >= 2).ToList();
            ViewBag.GrupBy = filteredGroups;
            return View();
        }


        public IActionResult GetAllInstructors(string instructorName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_environment.WebRootPath + "/Instructor.xml");
            // Extract the student data using XPath expressions
            XmlNodeList studentNodes = xmlDoc.SelectNodes("//Instructor");
            List<Instructor> InstructorsList = new List<Instructor>();

            foreach (XmlNode studentNode in studentNodes)
            {
                string Name = studentNode.SelectSingleNode("Name").InnerText;
                string OfficeNumber = studentNode.SelectSingleNode("OfficeNumber").InnerText;
                string Email = studentNode.SelectSingleNode("Email").InnerText;

                var item = new Instructor
                {
                    Name = Name,
                    Email = Email,
                    OfficeNumber = OfficeNumber
                };
                InstructorsList.Add(item);
            }
            return View(InstructorsList);
        }


        public IActionResult TaskTwoPoint2(string instructorName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_environment.WebRootPath + "/Instructor.xml");
            // Extract the student data using XPath expressions
            XmlNodeList studentNodes = xmlDoc.SelectNodes("//Instructor");
            List<Instructor> InstructorsList = new List<Instructor>();

            foreach (XmlNode studentNode in studentNodes)
            {
                string Name = studentNode.SelectSingleNode("Name").InnerText;
                string OfficeNumber = studentNode.SelectSingleNode("OfficeNumber").InnerText;
                string Email = studentNode.SelectSingleNode("Email").InnerText;

                var item = new Instructor
                {
                    Name = Name,
                    Email = Email,
                    OfficeNumber = OfficeNumber
                };
                InstructorsList.Add(item);
            }


            //Read the COurse XMl Noe..
            // Load the XML file into an XmlDocument object
            XmlDocument xmlDoc1 = new XmlDocument();
            xmlDoc1.Load("Courses.xml");

            // Extract the student data using XPath expressions
            XmlNodeList CoursesNodes = xmlDoc1.SelectNodes("//Course");

            var CoursesList = new List<GSCV>();

            foreach (XmlNode studentNode in CoursesNodes)
            {

                string SubjectAndCode = studentNode.SelectSingleNode("SubjectAndCode").InnerText;
                string Title = studentNode.SelectSingleNode("Title").InnerText;
                string CourseId = studentNode.SelectSingleNode("CourseId").InnerText;
                string Instructor = studentNode.SelectSingleNode("Instructor").InnerText;
                string Days = studentNode.SelectSingleNode("Days").InnerText;
                string Start = studentNode.SelectSingleNode("Start").InnerText;
                string End = studentNode.SelectSingleNode("End").InnerText;
                string Location = studentNode.SelectSingleNode("Location").InnerText;
                string Dates = studentNode.SelectSingleNode("Dates").InnerText;
                string Units = studentNode.SelectSingleNode("Units").InnerText;
                string Enrollment = studentNode.SelectSingleNode("Enrollment").InnerText;


                CoursesList.Add(new GSCV()
                {
                    Code = SubjectAndCode.Split(" ")[1],
                    Subject = SubjectAndCode.Split(" ")[0],
                    CourseId = CourseId,
                    Instructor = Instructor,
                    Title = Title,

                });

            }

            var query = from course in CoursesList
                        join instructor in InstructorsList
                        on course.Instructor equals instructor.Name
                        where (int.TryParse(course.Code, out int code) && code >= 200 && code <= 299 && instructor.Name== instructorName)
                        orderby course.Code ascending
                        select new CoursesWithTeachersViewModel
                        {
                            Subject = $"{course.Subject} {course.Code}",
                            InstructorEmail = instructor?.Email ?? ""
                        };

            var result = query.ToList();

            return View(result);
        }
    }
}
