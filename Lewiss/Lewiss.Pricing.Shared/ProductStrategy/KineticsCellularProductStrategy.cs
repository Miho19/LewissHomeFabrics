using FluentResults;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Model.Fabric.Type;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.ProductDTO;
using Lewiss.Pricing.Shared.ProductStrategy;
using Lewiss.Pricing.Shared.Services;

namespace Lewiss.Pricing.Shared.ProductStrategy;

public class KineticsCellularProductStrategy : IProductStrategy<KineticsCellularFabric>
{
    private readonly ProductService _productService;

    private readonly SharedUtilityService _sharedUtilityService;

    public string ProductType => ProductTypeOption.KineticsCellular.Value;


    public KineticsCellularProductStrategy(ProductService productService, SharedUtilityService sharedUtilityService)
    {
        _productService = productService;
        _sharedUtilityService = sharedUtilityService;
    }

    public async Task<Result<Product>> CreateProductAsync(Guid externalCustomerId, ProductCreateInputDTO productCreateDTO, Worksheet worksheet, CancellationToken cancellationToken = default)
    {
        var productModel = productCreateDTO.ToProductEntity(worksheet);

        /// from here
        /// 
        /// 
        var populateGeneralConfigurationResult = await _productService.PopulateProductOptionVariationList(productCreateDTO, typeof(ProductCreateInputDTO), cancellationToken);
        if (populateGeneralConfigurationResult.IsFailed)
        {
            return Result.Fail(populateGeneralConfigurationResult.Errors);
        }

        var GeneralConfigurationList = populateGeneralConfigurationResult.Value;


        var productTypeSpecificProductOptionVariationListResult = await PopulateProductOptionVariationList_ProductTypeSpecificConfigurationAsync(productCreateDTO, cancellationToken);
        if (productTypeSpecificProductOptionVariationListResult.IsFailed)
        {
            return Result.Fail(productTypeSpecificProductOptionVariationListResult.Errors);
        }

        var productTypeSpecificProductOptionVariationList = productTypeSpecificProductOptionVariationListResult.Value;

        product.OptionVariations = [.. GeneralConfigurationList, .. productTypeSpecificProductOptionVariationList];

        // Get fabric price info --> add to price total

        var totalPriceProductOptionVariationList = GetProductOptionVariationListTotalPrice(product.OptionVariations.ToList());
        var fabricPriceResult = await GetProductFabricPriceOutputDTO(product.OptionVariations.ToList(), product.Width, product.Height, cancellationToken);
        if (fabricPriceResult.IsFailed)
        {
            return Result.Fail(fabricPriceResult.Errors);
        }

        var fabricPrice = fabricPriceResult.Value;


        product.Price = totalPriceProductOptionVariationList + fabricPrice.Price;



        return Result.Ok();
    }
}