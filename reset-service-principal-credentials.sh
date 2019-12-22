APP_FRIENDLY_NAME="organics"
LOCATION="eastus2"
RESOURCE_GROUP_NAME="rg-$APP_FRIENDLY_NAME-openhack"
STORAGE_ACCOUNT_NAME="st"$APP_FRIENDLY_NAME"openhack"
FUNCTION_NAME="azfun-$APP_FRIENDLY_NAME"
APP_REGISTRATION="appreg-$APP_FRIENDLY_NAME"
KV_NAME="kv-"$APP_FRIENDLY_NAME

die () {
    echo >&2 "$@"
    echo "$ ./reset-service-principal-credentials.sh [service-principal-name]"
    exit 1
}
REQUIRED_ARGS=1
[ "$#" -eq $REQUIRED_ARGS ] || die "$REQUIRED_ARGS argument required, $# provided"

 
SP_NAME=$1
CREDENTIALS=$(az ad sp credential reset --name $SP_NAME)
echo $CREDENTIALS

echo $CREDENTIALS|jq '.appId'
echo $CREDENTIALS|jq '.name'
echo $CREDENTIALS|jq '.password'
echo $CREDENTIALS|jq '.tenant'
echo "....."

appId=$( echo "$CREDENTIALS" | jq '.["appId"]' )
name=$( echo "$CREDENTIALS" | jq '.["name"]' )
password=$( echo "$CREDENTIALS" | jq '.["password"]' )
tenant=$( echo "$CREDENTIALS" | jq '.["tenant"]' )

appId=$( echo "$appId" | tr -d '"')
name=$( echo "$name" | tr -d '"')
password=$( echo "$password" | tr -d '"')
tenant=$( echo "$tenant" | tr -d '"')



echo "appId:$appId"
echo "name:$name"
echo "password:$password"
echo "tenant:$tenant"

sh ./set-keyvault-secret.sh $KV_NAME $FUNCTION_NAME "storage-sp-tenant-id" $tenant
sh ./set-keyvault-secret.sh $KV_NAME $FUNCTION_NAME "storage-sp-client-id" $appId
sh ./set-keyvault-secret.sh $KV_NAME $FUNCTION_NAME "storage-sp-client-secret" $password