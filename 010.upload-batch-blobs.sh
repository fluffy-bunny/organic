# https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-event-quickstart?toc=%2fazure%2fevent-grid%2ftoc.json
RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"
STORAGE_ACCOUNT_NAME="storganicsopenhackblob"

export AZURE_STORAGE_ACCOUNT=$STORAGE_ACCOUNT_NAME
echo STORAGE_ACCOUNT_NAME: $STORAGE_ACCOUNT_NAME
AZURE_STORAGE_ACCESS_KEY="$(az storage account keys list --account-name $STORAGE_ACCOUNT_NAME --resource-group $RESOURCE_GROUP_NAME --query "[0].value" --output tsv)"
echo AZURE_STORAGE_ACCESS_KEY: $AZURE_STORAGE_ACCESS_KEY
export AZURE_STORAGE_ACCESS_KEY=$AZURE_STORAGE_ACCESS_KEY

az storage container create --name testcontainer

# Generate a unique suffix for the service name
let randomNum=$RANDOM*$RANDOM
# Generate a unique sitename
SITENAME=eventviewer-$randomNum

FILE_A=$randomNum.A.txt
FILE_B=$randomNum.B.txt
FILE_C=$randomNum.C.txt

files=( $FILE_A $FILE_B $FILE_C )
for file in "${files[@]}"
do
   : 
   # do whatever on $i
   echo $file
   touch $file
   az storage blob upload --file $file --container-name testcontainer --name $file

   # Clean up temporary file
   rm -f $file

done

 