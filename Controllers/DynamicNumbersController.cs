using Microsoft.AspNetCore.Mvc;
using Twilio.Example.Models;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account.AvailablePhoneNumberCountry;
using Microsoft.EntityFrameworkCore;


namespace Twilio.Example.Controllers;

[ApiController]
[Route("api")]
public class DynamicNumberController : ControllerBase
{
    private readonly ILogger<DynamicNumberController> _logger;
    private readonly IConfiguration _configuration;
    private ITwilioRestClient _client;

    private readonly Twilio.Example.Data.CallTrackingContext _context;

    public DynamicNumberController(IConfiguration configuration, ILogger<DynamicNumberController> logger, Twilio.Example.Data.CallTrackingContext context)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;

        var TWILIO_SID = _configuration["Twilio:TWILIO_ACCOUNT_SID"];
        var TWILIO_AUTH_TOKEN = _configuration["Twilio:TWILIO_AUTH_TOKEN"];

        _logger.LogInformation("Twilio Config SID: {sid}, Token: {token}", TWILIO_SID, TWILIO_AUTH_TOKEN);
        _client = new TwilioRestClient(TWILIO_SID, TWILIO_AUTH_TOKEN);
    }

    [Route("twilio/available")]
    [HttpGet]
    public async Task<IEnumerable<DynamicNumber>> GetAvailableTwilioNumbersInRegion(string region)
    {
        var localNumbers = await LocalResource.ReadAsync(
            pathCountryCode: "AU", inRegion: region, client: _client, limit: 20);

        var availableNumbers = new List<DynamicNumber>();

        foreach (var record in localNumbers.ToList())
        {
            var number = new DynamicNumber
            {
                PhoneNumber = record.PhoneNumber.ToString(),
                NumberStatus = Status.Available
            };
            availableNumbers.Add(number);
        }
        return availableNumbers;
    }

    [Route("numbers")]
    [HttpGet]
    public IEnumerable<DynamicNumber> GetFromDB()
    {
        return _context?.DynamicNumber?.Include(dn => dn.Dealer).Include(dn => dn.Listing).ToList() ?? new List<DynamicNumber>();
    }

    [Route("listing/{ListingId}")]
    [HttpGet]
    public IActionResult GetNumber(int ListingId)
    {
#pragma warning disable CS8602
        var number = _context.DynamicNumber?.Include(dn => dn.Dealer).Include(dn => dn.Listing).FirstOrDefault(pn => pn.Listing.ListingId == ListingId);

        if (number != null)
        {
            return new OkObjectResult(number);
        }
        else
        {
            return StatusCode(404);
        }
    }


    [Route("retire/{ListingId}")]
    [HttpGet]
    public IActionResult RetireListing(int ListingId)
    {
        var dn = _context.DynamicNumber?.FirstOrDefault(pn => pn.Listing.ListingId == ListingId);

        if (dn != null)
        {
            try
            {
                dn.Listing = null;
                dn.ListingId = null;
                dn.NumberStatus = Status.Available;
                dn.LastUsedDate = DateTime.Now;

                _context.SaveChanges();

                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error saving changes to database", ex);
                return StatusCode(500, "Error saving to database");
            }
        }
        else
        {
            return StatusCode(404);
        }
    }

    [Route("allocate")]
    [HttpGet]
    public async Task<IActionResult> AllocateNumberToListing(int ListingId, int DealerId)
    {
        // Check to see if listing exists
        var listing = _context.Listing?.FirstOrDefault(l => l.ListingId == ListingId);
        if (listing == null)
        {
            return StatusCode(404, "Listing not found");
        }

        // Check if number already allocated
        var dn = _context.DynamicNumber?.FirstOrDefault(pn => pn.Listing.ListingId == ListingId);
        if (dn != null)
        {
            _logger.LogInformation("Listing already has allocated DN");
            return new OkObjectResult(dn);
        }

        // Check to see if dealer exists
        var dealer = _context.Dealer?.FirstOrDefault(d => d.DealerId == DealerId);
        if (dealer == null)
        {
            return StatusCode(404, "Dealer not found");
        }

        // Allocate from pool
        var dyn = _context.DynamicNumber?.OrderByDescending(dyn => dyn.LastUsedDate).FirstOrDefault(dyn => dyn.NumberStatus != Status.Assigned);
        if (dyn != null)
        {
            try
            {
                dyn.DealerId = DealerId;
                dyn.ListingId = ListingId;
                dyn.NumberStatus = Status.Assigned;

                _context.SaveChanges();

                return new OkObjectResult(dyn);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error saving changes to database", ex);
                return StatusCode(500);
            }
        }
        else
        {
            _logger.LogInformation("No available dynamic numbers, need to purchase and add to pool");
        }

        // *************************************************
        // * Logic here:
        // * 1. Get region name from Dealer
        // * 2. Find available number in Twilio
        // * 3. Purchase number *** NOT DONE FOR SAFETY ***
        // * 4. Allocate number (Always allocates available)
        // *************************************************


        try
        {
            var availableNumbers = await LocalResource.ReadAsync(pathCountryCode: "AU", client: _client, limit: 1);
            _logger.LogInformation("Getting number in region {region}", availableNumbers.ToString());

            var twilioNumber = availableNumbers.FirstOrDefault();

            if (twilioNumber == null)
            {
                _logger.LogError("No available numbers in region");
                return StatusCode(501, "No numbers in region");
            }

            _logger.LogInformation("Available Twilio Number {twilioNumber}", twilioNumber.FriendlyName);

            var newDynamicNumber = new DynamicNumber();
            newDynamicNumber.PhoneNumber = twilioNumber.PhoneNumber.ToString();
            newDynamicNumber.NumberStatus = Status.Assigned;
            newDynamicNumber.DealerId = DealerId;
            newDynamicNumber.ListingId = ListingId;
            newDynamicNumber.LastUsedDate = DateTime.Now;

            _context.Add(newDynamicNumber);

            _context.SaveChanges();

            // *************************************************
            // * TODO:  Add logic here to purchase number
            // * 1. Purchase number from available
            // * 2. Fail of cannot purchase or allocate
            // *************************************************

            return new OkObjectResult(newDynamicNumber);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString(), ex);
            _logger.LogError("Error saving changes to database", ex);
            return StatusCode(500);
        }
    }
}
