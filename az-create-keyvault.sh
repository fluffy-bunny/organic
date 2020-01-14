die () {
    echo >&2 "$@"
    echo "$ ./az-create-keyvault.sh [name]"
    exit 1
}
REQUIRED_ARGS=1
[ "$#" -eq $REQUIRED_ARGS ] || die "$REQUIRED_ARGS argument required, $# provided"

RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"

KV_NAME="kv-"$1
echo "====== Creating KEY VAULT:  $KV_NAME ================="
az keyvault create --location $LOCATION --name $KV_NAME --resource-group $RESOURCE_GROUP_NAME

SECRET_NAME="MySecret"
VALUE="my secret"
az keyvault secret set -n $SECRET_NAME --vault-name $KV_NAME --value "$VALUE"

# view the secret
az keyvault secret show -n $SECRET_NAME --vault-name $KV_NAME