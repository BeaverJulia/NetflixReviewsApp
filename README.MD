# NetflixReviewsApp API :clapper: :speech_balloon:

NetflixReviewsApp is a WebApi to add reviews to shows, provided by 
https://netflix-app.openwrks.com

## Installation

This app uses an MSSQL database, the connection string is set to a local database. To create a database locally please run the following commands in the console

```cmd
update-database
add-migration init
update-database
```

## Authorization
Credentials are stored locally in a .json file. To authorize add OpenWrksCredentials.json in NetflixReviewsApp.api folder with the following structure
```JSON
{
  "Url": "https://netflix-auth.openwrks.com/connect/token",
  "Scope": "netflix.shows.read",
  "GrantType": "client_credentials",
  "ClientId": "applicant",
  "ClientSecret": "g*8gdw24XX45gsawfDDcsza@e"
}
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)