using System.Reflection.Metadata.Ecma335;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Aggregates;

public sealed class Product
{
	public ProductId Id { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public Money Price { get; private set; }
	public Sku Sku { get; private set; }

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