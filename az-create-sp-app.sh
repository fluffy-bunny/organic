die () {
    echo >&2 "$@"
    echo "$ ./az-create-sp-app.sh [sp-name] [storage-account] [container-name]"
    exit 1
}
[ "$#" -eq 3 ] || die "3 argument required, $# provided"
RESOURCE_GROUP_NAME="rg-organics-openhack"
SP_NAME=$1
STORAGE_ACCOUNT_NAME=$2
CONTAINER_NAME=$3

echo ====== Creating Service Principal =================
STORAGE_ACCOUNT_ID=$(sh ./az-storage-account-id.sh $STORAGE_ACCOUNT_NAME)
CONTAINER_ID=$(sh ./az-storage-account-container-id.sh $STORAGE_ACCOUNT_NAME $CONTAINER_NAME)
echo 'SP_NAME = '$SP_NAME
echo 'STORAGE_ACCOUNT_NAME = '$STORAGE_ACCOUNT_NAME
echo 'STORAGE_ACCOUNT_ID = '$STORAGE_ACCOUNT_ID
echo 'CONTAINER_NAME = '$CONTAINER_NAME
echo 'CONTAINER_ID = '$CONTAINER_ID
# create a service principal
 
az ad sp create-for-rbac --name $SP_NAME \
                --role contributor \
                --scopes $CONTAINER_ID

# get the app id of the service principal

servicePrincipalAppId=$(sh ./az-service-principal-id.sh $SP_NAME)
echo "servicePrincipalAppId ="$servicePrincipalAppId

#servicePrincipalAppId=$(az ad sp list --display-name $SP_NAME --query "[].appId" -o tsv)

(sh ./create-role-file.sh $RESOURCE_GROUP_NAME)

az role assignment create \
    --role contributor \
    --assignee $servicePrincipalAppId \
    --scope $STORAGE_ACCOUNT_ID

az role assignment create \
    --role "Storage Blob Data Contributor" \
    --assignee $servicePrincipalAppId \
    --scope $STORAGE_ACCOUNT_ID
 
az role assignment create \
    --role "custom-blob-storage-writer" \
    --assignee $servicePrincipalAppId \
    --scope $CONTAINER_ID

az role assignment create \
    --role "custom-blob-storage-reader" \
    --assignee $servicePrincipalAppId \
    --scope $CONTAINER_ID

az role assignment create \
    --role "Storage Blob Data Contributor" \
    --assignee $servicePrincipalAppId \
    --scope $CONTAINER_ID
    
echo 'servicePrincipalAppId = '$servicePrincipalAppId
az role assignment list --assignee $servicePrincipalAppId --all