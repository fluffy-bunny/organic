# https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-event-quickstart?toc=%2fazure%2fevent-grid%2ftoc.json
RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"
STORAGE_ACCOUNT_NAME="storganicsopenhackblob"

export AZURE_STORAGE_ACCOUNT=$STORAGE_ACCOUNT_NAME
echo STORAGE_ACCOUNT_NAME: $STORAGE_ACCOUNT_NAME
AZURE_STORAGE_ACCESS_KEY="$(az storage account keys list --account-name $STORAGE_ACCOUNT_NAME --resource-group $RESOURCE_GROUP_NAME --query "[0].value" --output tsv)"
echo AZURE_STORAGE_ACCESS_KEY: $AZURE_STORAGE_ACCESS_KEY
export AZURE_STORAGE_ACCESS_KEY=$AZURE_STORAGE_ACCESS_KEY

az storage remove --container-name testcontainer --name testfile.txt

