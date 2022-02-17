using StudentLib.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLib
{
    public enum CommandName
    {
        Invalid,
        Exit,
        Open,
        List,
        Select,
        Help,
        Clear,
        Show,
        Modify,
        Delete,
        Filter,
        Add,
        AddCourse,
        Transcript,
        Importcsv,
        Exportcsv,
        Catalog,
        ShowCatalog
    }

    public class StudentAPI
    {
        private Dictionary<CommandName, ICommandProcessor> m_CommandProcessors;
        private Dictionary<int, Student> m_FilteredStudents;
        private Student m_CurrentStudent;
        private StudentDataManager m_DataManager;
        private List<CatalogEntry> m_Entries;
        private CatalogDataManager m_CatalogDataManager;

        public Dictionary<int, Student> FilteredStudents => m_FilteredStudents;
        public Student CurrentStudent => m_CurrentStudent;
        public List<CatalogEntry> Entries => m_Entries;
        
        public StudentAPI()
        {
            //Add Student Data Manager
            m_DataManager = new();
            //Add Catalog Data Manager
            m_CatalogDataManager = new();
            // Initialize command processors
            m_CommandProcessors = new();
            m_CommandProcessors.Add(CommandName.Help, new HelpCommandProcessor(m_CommandProcessors));
            m_CommandProcessors.Add(CommandName.Catalog, new CatalogCommandProcessor(m_CatalogDataManager));
            m_CommandProcessors.Add(CommandName.AddCourse, new AddCourseCommandProcessor(m_CatalogDataManager));
            m_CommandProcessors.Add(CommandName.ShowCatalog, new ShowCatalogCommandProcessor(m_CatalogDataManager));
            m_CommandProcessors.Add(CommandName.Open, new OpenCommandProcessor(m_DataManager));
            m_CommandProcessors.Add(CommandName.List, new ListCommandProcessor(m_DataManager));
            m_CommandProcessors.Add(CommandName.Transcript, new TranscriptCommandProcessor(m_DataManager));
            m_CommandProcessors.Add(CommandName.Select, new SelectCommandProcessor());
            m_CommandProcessors.Add(CommandName.Show, new ShowCommandProcessor());
            m_CommandProcessors.Add(CommandName.Modify, new ModifyCommandProcessor(m_DataManager));
            m_CommandProcessors.Add(CommandName.Delete, new DeleteCommandProcessor());
            m_CommandProcessors.Add(CommandName.Filter, new FilterCommandProcessor());
            m_CommandProcessors.Add(CommandName.Importcsv, new ImportCSVCommandProcessor(m_DataManager));
            m_CommandProcessors.Add(CommandName.Exportcsv, new ExportCSVCommandProcessor(m_DataManager));
            m_CommandProcessors.Add(CommandName.Add, new AddCommandProcessor(m_DataManager));
        }

        public CommandParseResult ParseCommandName(string inputLine)
        {
           
            string[] parts = inputLine.Split(' ', 2); 
            string commandVerb = parts[0].ToLower();
            string[] args = null;
            if (parts.Length > 1)
                args = parts[1].Split(' '); //string[] // Mechanical Engineering 
            switch (commandVerb)
            {
                case "exit": return new CommandParseResult(CommandName.Exit, commandVerb, args);
                case "open": return new CommandParseResult(CommandName.Open, commandVerb, args);
                case "list": return new CommandParseResult(CommandName.List, commandVerb, args);
                case "help": return new CommandParseResult(CommandName.Help, commandVerb, args);
                case "clear": return new CommandParseResult(CommandName.Clear, commandVerb, args);
                case "select": return new CommandParseResult(CommandName.Select, commandVerb, args);
                case "show": return new CommandParseResult(CommandName.Show, commandVerb, args);
                case "modify": return new CommandParseResult(CommandName.Modify, commandVerb,  args);
                case "delete": return new CommandParseResult(CommandName.Delete, commandVerb, args);
                case "filter": return new CommandParseResult(CommandName.Filter, commandVerb, args);
                case "transcript": return new CommandParseResult(CommandName.Transcript, commandVerb, args);
                case "importcsv": return new CommandParseResult(CommandName.Importcsv, commandVerb, args);
                case "exportcsv": return new CommandParseResult(CommandName.Exportcsv, commandVerb, args);
                case "catalog": return new CommandParseResult(CommandName.Catalog, commandVerb, args);
                case "addcourse": return new CommandParseResult(CommandName.AddCourse, commandVerb, args);
                case "showcatalog": return new CommandParseResult(CommandName.ShowCatalog, commandVerb, args);
                default: return new CommandParseResult(CommandName.Invalid, commandVerb, args);
            }
        }

        public CommandProcessResult ProcessCommand(CommandParseResult parseResult)
        {
            return m_CommandProcessors[parseResult.CommandName].Execute(parseResult, ref m_Entries, ref m_FilteredStudents, ref m_CurrentStudent);
        }

        #region CatalogCommandProcessor
        public class CatalogCommandProcessor : ICommandProcessor
        {
            private CatalogDataManager m_catalogDataManager;
            public string CommandVerb => "catalog";
            public string HelpText => "Course catalog for each major : Enter 'catalog Business' to view the business catalog";

            public CatalogCommandProcessor(CatalogDataManager catalogDataManager)
            {
                m_catalogDataManager = catalogDataManager;
            }
            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();
                if(parseResult.CommandArgs != null && parseResult.CommandArgs.Length != 0)
                {
                    messageLines.Add("-------------------------------------------------------------------------------------------------------------------------");
                    messageLines.Add("Course Catalog".PadLeft(20).PadRight(20));
                    messageLines.Add("-------------------------------------------------------------------------------------------------------------------------");
                    messageLines.Add(String.Format("\t{0, -15} | {1, -15} | {2, -6} | {3, -15}", "Major", "Class Number", "Credits", "Class Name"));
                    messageLines.Add("-------------------------------------------------------------------------------------------------------------------------");
                    entries = m_catalogDataManager.openCatalog(parseResult.CommandArgs[0].ToUpper());
                    for (int i = 0; i < entries.Count; i++)
                    {
                        messageLines.Add(entries[i].ToString());
                    }
                }
                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion

        #region AddCourseCommandProcessor
        public class AddCourseCommandProcessor : ICommandProcessor
        {
            private CatalogDataManager m_catalogDataManager;
            public string CommandVerb => "addcourse";
            public string HelpText => "Add a course. Entry should follow the pattern of major, class number, credits. Example 'addcourse Business 116 4'.";

            public AddCourseCommandProcessor(CatalogDataManager catalogDataManager)
            {
                m_catalogDataManager = catalogDataManager;
            }
            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();
                CatalogEntry entry = new();
                string major = parseResult.CommandArgs[0].ToUpper();
                int classNumber = Int32.Parse(parseResult.CommandArgs[1]);
                int credits = Int32.Parse(parseResult.CommandArgs[2]);
                entry.Major = major;
                entry.ClassNumber = classNumber;
                entry.Credits = credits;
                entries.Add(entry);
                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion

        #region ShowCatalogCommandProcessor
        public class ShowCatalogCommandProcessor : ICommandProcessor
        {
            private CatalogDataManager m_catalogDataManager;
            public string CommandVerb => "showcatalog";
            public string HelpText => "Show the catalog.";

            public ShowCatalogCommandProcessor(CatalogDataManager catalogDataManager)
            {
                m_catalogDataManager = catalogDataManager;
            }
            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();

                messageLines.Add("-------------------------------------------------------------------------------------------------------------------------");
                messageLines.Add("Course Catalog".PadLeft(20).PadRight(20));
                messageLines.Add("-------------------------------------------------------------------------------------------------------------------------");
                messageLines.Add(String.Format("\t{0, -15} | {1, -15} | {2, -6} | {3, -15}", "Major", "Class Number", "Credits", "Class Name"));
                messageLines.Add("-------------------------------------------------------------------------------------------------------------------------");
                for (int i = 0; i < entries.Count; i++)
                {
                    messageLines.Add(entries[i].ToString());
                }
                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion

        #region HelpCommandProcessor
        public class HelpCommandProcessor : ICommandProcessor
        {
            private Dictionary<CommandName, ICommandProcessor> m_CommandProcessors;
            public string CommandVerb => "help";
            public string HelpText => "Print this help text";

            public HelpCommandProcessor(Dictionary<CommandName, ICommandProcessor> commandProcessors)
            {
                m_CommandProcessors = commandProcessors;
            }

            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
            

                List<string> messageLines = new();
                messageLines.Add("clear".PadRight(12) + "Clear the terminal screen");
                messageLines.Add("exit".PadRight(12) + "Terminate the Student REPL");
                foreach (ICommandProcessor processor in m_CommandProcessors.Values)
                    messageLines.Add(processor.CommandVerb.PadRight(12) + processor.HelpText);
                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion

        //convert to accept number of students and seed args
        #region OpenCommandProcessor
        public class OpenCommandProcessor : ICommandProcessor
        {
            private StudentDataManager m_DataManager;

            public string CommandVerb => "open";
            public string HelpText => "Open a list of students";

            public OpenCommandProcessor(StudentDataManager dataManager)
            {
                m_DataManager = dataManager;
            }

            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();

                //TODO: Hande the number of students and seed arguments
                int numStudents = 20;
                int seed = 4;
                filteredStudents = m_DataManager.Open(numStudents, seed);
                messageLines.Add("List of " + numStudents + " students opened with seed " + seed);
                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion
        //Need to add header
        #region ListCommandProcessor
        public class ListCommandProcessor : ICommandProcessor
        {
            private StudentDataManager m_DataManager;

            public string CommandVerb => "list";
            public string HelpText => "Show ids and names for the current filtered student list";

            public ListCommandProcessor(StudentDataManager dataManager)
            {
                m_DataManager = dataManager;
            }

            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();

                if (filteredStudents == null)
                    messageLines.Add("\nNo student list selected. Use 'open' or 'filter' to create filtered list");
                else if (filteredStudents.Count == 0)
                    messageLines.Add("\nFiltered student list is empty");
                else
                {
                    string header = String.Format("\t{0, -15} | {1, -15} | {2, -6} | {3, -15} | {4, -4} | {5, -3} | {6, -3} | {7, -15} | {8, -15} | {9, -15} | {10, -10}",
                                "First Name",
                                "Last Name",
                                "ID",
                                "Major",
                                "GPA",
                                 "CC",
                               "CPS",
                                "Birth Date",
                                 "Date Enrolled",
                                "Graduation Date",
                                "Status");
                    messageLines.Add("---------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    messageLines.Add(header);
                    messageLines.Add("---------------------------------------------------------------------------------------------------------------------------------------------------------------------");

                    for (int i = 0; i < filteredStudents.Count; i++)
                    {
                        Student student = filteredStudents.ElementAt(i).Value;
                        messageLines.Add(student.ToString(i+1));
                    }
                    
                }

                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion

        #region TranscriptCommandProcessor
        public class TranscriptCommandProcessor : ICommandProcessor
        {
            private StudentDataManager m_DataManager;

            public string CommandVerb => "transcript";
            public string HelpText => "Show the classes completed by the selected student";

            public TranscriptCommandProcessor(StudentDataManager dataManager)
            {
                m_DataManager = dataManager;
            }

            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();

                if (filteredStudents == null)
                    messageLines.Add("\nNo student list selected. Use 'open'.");
                else if (filteredStudents.Count == 0)
                    messageLines.Add("\nFiltered student list is empty");
                else if (currentStudent == null)
                {
                    messageLines.Add("\nNo student has been selected.");
                }
                else
                {
                    messageLines.Add("-------------------------------------------------------------------------------------------------------------------------");
                    messageLines.Add("Transcript for Student : ".PadLeft(20) + currentStudent.FullName);
                    messageLines.Add("-------------------------------------------------------------------------------------------------------------------------");
                    List<string> studentTranscript = currentStudent.GetTranscript();
                    for (int i = 0; i < studentTranscript.Count; i++)
                    {
                        messageLines.Add(studentTranscript[i]);
                    }
                }

                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion


        #region AddCommandProcessor
        public class AddCommandProcessor : ICommandProcessor
        {
            private StudentDataManager m_DataManager;
            public string CommandVerb => "Add";
            public string HelpText => "Add a student to the list.";

            public AddCommandProcessor(StudentDataManager dataManager)
            {
                m_DataManager = dataManager;
            }
            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();

                if (parseResult.CommandArgs == null || parseResult.CommandArgs.Length == 0)
                {
                    messageLines.Add("\nDo you want to manually enter a new student, or upload a CSV file with the information?");
                    messageLines.Add("\nEnter 'm' for manual or 'c' for CSV");
                }
                else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 1].Contains("c"))
                {
                    messageLines.Add("\nUploading a new student via CSV file");
                }
                else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 1].Contains("c"))
                {
                    messageLines.Add("\nEnter First Name: ");
                }
                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion


        #region SelectCommandProcessor
        public class SelectCommandProcessor : ICommandProcessor
        {
            public string CommandVerb => "select";
            public string HelpText => "Select a student from the filtered student list by Id";
       
            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();
               
                if (parseResult.CommandArgs == null || parseResult.CommandArgs.Length == 0)
                    messageLines.Add("\nPlease provide the id of the student you want to select");
                else if (!Int32.TryParse(parseResult.CommandArgs[0], out int id))
                    messageLines.Add("\nStudent id must be a 6-digit number");
                else
                {
                    currentStudent = filteredStudents[id];
                    if (currentStudent == null)
                        messageLines.Add("\nStudent with id " + id + " does not exist or is not in filtered student list");
                    else
                        messageLines.Add("\nCurrent student is now " + currentStudent.Id + " " + currentStudent.FirstName + " " + currentStudent.LastName);
                }
                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion

        #region ShowCommandProcessor
        public class ShowCommandProcessor : ICommandProcessor
        {
            public string CommandVerb => "show";
            public string HelpText => "After selecting a student, enter 'show' to show more details.";


            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();

                if (parseResult.CommandArgs == null || parseResult.CommandArgs.Length == 0)
                    if (currentStudent == null)
                        messageLines.Add("\nNo student has been selected.");
                    else
                        messageLines.Add("\n"+currentStudent.ToString());

                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion

        #region ModifyCommandProcessor
        public class ModifyCommandProcessor : ICommandProcessor
        {
            private StudentDataManager m_DataManager;
            public string CommandVerb => "modify";
            public string HelpText => "Select a field to modify";

            public ModifyCommandProcessor(StudentDataManager dataManager)
            {
                m_DataManager = dataManager;
            }

            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();
               
                string switchVar = parseResult.CommandArgs[0].ToLower();
                string result = parseResult.CommandArgs[1];

                switch (switchVar)
                {
                    case "major":
                        currentStudent.Major = result;
                        break;
                    case "firstname":
                        currentStudent.FirstName = result;
                        break;
                    case "lastname":
                        currentStudent.LastName = result;
                        break;
                    case "gpa":
                        //try
                        //{
                            currentStudent.GPA = Double.Parse(result);
                        //}
                        //finally
                        //{
                           //messageLines.Add("Could not parse GPA - try a different value");
                        //}
                        break;
                    case "creditscompleted":
                        //try
                        //{
                            currentStudent.CreditsCompleted = Int32.Parse(result);
                        //}
                        //finally
                        //{
                            //messageLines.Add("Could not parse credits completed - try another value");
                        //}
                        break;

                    case "birthdate":
                        //try
                        //{
                            currentStudent.BirthDate = DateTime.Parse(result);
                        //}
                        //finally
                        //{
                            //messageLines.Add("Could not parse birth date - please enter a new date");
                        //}
                        break;

                    case "estimatedgraduationdate":
                        //try
                        //{
                            currentStudent.EstimatedGraduationDate = DateTime.Parse(result);
                        //}
                        //finally
                        //{
                           //messageLines.Add("Could not parse graduation date - please enter a new date");
                        //}
                        break;
                    case "credispersemester":
                        //try
                        //{
                            currentStudent.CreditsPerSemester = Int32.Parse(result);
                        //}
                        //finally
                        //{
                            //messageLines.Add("Could not parse credits per semester - try another value");
                       //}
                        break;
                    case "estimatedgraduationstatus":
                        //try
                        //{
                            currentStudent.Status = (GraduationStatus)Enum.Parse(typeof(GraduationStatus), result);
                        //}
                        //finally
                        //{
                            //messageLines.Add("Could not parse status - please select another status");
                        //}
                        break;
                    default:
                        messageLines.Add("Could not access that element to modify");
                        break;
                } 

                //if (currentStudent == null)
                //{
                //    messageLines.Add("No student has been selected.");
                //}
                //else if(parseResult.CommandArgs == null || parseResult.CommandArgs.Length == 0) 
                //{
                //    messageLines.Add("Please select a field to modify.");
                //}
                //else 
                //{
                    //string result = parseResult.CommandArgs[0].ToLower();
                    //if(result.Contains("major")) 
                    //{
                    //    currentStudent.Major = parseResult.CommandArgs[1];                          
                    //    // [0]major [1]engineering
                    //    //we need to set the current students major to the 1st index of the command args
                    //}
                    //else if(result.Contains("gpa"))
                    //{
                    //    try
                    //    {
                    //        currentStudent.GPA = Double.Parse(parseResult.CommandArgs[1]);
                    //    }
                    //    finally
                    //    {
                    //        messageLines.Add("Could not parse GPA - try a different value");
                    //    }
                    //}
                    //else if(result.Contains("firstName"))
                    //{
                    //    currentStudent.FirstName = parseResult.CommandArgs[1];
                    //}
                    //else if(result.Contains("lastName"))
                    //{
                    //    currentStudent.LastName = parseResult.CommandArgs[1];
                    //}
                    //else if(result.Contains("creditsCompleted"))
                    //{
                    //    try 
                    //    {
                    //        CreditsCompleted = Int32.Parse(parseResult.CommandArgs[1]);
                    //    }
                    //    finally
                    //    {
                    //        messageLines.Add("Could not parse credits completed - try another value");
                    //    }
                    //}
                    //else if(result.Contains("birthDate"))
                    //{
                    //    try 
                    //    {
                    //        currentStudent.BirthDate = DateTime.Parse(parseResult.CommandArgs[1]);
                    //    }
                    //    finally
                    //    {
                    //        messageLines.Add("Could not parse birth date - please enter a new date");
                    //    }
                    //}
                    //else if(result.Contains("estimatedGraduationDate"))
                    //{
                    //    try
                    //    {
                    //        currentStudent.EstimatedGraduationDate = DateTime.Parse(parseResult.CommandArgs[1]);
                    //    }
                    //    finally
                    //    {
                    //        messageLines.Add("Could not parse graduation date - please enter a new date");
                    //    }
                    //}
                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion

        #region DeleteCommandProcessor
        public class DeleteCommandProcessor : ICommandProcessor
        {
            public string CommandVerb => "delete";
            public string HelpText => "Select a student from the filtered student list by Id to be DELETED";

            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();

                    if (currentStudent == null)
                        messageLines.Add("\nStudent with id " + currentStudent.Id + " does not exist or is not in filtered student list");
                    else
                        filteredStudents.Remove(currentStudent.Id);
                        messageLines.Add("\nStudent " + currentStudent.FirstName + " " + currentStudent.LastName + " has been deleted.");

                currentStudent = null;
                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion

        #region FilterCommandProcessor
        public class FilterCommandProcessor : ICommandProcessor
        {

            public string CommandVerb => "filter";
            public string HelpText => "Filter the list of students by their name, birthdate, GPA, credits completed or major.";

            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();
                if (parseResult.CommandArgs == null || parseResult.CommandArgs.Length == 0)
                {
                    messageLines.Add("\nPlease provide attribute you wish to filter by: ");
                    messageLines.Add("  filter firstName  --> Filter the list of students by the letter of their first name.");
                    messageLines.Add("  filter lastName  --> Filter the list of students by the letter of their last name.");
                    messageLines.Add("  filter birthDate  --> Filter the list of students by their birthdate.");
                    messageLines.Add("  filter gpa  --> Filter the list of students, given a GPA range.");
                    messageLines.Add("  filter completedCredits  --> Filter the list of students by their Credits Completed given a range.");
                    messageLines.Add("  filter major  --> Filter the list of students by their major.\n");
                }
                else
                {
                    if (parseResult.CommandArgs[parseResult.CommandArgs.Length-1].Contains("firstName"))
                    {
                        messageLines.Add("Filter the list by the student's first name starting with that letter.");
                        messageLines.Add("Command:  filter firstName {?} ");
                        messageLines.Add("Example:   filter firstName A ");
                    }
                    else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 1].Contains("lastName"))
                    {
                        messageLines.Add("Filter the list of students by the letter of their last name.");
                        messageLines.Add("Command:  filter lastName {?} ");
                        messageLines.Add("Example:   filter lastName A ");
                    }
                    else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 1].Contains("birthDate"))
                    {
                        messageLines.Add("Filter the list of students by their birth year, month or day.");
                        messageLines.Add("Command:  filter birthDate {t} {?}");
                        messageLines.Add("Example:   filter birthDate yyyy 1993");
                        messageLines.Add("Example:   filter birthDate mm 03");
                        messageLines.Add("Example:   filter birthDate dd 01");
                    }
                    else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 1].Contains("gpa"))
                    {
                        messageLines.Add("Filter the list of students with GPA's that are within a range.");
                        messageLines.Add("Command:  filter gpa {h} {l}");
                        messageLines.Add("Example:  filter gpa 2.4 3.5");
                    }
                    else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 1].Contains("completedCredits"))
                    {
                        messageLines.Add("\nFilter the list of students with a minimum number of completed credits.");
                        messageLines.Add("Command:  filter completedCredits {?}");
                        messageLines.Add("Example:   filter completedCredits 58");
                    }
                    else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 1].Contains("major"))
                    {
                        List<string> majors = new List<string>();
                        foreach(Student s in filteredStudents.Values)
                        {
                            if (!majors.Contains(s.Major))
                            {
                                majors.Add(s.Major);
                            }                         
                        }
                        messageLines.Add("Filter the list of students by on of the following majors.");
                        foreach(string str in majors)
                        {
                            messageLines.Add("filter major "+str);
                        }
                    }
                    // [0]filter [1]firstname [2]J
                    // filter
                    // firstname J

                    //modify 389201
                    //:
                    //firstname
                    //:
                    //john

                    // [0]modify [1]major [2]Programming
                    // currentStudent.major = parseResult.CommandArgs[2]
                    // filter firstname K
                    // modify firstname Kelly
                    else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 2].Contains("firstName"))
                    {
                        List<Student> filterByName = new List<Student>();
                        string letterFilter = parseResult.CommandArgs[parseResult.CommandArgs.Length - 1].ToUpper();
                        // modify major mechanical engineering
                        //lineSplit.Split(' ', 2)
                        //major
                        //mechanical engineering
                        foreach (Student s in filteredStudents.Values)
                        {
                            if (s.FirstName.StartsWith(letterFilter))
                            {
                                filterByName.Add(s);
                            }
                        }
                        if (filterByName.Count == 0)
                        {
                            messageLines.Add("No student's first names start with the letter " + letterFilter + ".");
                        }
                        else
                        {
                            messageLines.Add("---------------------------------------------------------------");
                            messageLines.Add(("Id").PadRight(10) + "Name".PadRight(30) + "# of Students: " + filterByName.Count);
                            messageLines.Add("---------------------------------------------------------------");
                            foreach (Student s in filterByName)
                            {
                                messageLines.Add(s.Id.ToString().PadRight(10) + s.FirstName + " " + s.LastName);
                            }
                        }
                    }
                    else if(parseResult.CommandArgs[parseResult.CommandArgs.Length - 2].Contains("lastName"))
                    {
                        List<Student> filterByName = new List<Student>();
                        string letterFilter = parseResult.CommandArgs[parseResult.CommandArgs.Length - 1].ToUpper();

                        foreach (Student s in filteredStudents.Values)
                        {
                            if (s.LastName.StartsWith(letterFilter))
                            {
                                filterByName.Add(s);
                            }
                        }
                        if (filterByName.Count == 0)
                        {
                            messageLines.Add("No student's last names start with the letter " + letterFilter + ".");
                        }
                        else
                        {
                            messageLines.Add("---------------------------------------------------------------");
                            messageLines.Add(("Id").PadRight(10) + "Name".PadRight(30) + "# of Students: " + filterByName.Count);
                            messageLines.Add("---------------------------------------------------------------");
                            foreach (Student s in filterByName)
                            {
                                messageLines.Add(s.Id.ToString().PadRight(10) + s.FirstName + " " + s.LastName);
                            }
                        }
                    }
                    else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 3].Contains("birthDate"))
                    {
                        List<Student> filterByName = new List<Student>();

                        if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 2].Contains("yyyy"))
                        {
                            int yearFilter = int.Parse(parseResult.CommandArgs[parseResult.CommandArgs.Length - 1]);
                            messageLines.Add("Filtering by year " + yearFilter + "...");
                            foreach (Student s in filteredStudents.Values)
                            {
                                if (s.BirthDate.Year == yearFilter)
                                {
                                    filterByName.Add(s);
                                }
                            }
                        }
                        else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 2].Contains("mm"))
                        {
                            int monthFilter = int.Parse(parseResult.CommandArgs[parseResult.CommandArgs.Length - 1]);
                            messageLines.Add("Filtering by month " + monthFilter + "...");
                            foreach (Student s in filteredStudents.Values)
                            {
                                if (s.BirthDate.Month == monthFilter)
                                {
                                    filterByName.Add(s);
                                }
                            }
                        }
                        else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 2].Contains("dd"))
                        {
                            int dayFilter = int.Parse(parseResult.CommandArgs[parseResult.CommandArgs.Length - 1]);
                            messageLines.Add("Filtering by day " + dayFilter + "...");
                            foreach (Student s in filteredStudents.Values)
                            {
                                if (s.BirthDate.Day == dayFilter)
                                {
                                    filterByName.Add(s);
                                }
                            }
                        }

                        if (filterByName.Count == 0)
                        {
                            messageLines.Add("No student's have a birthdate matching your criteria.");
                        }
                        else
                        {
                            messageLines.Add("-----------------------------------------------------------------------------------");
                            messageLines.Add(("Id").PadRight(10) + "Birthdate".PadRight(15) + "Name".PadRight(30) + "# of Students: " + filterByName.Count);
                            messageLines.Add("-----------------------------------------------------------------------------------");
                            foreach (Student s in filterByName)
                            {
                                messageLines.Add(s.Id.ToString().PadRight(10) + s.BirthDate.ToShortDateString().PadRight(15) + s.FirstName + " " + s.LastName);
                            }
                        }
                    
                }
                    else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 3].Contains("gpa"))
                    {
                        double min = double.Parse(parseResult.CommandArgs[parseResult.CommandArgs.Length - 2]);
                        double max = double.Parse(parseResult.CommandArgs[parseResult.CommandArgs.Length - 1]);
                        List<Student> filteredByGPA = new List<Student>();
                        foreach(Student s in filteredStudents.Values)
                        {
                            if(s.GPA>=min && s.GPA <= max)


                            {
                                filteredByGPA.Add(s);
                            }
                        }
                        if (filteredByGPA.Count == 0)
                        {
                            messageLines.Add("\nNo students have a GPA within the range " + min+" - "+max+".");
                        }
                        else
                        {
                            foreach(Student s in filteredByGPA)
                            {
                                messageLines.Add("-----------------------------------------------------------------------------------");
                                messageLines.Add(("Id").PadRight(10) + "GPA".PadRight(10) + "Name".PadRight(30) + "# of Students: " + filteredByGPA.Count);
                                messageLines.Add("-----------------------------------------------------------------------------------");
                                messageLines.Add(s.Id.ToString().PadRight(10) + s.GPA.ToString().PadRight(10)+ s.FirstName+ " " + s.LastName );
                            }
                        }
                    }
                    else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 2].Contains("creditsCompleted"))
                    {

                    }
                    else if (parseResult.CommandArgs[parseResult.CommandArgs.Length - 2].Contains("major"))
                    {

                    }
                }                   
             
                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion

        #region ImportCSVCommandProcessor
        public class ImportCSVCommandProcessor : ICommandProcessor
        {
            private StudentDataManager m_DataManager;

            public string CommandVerb => "importcsv";
            public string HelpText => "Imports new students that have been created in a .csv file.";

            public ImportCSVCommandProcessor(StudentDataManager dataManager)
            {
                m_DataManager = dataManager;
            }

            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();              
                int seed = 4;
                Console.Write("Enter file path: ");
                string filepath = Console.ReadLine();
                Dictionary<int, Student> importedStudents = m_DataManager.AddStudentList(seed, filepath);
                foreach(KeyValuePair<int, Student> csvStudent in importedStudents)
               {
                    filteredStudents.Add(csvStudent.Key, csvStudent.Value);
               }
              
                messageLines.Add("\nNumber of imported students: " + importedStudents.Count+".");
                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion


        #region ExportCSVCommandProcessor
        public class ExportCSVCommandProcessor : ICommandProcessor
        {
            private StudentDataManager m_DataManager;

            public string CommandVerb => "exportcsv";
            public string HelpText => "Exports the current list of students to a .csv file.";

            public ExportCSVCommandProcessor(StudentDataManager dataManager)
            {
                m_DataManager = dataManager;
            }

            public CommandProcessResult Execute(CommandParseResult parseResult, ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent)
            {
                List<string> messageLines = new();
                string fileLocation = m_DataManager.ExportStudentList(filteredStudents);
      
                messageLines.Add("\n"+fileLocation);
                return new() { MessageLines = messageLines.ToArray() };
            }
        }
        #endregion



    }
}