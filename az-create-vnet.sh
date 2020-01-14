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

echo "APP_FRIENDLY_NAME $APP_FRIENDLY_NAME"
echo "VNET_NAME $VNET_NAME"
echo "SNET_NAME $SNET_NAME"
echo "LOCATION $LOCATION"
echo "RESOURCE_GROUP_NAME $RESOURCE_GROUP_NAME"


az network vnet create \
    --name $VNET_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --address-prefix 10.0.0.0/16 \
    --subnet-name $SNET_NAME \
    --subnet-prefix 10.0.0.0/24 

