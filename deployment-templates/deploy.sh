APP_FRIENDLY_NAME="gmo"
LOCATION="eastus2"
RESOURCE_GROUP_NAME="rg-$APP_FRIENDLY_NAME"


echo "==== Creating Resource Group: $RESOURCE_GROUP_NAME in Location: $LOCATION"
az group create --name $RESOURCE_GROUP_NAME --location $LOCATION

templateFile="./azuredeploy.json"
DEPLOYMENT_NAME="blanktemplate"
az group deployment create \
  --name $DEPLOYMENT_NAME \
  --resource-group $RESOURCE_GROUP_NAME \
  --template-file $templateFile

templateFile="./storage-account-eventhub.json"
DEPLOYMENT_NAME="st-eventhub"
az group deployment create \
  --name $DEPLOYMENT_NAME \
  --resource-group $RESOURCE_GROUP_NAME \
  --template-file $templateFile



