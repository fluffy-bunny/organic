RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"
STORAGE_ACCOUNT_NAME="storganicsopenhack"
SKU="Standard_LRS"

az storage account create -n $STORAGE_ACCOUNT_NAME -g $RESOURCE_GROUP_NAME -l $LOCATION --sku $SKU