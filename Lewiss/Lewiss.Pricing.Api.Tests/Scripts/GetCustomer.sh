#!/bin/bash

# Script requires jq

if ! command -v jq &> /dev/null; then
    echo "jq is required for this script"
    exit 1
fi

SCRIPT_DIR=$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")
customerEntryDTO="${SCRIPT_DIR}/json/CustomerEntryDTO.json"


id=$(jq -r '.id' "$customerEntryDTO")

baseaddress="https://lewiss-dev-server-cjcpcgh4f8a4cpau.newzealandnorth-01.azurewebsites.net/api/v1/"

currentAddress="${baseAddress}/customer/${id}"

response=$(curl -s \
    -H "Content-Type: application/json" \
    $currentAddress
)

json=$(echo "$response" | jq .)


returnedCustomerEntryDTOPath="${SCRIPT_DIR}/json/ReturnedCustomerEntryDTO.json"

echo "$json" > "$returnedCustomerEntryDTOPath"
