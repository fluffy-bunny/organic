RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"
STORAGE_ACCOUNT_NAME="storganicsopenhack"
FUNCTION_NAME="azfun-organics"
OS_TYPE="Linux"

az functionapp create --consumption-plan-location $LOCATION --name $FUNCTION_NAME --os-type $OS_TYPE --resource-group $RESOURCE_GROUP_NAME --runtime dotnet --storage-account $STORAGE_ACCOUNT_NAME

