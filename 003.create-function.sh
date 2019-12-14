RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"
STORAGE_ACCOUNT_NAME="storganicsopenhack"
FUNCTION_NAME="azfun-organics"

az functionapp create -g $RESOURCE_GROUP_NAME --consumption-plan-location $LOCATION --name $FUNCTION_NAME --runtime dotnet --storage-account $STORAGE_ACCOUNT_NAME

