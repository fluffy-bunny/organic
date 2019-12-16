# https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-event-quickstart?toc=%2fazure%2fevent-grid%2ftoc.json
RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"
STORAGE_ACCOUNT_NAME="storganicsopenhackblob"
EVENT_SUBSCRIPTION_NAME="evtsub-blob-organics"
# Generate a unique suffix for the service name
let randomNum=$RANDOM*$RANDOM
# SITENAME from previous deployment
SITENAME="eventviewer-69940325"
echo $SITENAME

STORAGEID=$(az storage account show --name $STORAGE_ACCOUNT_NAME --resource-group $RESOURCE_GROUP_NAME --query id --output tsv)
echo STORAGEID: $STORAGEID

ENDPOINT=https://$SITENAME.azurewebsites.net/api/updates
echo ENDPOINT: $ENDPOINT

az eventgrid event-subscription create \
  --resource-id $STORAGEID \
  --name $EVENT_SUBSCRIPTION_NAME \
  --endpoint $ENDPOINT