#  az login --service-principal --username $servicePrincipalAppId --password $password --tenant $tenant 
# az storage container generate-sas -n testcontainer --account-name storganicsopenhackblob --https-only --permissions dlrw --expiry $end -o tsv  

STORAGE_ACCOUNT_NAME="storganicsopenhackblob"
PERMISSIONS="acdlrw"

die () {
    echo >&2 "$@"
    echo "$ ./generate-sas-via-sp.sh [servicePrincipal-id] [password] [tenant] [storage-account-name] [container-name]"
    exit 1
}
echo '$0 = '$0

REQUIRED_ARGS=5
[ "$#" -eq $REQUIRED_ARGS ] || die "$REQUIRED_ARGS argument required, $# provided"

echo '$1 = '$1
echo '$2 = '$2
echo '$3 = '$3
echo '$4 = '$4
echo '$5 = '$5

SERVICE_PRINCIPAL_ID=$1
PASSWORD=$2
TENANT=$3
STORAGE_ACCOUNT_NAME=$4
CONTAINER_NAME=$5

echo '$SERVICE_PRINCIPAL_ID = '$SERVICE_PRINCIPAL_ID
echo '$PASSWORD = '$PASSWORD
echo '$TENANT = '$TENANT
echo '$STORAGE_ACCOUNT_NAME = '$STORAGE_ACCOUNT_NAME
echo '$CONTAINER_NAME = '$CONTAINER_NAME

az login --service-principal --username $SERVICE_PRINCIPAL_ID --password $PASSWORD --tenant $TENANT 

end=`date -u -d "30 minutes" '+%Y-%m-%dT%H:%MZ'`

sas=`az storage container generate-sas \
    -n $CONTAINER_NAME \
    --account-name $STORAGE_ACCOUNT_NAME \
    --permissions $PERMISSIONS \
    --https-only --expiry $end \
    -o tsv`


echo sas-token: '"'$sas'"'
echo az storage blob upload \
    -c $CONTAINER_NAME \
    -f a.txt \
    -n 1234/a.txt \
    --auth-mode key \
    --account-name $STORAGE_ACCOUNT_NAME \
    --sas-token '"'$sas'"'