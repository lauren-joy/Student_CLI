using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StudentLib.DTO
{
	public enum GraduationStatus
	{
		InProgess,
		Delayed,
		Graduated
	}
	public class Student : Base
	{

		private int m_Id;
		private string m_FirstName;
		private string m_LastName;
		private DateTime m_BirthDate;
		private double m_GPA;
        private int m_CreditsCompleted;
        private string m_Major;
		private DateTime m_EnrollmentDate;
		private DateTime m_EstimatedGraduationDate;
		private int m_CreditsPerSemester;
		private GraduationStatus m_Status;

		public int Id
		{
			get
			{
				return m_Id;
			}
			set
			{
				if (value != m_Id)
				{
					m_Id = value;
					NotifyPropertyChanged("Id");
				}
			}
		}
		public string FirstName
		{
			get
			{
				return m_FirstName;
			}
			set
			{
				if (value != m_FirstName)
				{
					m_FirstName = value;
					NotifyPropertyChanged("FirstName");
				}
			}
		}
		public string LastName
		{
			get
			{
				return m_LastName;
			}
			set
			{
				if (value != m_LastName)
				{
					m_LastName = value;
					NotifyPropertyChanged("LastName");
				}
			}
		}
		public DateTime BirthDate
		{
			get
			{
				return m_BirthDate;
			}
			set
			{
				if (value != m_BirthDate)
				{
					m_BirthDate = value;
					NotifyPropertyChanged("BirthDate");
				}
			}
		}
		public double GPA
		{
			get
			{
				return m_GPA;
			}
			set
			{
				if (value != m_GPA)
				{
					m_GPA = value;
					NotifyPropertyChanged("GPA");
				}
			}
		}
        public int CreditsCompleted
        {
            get
            {
                return m_CreditsCompleted;
            }
            set
            {
                if (value != m_CreditsCompleted)
                {
                    m_CreditsCompleted = value;
					NotifyPropertyChanged("CreditsCompleted");
					NotifyPropertyChanged("Status");
				}
            }
        }
        public string Major
		{
			get
			{
				return m_Major;
			}
			set
			{
				if (value != m_Major)
				{
					m_Major = value;
					NotifyPropertyChanged("Major");
				}
			}
		}

		public DateTime EnrollmentDate
        {
			get
			{
				return m_EnrollmentDate;
			}
			set
			{
				if (value != m_EnrollmentDate)
				{
					m_EnrollmentDate = value;
					NotifyPropertyChanged("EnrollmentDate");

				}
			}
		}

		public DateTime EstimatedGraduationDate
		{
			get
			{
				return m_EstimatedGraduationDate;
			}
			set
			{
				if (value != m_EstimatedGraduationDate)
				{
					m_EstimatedGraduationDate = value;
					NotifyPropertyChanged("EstimatedGraduationDate");
				}
			}
		}

		public int CreditsPerSemester
		{
			get
			{
				return m_CreditsPerSemester;
			}
			set
			{
				if (value != m_CreditsPerSemester)
				{
					m_CreditsPerSemester = value;
					NotifyPropertyChanged("CreditsPerSemester");
				}
			}
		}

		public GraduationStatus Status
		{
			get
			{
				return m_Status;
			}
			set
			{
				if (value != m_Status)
				{
					m_Status = value;
					NotifyPropertyChanged("Status");
				}
			}
		}





		public bool CanEnroll()
        {
			if(this.EnrollmentDate != null && this.EnrollmentDate < DateTime.Now)
            {
				return true;
            }
			return false;
        }

		public bool CanGraduate()
		{
			if (m_Status == GraduationStatus.InProgess && CreditsCompleted >= 120)
			{
				return true;
			}
			return false;
		}
		public DateTime EstGradDate()
		{
			DateTime potentialGraduationDate = DateTime.Now;
			int CreditsPerMonth = CreditsPerSemester / 4;
			int RequiredMonths = (120 - CreditsCompleted) / CreditsPerMonth;
			return potentialGraduationDate.AddMonths(RequiredMonths);
		}

		public Dictionary<string, string> StudentReport()
        {
			Dictionary<string, string> output = new Dictionary<string, string>();
			output.Add("m_GPA", this.GPA.ToString());
			output.Add("m_CreditsCompleted", this.CreditsCompleted.ToString());
			output.Add("m_Major", this.Major);
			return output;
        }

		public string FullName
		{
			get { return (m_FirstName + " " + m_LastName); }
		}

		private List<string> m_Grades = new List<string>
		{
			"A",
			"A-",
			"B+",
			"B",
			"B-",
			"C+",
			"C",
			"C-",
			"D+",
			"D"
		};

		private List<string> m_transcriptClasses = new List<string>
		{
			" 101",
			" 102",
			" 103",
			" 111",
			" 115",
			" 136",
			" 145",
			" 155",
			" 188",
			" 189",
			" 190",
			" 199",
			" 201",
			" 202",
			" 203",
			" 204",
			" 211",
			" 212",
			" 213",
			" 301",
			" 302",
			" 303",
			" 306",
			" 308",
			" 319",
			" 321",
			" 356",
			" 364",
			" 401",
			" 402",
			" 403",
			" 499",
			" 410",
		};

		public List<string> GetTranscript()
        {
			List<string> output = new List<string>();
			int x = 0;
			Random r = new Random();
			int indexer = 0;
			List<string> gradeList = new List<string>();
			if (this.GPA >= 3.7)
			{
				gradeList = this.m_Grades.GetRange(0, 2);
			}
			else if (this.GPA >= 3.5)
			{
				gradeList = this.m_Grades.GetRange(0, 3);
			}
			else if(this.GPA >= 3.0)
            {
				gradeList = this.m_Grades.GetRange(0, 6);
            }
			else if (this.GPA >= 2.0)
			{
				gradeList = this.m_Grades.GetRange(2, 7);
			}
			else if(this.GPA < 2.0)
            {
				gradeList = this.m_Grades.GetRange(8, 2);
			}
			while (x < this.CreditsCompleted)
            {
				
			    output.Add(this.Major + this.m_transcriptClasses[indexer] + " : " + gradeList[r.Next(0, gradeList.Count)]);
				indexer++;
				x += 4;
            }
			return output;
        }

        public override string ToString()
		{
			//                     first  | last    | Id     | Major   | GPA    |  CC    | CPS    | BD     | ED      | GD      | Status 
			return String.Format("\t{0, -15} | {1, -15} | {2, -6} | {3, -15} | {4, -4} | {5, -3} | {6, -3} | {7, -15} | {8, -15} | {9, -15} | {10, -10}",
								this.FirstName,
								this.LastName,
								this.Id,
								this.Major,
								this.GPA,
								this.CreditsCompleted,
								this.CreditsPerSemester,
								this.BirthDate.ToString("MM/dd/yyyy"),
								this.EnrollmentDate.ToString("MM/dd/yyyy"),
								this.EstimatedGraduationDate.ToString("MM/dd/yyyy"),
								this.Status.ToString());
		}

		public string ToString(int index)
		{
			//                     first  | last    | Id     | Major   | GPA    |  CC    | CPS    | BD     | ED      | GD      | Status 
			return String.Format(index+ "\t{0, -15} | {1, -15} | {2, -6} | {3, -15} | {4, -4} | {5, -3} | {6, -3} | {7, -15} | {8, -15} | {9, -15} | {10, -10}",
								this.FirstName,
								this.LastName,
								this.Id,
								this.Major,
								this.GPA,
								this.CreditsCompleted,
								this.CreditsPerSemester,
								this.BirthDate.ToString("MM/dd/yyyy"),
								this.EnrollmentDate.ToString("MM/dd/yyyy"),
								this.EstimatedGraduationDate.ToString("MM/dd/yyyy"),
								this.Status.ToString());
		}
	}

}