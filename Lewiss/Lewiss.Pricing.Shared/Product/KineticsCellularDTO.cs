
using Lewiss.Pricing.Shared.Product;

public class KineticsCellularDTO : ISpecificConfiguration
{
    public string ProductType { get; } = "kineticscellular";
    public required string HeadRailColour { get; set; }
    public required string SideChannelColour { get; set; }
}