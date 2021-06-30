using BlazorPizzas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPizzas.Services
{
    public interface IPizzaManager
    {
        Task<IEnumerable<Pizza>> GetPizzas();
        Task<bool> AddOrUpdate(Pizza p);
    }
}
