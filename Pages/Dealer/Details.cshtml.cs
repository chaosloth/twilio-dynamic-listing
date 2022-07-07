using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Twilio.Example.Data;
using Twilio.Example.Models;

namespace Twilio.Example.Pages.Dealer
{
    public class DetailsModel : PageModel
    {
        private readonly Twilio.Example.Data.CallTrackingContext _context;

        public DetailsModel(Twilio.Example.Data.CallTrackingContext context)
        {
            _context = context;
        }

        public Twilio.Example.Models.Dealer Dealer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Dealer == null)
            {
                return NotFound();
            }

            var dealer = await _context.Dealer.FirstOrDefaultAsync(m => m.DealerId == id);
            if (dealer == null)
            {
                return NotFound();
            }
            else
            {
                Dealer = dealer;
            }
            return Page();
        }
    }
}
