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
    public class IndexModel : PageModel
    {
        private readonly Twilio.Example.Data.CallTrackingContext _context;

        public IndexModel(Twilio.Example.Data.CallTrackingContext context)
        {
            _context = context;
        }

        public IList<Twilio.Example.Models.Dealer> Dealer { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Dealer != null)
            {
                Dealer = await _context.Dealer.ToListAsync();
            }
        }
    }
}
