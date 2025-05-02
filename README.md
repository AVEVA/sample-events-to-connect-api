## About this sample

**Version:** 1.0.0

Built with:
- DotNet 8
- Visual Studio 2022 - 17.12.4
- AVEVA Events to CONNECT adapter 1.0.1.46

As noted in the [AVEVA Events to CONNECT adapter documentation](https://docs.aveva.com/bundle/events-to-connect/page/1252276.html): To publish the types and events in CONNECT data services by using AVEVA Events to CONNECT, **you must create a REST API endpoint.**

This is a sample API that follows the implementation outlined in the [documentation.](https://docs.aveva.com/bundle/events-to-connect/page/1252278.html) The adapter is designed to call an Auth0-protected REST API that returns events, types, and other context in a specific format. 

Below is an example of a message returned by this sample API that describes the creation of events of type ID "PumpStatus-EventsToCONNECT". It includes a `messageHeaders` object that specifies the type of message, action, format, and typeId. The `messageBody` contains 2 events, each having a value for the properties "pumpStatus", "site", and "asset".

GET https://localhost:7200/api/events

```json
{
  "messageHeaders": {
    "messageType": "Events",
    "action": "Create",
    "format": "Json",
    "typeId": "PumpStatus-EventsToCONNECT"
  },
  "messageBody": [
    {
      "id": "Pump1-On-20250502132902-EventsToCONNECT",
      "startTime": "2025-05-02T13:29:02.3227031-07:00",
      "endTime": "2025-05-02T14:29:02.3227052-07:00",
      "pumpStatus": "On",
      "site": {
        "id": "Site1-EventsToCONNECT"
      },
      "asset": {
        "id": "Pump1-EventsToCONNECT"
      }
    },
    {
      "id": "Pump2-Off-20250502132902-EventsToCONNECT",
      "startTime": "2025-05-02T13:29:02.3227031-07:00",
      "endTime": "2025-05-02T14:29:02.3227052-07:00",
      "pumpStatus": "Off",
      "site": {
        "id": "Site2-EventsToCONNECT"
      },
      "asset": {
        "id": "Pump2-EventsToCONNECT"
      }
    }
  ]
}
```

Below is a message returned by the API that dictates creation of event type "PumpStatus-EventsToCONNECT":

GET https://localhost:7200/api/events/eventtype

```json
{
  "messageHeaders": {
    "messageType": "EventTypes",
    "action": "Create",
    "format": "Json",
    "typeId": "PumpStatus-EventsToCONNECT"
  },
  "messageBody": [
    {
      "id": "PumpStatus-EventsToCONNECT",
      "name": "PumpStatus-EventsToCONNECT",
      "defaultAuthorizationTag": "PumpOperator-EventsToCONNECT",
      "properties": [
        {
          "propertyTypeCode": "String",
          "id": "PumpStatus",
          "name": "PumpStatus",
          "propertyTypeId": null
        },
        {
          "propertyTypeCode": "ReferenceData",
          "id": "Site",
          "name": "Site",
          "propertyTypeId": "Site-EventsToCONNECT"
        }
      ]
    }
  ]
}
```

## Sample API Details

The sample API is built to be as simple as possible while meeting each of the requirements of the Events to CONNECT Adapter, laying groundwork that can be expanded upon to reach more data sources and output the expected messages. It uses standard practices for building .NET API applications, including simple and secure authentication with standard Microsoft libraries.
### API Implementation: EventsController

The `EventsController` controller holds the API controller actions. the `Get()` action returns a response message that includes the expected header and body. The body includes a list of `EventMessageBody` which we get from `EventsService.Events`, optionally filtered by site ID if the "site" parameter is not empty.

**Controllers/EventsController.cs**
```cs
// GET: api/Events
[HttpGet]
public IActionResult Get(string site="")
{
    var header = new MessageHeader
    {
        MessageType = "Events",
        Action = "Create",
        Format = "Json",
        TypeId = EventTypeId
    };

    List<PumpEvent> events;

    if (!string.IsNullOrEmpty(site))
    {
        events = EventsService.Events.Where(e => e.Site?.Id?.Contains(site) ?? false).ToList();
    }
    else
    {
        events = EventsService.Events;
    }

    return Ok(new
    {
        MessageHeaders = header,
        MessageBody = events
    });
}
```
### Data Simulation: EventsService

The sample API's `EventsService` maintains a list of events, written as "EventMessageBody" objects. Every time `GetEvents()` is called, which happens on a 1 hour timer, it clears the events list and simply adds 3 new events for Site1, Site2, and Site3, each with a random value for "sample". The events start 1 hour in the past and end at current time.

**Services/EventsService.cs**
```cs
private void GetEvents()
{
    var startTime = DateTime.Now.AddHours(-1);
    var endTime = DateTime.Now;

    PumpEvent pump1Event = new PumpEvent
    {
        Id = $"Pump1-{(Pump1_On ? "On" : "Off")}-{startTime:yyyyMMddHHmmss}-EventsToCONNECT",
        PumpStatus = Pump1_On ? "On" : "Off",
        StartTime = startTime,
        EndTime = endTime,
        Site = new Reference { Id = "Site1-EventsToCONNECT" },
        Asset = new Reference { Id = "Pump1-EventsToCONNECT" }
    };

    PumpEvent pump2Event = new PumpEvent
    {
        Id = $"Pump2-{(Pump2_On ? "On" : "Off")}-{startTime:yyyyMMddHHmmss}-EventsToCONNECT",
        PumpStatus = Pump2_On ? "On" : "Off",
        StartTime = startTime,
        EndTime = endTime,
        Site = new Reference { Id = "Site2-EventsToCONNECT" },
        Asset = new Reference { Id = "Pump2-EventsToCONNECT" }
    };

    Events.Clear();
    Events.Add(pump1Event); 
    Events.Add(pump2Event);

    Pump1_On = !Pump1_On;
    Pump2_On = !Pump2_On;
}
```

This part of the sample simulates getting data from a data source. This would be the entry point to write your implementation of getting external events from some actual source. 

### Expanding on the event type

The `EventMessageBody` can include a number of custom properties that slot into properties of the event type on CONNECT Data Services. To have the event type automatically created with the custom properties, they need to be included in the "api/Events/EventType" action. The sample has an event type that includes the custom string property named "PumpStatus" and reference data named "Site". Created EventTypes will also have the [base event type properties.](https://docs.aveva.com/bundle/connect-data-services-developer/page/api-reference/event-type-store/event-type-store-event-types.html)

```cs
// GET: api/Events/Type
[HttpGet("Type")]
public IActionResult GetEventType()
{
    var header = new MessageHeader
    {
        MessageType = "EventTypes",
        Action = "Create",
        Format = "Json",
        TypeId = EventTypeId
    };

    var statusProperty = new PropertyDefinition
    {
        Id = "PumpStatus",
        Name = "PumpStatus",
        PropertyTypeCode = "String"
    };

    var siteProperty = new PropertyDefinition
    {
        Id = "Site",
        Name = "Site",
        PropertyTypeCode = "ReferenceData",
        PropertyTypeId = ReferenceDataTypeId
    };

    var typeDefinition = new TypeDefinition
    {
        Id = EventTypeId,
        Name = EventTypeId,
        DefaultAuthorizationTag = AuthorizationTagId
    };

    typeDefinition.Properties.AddRange([statusProperty, siteProperty]);

    return Ok(new
    {
        MessageHeaders = header,
        MessageBody = new[] { typeDefinition }
    });
}
```
## Appsettings.json

The sample includes an `appsettings.placeholder.json` file that needs to be filled out and renamed to `appsettings.json`:

```json
{
  "auth0": {
    "Authority": "https://<Auth0 Authority>.com/",
    "Audience": "https://localhost:7200"
  },
  "eventTypeId": "PumpStatus-EventsToCONNECT",
  "referenceDataTypeId": "Site-EventsToCONNECT",
  "assetTypeId": "Pump-EventsToCONNECT",
  "authorizationTagId": "PumpOperator-EventsToCONNECT",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

| Setting             | Description                                                                                                                                                                                         |
| ------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| auth0.Authority      | The Auth0 URL to use for authentication.                                                                                                                                                            |
| auth0.Audience      | The Auth0 audience to limit/scope our tokens to our API application.                                                                                                                                |
| eventTypeId         | A type ID the adapter will use for creating the event type. |
| referenceDataTypeId   | A type ID the adapter will use for creating the reference data type.  |
| assetTypeId   | A type ID the adapter will use for creating the asset type.  |
| authorizationTagId   | An ID the adapter will use for creating the authorization tag.  |


## Auth0 Setup

For this sample, on [auth0.com](https://auth0.com) we register an API named "EventsToCONNECTSampleAPI" and a client application that is authorized to use it named "EventsToCONNECTSampleAPI Client". 

To register an API and corresponding client application on Auth0, first follow the Auth0 documentation for registering an API: [Register APIs](https://auth0.com/docs/get-started/auth0-overview/set-up-apis). The "Identifier" will be the URL of our API (https://localhost:7200). Step 3 can be skipped since it was already completed in this sample project. 

After that, create a Machine to Machine application that has permissions to access the registered API: [Register Machine-to-Machine Applications](https://auth0.com/docs/get-started/auth0-overview/create-applications/machine-to-machine-apps). Note that the API has no scopes that need configuring, so there won't be any listed in the "Authorize Machine to Machine Application" step.

![Pasted image 20250325115429](https://github.com/user-attachments/assets/f690fc16-3e8f-4c6b-9bdd-3a4eaef6f7e4)

The "Domain" field contains the value we will use for the "Authority" setting in `appsettings.json`. The Client ID will be used in the Adapter Data Source client ID setting, same with the secret. The "Token Audience" data source setting will be the "Identifier" we specified in Auth0, which comes in the bearer token and is for the receiving application (in our case, the sample API) to validate that the token is scoped correctly for it.

## Adapter Configuration

Import the included `AdapterConf.json` file to the Adapter Configurator utility. This will import a data ingress component that will work with the sample API, after the Auth0 endpoint and credentials are entered. 

![image](https://github.com/user-attachments/assets/f1bd9d27-66fa-4e3c-a842-78113a33db94)

Replace the Token Endpoint with the Auth0 authority, and paste in the client ID and client secret from the Auth0 application. The endpoint can be changed if the API is not being run on the same machine as the Adapter.

For more information on the Data Source configuration, check out the documentation: [Configure AVEVA Events to CONNECT data source using the Configurator plugin](https://docs.aveva.com/bundle/events-to-connect/page/1236323.html)

### Data Egress

After the ingress component is imported and configured, the [adapter must be configured for data egress to CONNECT.](https://docs.aveva.com/bundle/events-to-connect/page/1252608.html)

### Adapter Operation

Once fully configured, the adapter automatically handles the following:

1. Authentication to CONNECT
2. It runs each "data selection" every time as scheduled, polling the API events, assets, reference data, and authorization tags and sending the corresponding OMF messages to CONNECT to actually create everything
3. It logs any failed requests to the API to the "FailedRequests" controller
4. It queries the "HealthCheck" controller to check the health of the API (in our case, the API only ever says it's healthy)

## Running the sample

The easiest way to run the sample is to open the solution in Visual Studio, and then run the configuration "https". This will serve the API locally on your machine.

### Serving the API long-term

Since the sample is a .NET API application, it can be hosted using IIS web server. Open the project in Visual Studio and publish it to your IIS server by clicking "Build" - "Publish "EventsToCONNECTAPISample" and following the wizard.

### Testing the sample

The sample includes unit and integration tests to validate its functionality. The tests can be run from the Visual Studio Test menu (Test > Run all tests). 
