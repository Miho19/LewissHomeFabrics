#!/bin/bash

# Script requires jq

if ! command -v jq &> /dev/null; then
    echo "jq is required for this script"
    exit 1
fi

SCRIPT_DIR=$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")



baseAddress="http://localhost:5085/api/v1"

currentAddress="${baseAddress}/customer?familyName=April"

response=$(curl -s \
    -H "Content-Type: application/json" \
    $currentAddress
)

json=$(echo "$response" | jq .)


returnedCustomerEntryDTOPath="${SCRIPT_DIR}/json/ReturnedCustomerEntryDTOList.json"

echo "$json" > "$returnedCustomerEntryDTOPath"
