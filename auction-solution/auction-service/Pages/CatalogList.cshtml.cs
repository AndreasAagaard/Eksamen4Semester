using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using auction_service.Models;

namespace auction_service.Pages.CatalogList
{
    public class CatalogListModel : PageModel
    {
        public result _result {get; set;}
        public List<AuctionItemDTO>? Auctions {get; set;}
        
        public async void OnGet() 
        { 
            using HttpClient client = new() 
            { 
                BaseAddress = new Uri("http://localhost:80/") 
            }; 
    
            // Get the user information. 
            Auctions = client.GetFromJsonAsync<List<AuctionItemDTO>>("Auction/GetAuction").Result; 
        }
    }
}
