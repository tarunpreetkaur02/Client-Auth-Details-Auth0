# Auth0 Client Authorization and Authentication Details 

An application that allows only whitelisted users to view the authentication and authorization details of a client.

## Overview

This application has been developed in Asp.Net and it lists the applications associated with a client and the respective authentication and authorizations details.
The sample application on [https://auth0.com/docs/quickstart/webapp/aspnet-core/01-login] has been used as the basis for implementing Auth0 authentication and further modifications have been done to include the authentication and authorization details for the clients.
The following calls have been done in the code

| Route calls      						   | Fields to display| Additional info  |
| ----------------------------------------:|-----------------:| -------------------------------------------------------------------------------------------------:|
| Get all clients route (/v2/clients)      | $.name field     | For displaying the list of applications and $.client_id to make further calls                     |
| Get all connections (/v2/connections)    | $.name field     | $.client_id field to lookup in enabled_clients  and display the corresponding authentication name |
| Get all client grants (v2/client-grants) | $.audience field | Search the $.client_id field and display corresponding $.audience                                 |

### Description

1. The application created in Auth0 is an ASP.NET Core v3.0 Regular web application.
2. The application should have Client Credentials enabled in Grant Types under Advanced settings.
3. Make a note of the values of client_id, client_secret and audience.
4. Add the below URL's in Allowed Callback URLs or modify as per your organization URL
   [http://localhost:3000], [http://localhost:3000/callback]
   and below URL in Allowed Logout URLs
   [http://localhost:3000]
5. The regular web application created should be authorized to request access tokens. 
6. The management API should authorize this web app to request access tokens under Auth0 Management API -> Machine to Machine Applications. Toggle the button to Authorized for this regular web app.
7. Create a rule(Whitelist for a Specific App) under Auth Pipeline in the Auth0Dashboard and provide the name of your application and the email of the authorized user in the code.

```
function userWhitelistForSpecificApp(user, context, callback) {
  // Access should only be granted to verified users.
  if (!user.email || !user.email_verified) {
    return callback(new UnauthorizedError('Access denied.'));
  }

  // only enforce for NameOfTheAppWithWhiteList
  // bypass this rule for all other apps
  if (context.clientName !== 'Test App') {
    return callback(null, user, context);
  }

  const whitelist = ['tarun.yps@gmail.com']; // authorized users
  const userHasAccess = whitelist.some(function (email) {
    return email === user.email;
  });

  if (!userHasAccess) {
    return callback(new UnauthorizedError('Access denied.'));
  }

  callback(null, user, context);
}
```

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) (.NET Core 3.1 or .NET 5.0+)

## Usage

1. Ensure that you have replaced the `appsettings.json` file with the values for your Auth0 account. 

2. Run the application from the command line:

```bash
dotnet run
```

3. Go to `http://localhost:3000` in your web browser to view the website. 

## To run this project with Docker

In order to run the example with Docker you need to have [Docker](https://docker.com/products/docker-desktop) installed.

To build the Docker image and run the project inside a container, run the following command in a terminal, depending on your operating system:

```
# Mac
sh exec.sh

# Windows (using Powershell)
.\exec.ps1
```

## Packages Installed
The following packages were installed using the Nuget Package Manager -> Package Manager console:
1. Install-Package Newtonsoft.Json.Schema -Version 3.0.14
2. Install-Package RestSharp -Version 106.11.7
3. Install-Package Microsoft.AspNetCore.Authentication.Cookies
4. Install-Package Microsoft.AspNetCore.Authentication.OpenIdConnect
5. Install-Package Json.Net -Version 1.0.33