die () {
    echo >&2 "$@"
    echo "$ ./reset-service-principal-credentials.sh [service-principal-name]"
    exit 1
}
REQUIRED_ARGS=1
[ "$#" -eq $REQUIRED_ARGS ] || die "$REQUIRED_ARGS argument required, $# provided"

echo '$0 = '$0
echo '$1 = '$1
SP_NAME=$1
az ad sp credential reset --name $SP_NAME