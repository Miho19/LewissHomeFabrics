USE lewiss;

SELECT
    Product.ProductId,
    Product.ExternalMapping,
    ProductOption.Name,
    ProductOptionVariation.Value
FROM
    Product
JOIN 
    ProductProductOptionVariation ON Product.ProductId = ProductProductOptionVariation.ProductsProductId
JOIN
    ProductOptionVariation ON ProductProductOptionVariation.OptionVariationsProductOptionVariationId = ProductOptionVariation.ProductOptionVariationId
JOIN
    ProductOption ON ProductOptionVariation.ProductOptionId = ProductOption.ProductOptionId
    
WHERE Product.ProductId=6;
