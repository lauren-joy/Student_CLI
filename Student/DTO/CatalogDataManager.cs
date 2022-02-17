using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLib.DTO
{
    public class CatalogDataManager
    {
        private static List<string> m_MajorsList = new List<string>
        {
        "Business", "CompSci", "Japanese", "Analytics", "Math", "Engineering", "History"
        };

        private List<CatalogEntry> CatalogEntries = new List<CatalogEntry>();
        public List<CatalogEntry> openCatalog(string major)
        {
            CatalogEntries = CreateCatalog(major);
            return CatalogEntries;
        }

        public List<CatalogEntry> CreateCatalog(string major)
        {
            List<CatalogEntry> catalogEntries = new List<CatalogEntry>();
            Random r = new Random();
                for (int j = 0; j < 33; j++)
                {
                    CatalogEntry entry = new CatalogEntry();
                    entry.Major = major;
                    entry.ClassNumber = r.Next(100,599);
                    entry.Credits = r.Next(3, 5);
                    while(catalogEntries.Select(c => c.ClassNumber).ToList().Contains(entry.ClassNumber))
                    {
                        entry.ClassNumber = r.Next(100, 599);
                    }
                    catalogEntries.Add(entry);
                }
            catalogEntries.Sort();
            return catalogEntries;
        }
       
    }
}
