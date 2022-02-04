[![Build status](https://dev.azure.com/APS-SD-Stewards/APS-SD/_apis/build/status/proscrumdev.battleship-dotnetcore-CI)](https://dev.azure.com/APS-SD-Stewards/APS-SD/_build/latest?definitionId=28)

# Build the application 
In the root folder of the application there is a Battleship.sln which can be used to build all projects by executing
```bash
dotnet build 
```

# Run the application

## Running locally
You neet to have the .NET6 runtime installed on your local machine
https://www.microsoft.com/net/download

You can now run the game with
```bash
dotnet run --project .\Battleship.Ascii\Battleship.Ascii.csproj
```


## Running within a docker container
You can easily run the game within a docker container.

Change into the Battleship folder

```bash
docker run -it -v ${PWD}:/battleship mcr.microsoft.com/dotnet/sdk:6.0 bash
```

This starts a new container and maps your current folder on your host machine as the folder battleship in your container and opens a bash console. In bash then change to the folder battleship/Battleship.Ascii and run
```bash
dotnet run 
```

# Execute Tests
You can run tests on the console by using
```bash
dotnet test 
```

If you want to run tests within VSCode, you can install the [.NET Core Test Explorer Extension](https://marketplace.visualstudio.com/items?itemName=formulahendry.dotnet-test-explorer). In this case make sure, you set the Propery "Test Project Path" in the extension setting to this value:
```bash
**/*[Test|ATDD]*/*.csproj
```

# Telemetry data
This application is collecting telemetry data with Microsoft Application Insights.
For more details see https://docs.microsoft.com/en-us/azure/azure-monitor/app/console.

To send the telemetry data to a specific instance of Application Insights, the connection string has to be adjusted in ApplicationInsights.config.
