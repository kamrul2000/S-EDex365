using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Interfaces
{
    public interface IEnglishClassService
    {
        Task<List<EnglishClass>> GetAllEnglishClassAsync();
        Task<bool> InsertEnglishClassAsync(EnglishClass englishClass);
        Task<EnglishClass> GetEnglishClassByIdAsync(Guid classId);
        Task<bool> UpdateEnglishClassAsync(EnglishClass englishClass);
        Task<bool> DeleteEnglishClassAsync(Guid classId);
    }
}
