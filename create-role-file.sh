die () {
    echo >&2 "$@"
    echo "$ ./create-role-file.sh [resource-group-name] "
    exit 1
}
[ "$#" -eq 1 ] || die "1 argument required, $# provided"
 
echo ====== BEGIN Creating Roles =================
RESOURCE_GROUP_NAME=$1 #rg-organics-openhack
echo RESOURCE_GROUP_NAME=$RESOURCE_GROUP_NAME

RESOURCE_ID=$(sh ./az-resource-group-id.sh $RESOURCE_GROUP_NAME)
echo RESOURCE_ID=$RESOURCE_ID
UNIQUE_ID=$(env LC_CTYPE=C tr -dc 'a-z0-9' < /dev/urandom | fold -w 10 | head -n 1)

TARGET_DIR=".junk"
### Check if a directory does not exist ###
if [ ! -d $TARGET_DIR ] 
then
   echo "Directory /path/to/dir DOES NOT exists." 
   mkdir $TARGET_DIR
fi
search_dir="Templates"

for template in "$search_dir"/*
do
   echo "$template"
   target="$TARGET_DIR/$UNIQUE_ID.$templateFile"
   cp $template $target
   sed -i "s|{{ASSIGNABLE_SCOPE}}|${RESOURCE_ID}|g" $target
      
   az role definition create --role-definition "$target"
   rm -f $target
done



az role definition list -n custom-blob-storage-reader
az role definition list -n custom-blob-storage-writer

echo ====== END Creating Roles =================