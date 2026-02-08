#!/bin/bash

# Script requires jq

if ! command -v jq &> /dev/null; then
    echo "jq is required for this script"
    exit 1
fi

SCRIPT_DIR=$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")
JSON_FILE_PATH="${SCRIPT_DIR}/json/Customer.json"
output_file_path="${SCRIPT_DIR}/json/CustomerEntryDTO.json"




baseaddress="https://lewissdev.azurewebsites.net/api/v1"

currentaddress="${baseaddress}/customer"

response=$(curl -s -X POST \
    -H "Content-Type: application/json" \
    --data "@$JSON_FILE_PATH" \
    $currentaddress
)

json=$(echo "$response" | jq .)


echo "$json" > "$output_file_path"
