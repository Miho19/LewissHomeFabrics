#!/bin/bash

# Script requires jq

if ! command -v jq &> /dev/null; then
    echo "jq is required for this script"
    exit 1
fi

SCRIPT_DIR=$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")

customerEntryDTO="${SCRIPT_DIR}/json/CustomerEntryDTO.json"
worksheetDTO="${SCRIPT_DIR}/json/Worksheet.json"

customerId=$(jq -r '.id' "$customerEntryDTO")
worksheetId=$(jq -r '.id' "$worksheetDTO")


baseaddress="http://localhost:5085/api/v1"

currentaddress="${baseaddress}/pricing/customer/${customerId}/worksheet/${worksheetId}/product"


JSON_FILE_PATH="${SCRIPT_DIR}/json/ProductCreateDTOKineticsCellular.json"
# JSON_FILE_PATH="${SCRIPT_DIR}/json/ProductCreateDTOKineticsRoller.json"


response=$(curl -X POST \
    -H "Content-Type: application/json" \
    --data "@$JSON_FILE_PATH" \
    $currentaddress
)

echo "$response" | jq .
json=$(echo "$response" | jq .)
outputFile1="${SCRIPT_DIR}/json/ProductKineticsCellular.json"
echo "$json" > "$outputFile1"

