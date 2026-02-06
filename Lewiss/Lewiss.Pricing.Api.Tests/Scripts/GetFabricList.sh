#!/bin/bash

SCRIPT_DIR=$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")

# Script requires jq

if ! command -v jq &> /dev/null; then
    echo "jq is required for this script"
    exit 1
fi


baseaddress="http://localhost:5085/api/v1"
currentaddress="${baseaddress}/fabric?productType=KineticsRoller"

response=$(curl -s \
    -H "Content-Type: application/json" \
    $currentaddress
)

echo "$response" | jq .