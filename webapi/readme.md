# Using intaker in a Web Api

## Deploying Web Api as an Azure WebApp using Azure CLI

For more information [Azure CLI - az webapp up](https://learn.microsoft.com/en-us/cli/azure/webapp?view=azure-cli-latest#az-webapp-up())

**Creating a resource group in Azure**
```
az group create -l <region> -n <resource-group-name>
az group create -l eastus -n rg-20231009
```

**Creating/redeploying a WebApp**

Must be executed in the solution folder.0\publish`

This command will create a service plan the first time. Use `-p` to add the new WebApp to an existent app service plan.
`<app-service-name>` must be unique within Azure since the name is part of the domain name `<app-service-name>.azurewebsites.net`

```
az webapp up -g <resource-group-name> -n <app-service-name>
az webapp up -g rg-20231009 -n web-app-20231009
```

**Client Application**

Project FileValidator.Client is a console app that sends request to the Web Api.
It can be used to send multiple requests to test autoscaling.
