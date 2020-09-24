
using Blazored.LocalStorage;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrönaFastigheter
{
    public class RealEstateHttpsRepository : IRealEstateHttpsRepository
    {

        private readonly HttpClient http;

        public ILocalStorageService LocalStorage { get; }

        public RealEstateHttpsRepository(HttpClient http, ILocalStorageService localStorage)
        {
            this.http = http;
            LocalStorage = localStorage;
        }

        public async void TestRepo()
        {
            // RealEstate realEstate = await GetRealEstateById(1); // FUNKAR
            // IEnumerable<RealEstate> realEstates = await GetRealEstates(); // FUNKAR
            // User user = await GetUserByUserName("USERNAME"); // FUNKAR
            // IEnumerable<Comment> commentByUser = await GetCommentsByUser("USERNAME"); // FUNKAR
            // IEnumerable<Comment> comentsById = await GetCommentsByRealEstateId(1); // FUNKAR ej


            // RealEstate estate = await PostNewRealEstate(realEstate);
            string stop = "stop";


        }

        public async Task<IEnumerable<Comment>> GetCommentsByUser(string Username, int Page = 2, int NumItems = 5)
        {
            if (Username == null)
            {
                Username = "USERNAME";
            }
            IEnumerable<Comment> comments;
            try
            {
                string userUrl = $"/api/Comments/ByUser/{Username}?skip={Page}&take={NumItems}";
                comments = await http.GetFromJsonAsync<IEnumerable<Comment>>(userUrl);
                Console.WriteLine(comments);
                return comments;

            }
            catch (HttpRequestException)
            {
                Console.WriteLine("An error Occured");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Content type is not supported");
            }
            catch (System.Text.Json.JsonException message)
            {
                Console.WriteLine(message);
            }
            return null;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByRealEstateId(int id, int NumItemsToSkip = 2, int NumItems = 5)
        {
            IEnumerable<Comment> comment;
            try
            {
                string userUrl = $"/api/Comments/{id}?skip={NumItemsToSkip}&take={NumItems}";
                comment = await http.GetFromJsonAsync<IEnumerable<Comment>>(userUrl);
                return comment;

            }
            catch (HttpRequestException)
            {
                Console.WriteLine("An error Occured");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Content type is not supported");
            }
            catch (System.Text.Json.JsonException ex)
            {
                Console.WriteLine("Invalid json from /api/comments/id" + ex);
            }
            return null;
        }

        public async Task<User> GetUserByUserName(string Username)
        {
            if (Username == null)
            {
                Username = "USERNAME";
            }
            try
            {
                string userUrl = $"/api/Users/{Username}";
                User task = await http.GetFromJsonAsync<User>(userUrl);
                return task;

            }
            catch (HttpRequestException)
            {
                Console.WriteLine("An error Occured");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Content type is not supported");
            }
            catch (System.Text.Json.JsonException)
            {
                Console.WriteLine("Invalid Json");
            }
            return null;
        }

        public async Task<IEnumerable<RealEstate>> GetRealEstates(int NumItemsToSkip = 2, int NumItems = 5)
        {
            try
            {
                string userUrl = $"/api/RealEstates?skip={NumItemsToSkip}&take={NumItems}";
                return await http.GetFromJsonAsync<IEnumerable<RealEstate>>(userUrl);
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("An error Occured");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Content type is not supported");
            }
            catch (System.Text.Json.JsonException)
            {
                Console.WriteLine("Invalid Json");
            }
            return null;
        }
        public async Task<RealEstate> GetRealEstateById(int Id)
        {
            try
            {
                string userUrl = $"/api/RealEstates/{Id}";
                return await http.GetFromJsonAsync<RealEstate>(userUrl);

            }
            catch (HttpRequestException)
            {
                Console.WriteLine("An error Occured");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Content type is not supported");
            }
            catch (System.Text.Json.JsonException)
            {
                Console.WriteLine("Invalid Json");
            }
            return null;
        }

        public async Task<RealEstate> PostNewRealEstate(RealEstate realEstate) //Kan behöva optimering men funkar, status code 200
        {
            try
            {
                //http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                HttpResponseMessage response = await http.PostAsJsonAsync("api/Realestates", realEstate);

                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    RealEstate newEstate = JsonSerializer.Deserialize<RealEstate>(responseContent);
                    return newEstate;
                }
                return null;
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("An error Occured");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Content type is not supported");
            }
            catch (System.Text.Json.JsonException)
            {
                Console.WriteLine("Invalid Json");
            }
            return null;
        }
        public async Task<bool> PostComment(Comment comment) //MAN MÅSTE HA MED SIG BEARER TOKEN FÖR ATT FÅ LÄGGA TILL COMMENT. FIXA
        {
            try
            {
                HttpResponseMessage response = await http.PostAsJsonAsync("api/Comments", comment);

                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Comment newComment = JsonSerializer.Deserialize<Comment>(responseContent);
                    return true;
                }
                return false;
            }
             catch (HttpRequestException)
            {
                Console.WriteLine("An error Occured");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Content type is not supported");
            }
            catch (System.Text.Json.JsonException)
            {
                Console.WriteLine("Invalid Json");
            }
            return false;
        }
        public void PostRating(int rating, int userId) //NYI
        {
            rating += 1;
            var result = http.PostAsJsonAsync("/api/Users/Rate", rating);
            
        }



    }
}
