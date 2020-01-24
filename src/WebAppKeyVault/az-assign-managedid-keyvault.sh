az login
az webapp identity assign --name app-WebAppKeyVault  --resource-group  rg-organics-openhack
principalId=$(az webapp show -n app-WebAppKeyVault -g rg-organics-openhack --query 'identity.principalId' -o json)
az keyvault set-policy --name kv-organics --object-id $principalId --secret-permissions get list   