die () {
    echo >&2 "$@"
    echo "$ ./az-resource-group-id.sh [resource-group]"
    exit 1
}
[ "$#" -eq 1 ] || die "1 argument required, $# provided"
RESOURCE_GROUP_NAME=$1


RESOURCE_GROUP_ID=$(az group show --name $RESOURCE_GROUP_NAME --query id --output tsv)
echo $RESOURCE_GROUP_ID



 