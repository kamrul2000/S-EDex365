using Microsoft.Extensions.Configuration;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Services
{
    public class EnglishClassService : IEnglishClassService
    {
        private readonly string _connectionString;
        public EnglishClassService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }
        public Task<bool> DeleteEnglishClassAsync(Guid classId)
        {
            throw new NotImplementedException();
        }

        public Task<List<EnglishClass>> GetAllEnglishClassAsync()
        {
            throw new NotImplementedException();
        }

        public Task<EnglishClass> GetEnglishClassByIdAsync(Guid classId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertEnglishClassAsync(EnglishClass englishClass)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEnglishClassAsync(EnglishClass englishClass)
        {
            throw new NotImplementedException();
        }
    }
}
