APP_FRIENDLY_NAME="organics"
LOCATION="eastus2"
RESOURCE_GROUP_NAME="rg-$APP_FRIENDLY_NAME-openhack"
 
die () {
    echo >&2 "$@"
    echo "$ ./[shell-script].sh [key-vault-name] [func-name] [secret-name] [secret-value]"
    exit 1
}
REQUIRED_ARGS=4
[ "$#" -eq $REQUIRED_ARGS ] || die "$REQUIRED_ARGS argument required, $# provided"

KV_NAME=$1
FUNCTION_NAME=$2
SECRET_NAME=$3
SECRET_VALUE=$4

az keyvault secret set -n $SECRET_NAME --vault-name $KV_NAME --value "$SECRET_VALUE"

# view the secret
az keyvault secret show -n $SECRET_NAME --vault-name $KV_NAME

SECRET_ID=$(az keyvault secret show -n $SECRET_NAME --vault-name $KV_NAME --query "id" -o tsv)
echo "SECRET_ID: $SECRET_ID"

az functionapp config appsettings set \
    -n $FUNCTION_NAME \
    -g $RESOURCE_GROUP_NAME \
    --settings "$SECRET_NAME=@Microsoft.KeyVault(SecretUri=$SECRET_ID)"
