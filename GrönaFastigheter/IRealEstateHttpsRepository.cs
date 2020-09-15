using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrönaFastigheter
{
    public interface IRealEstateHttpsRepository
    {
        Task<IEnumerable<Comment>> GetCommentsByRealEstateId(int id, int Page = 2, int NumItems = 5);
        Task<IEnumerable<Comment>> GetCommentsByUser(string Username, int Page = 2, int NumItems = 5);
        Task<RealEstate> GetRealEstateById(int Id);
        Task<IEnumerable<RealEstate>> GetRealEstates(int Page = 2, int NumItems = 5);
        Task<User> GetUserByUserName(string Username);
        void TestRepo();

    }
}