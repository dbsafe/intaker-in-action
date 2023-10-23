# Using intaker in a Blazor application

### Dependencies
- .NET 6.0
- Blazor
- Intaker - https://github.com/dbsafe/intaker
- MatBlazor - https://www.matblazor.com/
- Tabulator - http://tabulator.info/
- Ace Editor - https://ace.c9.io/

## Deploying Blazor WebAssembly application to an Azure Bob Storage as Static Site Hosting using Azure CLI

For more information:

[Host a Static Website in Azure in 60 Seconds](https://www.youtube.com/watch?v=ttmEOLKk3Cw)

**Publishing application and dependencies to a folder for deployment**

Must be executed in the project folder `\FileValidator.Blazor`. Creates the folder `FileValidator.Blazor\bin\Release\net6.0\publish`
```
dotnet publish -c Release
```

**Creating a resource group in Azure**
```
az group create -l <region> -n <resource-group-name>
az group create -l eastus -n rg-20231023
```

**Create a Storage Account**
```
az storage account create -n sa20231023 -g rg-20231023 -l eastus --sku Standard_LRS --kind StorageV2 --allow-blob-public-access false
```

**Update the service properties for the account to enable static website**

This creates the `$web` container
```
az storage blob service-properties update --account-name sa20231023 --static-website --index-document index.html --404-document error.html
```

**Upload static files to the `$web` container**

Must be executed in the publish folder `FileValidator.Blazor\bin\Release\net6.0\publish\wwwroot`

```
az storage blob upload-batch -s . -d '$web' --account-name sa20231023
```

**Retrieve endpoint to the website**
```
az storage account show -n sa20231023 -g rg-20231023 --query 'primaryEndpoints.web' --output tsv
```

## Deploying Blazor WebAssembly application as an Azure WebApp using Azure CLI

Blazor WebAssembly applications does not perform server side rendering. Azure WebApp is not the best choice for deploying Blazor WebAssembly applications.

For more information:

[Quickstart: Deploy an ASP.NET web app](https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore?tabs=net70&pivots=development-environment-cli)

[Azure CLI - az webapp up](https://learn.microsoft.com/en-us/cli/azure/webapp?view=azure-cli-latest#az-webapp-up())

**Publishing application and dependencies to a folder for deployment**

Must be executed in the project folder `\FileValidator.Blazor`. Creates the folder `FileValidator.Blazor\bin\Release\net6.0\publish`
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
