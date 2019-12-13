RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"

echo "Positional Parameters"
echo '$0 = '$0
echo '$1 = '$1
subscription_id=$1 
echo 'subscription_id = '$subscription_id

az aks install-cli
az login
az account set --subscription $subscription_id
echo '----- Current Account -----'
az account show
 