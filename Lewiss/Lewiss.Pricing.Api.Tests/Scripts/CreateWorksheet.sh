#!/bin/bash

# Script requires jq

if ! command -v jq &> /dev/null; then
    echo "jq is required for this script"
    exit 1
fi

baseaddress="http://localhost:5085/api/v1"

currentaddress="${baseaddress}/pricing/customer/$1/worksheet"

response=$(curl -s -X POST \
    -H "Content-Type: application/json" \
    $currentaddress
)

echo "$response" | jq .