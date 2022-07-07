using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Twilio.Example.Data;
using Twilio.Example.Models;

namespace Twilio.Example.Pages.DynamicNumber
{
    public class DeleteModel : PageModel
    {
        private readonly Twilio.Example.Data.CallTrackingContext _context;

        public DeleteModel(Twilio.Example.Data.CallTrackingContext context)
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
            else
            {
                DynamicNumber = dynamicnumber;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.DynamicNumber == null)
            {
                return NotFound();
            }
            var dynamicnumber = await _context.DynamicNumber.FindAsync(id);

            if (dynamicnumber != null)
            {
                DynamicNumber = dynamicnumber;
                _context.DynamicNumber.Remove(DynamicNumber);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
