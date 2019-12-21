APP_FRIENDLY_NAME="organics"
LOCATION="eastus2"
RESOURCE_GROUP_NAME="rg-$APP_FRIENDLY_NAME-openhack"
COSMOSDB_NAME="cosdb-$APP_FRIENDLY_NAME"
DB_NAME="$APP_FRIENDLY_NAME"

CONTAINER_NAME="ratings"
# Generate a unique 10 character alphanumeric string to ensure unique resource names
uniqueId=$(env LC_CTYPE=C tr -dc 'a-z0-9' < /dev/urandom | fold -w 10 | head -n 1)

# Create a Cosmos account for SQL API
az cosmosdb create \
    -n $COSMOSDB_NAME \
    -g $RESOURCE_GROUP_NAME \
    --default-consistency-level Eventual \
    --locations regionName=$LOCATION isZoneRedundant=False

az cosmosdb sql database create \
    -a $COSMOSDB_NAME \
    -g $RESOURCE_GROUP_NAME \
    -n $DB_NAME

 # Define the index policy for the container, include spatial and composite indexes
idxpolicy=$(cat << EOF 
{
    "indexingMode": "consistent",
    "automatic": true,
    "includedPaths": [
        {
            "path": "/*"
        }
    ],
    "excludedPaths": [
        {
            "path": "/\"_etag\"/?"
        }
    ]
}
EOF
)
# Persist index policy to json file
echo "$idxpolicy" > "idxpolicy-$uniqueId.json"

# Create a SQL API container
az cosmosdb sql container create \
    -a $COSMOSDB_NAME\
    -g $RESOURCE_GROUP_NAME \
    -d $DB_NAME \
    -n $CONTAINER_NAME \
    -p '/productId' \
    --throughput 400 \
    --idx @idxpolicy-$uniqueId.json

# Clean up temporary index policy file
rm -f "idxpolicy-$uniqueId.json"