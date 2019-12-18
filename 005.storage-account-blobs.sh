RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"
STORAGE_ACCOUNT_NAME="storganicsopenhackblob"
SKU="Standard_LRS"
KIND="BlobStorage"
SAS_NAME="sasUploadOnly"
CONTAINER_NAME="testcontainer"

az storage account create \
    -n $STORAGE_ACCOUNT_NAME \
    -g $RESOURCE_GROUP_NAME \
    -l $LOCATION \
    --sku $SKU \
    --kind $KIND \
    --access-tier Hot

export AZURE_STORAGE_ACCOUNT=$STORAGE_ACCOUNT_NAME
AZURE_STORAGE_ACCESS_KEY="$(az storage account keys list \
                                --account-name $STORAGE_ACCOUNT_NAME \
                                --resource-group $RESOURCE_GROUP_NAME \
                                --query "[0].value" \
                                --output tsv)"
export AZURE_STORAGE_ACCESS_KEY=$AZURE_STORAGE_ACCESS_KEY
export AZURE_STORAGE_KEY=$AZURE_STORAGE_ACCESS_KEY

echo STORAGE_ACCOUNT_NAME: $STORAGE_ACCOUNT_NAME
echo AZURE_STORAGE_ACCESS_KEY: $AZURE_STORAGE_ACCESS_KEY

az storage container create \
    -n $CONTAINER_NAME \
    --auth-mode login \
    --account-name $STORAGE_ACCOUNT_NAME \
    --account-key $AZURE_STORAGE_ACCESS_KEY

