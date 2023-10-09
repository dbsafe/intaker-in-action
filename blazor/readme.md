# Using intaker in a Blazor application

### Dependencies
- .NET 6.0
- Blazor
- Intaker - https://github.com/dbsafe/intaker
- MatBlazor - https://www.matblazor.com/
- Tabulator - http://tabulator.info/
- Ace Editor - https://ace.c9.io/

## Deploying Blazor WebAssembly as a Azure WebApp using Azure CLI

For more information [Azure CLI - az webapp up](https://learn.microsoft.com/en-us/cli/azure/webapp?view=azure-cli-latest#az-webapp-up())

**Publishing application and dependencies to a folder for deployment**

Must be executed in the project folder `\FileValidator.Blazor`. Creates the folder ``FileValidator.Blazor\bin\Release\net6.0\publish``
```
dotnet publish -c Release
```

**Creating a resource group in Azure**
```
az group create -l <region> -n <resource-group-name>
az group create -l eastus -n rg-20231009
```

**Creating/redeploying a WebApp as static HTML app**

Must be executed in the publish folder `FileValidator.Blazor\bin\Release\net6.0\publish`

This command will create a service plan the first time. Use `-p` to add the new WebApp to an existent app service plan.
`<app-service-name>` must be unique within Azure since the name is part of the domain name `<app-service-name>.azurewebsites.net`

```
az webapp up -g <resource-group-name> -n <app-service-name> --html
az webapp up -g rg-20231009 -n web-app-20231009 --html
```
