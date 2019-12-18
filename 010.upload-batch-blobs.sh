# https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-event-quickstart?toc=%2fazure%2fevent-grid%2ftoc.json
RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"
STORAGE_ACCOUNT_NAME="storganicsopenhackblob"
CONTAINER_NAME="testcontainer"

export AZURE_STORAGE_ACCOUNT=$STORAGE_ACCOUNT_NAME
echo STORAGE_ACCOUNT_NAME: $STORAGE_ACCOUNT_NAME
AZURE_STORAGE_ACCESS_KEY="$(az storage account keys list --account-name $STORAGE_ACCOUNT_NAME \
                                                         --resource-group $RESOURCE_GROUP_NAME \
                                                         --query "[0].value" --output tsv)"
echo AZURE_STORAGE_ACCESS_KEY: $AZURE_STORAGE_ACCESS_KEY
export AZURE_STORAGE_ACCESS_KEY=$AZURE_STORAGE_ACCESS_KEY

# az storage container create --name $CONTAINER_NAME

# Generate a unique suffix for the service name
# let randomNum=$RANDOM*$RANDOM
UNIQUE_ID=$(env LC_CTYPE=C tr -dc 'a-z0-9' < /dev/urandom | fold -w 10 | head -n 1)


FILE_A=$UNIQUE_ID.A.txt
FILE_B=$UNIQUE_ID.B.txt
FILE_C=$UNIQUE_ID.C.txt

 # Define the index policy for the container, include spatial and composite indexes
JSON_STRING=$( jq -n \
                  --arg fa "$UNIQUE_ID/$FILE_A" \
                  --arg fb "$UNIQUE_ID/$FILE_B" \
                  --arg fc "$UNIQUE_ID/$FILE_C" \
                  '{files: [$fa,$fb,$fc]}' )



# Persist index policy to json file
MANIFEST_FILE=$UNIQUE_ID.manifest.json 
echo $JSON_STRING
echo "$JSON_STRING" > "$MANIFEST_FILE"
echo $MANIFEST_FILE

if [ 1 -eq 1 ]; then
   az storage blob upload  --file $MANIFEST_FILE \
                           --container-name $CONTAINER_NAME \
                           --name $UNIQUE_ID/$MANIFEST_FILE \
                           --metadata a=b b=c c=d

   files=( $FILE_A $FILE_B $FILE_C )
   for file in "${files[@]}"
   do
      : 
      # do whatever on $i
      echo $file
      touch $file
      az storage blob upload  --file $file \
                              --container-name $CONTAINER_NAME \
                              --name $UNIQUE_ID/$file \
                              --metadata a=b b=c c=d

      # Clean up temporary file
      rm -f $file

   done
   rm -f "$MANIFEST_FILE"
fi