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
                var responseTaskPost = await _client.DeleteAsync("http://localhost:5200/api/autoDelete/post");
                var responseTaskCode = await _client.DeleteAsync("http://localhost:5200/api/autoDelete/emailCode");
            }
            catch (Exception ex)
            {

            }
            
        }
    }
}
