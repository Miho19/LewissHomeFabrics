#!/bin/bash

# Script requires jq

if ! command -v jq &> /dev/null; then
    echo "jq is required for this script"
    exit 1
fi

SCRIPT_DIR=$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")
customerEntryDTO="${SCRIPT_DIR}/json/CustomerEntryDTO.json"

id=$(jq -r '.id' "$customerEntryDTO")

baseaddress="http://localhost:5085/api/v1"

currentaddress="${baseaddress}/pricing/customer/${id}/worksheet"

response=$(curl -s -X POST \
    -H "Content-Type: application/json" \
    $currentaddress
)


json=$(echo "$response" | jq .)


outputFile="${SCRIPT_DIR}/json/Worksheet.json"


echo "$json" > "$outputFile"

