using Entities.Models;
using Microsoft.AspNetCore.Http;
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
        /// <summary>
        /// Fixa skicka med bearer token och null returns.
        /// </summary>
        /// <param name="newRealEstate"></param>
        /// <returns></returns>
        Task<RealEstate> PostNewRealEstate(RealEstate newRealEstate);

        /// <summary>
        /// fixa null returns vid felaktigt statuscode. isttälet notfound. Skicka också med bearer token.
        /// </summary>vet inte
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Comment> PostComment(Comment comment);
        void PostRating(int rating, int userId);
        void TestRepo();

    }
}