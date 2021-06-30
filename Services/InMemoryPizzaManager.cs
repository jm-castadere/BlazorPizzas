using BlazorPizzas.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPizzas.Services
{
    public class InMemoryPizzaManager : IPizzaManager
    {
        private ConcurrentBag<Pizza> pizzas;

        public InMemoryPizzaManager() => pizzas = new ConcurrentBag<Pizza>
            {
                new Pizza{ Id =1, Name ="Bacon", Price = 12, Ingredients = new[] { "bacon", "mozzarella", "champignon", "emmental" }, ImageName = "bacon.jpg"  },
                new Pizza{ Id =2, Name ="4 fromages", Price= 11, Ingredients = new[] { "cantal", "mozzarella", "fromage de chèvre", "gruyère" }, ImageName = "cheese.jpg"  },
                new Pizza{ Id =3, Name ="Margherita", Price = 10, Ingredients = new[] { "sauce tomate", "mozzarella", "basilic" }, ImageName = "margherita.jpg"  },
                new Pizza{ Id =4, Name ="Mexicaine", Price=12, Ingredients = new[] { "boeuf", "mozzarella", "maïs", "tomates", "oignon", "coriandre" }, ImageName = "meaty.jpg"  },
                new Pizza{ Id =5, Name ="Reine", Price=11, Ingredients = new[] { "jambon", "champignons", "mozzarella" }, ImageName = "mushroom.jpg"  },
                new Pizza{ Id =6, Name ="Pepperoni", Price=11, Ingredients = new[] { "mozzarella", "pepperoni", "tomates" }, ImageName = "pepperoni.jpg"  },
                new Pizza{ Id =7, Name ="Végétarienne",Price = 10, Ingredients = new[] { "champignons", "roquette", "artichauts", "aubergine" }, ImageName = "veggie.jpg"  }
            };

        public Task<bool> AddOrUpdate(Pizza p)
        {
            var pizza = pizzas.FirstOrDefault(piz => piz.Id == p.Id);
            if (pizza != null)
            {
                pizzas = new ConcurrentBag<Pizza>(pizzas.Where(piz => piz.Id != p.Id));
            }
            pizzas.Add(p);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Pizza>> GetPizzas() => Task.FromResult(pizzas.AsEnumerable());
    }
}
