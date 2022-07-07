using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Twilio.Example.Models;

namespace Twilio.Example.Data
{
    public class CallTrackingContext : DbContext
    {
        public CallTrackingContext(DbContextOptions<CallTrackingContext> options)
            : base(options)
        {
        }

        public DbSet<Twilio.Example.Models.Listing>? Listing { get; set; }

        public DbSet<Twilio.Example.Models.DynamicNumber>? DynamicNumber { get; set; }

        public DbSet<Twilio.Example.Models.Dealer>? Dealer { get; set; }
    }
}
