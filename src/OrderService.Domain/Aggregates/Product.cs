using System.Reflection.Metadata.Ecma335;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Aggregates;

/// <summary>
/// Represents a product in the order service.
/// </summary>
public sealed class Product
{
	public ProductId Id { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public Money Price { get; private set; }
	public Sku Sku { get; private set; }

	/// <summary>
	/// Creates a new product.
	/// </summary>
	/// <param name="productId">The product identifier.</param>
	/// <param name="name">The name of the product.</param>
	/// <param name="price">The price of the product.</param>
	/// <param name="sku">The SKU of the product.</param>
	/// <returns>A new <see cref="Product"/> instance.</returns>
	public static Product Create(ProductId productId, string name, Money price, Sku sku)
	{
		return new Product(productId, name, price, sku);
	}
	
	private Product(ProductId productId, string name, Money price, Sku sku)
	{
		Id = productId;
		Name = name;
		Price = price;
		Sku = sku;
	}
}