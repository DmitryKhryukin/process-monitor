# Process Monitor API

* Monitors currently running processes
* Provides Server-Sent Events endpoing for automatic updates

## Prerequisites
.NET Core 3.1

## Endpoint

* `GET /api/processes`

Response:

```json
data:
{
"Processes":[
              {
                "Id":42,
                "ProcessName":"firstProcess",
                "TotalProcessorTime":273838.5829,
                "ThreadsCount":2,
                "PhysicalMemoryUsage":16306176,
                "UserProcessorTime":199349.0842,
                "PrivilegedProcessorTime":74489.4987,
                "State":"running"
              },
              {
                "Id":7019,
                "ProcessName":"secondProcess",
                "TotalProcessorTime":214658.2588,
                "ThreadsCount":4,
                "PhysicalMemoryUsage":43266048,
                "UserProcessorTime":150036.7363,
                "PrivilegedProcessorTime":64621.5225,
                "State":"running”
              }
            ]
}
```

Provided processes info:

```
Id (type: int) - process id

ProcessName (type: string)  - process name.

TotalProcessorTime (type: double) - total processor time for this process in milliseconds.

ThreadsCount (type: int) - number of threads that are running in the associated process.

PhysicalMemoryUsage (type: int) - amount of physical memory, in bytes, allocated for the associated process.

UserProcessorTime (type: double) - user processor time for this process in milliseconds.

PrivilegedProcessorTime (type: double) - privileged processor time for this process in milliseconds.

State (type: string) - process state "running” or "sleeping".
```

## Server-sent events

A [server-sent events](https://javascript.info/server-sent-events) approach is used to get automatic updates from the server every 3 second.

To open a connection to the server you have to used [EventSource](https://developer.mozilla.org/en-US/docs/Web/API/EventSource) interface ([npm package](https://www.npmjs.com/package/eventsource))

How to use EventSource:

    let eventSource = new EventSource('https://yourHost/api/processes');
    
    eventSource.addEventListener("open", function (e) {
      // connection is open
    });
    
    eventSource.addEventListener("message", function (e) {
    
      // got an update from the server
      // parse e.data json to show updated info
    });
    
    eventSource.addEventListener("error", function (e) {
    
      // server error
    });

You can find a web client example [here](https://github.com/DmitryKhryukin/process-monitor/tree/master/src/Clients/Web)

## Known issues and roadblocks 

We don't show all system processes because of the following issue:
[Process Property TotalProcessorTime throws InvalidOperationException exception on macOS](https://github.com/dotnet/runtime/issues/36777)

Currently we can't show CPU load because there is no way to calculate it using .NET Core on all platforms:
- Performance counters are not cross platform;
- we can't calculate sum of all processes CPU usage because we can't get TotalProcessorTime of all processes (as described above)
