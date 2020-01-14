die () {
    echo >&2 "$@"
    echo "$ ./az-create-app-registration.sh [name]"
    exit 1
}
REQUIRED_ARGS=1
[ "$#" -eq $REQUIRED_ARGS ] || die "$REQUIRED_ARGS argument required, $# provided"

RESOURCE_GROUP_NAME="rg-organics-openhack"
LOCATION="eastus2"

APP_REGISTRATION_NAME="appreg-"$1
echo "====== Creating App Registration:  $APP_REGISTRATION_NAME ================="

az ad app create --display-name $APP_REGISTRATION_NAME