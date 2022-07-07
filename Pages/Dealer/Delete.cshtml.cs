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
    public class DeleteModel : PageModel
    {
        private readonly Twilio.Example.Data.CallTrackingContext _context;

        public DeleteModel(Twilio.Example.Data.CallTrackingContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Dealer == null)
            {
                return NotFound();
            }
            var dealer = await _context.Dealer.FindAsync(id);

            if (dealer != null)
            {
                Dealer = dealer;
                _context.Dealer.Remove(Dealer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
