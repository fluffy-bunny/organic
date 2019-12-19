die () {
    echo >&2 "$@"
    echo "$ ./az-create-sp-app.sh [sp-name] [storage-account] [container-name]"
    exit 1
}
[ "$#" -eq 3 ] || die "3 argument required, $# provided"
RESOURCE_GROUP_NAME="rg-organics-openhack"
APP_NAME=$1
STORAGE_ACCOUNT_NAME=$2
CONTAINER_NAME=$3

CONTAINER_ID=$(sh ./az-storage-account-container-id.sh $STORAGE_ACCOUNT_NAME $CONTAINER_NAME)
echo 'CONTAINER_ID = '$CONTAINER_ID
# create a service principal
# az ad sp create-for-rbac --name $APP_ID --password $SERVICE_PRINCIPAL_PASSWORD 
az ad sp create-for-rbac --name $APP_NAME \
                --role contributor \
                --scopes $CONTAINER_ID

# get the app id of the service principal
servicePrincipalAppId=$(az ad sp list --display-name $APP_NAME --query "[].appId" -o tsv)

az role assignment create \
    --role "Storage Blob Data Contributor" \
    --assignee $servicePrincipalAppId \
    --scope $CONTAINER_ID
    
echo 'servicePrincipalAppId = '$servicePrincipalAppId
az role assignment list --assignee $servicePrincipalAppId --all