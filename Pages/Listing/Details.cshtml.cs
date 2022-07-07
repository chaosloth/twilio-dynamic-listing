using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Twilio.Example.Data;
using Twilio.Example.Models;

namespace Twilio.Example.Pages.Listing
{
    public class DetailsModel : PageModel
    {
        private readonly Twilio.Example.Data.CallTrackingContext _context;

        public DetailsModel(Twilio.Example.Data.CallTrackingContext context)
        {
            _context = context;
        }

        public Twilio.Example.Models.Listing Listing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Listing == null)
            {
                return NotFound();
            }

            var listing = await _context.Listing.FirstOrDefaultAsync(m => m.ListingId == id);
            if (listing == null)
            {
                return NotFound();
            }
            else
            {
                Listing = listing;
            }
            return Page();
        }
    }
}
