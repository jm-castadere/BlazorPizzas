using BlazorPizzas.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorPizzas.Services
{
    /// <summary>
    /// API Http 
    /// </summary>
    public class HttpPizzaManager : IPizzaManager
    {
        private readonly HttpClient _client;

        public HttpPizzaManager(HttpClient client)
        {
            _client = client;
        }


        /// <summary>
        /// Update pizza
        /// </summary>
        /// <param name="valPizza">value pizza to update or add</param>
        /// <returns></returns>
        public async Task<bool> AddOrUpdate(Pizza valPizza)
        {
            //call API  [Route("api/[controller]")]
            var response = await _client.PostAsJsonAsync("api/pizzas", valPizza);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Get all Pizza
        /// </summary>
        /// <returns>pizza data öist</returns>
        public Task<IEnumerable<Pizza>> GetPizzas()
        {
            return _client.GetFromJsonAsync<IEnumerable<Pizza>>("api/pizzas");
        }
    }
}
