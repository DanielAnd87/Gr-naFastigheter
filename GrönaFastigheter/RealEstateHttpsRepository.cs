
using Blazored.LocalStorage;
using Entities.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GrönaFastigheter
{
    /// <summary>
    /// Class with methods to interact with the api. post gets etc.
    /// </summary>
    public class RealEstateHttpsRepository : IRealEstateHttpsRepository
    {

        private readonly HttpClient http;
        public Dictionary<int, BackgroundData> BackgroundDatas { get; set; } = new Dictionary<int, BackgroundData>();
        public ILocalStorageService LocalStorage { get; }
        public NavigationManager NavManager { get; }
        public EventHandler EventHandler { get; set; }

        public RealEstateHttpsRepository(HttpClient http, ILocalStorageService localStorage, NavigationManager NavManager)
        {
            this.http = http;
            this.NavManager = NavManager;
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
        /// <summary>
        /// Recieves all comments with this username
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Page"></param>
        /// <param name="NumItems"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetCommentsByUser(string Username, int Page = 0, int NumItems = 5)
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
            catch
            {
                NavManager.NavigateTo("./Content/offline");

            }
            return null;
        }
        /// <summary>
        /// All RealEstates with this id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="NumItemsToSkip"></param>
        /// <param name="NumItems"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetCommentsByRealEstateId(int id, int NumItemsToSkip = 2, int NumItems = 5)
        {
            //todo: remove default values when it safe to do so.
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
            catch
            {
                Console.WriteLine("servicec worker error");
            }
            return null;
        }
        /// <summary>
        /// Gets userdata with this username. Recievs different values if logged in (having correct token in default headervalues)
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
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
            catch
            {
                Console.WriteLine("servicec worker error");
            }
            return null;
        }
        /// <summary>
        /// Gets realestates
        /// </summary>
        /// <param name="NumItemsToSkip"></param>
        /// <param name="NumItems"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RealEstate>> GetRealEstates(int NumItemsToSkip = 0, int NumItems = 100)
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
            catch (System.Text.Json.JsonException e)
            {
                Console.WriteLine("Invalid Json");
            }
            catch (Exception e)
            {
                Console.WriteLine("EX E");
                Console.WriteLine(e);
            }
            return null;
        }
        public async Task<RealEstate> GetRealEstateById(int Id)
        {
            try
            {
                string userUrl = $"/api/RealEstates/{Id}/secure";
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
            catch
            {
                Console.WriteLine("servicec worker error");
            }
            return null;
        }
        /// <summary>
        /// Sends newly created RealEstate to backen. Repeats request if no connection
        /// </summary>
        /// <param name="realEstate"></param>
        /// <returns></returns>
        public async Task<RealEstate> PostNewRealEstate(RealEstate realEstate) //Kan behöva optimering men funkar, status code 200
        {

            Console.WriteLine(realEstate.ToString());


            try
            {
                //http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                HttpResponseMessage response = await http.PostAsJsonAsync("api/Realestates", realEstate);

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
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
            catch (JsonException)
            {
                Console.WriteLine("Invalid Json");
            }
            catch
            {
                CancellationToken cancellationToken = new CancellationToken();
                RepeatPOST(
                    () => http.PostAsJsonAsync("api/Realestates", realEstate), // Request that will be sent offline
                    5,                                                         // time intervall between tries
                    realEstate.Address,                                        // Description to show for user
                    cancellationToken);                                        // A way to instantly abort this operation 
            }
            return null;
        }

        /// <summary>
        /// Sends newly created comment to backen. Repeats request if no connection
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task<Comment> PostComment(Comment comment)
        {

            Console.WriteLine(comment.ToString());
            Comment newComment = null;
            try
            {
                HttpResponseMessage response = await http.PostAsJsonAsync("/api/Comments", comment, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    newComment = JsonSerializer.Deserialize<Comment>(responseContent);
                    Console.WriteLine("Hit når vi");
                    return newComment;
                }
                else
                {
                    return new Comment { Content = "Servererror", RealEstateId = -1, UserName = "Error" };
                }
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
            catch (Exception e)
            {
                Console.WriteLine("MESSAGE: \n");
                Console.WriteLine(e.Message);
                CancellationToken cancellationToken = new CancellationToken();
                RepeatPOST(
                    () => http.PostAsJsonAsync("/api/Comments", comment, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }), // Request that will be sent offline
                    5,                                                                                                                      // time intervall between tries
                    comment.Content,                                                                                                        // Description to show for user
                    cancellationToken);                                                                                                     // A way to instantly abort this operation 


            }
            return newComment;
        }
        /// <summary>
        /// Posts a rating to a user through the api. Repeats request if no connection.
        /// </summary>
        /// <param name="rating">the rating value</param>
        /// <param name="userId">userId</param>
        /// <returns>true if successfull, false if not.</returns>
        public async Task<bool> PostRating(int rating, int userId)
        {
            var requestBody = new { UserId = userId.ToString(), Value = rating };
            try
            {
                //HttpResponseMessage result = await http.PostAsJsonAsync("/api/Users/Rate", requestBody);
                HttpResponseMessage result = await http.PutAsJsonAsync("/api/Users/Rate", requestBody);
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
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
            // todo: Find the exception used for no connection.
            catch (TaskCanceledException e)
            {
                Console.WriteLine(e.Message);
                CancellationToken cancellationToken = new CancellationToken();
                RepeatPOST(
                    () => http.PostAsJsonAsync("/api/Users/Rate", requestBody), // Request that will be sent offline
                    5,                                                          // time intervall between tries
                    "Rating of: " + rating,                                     // Description to show for user
                    cancellationToken);                                         // A way to instantly abort this operation 


            }
            return false;
        }

        /// <summary>
        /// Runs asyncronous until request is completed or canceled using token.
        /// </summary>
        /// <param name="action">A func delegate that returns a HttpResponseMessage Task</HttpResponseMessage></param>
        /// <param name="seconds">Interval between connection tries</param>
        /// <param name="description">A descritption for user to check progress</param>
        /// <param name="token">A token to instantly end operation</param>
        private void RepeatPOST(Func<Task<HttpResponseMessage>> action, int seconds, string description, CancellationToken token)
        {
            if (action == null)
            {
                return;
            }

            Task.Run(async () =>
            {
                int id = -1;
                foreach (int key in BackgroundDatas.Keys)
                {
                    if (key > id)
                    {
                        id = key;
                    }
                }
                id++;
                int length = description.Length >= 20 ? 20 : description.Length;
                BackgroundDatas.Add(id, new BackgroundData(description.Substring(0, length), true));

                EventHandler.Invoke(this, new EventArgs());
                while (BackgroundDatas[id].IsRunning && !token.IsCancellationRequested)
                {
                    try
                    {
                        HttpResponseMessage response = await action();
                        string responseContent = await response.Content.ReadAsStringAsync();
                        BackgroundDatas[id].IsRunning = !response.IsSuccessStatusCode;
                        Console.WriteLine(response.StatusCode);
                        BackgroundDatas.Remove(id);
                        EventHandler.Invoke(this, new EventArgs());
                        break;
                    }
                    catch
                    {
                        await Task.Delay(TimeSpan.FromSeconds(seconds), token);
                    }
                }
            }, token);
        }
    }
}
