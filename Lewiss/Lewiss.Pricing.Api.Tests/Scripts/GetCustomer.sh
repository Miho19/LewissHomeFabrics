#!/bin/bash

# Script requires jq

prod="${1:-1}"

if ! command -v jq &> /dev/null; then
    echo "jq is required for this script"
    exit 1
fi

SCRIPT_DIR=$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")
customerEntryDTO="${SCRIPT_DIR}/json/CustomerEntryDTO.json"


id=$(jq -r '.id' "$customerEntryDTO")

baseaddress=""

echo "$prod"

if [ "$prod" -eq 0 ]; then
    
    baseaddress="http://localhost:5085/api/v1"
    
else

    baseaddress="https://lewiss-dev-server-cjcpcgh4f8a4cpau.newzealandnorth-01.azurewebsites.net/api/v1"
fi




currentAddress="${baseaddress}/customer/${id}"

echo "$currentAddress"


response=$(curl \
    -H "Content-Type: application/json" \
    $currentAddress
)

echo "${response}"

json=$(echo "$response" | jq .)

returnedCustomerEntryDTOPath="${SCRIPT_DIR}/json/ReturnedCustomerEntryDTO.json"

echo "$json" > "$returnedCustomerEntryDTOPath"
