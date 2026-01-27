#!/bin/bash



SCRIPT_DIR=$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")


CreateCustomerPath="${SCRIPT_DIR}/CreateCustomer.sh"

customerEntryDTO=$(bash $CreateCustomerPath)
echo "Customer Entry DTO"
echo "$customerEntryDTO"



