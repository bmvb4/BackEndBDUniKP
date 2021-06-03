using BackEndBDAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackEndBDAPP.Utils
{
    public class AutoDeletePosts
    {
        static HttpClient _client = new HttpClient();

 
        public static async void delete() {
            try
            {
                var responseTaskPost = await _client.DeleteAsync("https://localhost:5001/autoDelete/post");
                var responseTaskCode = await _client.DeleteAsync("https://localhost:5001/autoDelete/emailCode");
            }
            catch (Exception)
            {

            }
            
        }
    }
}
