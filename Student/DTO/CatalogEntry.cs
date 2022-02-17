using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLib.DTO
{
	public class CatalogEntry : Base, IEquatable<CatalogEntry>, IComparable<CatalogEntry>
	{
		private string m_Major;
		private int m_ClassNumber;
		private int m_Credits;
		public string Major
		{
			get
			{
				return m_Major;
			}
			set
			{
				m_Major = value;
				NotifyPropertyChanged("Major");
			}
		}
		public int ClassNumber
		{
			get
			{
				return m_ClassNumber;
			}
			set
			{
				m_ClassNumber = value;
				NotifyPropertyChanged("ClassNumber");
			}
		}
		public int Credits
		{
			get
			{
				return m_Credits;
			}
			set
			{
				m_Credits = value;
				NotifyPropertyChanged("Credits");
			}
		}
		public string ClassName
		{
			get
			{
				return m_Major.Substring(0, 3) + " " + m_ClassNumber.ToString();
			}
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			CatalogEntry objAsEntry = obj as CatalogEntry;
			if (objAsEntry == null) return false;
			else return Equals(objAsEntry);
		}

		public int CompareTo(CatalogEntry other)
        {
			if (other == null)
				return 1;

			else
				return this.ClassNumber.CompareTo(other.ClassNumber);
		}

		public bool Equals(CatalogEntry other)
        {
			if (other == null) return false;
			return (this.ClassNumber.Equals(other.ClassNumber));
		}

        public override string ToString()
		{
			//                     first  | last    | Id     | Major   | GPA    |  CC    | CPS    | BD     | ED      | GD      | Status 
			return String.Format("\t{0, -15} | {1, -15} | {2, -6} | {3, -15}",
								this.Major,
								this.ClassNumber,
								this.Credits,
								this.ClassName);
		}
	}
}
