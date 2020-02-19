# GeoTask

Web service for getting location by Ip-Address

## Getting Started

1. Go to  [MaxMind](https://www.maxmind.com/en/home/), register an account and get free license key for Geo2Lite service.
2. Install postgre, and create the database
3. Apply migrations starting GeoTask.WebApi or execute it directly to your database (**GeoTask.WebApi/Db/Migrations**).
4. Now you can run GeoTask.GeoUpdate console app to fill database with last Geo2Lite data. There are some possible parameters:
    - **-db** "connection string"
    - **-licenseKey** "your [MaxMind](https://www.maxmind.com/en/home/) license key"
    - **-force**
> GeoTask.UpdateService store MD5 hash of last imported Geo2Lite data. If remote hash was not changed, it will not be downloaded, until it run with **-force** parameter
 5. Geo2Lite data import will take about 1 minute. It updates with no-blocking way, so you can use WebAPI while GeoUpdate is running
 
## Examples

**GET** api/Location?ip=109.254.21.84&language=en
```
{
    "ipAddress": "109.254.21.84",
    "longitude": "37.7857",
    "latitude": "47.9948",
    "accuracyRadius": 20,
    "timeZone": "Europe/Kiev",
    "metroCode": null,
    "countryName": "Ukraine",
    "countryIsoCode": "UA",
    "continentName": "Europe",
    "continentCode": "EU",
    "cityName": "Donetsk",
    "isInEuropeanUnion": false,
    "language": "en"
}
```
#### Geo2Lite provides 8 languages. There are locale_codes for all of them:
  - en
  - ru
  - de
  - fr
  - es
  - fr
  - ja
  - pt-BR
  - zh-CN
  
