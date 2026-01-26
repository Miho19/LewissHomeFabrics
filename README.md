# Lewiss Home Fabrics Pricing

## Current Task

- Repositories for Worksheet and Customer
- Unit of Work for repositories
- Service layer for the Pricing Controller

## Front-end Interface

### Customer DTO

```
Id: string
Family Name: string
Street: string
Suburb: string
Mobile: string
Email: string
```

### Worksheet DTO

```
Id: string
CustomerId: string
Price: decimal
Discount: decimal
NewBuild: bool
CallOutFee: decimal
```

### Product DTO

```
ProductId: string
WorksheetId: string
Price: decimal
Location: string
Width: integer
Height: integer
Reveal: integer
FitType: string
FixingTo: string
AboveHeightConstraint: bool
ProductType: string
Fabric: string
OperationType: string
OperationSide: string
RemoteNumber: integer
RemoteChannel: integer
Configuration: oneOf [Kinetics Cellular, Kinetics Roller]
ButtingTo: string
```

### Kinetics Cellular

```
HeadrailColour: string
SideChannelColour: string
```

### Kinetics Roller

```
RollType: string
ChainColour: string
ChainLength: integer
BracketType: string
BracketColour: string
BottomRailType: string
BottomRailColour: string
PelmetType: string
PelmetColour: string
```

## API

### Customer

`CreateCustomer` returns [Customer DTO](#customer-dto)

### Worksheet

`CreateWorksheet` [Worksheet DTO](#worksheet-dto)
