# Twilio Example for Listings

## Configure Twilio Account

Edit the `appsettings.json` file, add your Twilio account SID and AUTH Token

## Database

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

The code generation from the aspnet-codegenerator for razor pages is a little wonky in that it doesn't include the full namespace, to fix manually edit the generated pages and add the `Twilio.Example.Models.XXXX` namespace to Dealer | Listing | DynamicNumber

### Issue 2 - Dropbox in Razor pages

The select dropdown is not populated with data for Enums, to fix manually edit the .cshtml page and add the select, e.g.

````html
<select
  asp-for="DynamicNumber.NumberStatus"
  asp-items="Html.GetEnumSelectList<Twilio.Example.Models.Status>()"
  class="form-control"
>
  <option>Select type ...</option>
</select>
```
````
