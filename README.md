[logo]: https://s3.amazonaws.com/emailhippo/bizbranding/co.logos/eh-horiz-695x161.png "Email Hippo"
[Email Hippo]: https://www.emailhippo.com
[Docs]: http://api-docs.emailhippo.com
[Microsoft.Extensions.Logging]: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging

![alt text][logo]

# Email Verification API Client (Version 3)

## About
This is a .NET package built for easy integration with [Email Hippo] RESTful (v3) API service editions known as 'Core' and 'More'. For
further information on the RESTful server side implementation, please see the [Docs].

## How to get the package
From [Nuget](http://nuget.org).
```
Install-Package EmailHippo.EmailVerify.Api.V3.Client
Install-Package System.Runtime.Serialization.Primitives
```

## Who is the package for?
 * __.NET__ developers and system integrators needing a fast start to using [Email Hippo] technology.

## What this package can do
If you're working in the .NET environment, this package can save you __hours of work__ writing your own JSON parsers, message pumping logic, threading and logging code.

## Prerequisites
 * __Visual Studio__ 2015 or later
 * __.NET Standard 1.4__ or later
 * __API license key__ from [Email Hippo]

## Features
 * Built for __high performance__ throughput. Will scale for concurrency and performance based on your hardware configuration (i.e. more CPU cores = more throughput).
 * __Sync__ and __async__ methods.
 * __Parallel__ batch processing available.
 * __Progress reporting__ via event callbacks built in.
 * __Extensive Logging__ built in using [Microsoft.Extensions.Logging].

## Performance
Fast throughput can be achieved by sending lists (IList<string>) of emails for processing. Speed of overall processing depends on your hardware configuration (i.e. number of effective CPU cores and available RAM).

Processing for lists of email is executed in parallel using multiple threads.

### Typical Processing Speed Results

#### Test Hardware Configuration
* __CPU__ : Intel 6850k (6 core + HT = 12 effective cores)
* __RAM__ : 64GB
* __Network (WAN)__ : xDSL (20Mb/sec)

__notes on tests__ :
 * tests run on sequence of randomized @gmail email addresses
 * caching not a test factor (as using random email addresses)

__'core'__ edition timings:

| # Emails | Run Time to Completion (ms)  | Processing Rate  (emails /sec) |
|---------:|-----------------------------:|-------------------------------:|
|       20 |                        1,583 |                          12.63 |
|       50 |                        1,607 |                          31.13 |
|      100 |                        4,609 |                          21.69 |

__'more'__ edition timings:

| # Emails | Run Time to Completion (ms)  | Processing Rate  (emails /sec) |
|---------:|-----------------------------:|-------------------------------:|
|       20 |                        1,419 |                          14.09 |
|       50 |                        3,743 |                          13.36 |
|      100 |                        4,888 |                          20.45 |

## How to use the package
Please note that full example code for all of the snippets below are available in the "EmailHippo.EmailVerify.Api.V3.Client.Tests" 
project which can be found in the GitHub repository for this project.

### Step 1 - license and initialize
This software must be initialized before use. Initializaton is only needed once per app domain. The best place to do this in in the hosting process bootstrap code. For example, a web app use global.asax, a console app use Main() method.

Supply license configuration to the software by providing the license key in code as part of initialization

Invoke static method ApiClientFactoryV3.Initialize(string licenseKey = null)... as follows if supplying the license in code:
```C#
/*Visit https://www.emailhippo.com to get a license key*/
ApiClientFactoryV3.Initialize("{your license key}", {Custom logger factory} [optional]);
```
The logger factory is of type [Microsoft.Extensions.Logging] and allows integration with Serilog, console, NLog and many more logging providers.


### Step 2 - create
The main client object is created using a static factory as follows:

__Example 2__ - creating the client
```c#
var myService = ApiClientFactoryV3.Create();
```

### Step 3 - use
Once you have a reference to the client object, go ahead and use it.

__Example 3__ - checking one or more email address synchronously
```c#
var responses = myService.Process(new VerificationRequest{Emails = new List<string>{"me@here.com"}, ServiceType = ServiceType.More });

/*Process responses*/
/*..responses*/
```

__Example 4__ - checking more than one email address asynchronously
```c#
var responses = myService.ProcessAsync(new VerificationRequest{Emails = new List<string>{"me@here.com","me2@here.com"}, ServiceType = ServiceType.More}, CancellationToken.None).Result;

/*Process responses*/
/*..responses*/
```

__Example 5__ - progress reporting

Progress can be captured using the built in event delegate "ProgressChanged" as follows
```c#
myService.ProgressChanged += (o, args) => Console.WriteLine(JsonConvert.SerializeObject(args));
```

__Example 6__ - logging

Logging is provided using [Microsoft.Extensions.Logging].

Enable logging using standard [Microsoft.Extensions.Logging] listeners.
e.g.
```c#
public class Startup
{
private static readonly ILoggerFactory MyLoggerFactory = new LoggerFactory();

/// <summary>
/// Setup and add serilog listeners to Microsoft logging extensions.
/// </summary>
public Startup(){
    Log.Logger = new LoggerConfiguration()
                    .Enrich
                    .FromLogContext()
                    .WriteTo.LiterateConsole()
                    .CreateLogger();

            MyLoggerFactory
                .AddSerilog();
    }

}
```

For full details of logging options see [Microsoft.Extensions.Logging].
