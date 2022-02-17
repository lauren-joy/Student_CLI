using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLib.DTO
{
    public class CSVImporter
    {

    

        public List<CsvLine>Import(string FileUpload)
        {

          List<CsvLine> lines = new List<CsvLine>();
            using (var reader = new StreamReader(FileUpload))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")))
            {
                lines = csv.GetRecords<CsvLine>().ToList();
            }
            return lines;
        

        }

        public string Export(Dictionary<int, Student> students)
        {
            DataTable table = new DataTable();
            table.Columns.Add("firstName", typeof(string));
            table.Columns.Add("lastName", typeof(string));
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("major", typeof(string));
            table.Columns.Add("gpa", typeof(double));
            table.Columns.Add("cc", typeof(int));
            table.Columns.Add("cpa", typeof(int));
            table.Columns.Add("birthDate", typeof(DateTime));
            table.Columns.Add("dateEnrolled", typeof(DateTime));
            table.Columns.Add("graduationDate", typeof(DateTime));
            table.Columns.Add("status", typeof(string));

            foreach(KeyValuePair<int, Student> s in students)
            {
                table.Rows.Add
                    (
                        s.Value.FirstName,
                        s.Value.LastName,  
                        s.Value.Id,
                        s.Value.Major,
                        s.Value.GPA,
                        s.Value.CreditsCompleted,
                        s.Value.CreditsPerSemester,
                        s.Value.BirthDate,
                        s.Value.EnrollmentDate,
                        s.Value.EstimatedGraduationDate,
                        s.Value.Status
                    );
            }
            string fileName = "studentExport_" + DateTime.Now.ToString("MM-dd-yyyy")+"T"+DateTime.Now.ToString("HH.mm")+".csv";
            string filePath = Path.Combine("csv_files", fileName);
            StreamWriter sw = new StreamWriter(filePath, false);
            for(int i = 0; i < table.Columns.Count; i++)
            {
                sw.Write(table.Columns[i]);
                if (i < table.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
           foreach(DataRow dr in table.Rows)
            {

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < table.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();



            return "Exported students to file location: "+Environment.CurrentDirectory+filePath;


        }
    }
}
