die () {
    echo >&2 "$@"
    echo "$ ./az-storage-account-id.sh [storage-account] [container-name]"
    exit 1
}
[ "$#" -eq 2 ] || die "2 argument required, $# provided"
RESOURCE_GROUP_NAME="rg-organics-openhack"

STORAGE_ACCOUNT_NAME=$1
CONTAINER_NAME=$2
STORAGE_ID=$(sh ./az-storage-account-id.sh $STORAGE_ACCOUNT_NAME)

STORAGEID=$(az storage account show --name $STORAGE_ACCOUNT_NAME --resource-group $RESOURCE_GROUP_NAME --query id --output tsv)
echo $STORAGEID'/blobServices/default/containers/'$CONTAINER_NAME



 