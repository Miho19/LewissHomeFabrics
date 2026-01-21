# Lewiss Home Fabrics

## Current Task

    - PricingController Branch
        - Ability to create worksheets

## Objects

    Customer DTO
        - Family Name
        - Street
        - Suburb
        - Mobile
        - Email
        - Consultant
        - Measurer

    Worksheet DTO
        - WorksheetId
        - Customer
        - Products
        - Additional

    Product DTO
        - ProductId
        - WorksheetId
        - Price
        - Location
        - Width
        - Height
        - Fit Type
            - IN / OUT
        - FixingTo
            - TBC
        - ProductType

        - Fabric
        - OperationType
        - OperationSide

        Configuration

            - Kinetics Cellular
               - Headrail Colour
               - Side Channel Colour
                - Butting

            - Kinetics Roller
                - Chain Type
                - Chain Length
                - Bottom Rail Colour
                - Bracket Type
                    - Standard, Extra Large, Combo Bracket
                - Bracket Colour
                - Pelmet Type
                - Pelmet Colour
                - Butting

## Creating Worksheet

1. Send Customer information in
2. Send back a worksheet object

## Database Tables

    Customer Table
    - CustomerId
    - Family Name
    - Street
    - Suburb
    - Mobile
    - Email
    - Consultant
    - Measurer
    - CreatedAt

    - Current Worksheets
        - One to Many
    - Past Worksheets
        - One to Many


    Worksheet
    - WorksheetId
    - CreatedAt
    - CustomerId
        - One to One

    Product
    - ProductId
    - Price
    - Location
    - Width
    - Height
    - WorksheetId
        - One to One
    OptionVariations
        - Many to Many

    Option
    - OptionId
    - Name
    These are the available attributes that can change for a product

    OptionVariations
    - OptionVariationId
    - Price
    - OptionId
        - many to One
    - Value
