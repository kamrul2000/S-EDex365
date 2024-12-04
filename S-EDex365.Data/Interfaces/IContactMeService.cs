using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Interfaces
{
    public interface IContactMeService
    {
        Task<bool> InsertContactMeAsync(ContactMe contactMe);
        Task<List<ContactMe>> GetAllContactMeAsync();
    }
}
