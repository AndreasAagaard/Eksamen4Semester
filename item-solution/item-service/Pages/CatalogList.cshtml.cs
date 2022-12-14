using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using item_service.Models;

namespace item_service.Pages.CatalogList
{
    public class CatalogListModel : PageModel
    {
         public List<ProductItemDTO>? Products {get; set;} 

        public async void OnGetAsync() 
        { 
            using HttpClient client = new() 
            { 
                BaseAddress = new Uri("http://catalog:80/")
            }; 
    
            // Get the user information. 
            Products = await client.GetFromJsonAsync<List<ProductItemDTO>>("catalog/getproduct"); 
        } 
    }
}
