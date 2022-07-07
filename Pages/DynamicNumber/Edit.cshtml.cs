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

namespace Twilio.Example.Pages.DynamicNumber
{
    public class EditModel : PageModel
    {
        private readonly Twilio.Example.Data.CallTrackingContext _context;

        public EditModel(Twilio.Example.Data.CallTrackingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Twilio.Example.Models.DynamicNumber DynamicNumber { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.DynamicNumber == null)
            {
                return NotFound();
            }

            var dynamicnumber = await _context.DynamicNumber.FirstOrDefaultAsync(m => m.DynamicNumberId == id);
            if (dynamicnumber == null)
            {
                return NotFound();
            }
            DynamicNumber = dynamicnumber;
            ViewData["DealerId"] = new SelectList(_context.Dealer, "DealerId", "DealerId");
            ViewData["ListingId"] = new SelectList(_context.Listing, "ListingId", "ListingId");
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

            _context.Attach(DynamicNumber).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DynamicNumberExists(DynamicNumber.DynamicNumberId))
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

        private bool DynamicNumberExists(int id)
        {
            return (_context.DynamicNumber?.Any(e => e.DynamicNumberId == id)).GetValueOrDefault();
        }
    }
}
