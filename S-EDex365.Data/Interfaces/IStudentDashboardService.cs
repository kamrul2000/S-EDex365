using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Interfaces
{
    public interface IStudentDashboardService
    {
        Task<string?> GetAllTotalProblemAsync(Guid userId);
        Task<string?> GetAllSolutionAsync(Guid userId);
        Task<string?> GetAllPendingProblemAsync(string userId);
    }
}
