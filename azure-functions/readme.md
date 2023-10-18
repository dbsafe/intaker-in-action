# Using intaker in an Azure Function

Created project using the following references:

[Quickstart: Create your first C# function in Azure using Visual Studio](https://learn.microsoft.com/en-us/azure/azure-functions/functions-create-your-first-function-visual-studio)

[Quickstart: Create a C# function in Azure from the command line](https://learn.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=windows%2Cazure-cli)

Updated the initial project using:

[Guide for running C# Azure Functions in an isolated worker process - HTTP trigger](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide#http-trigger)

## Deploying the Function using Azure CLI

### Creating a resource group in Azure
```
az group create -l <region> -n <resource-group-name>
az group create -l eastus -n rg-20231018
```

### Create a general-purpose storage account in your resource group and region
```
az storage account create -n safilevalidator20231018 -l eastus -g rg-20231018 --sku Standard_LRS --allow-blob-public-access false
```

### Create the function app in Azure
```
az functionapp create -g rg-20231018 --consumption-plan-location eastus --runtime dotnet-isolated --functions-version 4 --name func-file-validator-20231018 --storage-account safilevalidator20231018
```

### Deploy the function project to Azure
Execute in the project folder
```
func azure functionapp publish func-file-validator-20231018
```

### Testing

See the section **Using FileValidator.Client** in [Using intaker in a Web Api](/webapi/readme.md)
