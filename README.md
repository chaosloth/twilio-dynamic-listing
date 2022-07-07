# Twilio Example for Listings

Hello and welcome, this little project models out a small API service that allocates unique phone numbers against a listing (e.g. Car, Property, Flooring, etc). Numbers are allocated from a pool, using the last assigned date in descending order. When no numbers are available in the pool, the logic in the `DynamicNumbersController.cs` will purchase a new Twilio number and add it to the pool. As listings are removed (retired) the phone number is returned to the pool for allocation to a new listing.

Note that in this code snippet the actual purchase of a number has been removed but can be quickly added with a couple of lines of code. See the `AllocateNumberToListing` function in the `DynamicNumbersController.cs` for more information.

This project exposes the following API endpoints, see **Swagger** documentation for more detail:

- /api/twilio/available
- /api/numbers
- /api/listing/{listing id}
- /api/retire/{listing id}
- /api/allocate

The data model is contained in `Models/ProjectModels.cs` with the following entities:

- Dealer
- Listing
- DynamicNumber

Microsoft Entity Framework has been used to generate the SQL schema from the above models, more detail on that below.

For convenience a few administrative pages have been created to enable quick CRUD of the data, these are accessible from the /Index page

# Configure Twilio Account

Edit the `appsettings.json` file, add your Twilio account SID and AUTH Token. You may also change the database connection string if needed

It should look like the following with your account SID and AUTH_TOKEN

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Twilio": {
    "TWILIO_ACCOUNT_SID": "ACxxxxx",
    "TWILIO_AUTH_TOKEN": "XXXXXX"
  },
  "ConnectionStrings": {
    "CallTrackingContext": "Data Source=TwilioTracking.db"
  }
}
```

# Database

This project uses SQLLite as a provider, this may be changed to SQL Server or MonogoDB or some other supported database

## Development notes

You shouldn't need to run these commands again unless the model changes

### Initialize database

If for some reason the database needs to be reinitialized run the following commands:
`dotnet tool install --global dotnet-ef` - This installed the Entity Framework tool

`dotnet ef migrations add InitialCreate` - Create db schema

`dotnet ef database update` - Upon model changes run this to keep schema up to date

### Create web pages to edit model

`dotnet-aspnet-codegenerator razorpage -m Listing -dc CallTrackingContext -udl -outDir Pages/Listing --referenceScriptLibraries -sqlite`

`dotnet-aspnet-codegenerator razorpage -m DynamicNumber -dc CallTrackingContext -udl -outDir Pages/DynamicNumber --referenceScriptLibraries -sqlite`

`dotnet-aspnet-codegenerator razorpage -m Dealer -dc CallTrackingContext -udl -outDir Pages/Dealer --referenceScriptLibraries -sqlite`

**_ NOTE: _** As of time of writing there is a bug in the code generation that will NOT prefix the Model class names, leading to an error (CS0118) being shown. The fix is to manually enter the namespace prefix for these classes.

### Additional reading

Microsoft has a great tutorial [here](https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/model?view=aspnetcore-6.0&tabs=visual-studio-code)

# Code fixes

If you decide to re-generate the Razor pages for some reason, here's couple of issues you might come across

### Issue 1 - Namespace fix

The code generation from the aspnet-codegenerator for razor pages is a little wonky in that it doesn't include the full namespace, to fix manually edit the generated pages and add the `Twilio.Example.Models.XXXX` namespace to Dealer | Listing | DynamicNumber

### Issue 2 - Dropdown in Razor pages

The select dropdown is not populated with data for Enums, to fix manually edit the .cshtml page and add the select, e.g.

```html
<select
  asp-for="DynamicNumber.NumberStatus"
  asp-items="Html.GetEnumSelectList<Twilio.Example.Models.Status>()"
  class="form-control"
>
  <option>Select type ...</option>
</select>
```
