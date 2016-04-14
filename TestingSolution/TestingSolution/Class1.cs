using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSolution
{
    public class Resume
    {
        #region Personal Information
        public string FirstName { get { return "Artur"; } }
        public string LastName { get { return "Kordzik"; } }
        #endregion

        #region Professional Experience
        public Position ForEService { get { return new Position {
                    EmployedSince = new DateTime(2015, 12, 01),
                    EmployedTo = null,
                    CompanyName = "eService Sp. z o.o.",
                    PositionName = ".NET Developer",
                    Projects = new List<Project> {
                        new Project {
                            ProjectName = "ComplaintTool",
                            Role = "Backend developer",
                            TechnologiesUsed = new List<string> {
                                ".NET 4.5", "EntityFramework", "Microsoft SQL Server",
                                "PowerShell", "ASP.NET MVC"
                            } },
                        new Project {
                            ProjectName = "DCC",
                            Role = "Developer",
                            TechnologiesUsed = new List<string> {
                                ".NET 4.5", "WinForms", "Microsoft SQL Server"
                            } },
                        new Project {
                            ProjectName = "eVoucher",
                            Role = "Backend Developer",
                            TechnologiesUsed = new List<string> {
                                "Microsoft SQL Server", "Web Services"
                            } }
                    } }; } }
        public Position ForAccenture { get { return new Position {
                    EmployedSince = new DateTime(2015,07,01),
                    EmployedTo = new DateTime(2015,11,30),
                    CompanyName = "Accenture Services",
                    PositionName = "Software Developer Senior Analyst",
                    Projects = new List<Project>  {
                        new Project {
                            ProjectName = "Automation",
                            Role = "Software Developer",
                            TechnologiesUsed = new List<string> {
                                ".NET 4.5", "VBA", "Win32 API", "Sharepoint"
                            } }
                    } }; } }
        #endregion
    }

    public class Position
    {
        public DateTime EmployedSince { get; set; }
        public DateTime? EmployedTo { get; set; }
        public string CompanyName { get; set; }
        public string PositionName { get; set; }
        public IEnumerable<Project> Projects { get; set; }
    }

    public class Project
    {
        public string ProjectName { get; set; }
        public IEnumerable<string> TechnologiesUsed { get; set; }
        public string Role { get; set; }
        public IEnumerable<string> Responsibilities { get; set; }
    }
}
