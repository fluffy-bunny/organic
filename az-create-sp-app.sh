STORAGE_ACCOUNT_NAME="storganicsopenhackblob"
die () {
    echo >&2 "$@"
    echo "$ ./az-create-sp-app.sh [app-name]"
    exit 1
}
[ "$#" -eq 1 ] || die "1 argument required, $# provided"

echo "Positional Parameters"
echo '$0 = '$0
echo '$1 = '$1
APP_NAME=$1 

# sudo apt install pwgen
# SERVICE_PRINCIPAL_PASSWORD=$(pwgen -N 1 -B 32)
# create an Azure AD app
# echo 'SERVICE_PRINCIPAL_PASSWORD = '$SERVICE_PRINCIPAL_PASSWORD

#az ad app create \
#   --display-name $APP_NAME 
# get the app id
#APP_ID=$(az ad app list --display-name $APP_NAME --query [].appId -o tsv)

STORAGE_ID=$(sh ./az-storage-account-id.sh $STORAGE_ACCOUNT_NAME)
echo 'STORAGE_ID = '$STORAGE_ID
# create a service principal
# az ad sp create-for-rbac --name $APP_ID --password $SERVICE_PRINCIPAL_PASSWORD 
az ad sp create-for-rbac --name $APP_NAME \
                --role contributor \
                --scopes $STORAGE_ID

# get the app id of the service principal
servicePrincipalAppId=$(az ad sp list --display-name $APP_NAME --query "[].appId" -o tsv)
echo 'servicePrincipalAppId = '$servicePrincipalAppId