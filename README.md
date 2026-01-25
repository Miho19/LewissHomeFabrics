# Lewiss Home Fabrics Pricing

## Current Task

- Repositories for Worksheet and Customer
- Unit of Work for repositories
- Service layer for the Pricing Controller

## Front-end Interface

### Staff DTO

```
Name: string
Role: string
Mobile: string
Email: string
Department: string
```

### Customer DTO

```
Family Name: string
Street: string
Suburb: string
Mobile: string
Email: string
```

### Worksheet DTO

```
WorksheetId: string
Customer: Customer DTO
Price: decimal
Discount: decimal
NewBuild: bool
CallOutFee: decimal
Consultant: Staff DTO
Measurer: Staff DTO
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

## Creating Worksheet

1. Send in Customer DTO
2. Server responds with Worksheet DTO
