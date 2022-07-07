using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Twilio.Example.Data;
using Twilio.Example.Models;

namespace Twilio.Example.Pages.DynamicNumber
{
    public class CreateModel : PageModel
    {
        private readonly Twilio.Example.Data.CallTrackingContext _context;

        public CreateModel(Twilio.Example.Data.CallTrackingContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["DealerId"] = new SelectList(_context.Dealer, "DealerId", "DealerId");
            ViewData["ListingId"] = new SelectList(_context.Listing, "ListingId", "ListingId");
            return Page();
        }

        [BindProperty]
        public Twilio.Example.Models.DynamicNumber DynamicNumber { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.DynamicNumber == null || DynamicNumber == null)
            {
                return Page();
            }

            _context.DynamicNumber.Add(DynamicNumber);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
