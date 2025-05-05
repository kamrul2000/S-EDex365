using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Interfaces
{
    public interface IBanglaClassService
    {
        Task<List<BanglaClass>> GetAllBanglaClassAsync();
        Task<bool> InsertBanglaClassAsync(BanglaClass banglaClass);
        Task<BanglaClass> GetBanglaClassByIdAsync(Guid classId);
        Task<bool> UpdateBanglaClassAsync(BanglaClass banglaClass);
        Task<bool> DeleteBanglaClassAsync(Guid classId);
    }
}
