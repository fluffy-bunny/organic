# https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-event-quickstart?toc=%2fazure%2fevent-grid%2ftoc.json
RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"

# Generate a unique suffix for the service name
let randomNum=$RANDOM*$RANDOM
# Generate a unique sitename
SITENAME=eventviewer-$randomNum
 
echo $SITENAME

az group deployment create \
  --resource-group $RESOURCE_GROUP_NAME \
  --template-uri "https://raw.githubusercontent.com/Azure-Samples/azure-event-grid-viewer/master/azuredeploy.json" \
  --parameters siteName=$SITENAME hostingPlanName=viewerhost