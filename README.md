# Smile Direct Challenge

## Prerequisites

* .NET Core 2.2

## Building

1. `dotnet restore`
2. `dotnet build`

## Starting the API

1. `cd src/SmileDirect.Web`
2. `dotnet run`

Now that the API is running, you can use the console to query the API:

```bash
# Get all launchpads
curl -k https://localhost:5001/api/launchpads --silent | jq

# Get all launchpads with a name containing space
curl -k "https://localhost:5001/api/launchpads?filters[0].field=name&filters[0]value=space" | jq

# Get all launchpads with id containing atoll with a status of retired
curl -k "https://localhost:5001/api/launchpads?filters[0].field=id&filters[0].value=atoll&filters[1].field=status&filters[1].value=retired" | jq
```

## Running Tests

1. `cd test/SmileDirect.Web.Tests`
2. `dotnet test`