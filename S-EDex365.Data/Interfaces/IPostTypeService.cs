using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Interfaces
{
    public interface IPostTypeService
    {
        Task<List<PostType>> GetAllPostTypeAsync();
        Task<bool> InsertPostTypeAsync(PostType postType);
        Task<PostType> GetPostTypeByIdAsync(Guid postTypeId);
        Task<bool> UpdatePostTypeAsync(PostType postType);
        Task<bool> DeletePostTypeAsync(Guid postTypeId);
    }
}
