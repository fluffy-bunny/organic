RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"

echo "==== Creating Resource Group: $RESOURCE_GROUP_NAME in Location: $LOCATION"
az group create --name $RESOURCE_GROUP_NAME --location $LOCATION
