#!/bin/bash

# user creates a customer
# then create a new worksheet for that customer
# add a new product


SCRIPT_DIR=$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")


create_customer () {
    CreateCustomerPath="${SCRIPT_DIR}/CreateCustomer.sh"
    customerEntryDTO=$(bash $CreateCustomerPath)

    if [ $? -ne 0 ]; then
        echo "Failed to create customer.">&2
        exit 1
    fi

    externalCustomerId=$(echo "$customerEntryDTO" | jq -r '.id')
    echo "$externalCustomerId"
}

create_worksheet () {
    CreateWorksheetPath="${SCRIPT_DIR}/CreateWorksheet.sh"
    worksheetDTO=$(bash $CreateWorksheetPath "$1")

    if [ $? -ne 0 ]; then
        echo "Failed to create worksheet.">&2
        exit 1
    fi

    externalWorksheetId=$(echo "$worksheetDTO" | jq -r '.id')

    echo "$externalWorksheetId"
}

create_product () {
    
    create_product_path="${SCRIPT_DIR}/CreateProduct.sh"
    

    productDTO=$(bash $create_product_path "$1" "$2")

    if [ $? -ne 0 ]; then
        echo "Failed to create product.">&2
        exit 1
    fi

    echo "$productDTO"
}

#customerId=$(create_customer)
customerId="019bff20-6de5-732c-a872-751a20bf9a4b"

# # worksheetId=$(create_worksheet "$customerId")

worksheetId="019c0300-bcce-7a1f-b6f4-8151b86e2bfb"

# create_product "$customerId" "$worksheetId"

# productId="019c0841-c4b1-72fd-9b47-8cff3464af27"

baseaddress="http://localhost:5085/api/v1"

currentaddress="${baseaddress}/pricing/customer/${customerId}/worksheet/${worksheetId}/product"

response=$(curl -s -X GET \
    -H "Content-Type: application/json" \
    $currentaddress
)


echo "$response" | jq .




