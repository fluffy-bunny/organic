APP_FRIENDLY_NAME="organics"
LOCATION="eastus2"
RESOURCE_GROUP_NAME="rg-$APP_FRIENDLY_NAME-openhack"
STORAGE_ACCOUNT_NAME="st"$APP_FRIENDLY_NAME"openhack"
SKU="Standard_LRS"
echo "=== Create Storage Account: $STORAGE_ACCOUNT_NAME in ResourceGroup $RESOURCE_GROUP_NAME at Location $LOCATION ==="
az storage account create -n $STORAGE_ACCOUNT_NAME -g $RESOURCE_GROUP_NAME -l $LOCATION --sku $SKU