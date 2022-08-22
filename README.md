# Payments Gateway API

## Overview

Payments Gateway API allows to pass through requests from merchants to target bank(s).  
High level schema looks like this:  

![](docs/2022-08-22-00-53-58.png)

To process payment it's also needed to pass such information:  
- Card details
- Expense details (e.g. amount to spend)

## Architecture details (for the demo)

Below you can see detailed schema for created payments gateway API.  
Taking into account restricted timeline, there was no goal to have production quality solution.  
However there was a goal to show that many steps that will be needed to make this solution with a good quality.  

![](docs/2022-08-22-00-57-14.png)

- Known and +- fast way to build API => with REST. Though, it makes more sense to switch to GRPC where possible
- In real life - we would definitely go with real storage and EFCore/Dapper/ADO. Here, however, there is custom quick simulator of DBContext. Data is shared in-memory.
- As processing of operation by bank may take time - interaction should be happening in background
  Ideally that should be distributed queue + workers. However setup of such pair takes time.  
  Here .NET Channels & Background hosted service are used. Yeah, they are not scaling well and that's not for prod, but for the demo!
- Merchant will have to ask payments API about payment using polling. It's better to use web sockets or separate pub/sub server to avoid such round-trips

## Desired architecture

Here you can find some ideas how this solution should really look.  

![](docs/2022-08-22-01-06-05.png)

- It's 2022 and it's distributed architectures trend
- We can have multiple workers and scale them both - vertically & horizontally
- API is independent and also scales really well
- We can use GRPC or both (+REST). But GRPC is much faster because of HTTP2
- Payments won't get lost because of distributed queue setup
- Also payments are getting into database which scales well and can provide encryption of data, backups etc
- Merchant just needs to subscribe and get notified once payment is processed
- It will be awesome as well if bank platform can also provide sort of event driven design approach

## Getting started

The application is built on the .NET 6 framework and provides functionality via REST API. 

### Prerequisites

1. IDE: VS 2022 Community / VS Code / Rider.  
   NOTE: I used Rider at MacOS
2. SDK: .NET6.

### Running solution locally using Docker

- Navigate to root of solution folder.
- `docker build . -t payments_gateway_local && cd BankSimulator && docker build . -t bank_of_uk_local && cd .. && docker-compose up`
- Navigate to http://localhost:7007/swagger/index.html.
- After you're done, it's good to do `docker-compose stop`

**NOTE:** MacOS has known issue related to Docker. It cannot resolve host name when in compose we need to connect 2 containers. The only one working is: _host.docker.internal_

### Running solution from IDE

- Use Debug build configuration.
- Setup **Api.Host** project as startup project and run it.
- Also start **CKO.BankOfUk.Emulator** project in parallel.
- Go to:
    - Swagger: https://localhost:7007/swagger/index.html
    - Healthcheck: https://localhost:7007/healthz

## Sample flow

Capturing some requests used for testing. You can also see payload that can be used in swagger as well.

<details>
<summary>CREATE PAYMENT</summary>

**POST**
```
curl 'http://localhost:63120/v1/payments' \
  -H 'Accept-Language: en-US,en;q=0.9,ru;q=0.8' \
  -H 'Cache-Control: no-cache' \
  -H 'Connection: keep-alive' \
  -H 'Content-Type: application/json' \
  -H 'Origin: http://localhost:63120' \
  -H 'Pragma: no-cache' \
  -H 'Referer: http://localhost:63120/swagger/index.html' \
  -H 'Sec-Fetch-Dest: empty' \
  -H 'Sec-Fetch-Mode: cors' \
  -H 'Sec-Fetch-Site: same-origin' \
  -H 'User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36' \
  -H 'accept: */*' \
  -H 'sec-ch-ua: "Chromium";v="104", " Not A;Brand";v="99", "Google Chrome";v="104"' \
  -H 'sec-ch-ua-mobile: ?0' \
  -H 'sec-ch-ua-platform: "macOS"' \
  --data-raw $'{\n  "card": {\n    "number": "3333-4444-5555-6666",\n    "expireMonth": 12,\n    "expireYear": 2022,\n    "cvvCode": 850\n  },\n  "expense": {\n    "amount": 100,\n    "currency": "USD"\n  }\n}' \
  --compressed
  ```
</details>

<details>
<summary>GET PAYMENT</summary>

**GET**
```
curl 'http://localhost:63120/v1/payments/112370bb-65ba-44aa-9c15-15c383390315' \
  -H 'Accept-Language: en-US,en;q=0.9,ru;q=0.8' \
  -H 'Cache-Control: no-cache' \
  -H 'Connection: keep-alive' \
  -H 'Pragma: no-cache' \
  -H 'Referer: http://localhost:63120/swagger/index.html' \
  -H 'Sec-Fetch-Dest: empty' \
  -H 'Sec-Fetch-Mode: cors' \
  -H 'Sec-Fetch-Site: same-origin' \
  -H 'User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36' \
  -H 'accept: text/plain' \
  -H 'sec-ch-ua: "Chromium";v="104", " Not A;Brand";v="99", "Google Chrome";v="104"' \
  -H 'sec-ch-ua-mobile: ?0' \
  -H 'sec-ch-ua-platform: "macOS"' \
  --compressed
```

</details>

## About internal structure

Overall design is based on: clean architecture, onion architecture, DDD concepts, CQS & Mediator pattern.  
- Rich domain model which knows better about its own stuff
- Immutable entities. In some places records are used for simplicity.
- Integration event is processed by app layer and sent to processor for working with bank
- CVV code is not stored in payment details. I thought it's not secure. So passing it through - integration event is used
- There was no requirement to store card number as masked. So it's preprocessed on retrieval

## Bank simulator
It's very simple API with single POST method. Inside it generates random number 100-1000 and returns result based on it.

## API Documentation
Swagger is used to cover API documentation.  
Data annotations and versioning - in place.  

I did not cover all objects, but main are covered for example of mindset.

## Communication
To communicate from worker/hosted service to bank API - reusable http client is used, which is registered using best practices from MS.

Retry policies & Circuit breaker are used to make things stable and robust. Ideally, tests should be written to prove that those practices work.

## Code Coverage

This solution contains Tests for several assemblies. For product related work - I usually follow 80% + coverage. There was no goal to cover all code here.  
But you'll find good samples of how tests can be written.

NUnit, FluentAssertions are actively used here. If mocks & stubs are needed - I'd prefer Moq.

## HealthCheck

Service has sample HealthCheck, but it's really standard one.  
Typically it is extended to have more understanding about service health.

And this endpoint is usually actively used by k8s to get probes (healthy/unhealthy/degraded).

For local setup: http://<<serviceurl:port>>/healthz

## Code Styling

To keep code clean - such linter/analysis tools are used:

- StyleCop Analyzer. It's strict and will fail build if there is violation. It's important to have it on build pipeline as well

- ReSharper / Rider IDE - set of team rules which will allow to automate lots of cleanup things in IDE using simple keys combination.

- MS Code Analysis based on ruleset

What should be added? ConventionalCommits hook - to enforce using [conventional commits](https://www.conventionalcommits.org/en/v1.0.0/) practice

## Dependency injection

Though, MS has brilliant DI implementation for asp.net core - I'm big fan of Autofac.  
And it's used here.  

All dependencies are split into modules and registered at Infra layer.

## Logging & Tracing

It's not fully finished here, but there are some initial steps to make it good:
- Serilog is used for structured logging
- Logs are coming json format to console
- It's easy to enrich each log item with extras using Serilog
- A bit more time needed to finalize traceid & correlation as per open tracing standards
- Also it's possible to use ILogger from MS extensions, not from Serilog. But here it stays as is - not a big deal

What should be added? Typically there is tight integration with ELK / Appinsights / Datadog and there are some specifics. But I'd say in most of cases there is a separate agent which grabs logs from console and pushes to target servers. Mentioned libs provide cool stuff to work with trace id and correlation id and let us care less about it.

## Configuration

Service uses standard Microsoft.Configuration approach with ``appsettings.json`` in place.  
For daily routine - we should anyway work with ENV variables & templates OR key/vault (like in MS Azure).  

Setting that flow was not the gao of this demo.

## Pipeline

Typically there should be CI/CD set up. So that each time commit us pushed to server - it can build it, run tests and sometimes even deploy (depending on strategy).

You find this setup here at the moment, cause it was not the goal.

## Some more on what's missing

- Security concerns. Typically such API is secured using OAuth flow and JWT token
- Unit tests. To get solid coverage
- System tests. Using microsoft test host
- Load tests. I'd go with k6.io, cause it's very simple and fast
- Swagger tuning. It can generate way more params. But depends if company uses that at all
- Capturing extra metrics to understand how good payments api feels
- Domain concerns. There is definitely a lot to make better - change meaning of card number (now it's string but should be more complex), add more business checks etc
- Validation specifics. Validation of inputs and domain model - should for sure be not that simple. Only card numbers can have 10+ different rules related to standards
- DDD concerns. It's an interesting journey once we connect database and store items there. Also enums can be presented differently, but more boilerplate is needed
- Postman collections can be added for easier testing
- For docker - there are optimizations to do. For instance, copy less items to economy space.
- Setup pipeline. It's easy, but takes time. For this type of solution - any platform can provide almost automatic build. In many cases docker file we use for run can be used for that build purposes as well
- Though Json.net is used for serialization here, System.Text can also fit well. Cause there is no really complex serialization here
- In tests it makes sense to use more aligned fake/stub data. Usually there are libs that help to generate needed sample data - public or corporate (self created)

## Author

Eugene Pavliy
