#!/bin/bash

# Script requires jq




if ! command -v jq &> /dev/null; then
    echo "jq is required for this script"
    exit 1
fi

SCRIPT_DIR=$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")


JSON_FILE_PATH="${SCRIPT_DIR}/ProductCreateDTOKineticsCellular.json"
# JSON_FILE_PATH="${SCRIPT_DIR}/ProductCreateDTOKineticsRoller.json"

temp=$(mktemp)

jq '.worksheetid = "'"$2"'"' "$JSON_FILE_PATH" > "$temp" && mv "$temp" "$JSON_FILE_PATH"

echo "$JSON_FILE_PATH"

baseaddress="http://localhost:5085/api/v1"

currentaddress="${baseaddress}/pricing/customer/$1/worksheet/$2/product"

response=$(curl -s -X POST \
    -H "Content-Type: application/json" \
    --data "@$JSON_FILE_PATH" \
    $currentaddress
)

echo "$response" | jq .