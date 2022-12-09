# kaiko-api-client
C# implementation of a [Kaiko Stream](https://sdk.kaiko.com/#kaiko-stream) api client


## Creating your local .env file
In order to be able to run some integration tests, you should create a `.env` file in the `src` folder with the following variables:
```secretsEnvVariables
KaikoApiConfiguration__ApiKey=********
```

## AWS Parameters
In order to be able to run some integration tests you should ensure that you have access to the following AWS parameters :
```awsParams
/[environment]/Trakx/Kaiko/ApiClient/KaikoApiConfiguration/ApiKey
/[environment]/Trakx/Kaiko/ApiClient/KaikoApiConfiguration/ApiKey
/CiCd/Trakx/Kaiko/ApiClient/KaikoApiConfiguration/ApiKey

```

## How to regenerate C# API clients

-   If you work with external API, you probably need to update OpenAPI definition added to the project. It's usually openApi3.yaml file.
-   Do right click on the project and select Edit Project File. In the file change value of `GenerateApiClient` property to true.
-   Rebuild the project. `NSwag` target will be executed as post action.
-   The last thing to be done is to run integration test `OpenApiGeneratedCodeModifier` that will rewrite auto generated C# classes to use C# 9 features like records and init keyword.
