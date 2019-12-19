# https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-event-quickstart?toc=%2fazure%2fevent-grid%2ftoc.json
RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"
STORAGE_ACCOUNT_NAME="storganicsopenhackblob"


die () {
    echo >&2 "$@"
    echo "$ ./011.upload-batch-blobs-via-sas.sh [container-name] [sas-token]"
    exit 1
}
echo '$0 = '$0


[ "$#" -eq 2 ] || die "2 argument required, $# provided"
echo '$1 = '$1
echo '$2 = '$2
CONTAINER_NAME=$1
SAS_TOKEN=$2
echo '$SAS_TOKEN = '$SAS_TOKEN
echo '$CONTAINER_NAME = '$CONTAINER_NAME

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
   az storage blob upload \
        -c $CONTAINER_NAME \
        -f $MANIFEST_FILE \
        -n $UNIQUE_ID/$MANIFEST_FILE \
        --account-name $STORAGE_ACCOUNT_NAME \
        --sas-token $SAS_TOKEN \
        --auth-mode key \
        --metadata a=b b=c c=d

   files=( $FILE_A $FILE_B $FILE_C )
   for file in "${files[@]}"
   do
      : 
      # do whatever on $i
      echo $file
      touch $file
      az storage blob upload \
        -c $CONTAINER_NAME \
        -f $file \
        -n $UNIQUE_ID/$file \
        --account-name $STORAGE_ACCOUNT_NAME \
        --sas-token $SAS_TOKEN \
        --auth-mode key \
        --metadata a=b b=c c=d

     
      # Clean up temporary file
      rm -f $file

   done
   rm -f "$MANIFEST_FILE"
fi