# kaiko-api-client
C# implementation of clients for Kaiko APIs.

Kaiko currently provides two HTTP APIs:
-   Reference Data (Public)
-   Market Data (Authenticated)

In addition, Kaiko also offers a Stream product for real-time, low latency data:
-   Real-Time Stream SDK (Authenticated)

## Creating your local .env file
In order to be able to run some integration tests, you should create a `.env` file in the `src` folder with the following variables:
```secretsEnvVariables
KaikoApiConfiguration__ApiKey=********
KaikoStreamConfiguration__ApiKey=********
KaikoStreamConfiguration__ChannelUrl=https://gateway-v0-grpc.kaiko.ovh
```

## AWS Parameters for REST Client
In order to be able to run some integration tests, you should ensure that you have access to the AWS parameters starting in `/CiCd`.
In order for the applications in this solution to run correctly on AWS, please ensure that variables starting in `/[environment]` 
 are defined for all 3 environments (_Production_, _Staging_, _Development_) :
```awsParams
# REPOSITORY SECRETS
/[environment]/Trakx/Kaiko/ApiClient/KaikoApiConfiguration/ApiKey
/[environment]/Trakx/Kaiko/ApiClient/Stream/KaikoStreamConfiguration/ApiKey

# GLOBAL SECRETS
# Instead of creating a specific repository secret, can use the global one with the same [Key]
/[environment]/Global/KaikoApiConfiguration/ApiKey
/[environment]/Global/KaikoStreamConfiguration/ApiKey
```

## AWS Parameters for Stream Client
In order to be able to run some integration tests, you should ensure that you have access to the AWS parameters starting in `/CiCd`.
In order for the applications in this solution to run correctly on AWS, please ensure that variables starting in `/[environment]` 
 are defined for all 3 environments (_Production_, _Staging_, _Development_) :
```awsParams
/[environment]/Trakx/Kaiko/ApiClient/Stream/KaikoStreamConfiguration/ApiKey
```

## How to regenerate C# API clients

-   If you work with external API, you probably need to update OpenAPI definition added to the project. It's usually openApi3.yaml file.
-   Do right click on the project and select Edit Project File. In the file change value of `GenerateApiClient` property to true.
-   Rebuild the project. `NSwag` target will be executed as post action.
-   The last thing to be done is to run integration test `OpenApiGeneratedCodeModifier` that will rewrite auto generated C# classes to use C# 9 features like records and init keyword.
