echo "Positional Parameters"
echo '$0 = '$0
echo '$1 = '$1
SUBSCRIPTION_NAME=$1 
if [ -z "${SUBSCRIPTION_NAME}" ]; then
    echo "SUBSCRIPTION_NAME is unset or set to the empty string"
    SUBSCRIPTION_NAME="MUSE1-NS01"
fi
echo 'SUBSCRIPTION_NAME = '$SUBSCRIPTION_NAME

SUBSCRIPTION_ID="$(az account show -s $SUBSCRIPTION_NAME --query id -o tsv)"
echo 'SUBSCRIPTION_ID = '$SUBSCRIPTION_ID

az aks install-cli
az login
az account set --subscription $SUBSCRIPTION_ID
echo '----- Current Account -----'
az account show
echo '----- devops -----'
az extension add --name azure-devops
# NOTE: azfun_organics devops project need to be created and setup to github manually for now.
# az devops project create --name azfun_organics2 --org https://dev.azure.com/norton-artficier/    

az devops configure --defaults organization=https://dev.azure.com/norton-artficier project=azfun_organics
az devops service-endpoint create --service-endpoint-configuration azure_resource_manager_service_connection.json


sh ./001.resource-groups.sh
sh ./az-create-keyvault.sh organics
sh ./az-create-app-registration.sh organics
sh ./002.storage-account.sh
sh ./003.create-function.sh
sh ./004.create-cosmosdb.sh
sh ./005.storage-account-blobs.sh

