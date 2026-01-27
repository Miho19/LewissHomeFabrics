#!/bin/bash

# Script requires jq

if ! command -v jq &> /dev/null; then
    echo "jq is required for this script"
    exit 1
fi

# CustomerCreateDTO 

familyname="April"
street="12 street"
city="city"
suburb="suburb"
mobile="123 345 67543"
email="email.address@hotmail.com"

customerCreateDTO=$(jq -n \
    --arg familyname "$familyname" \
    --arg street "$street" \
    --arg city "$city" \
    --arg suburb "$suburb" \
    --arg mobile "$mobile" \
    --arg email "$email" \
    '{ "familyname" : $familyname, "street": $street,  "city": $city, "suburb": $suburb, "mobile": $mobile, "email": $email }'
)


baseaddress="http://localhost:5085/api/v1"

currentaddress="${baseaddress}/pricing/customer"

response=$(curl -X POST \
    -H "Content-Type: application/json" \
    --data "$customerCreateDTO" \
    $currentaddress
)

echo "$response" | jq .
