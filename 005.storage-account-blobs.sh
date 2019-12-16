RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"
STORAGE_ACCOUNT_NAME="storganicsopenhackblob"
SKU="Standard_LRS"
KIND="BlobStorage"

az storage account create -n $STORAGE_ACCOUNT_NAME -g $RESOURCE_GROUP_NAME -l $LOCATION --sku $SKU --kind $KIND --access-tier Hot
