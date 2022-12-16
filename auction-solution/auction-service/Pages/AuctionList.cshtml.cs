using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Net.Http;
using auction_service.Models;

namespace MyApp.Namespace
{
    public class AuctionListModel : PageModel
    {
        private readonly IHttpClientFactory? _clientFactory = null;

        public AuctionListModel(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        public List<AuctionItemDTO>? Auctions {get; set;}
        
        public async Task OnGetAsync() 
        { 
            Console.WriteLine("Calling 'OnGet'");
            using HttpClient client = _clientFactory?.CreateClient("gateway"); 
            try
            { 
                Auctions = await client.GetFromJsonAsync<List<AuctionItemDTO>>("auction/getauction"); 

                Console.WriteLine("Success 'OnGet'");
                Console.WriteLine(Auctions[0].AuctionId);
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
