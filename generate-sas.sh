STORAGE_ACCOUNT_NAME="storganicsopenhackblob"
PERMISSIONS="acdlrw"

die () {
    echo >&2 "$@"
    echo "$ ./generate-sas.sh [container-name] [connection-string]"
    exit 1
}
echo '$0 = '$0


[ "$#" -eq 2 ] || die "2 argument required, $# provided"
echo '$1 = '$1
echo '$2 = '$2
CONTAINER_NAME=$1
CONNECTION_STRING=$2

echo '$CONTAINER_NAME = '$CONTAINER_NAME
echo '$CONNECTION_STRING = '$CONNECTION_STRING

end=`date -u -d "30 minutes" '+%Y-%m-%dT%H:%MZ'`
sas=`az storage container generate-sas \
        -n $CONTAINER_NAME \
        --account-name $STORAGE_ACCOUNT_NAME \
        --permissions $PERMISSIONS \
        --connection-string $CONNECTION_STRING \
        --https-only --expiry $end -o tsv`

echo sas-token: '"'$sas'"'
echo az storage blob upload \
    -c $CONTAINER_NAME \
    -f a.txt \
    -n 1234/a.txt \
    --auth-mode key \
    --account-name $STORAGE_ACCOUNT_NAME \
    --sas-token '"'$sas'"'