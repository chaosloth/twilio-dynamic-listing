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
    public class DeleteModel : PageModel
    {
        private readonly Twilio.Example.Data.CallTrackingContext _context;

        public DeleteModel(Twilio.Example.Data.CallTrackingContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Listing == null)
            {
                return NotFound();
            }
            var listing = await _context.Listing.FindAsync(id);

            if (listing != null)
            {
                Listing = listing;
                _context.Listing.Remove(Listing);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
