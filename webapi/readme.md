# Using intaker in a Web Api

## Deploying Web Api as an Azure WebApp using Azure CLI

For more information [Azure CLI - az webapp up](https://learn.microsoft.com/en-us/cli/azure/webapp?view=azure-cli-latest#az-webapp-up())

### Creating a resource group in Azure
```
az group create -l <region> -n <resource-group-name>
az group create -l eastus -n rg-20231009
```

### Creating/redeploying a WebApp

Must be executed in the solution folder.

This command will create a service plan the first time. Use `-p` to add the new WebApp to an existent app service plan.
`<app-service-name>` must be unique within Azure since the name is part of the domain name `<app-service-name>.azurewebsites.net`

```
az webapp up -g <resource-group-name> -n <app-service-name>
az webapp up -g rg-20231009 -n web-app-20231009
```

### Creating a deployment slot
```
az webapp deployment slot create -g <resource-group-name> -n <app-service-name> -s <slot-name>
az webapp deployment slot create -g rg-20231017 -n web-app-20231017 -s staging
```

### Deploy a new version to the staging slot

Notice how `az webapp up` creates the webapp, builds (somewhere), and deploys the application in one command.
Deploying to a slot is not done in one command :-(

Build the WebApi project. Execute in the project folder. An advantage of using this command is that we can set the assembly version as part of the build.

```
// dotnet publish -c Release -o build2
dotnet publish -c Release -o build-1.0.0.5 /p:Version=1.0.0.5
```

Zip the files (PowerShell)
```
cd build-1.0.0.5
Compress-Archive * build-1.0.0.5.zip
```

Deploy to the staging slot
```
az webapp deployment source config-zip -g rg-20231017 -n web-app-20231017 --src build-1.0.0.5.zip --slot staging
```

### Swap Staging and Production Slots
```
az webapp deployment slot swap -g rg-20231017 -n web-app-20231017 --slot staging --target-slot production
```


## Client Application

### Using FileValidator.Client

Project `FileValidator.Client` is a console app that sends request to the Web Api.
It can be used to send multiple requests to test autoscaling.

### Using CURL
Execute in folder `webapi\FileValidator.Client\sample-files`
```
curl -X POST -H "accept: */*" -H "Content-Type: multipart/form-data" -F "files=@Balance10.xml;type=text/xml" -F "files=@single-data-type.v10.csv;type=text/csv" "https://localhost:7001/api/file-validator/validate"

curl -X POST -H "accept: */*" -H "Content-Type: multipart/form-data" -F "files=@Balance10.xml;type=text/xml" -F "files=@single-data-type.v10.csv;type=text/csv" "http://localhost:7000/api/file-validator/validate"

curl -X POST -H "accept: */*" -H "Content-Type: multipart/form-data" -F "files=@Balance10.xml;type=text/xml" -F "files=@single-data-type.v10.csv;type=text/csv" "https://func-file-validator-20231018.azurewebsites.net/api/file-validator/validate"
```