die () {
    echo >&2 "$@"
    echo "$ ./create-role-file.sh [subscription-id] "
    exit 1
}
[ "$#" -eq 1 ] || die "1 argument required, $# provided"
 
RESOURCE_GROUP_NAME=$1 #rg-organics-openhack

RESOURCE_ID=$(sh ./az-resource-group-id.sh $RESOURCE_GROUP_NAME)
echo RESOURCE_ID=$RESOURCE_ID
UNIQUE_ID=$(env LC_CTYPE=C tr -dc 'a-z0-9' < /dev/urandom | fold -w 10 | head -n 1)

TARGET_DIR=".junk"
mkdir $TARGET_DIR

files=( "storage-reader-role-definition.json" "storage-writer-role-definition.json" )

for file in "${files[@]}"
   do
      : 
      # do whatever on $i
      echo $file
      template="Templates/$file"
      target="$TARGET_DIR/$UNIQUE_ID.$file"
      cp $template $target
      sed -i "s|{{ASSIGNABLE_SCOPE}}|${RESOURCE_ID}|g" $target
      
      az role definition create --role-definition "$target"
      rm -f $target
   done


az role definition list -n custom-blob-storage-reader
az role definition list -n custom-blob-storage-writer