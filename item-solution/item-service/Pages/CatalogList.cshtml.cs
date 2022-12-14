using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using item_service.Models;
using System.Net.Http;

namespace MyApp.Namespace
{
    public class CatalogListModel : PageModel
    {
        private readonly IHttpClientFactory? _clientFactory = null;

        public CatalogListModel(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public List<ProductItemDTO?> Products { get; set; }

        public async Task OnGetAsync() 
        { 
            Console.WriteLine("Calling 'OnGet'");
            using HttpClient client = _clientFactory?.CreateClient("gateway"); 
            try
            { 
                Products = await client.GetFromJsonAsync<List<ProductItemDTO>>("catalog/getproduct"); 
                // Products = new List<ProductItemDTO?>(){
                //     new ProductItemDTO(),
                //     new ProductItemDTO()
                // };
                Console.WriteLine("Success 'OnGet'");
                Console.WriteLine(Products[0].ProductId);
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Fail 'OnGet'");

            }
            // Get the user information. 
        } 
    }
}
