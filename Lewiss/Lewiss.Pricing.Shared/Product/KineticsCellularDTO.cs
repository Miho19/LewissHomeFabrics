using Lewiss.Pricing.Shared.Product;

public class KineticsCellularDTO : IConfiguration
{
    public string ProductType { get; } = "KineticsCellular";
    public required string HeadRailColour { get; set; }
    public required string SideChannelColour { get; set; }
}