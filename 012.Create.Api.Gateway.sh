# REFERENCES
# https://markheath.net/post/managed-identity-key-vault-azure-functions
#
APP_FRIENDLY_NAME="organics"
LOCATION="eastus2"
RESOURCE_GROUP_NAME="rg-$APP_FRIENDLY_NAME-openhack"
SKU="Developer"
API_MANAGEMENT_NAME="apim-"$APP_FRIENDLY_NAME

az apim create \
    --name $API_MANAGEMENT_NAME \
    -g $RESOURCE_GROUP_NAME \
    -l $LOCATION \
    --sku-name $SKU \
    --publisher-email ghstahl@gmail.com \
    --publisher-name "Fluffy Bunny"