using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using auction_service.Models;

namespace auction_service.Pages.CatalogList
{
    public class CatalogListModel : PageModel
    {
        public List<AuctionItemDTO>? Auctions {get; set;}
        
        public async Task OnGet() 
        { 
            using HttpClient client = new() 
            { 
                BaseAddress = new Uri("http://localhost:80/") 
            }; 
    
            // Get the user information. 
            Auctions = await client.GetFromJsonAsync<List<AuctionItemDTO>>("Auction/GetAuction"); 
        }
    }
}
