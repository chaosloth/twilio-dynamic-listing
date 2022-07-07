using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Twilio.Example.Data;
using Twilio.Example.Models;

namespace Twilio.Example.Pages.Dealer
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
            return Page();
        }

        [BindProperty]
        public Twilio.Example.Models.Dealer Dealer { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Dealer == null || Dealer == null)
            {
                return Page();
            }

            _context.Dealer.Add(Dealer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
