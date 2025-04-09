using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Interfaces
{
    public interface IPostTypeDetailsService
    {
        Task<List<PostType>> GetAllPostTypeAsync();
        Task<List<PostTypeDetails>> GetAllPostTypeDetailsAsync();
        Task<bool> InsertPostTypeDetailsAsync(PostTypeDetails postTypeDetails);
        Task<PostTypeDetails> GetPostTypeDetailsByIdAsync(Guid postTypeDetailsId);
        Task<bool> UpdatePostTypeDetailsAsync(PostTypeDetails postTypeDetails);
        Task<bool> DeletePostTypeDetailsAsync(Guid postTypeDetailsId);
    }
}
