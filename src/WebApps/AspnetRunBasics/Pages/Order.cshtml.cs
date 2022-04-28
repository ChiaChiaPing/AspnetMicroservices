using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class OrderModel : PageModel
    {
        private readonly IOrderingService _orderingService;

        public OrderModel(IOrderingService orderingService)
        {
            _orderingService = orderingService ?? throw new ArgumentNullException(nameof(orderingService));
        }

        public IEnumerable<OrderResponseModel> Orders { get; set; } = new List<OrderResponseModel>();

        // Default method that would be invoked after load this page
        public async Task<IActionResult> OnGetAsync()
        {
            var userName = "swn";
            Orders = await _orderingService.GetOrdersWithUserName(userName);

            return Page();
        }       
    }
}