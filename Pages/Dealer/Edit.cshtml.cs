using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Twilio.Example.Data;
using Twilio.Example.Models;

namespace Twilio.Example.Pages.Dealer
{
    public class EditModel : PageModel
    {
        private readonly Twilio.Example.Data.CallTrackingContext _context;

        public EditModel(Twilio.Example.Data.CallTrackingContext context)
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
            Dealer = dealer;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Dealer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DealerExists(Dealer.DealerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DealerExists(int id)
        {
            return (_context.Dealer?.Any(e => e.DealerId == id)).GetValueOrDefault();
        }
    }
}
