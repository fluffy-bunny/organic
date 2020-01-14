die () {
    echo >&2 "$@"
    echo "$ ./az-create-vnet.sh [friendly-name]"
    exit 1
}
REQUIRED_ARGS=3
[ "$#" -eq $REQUIRED_ARGS ] || die "$REQUIRED_ARGS argument required, $# provided"


APP_FRIENDLY_NAME=$1
VNET_NAME="vnet-$APP_FRIENDLY_NAME"
SNET_NAME="snet-$APP_FRIENDLY_NAME"
LOCATION=$2
RESOURCE_GROUP_NAME=$3
 
STORAGE_ACCOUNT_NAME="st"$APP_FRIENDLY_NAME"private"

SKU="Standard_LRS"
KIND="StorageV2"
SAS_NAME="sasUploadOnly"
CONTAINER_NAME="testcontainer"


echo "APP_FRIENDLY_NAME $APP_FRIENDLY_NAME"
echo "VNET_NAME $VNET_NAME"
echo "SNET_NAME $SNET_NAME"
echo "LOCATION $LOCATION"
echo "RESOURCE_GROUP_NAME $RESOURCE_GROUP_NAME"
echo "STORAGE_ACCOUNT_NAME $STORAGE_ACCOUNT_NAME"
echo "SKU $SKU"
echo "KIND $KIND"

if [ 1 -eq 1 ]; then
az storage account create \
    -n $STORAGE_ACCOUNT_NAME \
    -g $RESOURCE_GROUP_NAME \
    -l $LOCATION \
    --sku $SKU \
    --kind $KIND \
    --access-tier Hot

 az storage account network-rule list \
    -g $RESOURCE_GROUP_NAME \
    --account-name $STORAGE_ACCOUNT_NAME \
    --query virtualNetworkRules

az network vnet subnet update \
    -g $RESOURCE_GROUP_NAME \
    --vnet-name $VNET_NAME \
    --name $SNET_NAME \
    --service-endpoints "Microsoft.Storage"  

fi

az storage account update \
     -g $RESOURCE_GROUP_NAME \
     --name $STORAGE_ACCOUNT_NAME \
     --default-action Deny

subnetid=`az network vnet subnet show \
    -g $RESOURCE_GROUP_NAME \
    --vnet-name $VNET_NAME \
    --name $SNET_NAME \
    --query id \
    -o tsv`

 
echo "subnetid: $subnetid"
az storage account network-rule add \
    --account-name $STORAGE_ACCOUNT_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --vnet $VNET_NAME \
    --subnet $SNET_NAME
