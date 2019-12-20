die () {
    echo >&2 "$@"
    echo "$ ./az-service-principal-id.sh [sp-name]"
    exit 1
}
[ "$#" -eq 1 ] || die "1 argument required, $# provided"
 

SP_NAME=$1
servicePrincipalAppId=$(az ad sp list --display-name $SP_NAME --query "[].appId" -o tsv)
echo $servicePrincipalAppId


 