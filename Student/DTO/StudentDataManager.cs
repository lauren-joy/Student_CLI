using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLib.DTO
{
	public class StudentDataManager
	{
		private List<string> m_FragmentList = new List<string>
		{
		"ar", "er", "en", "in", "car", "bar", "bin", "ard", "art", "and", "walt",
		"nor", "off", "min", "max", "tin", "rand", "son", "fer", "sir", "ver", "len",
		"por", "pho", "ind", "ger", "que", "sam", "ler", "tor", "vic", "der", "ere", "ete",
		"ee", "tan", "pat", "ret", "ran", "oon", "ye", "far", "all", "ash", "ish", "it",
		"ber", "oct", "sim", "ear", "per", "dam", "bre", "anna", "on", "pat", "bal",
		"ahl", "iam", "it", "mar", "li", "gab", "th", "ly", "ere", "an", "ke", "bra", "ny",
		"art"
		};

		private HashSet<string> studentNames = new();

		private static List<string> m_MajorsList = new List<string>
		{
		"Business", "CompSci", "Japanese", "Analytics", "Math", "Engineering", "History"
		};
		//lineSplit = Goose roger black white yellow Yam
		// Goose 
		// roger black white yellow Yam
		//lineSplit.Split(' ', 2)

		private Dictionary<int, Student> m_MasterStudentList = new();

		public Dictionary<int, Student> Open(int numberOfStudents, int seed)
		{
			m_MasterStudentList = GetStudentList(numberOfStudents, seed);
			return m_MasterStudentList;
		}
		private Dictionary<int, Student> GetStudentList(int numberOfStudents, int seed)
		{
			Random randomObj = new Random(seed);
			Dictionary<int, Student> students = new();
			for (int i = 0; i < numberOfStudents; i++)

			{
				Student student = new();
				student.FirstName = GetRandomName(randomObj);
				student.LastName = GetRandomName(randomObj);
				while (studentNames.Contains(student.FirstName + " " + student.LastName))
				{
					student.FirstName = GetRandomName(randomObj);
					student.LastName = GetRandomName(randomObj);
				}
				studentNames.Add(student.FirstName + " " + student.LastName);
				student.GPA = GetRandomGPA(randomObj);
				student.Major = GetRandomMajor(randomObj);
				student.BirthDate = GetRandomBirthDate(randomObj);
				student.CreditsCompleted = GetRandomCredits(randomObj);
				student.CreditsPerSemester = GetRandomCreditsPerSemester(randomObj);
				student.EnrollmentDate = GetRandomEnrollmentDate(randomObj, student);
				student.Status = GetGraduationStatus(student);
				student.EstimatedGraduationDate = GetRandomGraduationDate(randomObj, student);
				student.Id = GetRandomId(randomObj);
				while (students.ContainsKey(student.Id))
				{
					student.Id = GetRandomId(randomObj);
				}
				students.Add(student.Id, student);
			}
			return students;
		}

		public Dictionary<int, Student> AddStudentList(int seed, string filePath)
		{
			Random randomObj = new Random(seed);
			Dictionary<int, Student> students = new();
			CSVImporter studentCsv = new CSVImporter();
			List<CsvLine> studentCsvList = studentCsv.Import(filePath);

			foreach (CsvLine csv in studentCsvList)

			{
				Student student = new();
				student.FirstName = csv.FirstName;
				student.LastName = csv.LastName;
				studentNames.Add(student.FirstName + " " + student.LastName);
				student.GPA = GetRandomGPA(randomObj);
				student.Major = GetRandomMajor(randomObj);
				student.BirthDate = csv.BirthDate;
				student.CreditsCompleted = GetRandomCredits(randomObj);
				student.CreditsPerSemester = GetRandomCreditsPerSemester(randomObj);
				student.EnrollmentDate = GetRandomEnrollmentDate(randomObj, student);
				student.Status = GetGraduationStatus(student);
				student.EstimatedGraduationDate = GetRandomGraduationDate(randomObj, student);
				student.Id = GetRandomId(randomObj);
				while (students.ContainsKey(student.Id))
				{
					student.Id = GetRandomId(randomObj);
				}
				students.Add(student.Id, student);
			}
			return students;
		}

		public string ExportStudentList(Dictionary<int, Student> students)
		{
			CSVImporter csvImport = new CSVImporter();
			return csvImport.Export(students);
		}

		private DateTime GetRandomEnrollmentDate(Random random, Student student)
		{
			int creditsPerSemester = student.CreditsPerSemester;
			int studentCredits = student.CreditsCompleted + creditsPerSemester;
			int completedSemesters = (int)(studentCredits / creditsPerSemester);
			DateTime enrollmentDate = DateTime.Now;

			for (int i = 0; i < completedSemesters; i++)
			{
				enrollmentDate = enrollmentDate.AddMonths(-6);
			}
			return enrollmentDate;
		}

		private GraduationStatus GetGraduationStatus(Student student)
		{
			if (student.CreditsCompleted >= 120)
			{
				return GraduationStatus.Graduated;
			}
			else if (student.CreditsPerSemester < 6)
			{
				return GraduationStatus.Delayed;
			}
			else
			{
				return GraduationStatus.InProgess;
			}
		}

		private DateTime GetRandomGraduationDate(Random random, Student student)
		{
			int studentAccumulatedCredits = student.CreditsPerSemester;
			int creditsToBeCompleted = 128 - student.CreditsCompleted;
			double semestersRemaining = creditsToBeCompleted / studentAccumulatedCredits;
			int semesterIndex = (int)semestersRemaining;
			DateTime graduationDate = DateTime.Now;

			for (int i = 0; i < semesterIndex; i++)
			{
				graduationDate = graduationDate.AddMonths(6);
			}
			return graduationDate;
		}

		private int WeightedSample(Random random, List<int> sampleFromList, List<double> probs)
		{
			double p = random.NextDouble();
			double cumulativeProbability = 0.0;
			int index = 0;
			while (p > cumulativeProbability + probs[index])
			{
				cumulativeProbability += probs[index];
				index++;
			}
			return sampleFromList[index];
		}

		private int GetRandomId(Random random)
		{
			return random.Next(100000, 1000000);
		}

		private string GetRandomName(Random random)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 1; i <= random.Next(2, 6); ++i)
			{
				sb.Append(m_FragmentList[random.Next(m_FragmentList.Count)]);
			}
			string name = sb.ToString();
			return name.Substring(0, 1).ToUpper() + name.Substring(1);
		}

		private DateTime GetRandomBirthDate(Random random)
		{
			DateTime minDate = Convert.ToDateTime("1990/01/01");
			int dateRange = (DateTime.Today.AddDays(-5840) - minDate).Days;
			return minDate.AddDays(random.Next(dateRange));
		}

		private double GetRandomGPA(Random random)
		{
			/*
			The beta distribution is a probability distribution that samples numbers between 0 and 1
			The beta distribution has two parameters alpha and beta
			The mean is equal alpha / (alpha + beta)
			The To find the mean of the beta distribution of 2.55 / 4.0, we fix the alpha to 2
			and solve 2.55 / 4.0 = 2.0/ (2.0 + beta) for beta.
			Multiplying the beta distribution by 4 scales the beta distribution to sample numbers between 0 and 4,
			With a mean of 2.55
			*/
			double gpa = Beta.Sample(random, 2.0, 1.1372549019607845);
			gpa = Math.Round(gpa, 2);
			gpa = 4.0 * gpa;
			return gpa;
		}

		private int GetRandomCreditForClass(Random random)
		{
			List<int> creditList = new List<int> { 1, 2, 3, 4 };
			List<double> probsList = new List<double> { 0.01, 0.04, 0.9, 0.05 };
			return WeightedSample(random, creditList, probsList);
		}

		private int GetRandomCreditsPerSemester(Random random)
		{
			int numOfClassesTaken = random.Next(3, 6);
			int totalCredits = 0;
			for (int i = 0; i < numOfClassesTaken; i++)
			{
				totalCredits += GetRandomCreditForClass(random);
			}
			if (totalCredits > 15)
			{
				totalCredits = 15;
			}
			return totalCredits;
		}
		private int GetRandomCredits(Random random)
		{
			int classesTaken = random.Next(1, 45);
			int totalCredits = 0;
			for (int i = 0; i < classesTaken; ++i)
				totalCredits += GetRandomCreditForClass(random);
			if (totalCredits > 128)
			{
				totalCredits = 128;
			}
			return totalCredits;
		}

		private string GetRandomMajor(Random random)
		{
			return m_MajorsList[random.Next(m_MajorsList.Count)];
		}
	}
}