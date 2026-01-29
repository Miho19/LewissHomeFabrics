# Lewiss Home Fabrics Pricing

The pricing system realised as a C# web API project.

## Features

- Create and retrieve `Customers`, `Worksheets` and `Products`

## Usage

- The API returns JSON-encoded responses with standard HTTP response codes.
- The examples shown below use `curl` and `jq`. Example bash scripts can be found at `Lewiss.Pricing.Api.Tests.Scripts`

### Get a Customer

Used to retrieve an `Customer`

#### Request

`GET /customer/id`

```bash
    curl -s -H 'Accept: application/json' \
    http://localhost:5085/api/v1/pricing/customer/019bff20-6de5-732c-a872-751a20bf9a4b
```

#### JSON Response Body

```json
{
  "id": "019bff20-6de5-732c-a872-751a20bf9a4b",
  "familyName": "April",
  "street": "12 street",
  "city": "city",
  "suburb": "suburb",
  "mobile": "123 345 67543",
  "email": "email.address@hotmail.com"
}
```

### Create a Customer

Creates a new `Customer` in the system.

- `mobile` and `email` must be unique.
- If the `Customer` already exists, it will just return the `Customer`.
- The `id` within the response is needed for `Worksheet` and `Product` creation and retrieval.

#### Request

##### Customer.json

```json
{
  "familyname": "April",
  "street": "12 street",
  "city": "city",
  "suburb": "suburb",
  "mobile": "123 345 67543",
  "email": "email.address@hotmail.com"
}
```

`POST /customer/`

```bash
    curl -s -X POST \
    -H "Content-Type: application/json" \
    -d @Customer.json \
    http://localhost:5085/api/v1/pricing/customer
```

#### JSON Response Body

```json
{
  "id": "019bff20-6de5-732c-a872-751a20bf9a4b",
  "familyName": "April",
  "street": "12 street",
  "city": "city",
  "suburb": "suburb",
  "mobile": "123 345 67543",
  "email": "email.address@hotmail.com"
}
```

### Get Customers

- Returns a list of `Customers`
- Uses a query string to filter
  - FamilyName
  - Mobile
  - Email

#### Request

`GET /customer`

```bash
    curl -s -H 'Accept: application/json' \
    http://localhost:5085/api/v1/pricing/customer?familyname=April
```

#### JSON Response Body

```json
[
  {
    "id": "019bff20-6de5-732c-a872-751a20bf9a4b",
    "familyName": "April",
    "street": "12 street",
    "city": "city",
    "suburb": "suburb",
    "mobile": "123 345 67543",
    "email": "email.address@hotmail.com"
  }
]
```

### Get Worksheet

Retrieve a `Worksheet`

- The `Worksheet` must belong to the `Customer` for successful retrieval

#### Request

`GET /customer/id/worksheet/id`

```bash
    curl -s -H 'Accept: application/json' \
    http://localhost:5085/api/v1/pricing/customer/019bff20-6de5-732c-a872-751a20bf9a4b/worksheet/019c0300-bcce-7a1f-b6f4-8151b86e2bfb
```

#### JSON Response Body

```json
{
  "id": "019c0300-bcce-7a1f-b6f4-8151b86e2bfb",
  "customerId": "019bff20-6de5-732c-a872-751a20bf9a4b",
  "price": 0e-28,
  "discount": 0e-28,
  "newBuild": false,
  "callOutFee": 0e-28
}
```

### Get Customer Worksheet

Retrieves all `Worksheet` assoicated with the `Customer`

#### Request

`GET /customer/id/worksheet`

```bash
    curl -s -H 'Accept: application/json' \
    http://localhost:5085/api/v1/pricing/customer/019bff20-6de5-732c-a872-751a20bf9a4b/worksheet
```

#### JSON Response Body

```json
[
  {
    "id": "019c02ff-fa3a-7fb4-89bf-e60eab05b684",
    "customerId": "019bff20-6de5-732c-a872-751a20bf9a4b",
    "price": 0e-28,
    "discount": 0e-28,
    "newBuild": false,
    "callOutFee": 0e-28
  },
  {
    "id": "019c0300-bcce-7a1f-b6f4-8151b86e2bfb",
    "customerId": "019bff20-6de5-732c-a872-751a20bf9a4b",
    "price": 0e-28,
    "discount": 0e-28,
    "newBuild": false,
    "callOutFee": 0e-28
  }
]
```

### Create Worksheet

Creates a new `Worksheet`

#### Request

`POST /customer/id/worksheet`

```bash
    curl -s -X POST \
    -H "Content-Type: application/json" \
    http://localhost:5085/api/v1/pricing/customer/019bff20-6de5-732c-a872-751a20bf9a4b/worksheet
```

#### JSON Response Body

```json
{
  "id": "019c0835-0d9b-706d-bd96-ed09ab6f18e8",
  "customerId": "019bff20-6de5-732c-a872-751a20bf9a4b",
  "price": 0.0,
  "discount": 0.0,
  "newBuild": false,
  "callOutFee": 0.0
}
```

### Get Product

Retrieves a `Product` from a `Worksheet` assoicated with a `Customer`

#### Request

`GET /customer/id/worksheet/id/product/id`

```bash
    curl -s -H 'Accept: application/json' \
    http://localhost:5085/api/v1/pricing/customer/019bff20-6de5-732c-a872-751a20bf9a4b/worksheet/019c0300-bcce-7a1f-b6f4-8151b86e2bfb/product/019c0841-c4b1-72fd-9b47-8cff3464af27
```

#### JSON Reponse Body

```json
{
  "id": "019c0842-90a2-7566-831a-90bd4e636566",
  "worksheetId": "019c0300-bcce-7a1f-b6f4-8151b86e2bfb",
  "price": 0,
  "variableConfiguration": {
    "location": "Kitchen",
    "width": 1200,
    "height": 900,
    "reveal": 0,
    "remoteNumber": 0,
    "remoteChannel": 0,
    "installHeight": 1200
  },
  "fixedConfiguration": {
    "fitType": "inside",
    "fixingTo": "wood",
    "productType": "Kinetics Cellular",
    "fabric": "Translucent White",
    "operationType": "Cord",
    "operationSide": "Left"
  },
  "kineticsCellular": {
    "headrailColour": "White",
    "sideChannelColour": "White"
  },
  "kineticsRoller": null
}
```

### Create a Product

Create a `Product` for a `Worksheet` assoicated with a `Customer`

#### Request

##### Product.json

```json
{
  "worksheetid": "019c0300-bcce-7a1f-b6f4-8151b86e2bfb",
  "variableconfiguration": {
    "location": "Lounge",
    "width": 1200,
    "height": 900,
    "reveal": 0,
    "installheight": 1200,
    "remotenumber": 0,
    "remotechannel": 0
  },
  "fixedconfiguration": {
    "fittype": "inside",
    "fixingto": "wood",
    "producttype": "Kinetics Roller",
    "fabric": "Everyday Vinyl Collection - Polar",
    "operationtype": "Chain",
    "operationside": "Left"
  },
  "kineticsroller": {
    "rolltype": "Front",
    "ChainColour": "black",
    "ChainLength": 1500,
    "BracketType": "Standard",
    "BracketColour": "White",
    "BottomRailType": "Flat",
    "BottomRailColour": "White"
  }
}
```

`POST /customer/id/worksheet/id/product`

```bash
    curl -s -X POST \
    -H "Content-Type: application/json" \
    -d @Customer.json \
    http://localhost:5085/api/v1/pricing/customer/019bff20-6de5-732c-a872-751a20bf9a4b/worksheet/019c0300-bcce-7a1f-b6f4-8151b86e2bfb/product
```

#### JSON Response Body

```json
{
  "id": "019c084a-a213-7d9c-977f-0d973e2d538a",
  "worksheetId": "019c0300-bcce-7a1f-b6f4-8151b86e2bfb",
  "price": 0,
  "variableConfiguration": {
    "location": "Lounge",
    "width": 1200,
    "height": 900,
    "reveal": 0,
    "remoteNumber": 0,
    "remoteChannel": 0,
    "installHeight": 1200
  },
  "fixedConfiguration": {
    "fitType": "inside",
    "fixingTo": "wood",
    "productType": "Kinetics Roller",
    "fabric": "Everyday Vinyl Collection - Polar",
    "operationType": "Chain",
    "operationSide": "Left"
  },
  "kineticsCellular": null,
  "kineticsRoller": {
    "rollType": "Front",
    "chainColour": "black",
    "chainLength": 1500,
    "bracketType": "Standard",
    "bracketColour": "White",
    "bottomRailType": "Flat",
    "bottomRailColour": "White",
    "pelmetType": null,
    "pelmetColour": null
  }
}
```
