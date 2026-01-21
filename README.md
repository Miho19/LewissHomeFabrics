# Lewiss Home Fabrics

## Current Task

    - PricingController Branch
        - Ability to create worksheets

## Objects

    Customer
        - Family Name
        - Street
        - Suburb
        - Mobile
        - Email
        - Consultant
        - Measurer
        - Created at Date

    Worksheet
        - WorksheetId
        - Customer
        - Product Information
        - Additional

    Product
        - ProductId
        - WorksheetId
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
