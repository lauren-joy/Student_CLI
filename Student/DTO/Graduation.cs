//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using StudentLib.DTO;

//namespace StudentLib.DTO
//{
//    public enum GraduationStatus
//    {
//        InProgess,
//        Delayed,
//        Graduated,
//        DroppedOut,
//        NotEnrolled
//    }
//    public class Graduation 
//    {
//        private DateTime m_EstimatedGraduationDate;
//        private int m_CreditsCompleted;
//        private int m_CreditsPerSemester;
//        private GraduationStatus m_Status;
//        public event PropertyChangedEventHandler PropertyChanged;

//        public DateTime EstimatedGraduationDate
//        {
//            get
//            {
//                return m_EstimatedGraduationDate;
//            }
//            set
//            {
//                if (value != m_EstimatedGraduationDate)
//                {
//                    m_EstimatedGraduationDate = value;
//                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EstimatedGraduationDate)));
//                }
//            }
//        }

//        public int CreditsCompleted
//        {
//            get
//            {
//                return m_CreditsCompleted;
//            }
//            set
//            {
//                if (value != m_CreditsCompleted)
//                {
//                    m_CreditsCompleted = value;
//                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreditsCompleted)));
//                }
//            }
//        }

//        public int CreditsPerSemester
//        {
//            get
//            {
//                return m_CreditsPerSemester;
//            }
//            set
//            {
//                if (value != m_CreditsPerSemester)
//                {
//                    m_CreditsPerSemester = value;
//                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreditsPerSemester)));
//                }
//            }
//        }

//        public GraduationStatus Status
//        {
//            get
//            {
//                return m_Status;
//            }
//            set
//            {
//                if (value != m_Status)
//                {
//                    m_Status = value;
//                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GraduationStatus)));
//                }
//            }
//        }

//        public bool CanGraduate()
//        {
//            if(m_Status == GraduationStatus.InProgess && CreditsCompleted >= 120)
//            {
//                return true;
//            }
//            return false;
//        }

//        public DateTime EstGradDate()
//        {
//            DateTime potentialGraduationDate = DateTime.Now;
//            int CreditsPerMonth = CreditsPerSemester / 4;
//            int requiredMonths = (120 - CreditsCompleted) / CreditsPerMonth;
//            return potentialGraduationDate.AddMonths(requiredMonths); 
//        }  
//    } 
//}




