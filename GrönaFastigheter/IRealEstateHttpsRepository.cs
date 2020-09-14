using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrönaFastigheter
{
    public interface IRealEstateHttpsRepository
    {
        Task<IEnumerable<Comment>> GetCommentsById(string id, int Page = 2, int NumItems = 5);
        Task<IEnumerable<Comment>> GetCommentsByRealEstateId(string Username, int Page = 2, int NumItems = 5);
        Task<IEnumerable<Comment>> GetCommentsByUser(string Username, int Page = 2, int NumItems = 5);
        Task<RealEstate> GetRealEstateById(string Id);
        Task<IEnumerable<RealEstate>> GetRealEstates(int Page = 2, int NumItems = 5);
        Task<User> GetUserByUserName(string Username);
        void TestRepo();

    }
}