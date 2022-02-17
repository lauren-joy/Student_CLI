using StudentLib.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLib
{
    public interface ICommandProcessor
    {
        string CommandVerb { get; }
        string HelpText { get; }
        CommandProcessResult Execute(CommandParseResult parseResult,ref List<CatalogEntry> entries, ref Dictionary<int, Student> filteredStudents, ref Student currentStudent);
    }
}