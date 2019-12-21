# REFERENCES
# https://markheath.net/post/managed-identity-key-vault-azure-functions
#
RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"
STORAGE_ACCOUNT_NAME="storganicsopenhack"

APP_FRIENDLY_NAME="organics"
FUNCTION_NAME="azfun-$APP_FRIENDLY_NAME"
APP_REGISTRATION="appreg-$APP_FRIENDLY_NAME"
KV_NAME="kv-"$APP_FRIENDLY_NAME


echo "=== Creating Function: $FUNCTION_NAME in ResourceGroup: $RESOURCE_GROUP_NAME at Location: $LOCATION ==="
az functionapp create -g $RESOURCE_GROUP_NAME --consumption-plan-location $LOCATION --name $FUNCTION_NAME --runtime dotnet --storage-account $STORAGE_ACCOUNT_NAME
az functionapp identity assign -g $RESOURCE_GROUP_NAME --name $FUNCTION_NAME

PRINCIPAL_ID=$(az functionapp identity show -n $FUNCTION_NAME -g $RESOURCE_GROUP_NAME --query principalId -o tsv)
TENANT_ID=$(az functionapp identity show -n $FUNCTION_NAME -g $RESOURCE_GROUP_NAME --query tenantId -o tsv)

echo "=== System Identity for: $FUNCTION_NAME PrincipalId: $PRINCIPAL_ID in Tenant: $TENANT_ID ==="

az keyvault set-policy -n $KV_NAME -g $RESOURCE_GROUP_NAME --object-id $PRINCIPAL_ID --secret-permissions get
# to see the access policies added:
az keyvault show -n $KV_NAME -g $RESOURCE_GROUP_NAME --query "properties.accessPolicies[?objectId == '$PRINCIPAL_ID']"

# Save a secret in the key vault
SECRET_NAME="secretOrganics"
az keyvault secret set -n $SECRET_NAME --vault-name $KV_NAME --value "Super secret value!"

# view the secret
az keyvault secret show -n $SECRET_NAME --vault-name $KV_NAME

SECRET_ID=$(az keyvault secret show -n $SECRET_NAME --vault-name $KV_NAME --query "id" -o tsv)
echo "SECRET_ID: $SECRET_ID"

az functionapp config appsettings set \
    -n $FUNCTION_NAME \
    -g $RESOURCE_GROUP_NAME \
    --settings "Secret1=@Microsoft.KeyVault(SecretUri=$SECRET_ID)"