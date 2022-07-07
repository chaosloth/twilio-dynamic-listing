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
    public class DetailsModel : PageModel
    {
        private readonly Twilio.Example.Data.CallTrackingContext _context;

        public DetailsModel(Twilio.Example.Data.CallTrackingContext context)
        {
            _context = context;
        }

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
    }
}
