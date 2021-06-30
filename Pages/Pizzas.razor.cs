using BlazorPizzas.Models;
using BlazorPizzas.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPizzas.Pages
{
    public class PizzasBase : ComponentBase
    {
        [Inject] private IPizzaManager PizzaManager { get; set; }

        protected bool isAdmin;
        protected List<Pizza> Basket;
        protected List<Pizza> Pizzas;
        protected Pizza EditingPizza;

        protected override async Task OnInitializedAsync()
        {
            Basket = new List<Pizza>();
            Pizzas = (await PizzaManager.GetPizzas()).ToList();
            EditingPizza = null;
            isAdmin = false;
        }

        protected string Ingredients
        {
            get => EditingPizza != null ? string.Join(", ", EditingPizza.Ingredients) : null;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    EditingPizza.Ingredients = value.Split(',').Select(v => v.Trim()).ToArray();
                }
                else
                {
                    EditingPizza.Ingredients = null;
                }
            }
        }

        public void AddToBasket(Pizza pizza) => Basket.Add(pizza);
        public void RemoveFromBasket(Pizza pizza) => Basket.Remove(pizza);

        public void EditPizza(Pizza p)
        {
            EditingPizza = new Pizza
            {
                Id = p.Id,
                Name = p.Name,
                ImageName = p.ImageName,
                Ingredients = p.Ingredients,
                Price = p.Price
            };
        }

        public async Task Close()
        {
            await PizzaManager.AddOrUpdate(EditingPizza);
            var pizza = Pizzas.Find(p => p.Id == EditingPizza.Id);
            pizza.Price = EditingPizza.Price;
            pizza.Name = EditingPizza.Name;
            pizza.Ingredients = EditingPizza.Ingredients;
            EditingPizza = null;
        }
    }
}
