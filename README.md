# TOURISTO
## Project Goals

To create a website providing access to give show users where are the attractions that he chooses.

## Instalation

### 1. For the project to work you have to have the listed software installed

- Visual Studio 2019
- MySql Server

### 2. For the sake of testing the service you need

- Postman
- A web browser(chrome, brave, edge, whatever)

### 3. Configuring the MySql Server

For the project to work there has to be MySql Server existing, on localhost with username root and password root. If you already have an existing server with different username and passsword you need to change the files 'Startup.cs' and 'TestStartup.cs' in TouristoService folder. Also the server should allow remote connections.

Change the
```var connection = @"server=localhost;uid=root; pwd=root; database=touristo;"```
variable to match your server, username and password, leave database name as is.

Next thing to do is to import the standard databases from the folder Database, it will create the tables with existing data needed for the service(Standard user etc.).

With it the service should be ready to go, the tests should work and the interaction by postman will be possible.

### 4. Authorize user throught postman

To get access token you need to make a post request, throught postman, on the path ``localhost:52287/users/login`` with and existing user data. The standard user is "KhatAdmin" with password "SilneHasl@12".
Example:
```
{
	"username":"KhatAdmin",
	"password":"SilneHasl@12"
}
```
It returns an access token which can be put in the authorization section of postman, the type is Bearer Token. It gives access to the rest of the service.

To make a new user to get the access token you need to push a post request on path ``loclhost:52287/users`` with following data in json format:
```
{
 "username": "testUser6",
    "password": "Test6",
    "role": Admin
}
```

### 5. Configure MVC part

For the MVC part to work we need to write in the MVC projects nuget package command line the following command:
```
Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r
```

It will repair some files which get damaged while downloading the project(tried to solve that, didn't work).

## Running the project

After setting everything up, you can now start the service project and the mvc project. 
To the MVC you need to login with an existing account to get an authorization. Before doing that no data will be shown in any of the subpages. After logging in you should be ablo to view, update, delete and edit data in the database throught the service.

If you want to do this directly throught the service you need to use postman. The following paths and body templates:

- loclhost:52287/users 

POST:
```
{
    "username": "testUser6",
    "password": "Test6",
    "role": Admin
}

```
- localhost::52287/users/login

POST:
```
{
	"username":"KhatAdmin",
	"password":"SilneHasl@12"
}
```