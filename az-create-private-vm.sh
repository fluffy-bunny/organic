die () {
    echo >&2 "$@"
    echo "$ ./az-create-vnet.sh [friendly-name]"
    exit 1
}
REQUIRED_ARGS=3
[ "$#" -eq $REQUIRED_ARGS ] || die "$REQUIRED_ARGS argument required, $# provided"

sudo apt update 
sudo apt install diceware

ADMIN_PASSWORD=`diceware -n 3 -d @`!
APP_FRIENDLY_NAME=$1
VNET_NAME="vnet-$APP_FRIENDLY_NAME"
SNET_NAME="snet-$APP_FRIENDLY_NAME"
LOCATION=$2
RESOURCE_GROUP_NAME=$3
COMPUTER_NAME=`diceware -n 2`
VM_NAME="vm-private-$APP_FRIENDLY_NAME"
IMAGE_URN="MicrosoftWindowsDesktop:Windows-10:rs5-pro:17763.864.1911120152"

STORAGE_ACCOUNT_NAME="st"$APP_FRIENDLY_NAME"private"

echo "APP_FRIENDLY_NAME $APP_FRIENDLY_NAME"
echo "VNET_NAME $VNET_NAME"
echo "SNET_NAME $SNET_NAME"
echo "LOCATION $LOCATION"
echo "RESOURCE_GROUP_NAME $RESOURCE_GROUP_NAME"
echo "VM_NAME $VM_NAME"
echo "STORAGE_ACCOUNT_NAME $STORAGE_ACCOUNT_NAME"
echo "COMPUTER_NAME $COMPUTER_NAME"
ADMIN_USERNAME="ghstahl"
echo "---admin-credentials--------------------------------------"
echo "  $ADMIN_USERNAME"
echo "  $ADMIN_PASSWORD"
echo "---admin-credentials--------------------------------------"

az vm create \
    --resource-group $RESOURCE_GROUP_NAME \
    --location $LOCATION \
    --name $VM_NAME \
    --image $IMAGE_URN \
    --admin-password $ADMIN_PASSWORD \
    --admin-username $ADMIN_USERNAME \
    --computer-name $COMPUTER_NAME \
    --vnet-name $VNET_NAME \
    --subnet $SNET_NAME 



