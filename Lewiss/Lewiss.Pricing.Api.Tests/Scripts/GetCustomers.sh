#!/bin/bash

# Script requires jq

if ! command -v jq &> /dev/null; then
    echo "jq is required for this script"
    exit 1
fi

SCRIPT_DIR=$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")



baseaddress="https://lewiss-dev-server-cjcpcgh4f8a4cpau.newzealandnorth-01.azurewebsites.net/api/v1/"

currentAddress="${baseaddress}/customer?familyName=April"

response=$(curl -s \
    -H "Content-Type: application/json" \
    $currentAddress
)

json=$(echo "$response" | jq .)


returnedCustomerEntryDTOPath="${SCRIPT_DIR}/json/ReturnedCustomerEntryDTOList.json"

echo "$json" > "$returnedCustomerEntryDTOPath"
