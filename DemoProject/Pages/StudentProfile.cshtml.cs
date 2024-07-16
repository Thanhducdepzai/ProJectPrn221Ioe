using GoldBracelet_HE172196_HoangThuPhuong;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjectIoePrn.Models;

namespace ProjectIoePrn.Pages
{
    public class StudentProfileModel : PageModel
    {
        private readonly IOE_Project_Clone_PRN221Context _context;
        
        public Student stuTest;
        public Student stu;
        public School school;
        public int studentRank;
        

        public StudentProfileModel(IOE_Project_Clone_PRN221Context context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var studentJson2 = HttpContext.Session.GetObjectFromJson<Student>("Student");
            if (studentJson2!=null)
            {
                stu = studentJson2;
                school = _context.Schools.FirstOrDefault(x => x.SchoolId == stu.SchoolId);
                studentRank = GetStudentRank(stu.StudentId);
                ViewData["StudentRank"] = studentRank;
                
                ViewData["cr"] = GetCurrentRound(stu.StudentId);

                var topStudents = GetTop50Students(GetCurrentRound(stu.StudentId));
                var topStudentsPart = GetTop50StudentsPart(GetCurrentRound(stu.StudentId));
                ViewData["TopStudents"] = topStudents;
                ViewData["TopStudentsPart"] = topStudentsPart;
            }
        }

        private int GetCurrentRound(int studentId)
        {
            return _context.IndividualResultDetails
        .Where(ird => ird.UserId == studentId)
        .OrderByDescending(ird => ird.RoundId)
        .Select(ird => ird.RoundId)
        .FirstOrDefault();
        }

        private int GetStudentRank(int studentId)
        {
            var studentScores = _context.IndividualResultDetails
                .GroupBy(ird => ird.UserId)
                .Select(g => new
                {
                    StudentId = g.Key,
                    TotalScore = g.Sum(ird => ird.RoundScore)
                })
                .OrderByDescending(s => s.TotalScore)
                .ToList();

            var rank = studentScores
                .Select((s, index) => new { s.StudentId, Rank = index + 1 })
                .FirstOrDefault(s => s.StudentId == studentId)?.Rank ?? -1;

            return rank;
        }

        public class StudentScoreViewModel
        {
            public int Rank { get; set; }
            public int StudentId { get; set; }
            public string StudentName { get; set; }
            public string StudentClass { get; set; }
            public string SchoolName { get; set; }
            public string DistrictName { get; set; }
            public int Score { get; set; }
           // public int NumberOfAttempts { get; set; }
            public int TimeSpent { get; set; }
        }

        private List<StudentScoreViewModel> GetTop50Students(int roundId)
        {
            var topStudents = _context.IndividualResultDetails
                .Where(ird => ird.RoundId == roundId)
                .OrderByDescending(ird => ird.RoundScore)
                .Take(50)
                .ToList()
                .Select((ird, index) => new StudentScoreViewModel
                {
                    Rank = index + 1,
                    StudentId = ird.UserId,
                    StudentName = _context.Students.FirstOrDefault(st => st.StudentId == ird.UserId)?.StudentName,
                    StudentClass = _context.Students.FirstOrDefault(st => st.StudentId == ird.UserId)?.StudentClass,
                    SchoolName = _context.Schools.FirstOrDefault(sc => sc.SchoolId == _context.Students.FirstOrDefault(st => st.StudentId == ird.UserId).SchoolId)?.SchoolName,
                    DistrictName = _context.Districts.FirstOrDefault(d => d.DistricId == _context.Schools.FirstOrDefault(sc => sc.SchoolId == _context.Students.FirstOrDefault(st => st.StudentId == ird.UserId).SchoolId).DistrictId)?.DistricName,
                    Score = ird.RoundScore,
                  //  NumberOfAttempts = _context.PresentPartResultDetails.Count(p => p.IndividualResult.UserId == ird.UserId && p.Part.RoundId == roundId),
                    TimeSpent = ird.CompleteTime
                })
                .ToList();

            return topStudents;
        }

        private List<StudentScoreViewModel> GetTop50StudentsPart(int roundId)
        {
            var topStudents = (from ird in _context.IndividualResultDetails
                               join pprd in _context.PresentPartResultDetails on ird.IndividualResultId equals pprd.IndividualResultId
                               join s in _context.Students on ird.UserId equals s.StudentId
                               join sch in _context.Schools on s.SchoolId equals sch.SchoolId
                               join d in _context.Districts on sch.DistrictId equals d.DistricId
                               where ird.RoundId == roundId
                               group new { ird, pprd, s, sch, d } by new { ird.UserId, s.StudentName, s.StudentClass, sch.SchoolName, d.DistricName } into g
                               orderby g.Sum(x => x.pprd.Score) descending
                               select new StudentScoreViewModel
                               {
                                   StudentId = g.Key.UserId,
                                   Rank = 0, // Rank will be assigned later
                                   Score = g.Sum(x => x.pprd.Score),
                                   StudentName = g.Key.StudentName,
                                   StudentClass = g.Key.StudentClass,
                                   SchoolName = g.Key.SchoolName,
                                   DistrictName = g.Key.DistricName,
                                   TimeSpent = g.Sum(x => x.pprd.CompleteTime) // Assuming CompleteTime is a field in PresentPartResultDetail
                               }).Take(50).ToList();

            for (int i = 0; i < topStudents.Count; i++)
            {
                topStudents[i].Rank = i + 1;
            }

            return topStudents;
        }





    }
}
