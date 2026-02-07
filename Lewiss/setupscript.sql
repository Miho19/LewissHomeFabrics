IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Customer] (
    [CustomerId] int NOT NULL IDENTITY,
    [ExternalMapping] uniqueidentifier NOT NULL,
    [FamilyName] nvarchar(max) NOT NULL,
    [Street] nvarchar(max) NOT NULL,
    [City] nvarchar(max) NOT NULL,
    [Suburb] nvarchar(max) NOT NULL,
    [Mobile] nvarchar(450) NOT NULL,
    [Email] nvarchar(450) NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY ([CustomerId])
);

CREATE TABLE [FabricPrice] (
    [FabricPriceId] int NOT NULL IDENTITY,
    [Width] int NOT NULL,
    [Height] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [ProductType] nvarchar(450) NOT NULL,
    [Opacity] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_FabricPrice] PRIMARY KEY ([FabricPriceId]),
    CONSTRAINT [AK_FabricPrice_Width_Height_Opacity_ProductType] UNIQUE ([Width], [Height], [Opacity], [ProductType])
);

CREATE TABLE [ProductOption] (
    [ProductOptionId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ProductOption] PRIMARY KEY ([ProductOptionId])
);

CREATE TABLE [Worksheet] (
    [WorksheetId] int NOT NULL IDENTITY,
    [ExternalMapping] uniqueidentifier NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Discount] decimal(18,2) NOT NULL,
    [NewBuild] bit NOT NULL,
    [CallOutFee] decimal(18,2) NOT NULL,
    [CustomerId] int NOT NULL,
    CONSTRAINT [PK_Worksheet] PRIMARY KEY ([WorksheetId]),
    CONSTRAINT [FK_Worksheet_Customer_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customer] ([CustomerId]) ON DELETE CASCADE
);

CREATE TABLE [ProductOptionVariation] (
    [ProductOptionVariationId] int NOT NULL IDENTITY,
    [Price] decimal(18,2) NULL,
    [ProductOptionId] int NOT NULL,
    [Value] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ProductOptionVariation] PRIMARY KEY ([ProductOptionVariationId]),
    CONSTRAINT [FK_ProductOptionVariation_ProductOption_ProductOptionId] FOREIGN KEY ([ProductOptionId]) REFERENCES [ProductOption] ([ProductOptionId]) ON DELETE CASCADE
);

CREATE TABLE [Product] (
    [ProductId] int NOT NULL IDENTITY,
    [ExternalMapping] uniqueidentifier NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [Width] int NOT NULL,
    [Height] int NOT NULL,
    [Reveal] int NOT NULL,
    [InstallHeight] int NOT NULL,
    [RemoteNumber] int NOT NULL,
    [RemoteChannel] int NOT NULL,
    [WorksheetId] int NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY ([ProductId]),
    CONSTRAINT [FK_Product_Worksheet_WorksheetId] FOREIGN KEY ([WorksheetId]) REFERENCES [Worksheet] ([WorksheetId]) ON DELETE CASCADE
);

CREATE TABLE [KineticsCellularFabric] (
    [KineticsCellularFabricId] int NOT NULL IDENTITY,
    [Code] nvarchar(450) NOT NULL,
    [Colour] nvarchar(450) NOT NULL,
    [Opacity] nvarchar(450) NOT NULL,
    [Multiplier] decimal(18,2) NOT NULL,
    [ProductOptionVariationId] int NOT NULL,
    CONSTRAINT [PK_KineticsCellularFabric] PRIMARY KEY ([KineticsCellularFabricId]),
    CONSTRAINT [AK_KineticsCellularFabric_Code_Colour_Opacity] UNIQUE ([Code], [Colour], [Opacity]),
    CONSTRAINT [FK_KineticsCellularFabric_ProductOptionVariation_ProductOptionVariationId] FOREIGN KEY ([ProductOptionVariationId]) REFERENCES [ProductOptionVariation] ([ProductOptionVariationId]) ON DELETE CASCADE
);

CREATE TABLE [KineticsRollerFabric] (
    [KineticsRollerFabricId] int NOT NULL IDENTITY,
    [Fabric] nvarchar(450) NOT NULL,
    [Colour] nvarchar(450) NOT NULL,
    [Opacity] nvarchar(450) NOT NULL,
    [Multiplier] decimal(18,2) NOT NULL,
    [MaxWidth] int NOT NULL,
    [MaxHeight] int NOT NULL,
    [ProductOptionVariationId] int NOT NULL,
    CONSTRAINT [PK_KineticsRollerFabric] PRIMARY KEY ([KineticsRollerFabricId]),
    CONSTRAINT [AK_KineticsRollerFabric_Colour_Fabric_Opacity] UNIQUE ([Colour], [Fabric], [Opacity]),
    CONSTRAINT [FK_KineticsRollerFabric_ProductOptionVariation_ProductOptionVariationId] FOREIGN KEY ([ProductOptionVariationId]) REFERENCES [ProductOptionVariation] ([ProductOptionVariationId]) ON DELETE CASCADE
);

CREATE TABLE [ProductProductOptionVariation] (
    [OptionVariationsProductOptionVariationId] int NOT NULL,
    [ProductsProductId] int NOT NULL,
    CONSTRAINT [PK_ProductProductOptionVariation] PRIMARY KEY ([OptionVariationsProductOptionVariationId], [ProductsProductId]),
    CONSTRAINT [FK_ProductProductOptionVariation_ProductOptionVariation_OptionVariationsProductOptionVariationId] FOREIGN KEY ([OptionVariationsProductOptionVariationId]) REFERENCES [ProductOptionVariation] ([ProductOptionVariationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProductProductOptionVariation_Product_ProductsProductId] FOREIGN KEY ([ProductsProductId]) REFERENCES [Product] ([ProductId]) ON DELETE CASCADE
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ProductOptionId', N'Name') AND [object_id] = OBJECT_ID(N'[ProductOption]'))
    SET IDENTITY_INSERT [ProductOption] ON;
INSERT INTO [ProductOption] ([ProductOptionId], [Name])
VALUES (1, N'FitType'),
(2, N'FixingTo'),
(3, N'ProductType'),
(4, N'Fabric'),
(5, N'OperationType'),
(6, N'OperationSide'),
(7, N'HeadrailColour'),
(8, N'SideChannelColour'),
(9, N'RollType'),
(10, N'ChainColour'),
(11, N'ChainLength'),
(12, N'BracketType'),
(13, N'BracketColour'),
(14, N'BottomRailType'),
(15, N'BottomRailColour'),
(16, N'PelmetType'),
(17, N'PelmetColour');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ProductOptionId', N'Name') AND [object_id] = OBJECT_ID(N'[ProductOption]'))
    SET IDENTITY_INSERT [ProductOption] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ProductOptionVariationId', N'Price', N'ProductOptionId', N'Value') AND [object_id] = OBJECT_ID(N'[ProductOptionVariation]'))
    SET IDENTITY_INSERT [ProductOptionVariation] ON;
INSERT INTO [ProductOptionVariation] ([ProductOptionVariationId], [Price], [ProductOptionId], [Value])
VALUES (1, 0.0, 1, N'Inside'),
(2, 0.0, 1, N'Outside'),
(3, 0.0, 2, N'Wood'),
(4, 0.0, 3, N'Kinetics Cellular'),
(5, 0.0, 3, N'Kinetics Roller'),
(6, 250.0, 5, N'Lithium-ion'),
(7, 0.0, 5, N'Cord'),
(8, 0.0, 5, N'Chain'),
(9, 0.0, 6, N'Left'),
(10, 0.0, 6, N'Right'),
(11, 0.0, 7, N'White'),
(12, 0.0, 7, N'Black'),
(13, 0.0, 8, N'White'),
(14, 0.0, 8, N'Black'),
(15, 0.0, 9, N'Front'),
(16, 0.0, 9, N'Back'),
(17, 0.0, 10, N'White'),
(18, 0.0, 10, N'Black'),
(19, 0.0, 10, N'Grey'),
(20, 32.0, 10, N'Stainless'),
(21, 0.0, 11, N'750'),
(22, 0.0, 11, N'1000'),
(23, 0.0, 11, N'1250'),
(24, 0.0, 11, N'1500'),
(25, 0.0, 11, N'1750'),
(26, 0.0, 11, N'2000'),
(27, 0.0, 11, N'2250'),
(28, 0.0, 11, N'2500'),
(29, 0.0, 11, N'2750'),
(30, 0.0, 11, N'3000'),
(31, 0.0, 12, N'Standard'),
(32, 0.0, 12, N'Extra Large'),
(33, 0.0, 13, N'White'),
(34, 0.0, 13, N'Black'),
(35, 0.0, 14, N'Flat'),
(36, 25.0, 14, N'Deluxe'),
(37, 0.0, 15, N'White'),
(38, 0.0, 15, N'Black'),
(39, 0.0, 15, N'Silver'),
(40, 0.0, 15, N'Off White'),
(41, 0.0, 16, N'None'),
(42, 0.0, 17, N'White');
INSERT INTO [ProductOptionVariation] ([ProductOptionVariationId], [Price], [ProductOptionId], [Value])
VALUES (43, 0.0, 17, N'Black'),
(44, 0.0, 4, N'Adagio Black LF'),
(45, 0.0, 4, N'Adagio Chilli LF'),
(46, 0.0, 4, N'Adagio Taupe LF'),
(47, 0.0, 4, N'Austro Amaze BO'),
(48, 0.0, 4, N'Austro Anchor BO'),
(49, 0.0, 4, N'Austro Apple BO'),
(50, 0.0, 4, N'Fenescreen 10% Charcoal SS'),
(51, 0.0, 4, N'Fenescreen 10% Coyote SS'),
(52, 0.0, 4, N'Fenescreen 10% Glacier White SS'),
(53, 0.0, 4, N'001 Translucent Cotton'),
(54, 0.0, 4, N'005 Translucent Cream'),
(55, 0.0, 4, N'014 Translucent Water Green'),
(56, 0.0, 4, N'019 Translucent Agate Red'),
(57, 0.0, 4, N'021 Translucent Gray Sheen'),
(58, 0.0, 4, N'023 Translucent Royal Gray'),
(59, 0.0, 4, N'024 Translucent Jean Blue'),
(60, 0.0, 4, N'025 Translucent Black'),
(61, 0.0, 4, N'026 Translucent Federal Blue'),
(62, 0.0, 4, N'030 Translucent Steel Grey'),
(63, 0.0, 4, N'070 Translucent Winter White'),
(64, 0.0, 4, N'102 Translucent Aqua Glass'),
(65, 0.0, 4, N'333 Translucent Warm Chocolate'),
(66, 0.0, 4, N'416 Translucent Taupe A'),
(67, 0.0, 4, N'512 Translucent Flax Green');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ProductOptionVariationId', N'Price', N'ProductOptionId', N'Value') AND [object_id] = OBJECT_ID(N'[ProductOptionVariation]'))
    SET IDENTITY_INSERT [ProductOptionVariation] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'KineticsCellularFabricId', N'Code', N'Colour', N'Multiplier', N'Opacity', N'ProductOptionVariationId') AND [object_id] = OBJECT_ID(N'[KineticsCellularFabric]'))
    SET IDENTITY_INSERT [KineticsCellularFabric] ON;
INSERT INTO [KineticsCellularFabric] ([KineticsCellularFabricId], [Code], [Colour], [Multiplier], [Opacity], [ProductOptionVariationId])
VALUES (1, N'001', N'Cotton', 1.0, N'Translucent', 53),
(2, N'005', N'Cream', 1.0, N'Translucent', 54),
(3, N'014', N'Water Green', 1.0, N'Translucent', 55),
(4, N'019', N'Agate Red', 1.0, N'Translucent', 56),
(5, N'021', N'Gray Sheen', 1.0, N'Translucent', 57),
(6, N'023', N'Royal Gray', 1.0, N'Translucent', 58),
(7, N'024', N'Jean Blue', 1.0, N'Translucent', 59),
(8, N'025', N'Black', 1.0, N'Translucent', 60),
(9, N'026', N'Federal Blue', 1.0, N'Translucent', 61),
(10, N'030', N'Steel Grey', 1.0, N'Translucent', 62),
(11, N'070', N'Winter White', 1.0, N'Translucent', 63),
(12, N'102', N'Aqua Glass', 1.0, N'Translucent', 64),
(13, N'333', N'Warm Chocolate', 1.0, N'Translucent', 65),
(14, N'416', N'Taupe A', 1.0, N'Translucent', 66),
(15, N'512', N'Flax Green', 1.0, N'Translucent', 67);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'KineticsCellularFabricId', N'Code', N'Colour', N'Multiplier', N'Opacity', N'ProductOptionVariationId') AND [object_id] = OBJECT_ID(N'[KineticsCellularFabric]'))
    SET IDENTITY_INSERT [KineticsCellularFabric] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'KineticsRollerFabricId', N'Colour', N'Fabric', N'MaxHeight', N'MaxWidth', N'Multiplier', N'Opacity', N'ProductOptionVariationId') AND [object_id] = OBJECT_ID(N'[KineticsRollerFabric]'))
    SET IDENTITY_INSERT [KineticsRollerFabric] ON;
INSERT INTO [KineticsRollerFabric] ([KineticsRollerFabricId], [Colour], [Fabric], [MaxHeight], [MaxWidth], [Multiplier], [Opacity], [ProductOptionVariationId])
VALUES (1, N'Black', N'Adagio', 2010, 3100, 1.25, N'LF', 44),
(2, N'Chilli', N'Adagio', 2010, 3100, 1.25, N'LF', 45),
(3, N'Taupe', N'Adagio', 2010, 3100, 1.25, N'LF', 46),
(4, N'Amaze', N'Austro', 3000, 3000, 0.8, N'BO', 47),
(5, N'Anchor', N'Austro', 3000, 3000, 0.8, N'BO', 48),
(6, N'Apple', N'Austro', 3000, 3000, 0.8, N'BO', 49),
(7, N'Charcoal', N'Fenescreen 10%', 2200, 3000, 0.9, N'SS', 50),
(8, N'Coyote', N'Fenescreen 10%', 2200, 3000, 0.9, N'SS', 51),
(9, N'Glacier White', N'Fenescreen 10%', 2200, 3000, 0.9, N'SS', 52);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'KineticsRollerFabricId', N'Colour', N'Fabric', N'MaxHeight', N'MaxWidth', N'Multiplier', N'Opacity', N'ProductOptionVariationId') AND [object_id] = OBJECT_ID(N'[KineticsRollerFabric]'))
    SET IDENTITY_INSERT [KineticsRollerFabric] OFF;

CREATE UNIQUE INDEX [IX_Customer_Email] ON [Customer] ([Email]);

CREATE UNIQUE INDEX [IX_Customer_Mobile] ON [Customer] ([Mobile]);

CREATE UNIQUE INDEX [IX_KineticsCellularFabric_ProductOptionVariationId] ON [KineticsCellularFabric] ([ProductOptionVariationId]);

CREATE UNIQUE INDEX [IX_KineticsRollerFabric_ProductOptionVariationId] ON [KineticsRollerFabric] ([ProductOptionVariationId]);

CREATE INDEX [IX_Product_WorksheetId] ON [Product] ([WorksheetId]);

CREATE INDEX [IX_ProductOptionVariation_ProductOptionId] ON [ProductOptionVariation] ([ProductOptionId]);

CREATE INDEX [IX_ProductProductOptionVariation_ProductsProductId] ON [ProductProductOptionVariation] ([ProductsProductId]);

CREATE INDEX [IX_Worksheet_CustomerId] ON [Worksheet] ([CustomerId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260206045144_InitialCreateForSQLServer', N'10.0.2');

COMMIT;
GO

