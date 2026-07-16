using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OrderService.Domain.Aggregates;
using OrderService.Domain.ValueObjects;

namespace OrderService.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => OrderId.From(value))
            .IsRequired();

        builder.Property(o => o.CustomerId)
            .HasColumnName("customer_id")
            .HasConversion(
                customerId => customerId.Value,
                value => CustomerId.From(value))
            .IsRequired();

        builder.Property(o => o.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(o => o.Culture)
            .HasColumnName("culture")
            .HasConversion(
                culture => culture.Value,
                value => Culture.From(value))
            .HasMaxLength(10)
            .IsRequired();

        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.Street).HasColumnName("shipping_street").HasMaxLength(200).IsRequired();
            address.Property(a => a.City).HasColumnName("shipping_city").HasMaxLength(100).IsRequired();
            address.Property(a => a.State).HasColumnName("shipping_state").HasMaxLength(100).IsRequired();
            address.Property(a => a.PostalCode).HasColumnName("shipping_postal_code").HasMaxLength(20).IsRequired();
            address.Property(a => a.Country).HasColumnName("shipping_country").HasMaxLength(100).IsRequired();
        });

        builder.OwnsOne(o => o.BillingAddress, address =>
        {
            address.Property(a => a.Street).HasColumnName("billing_street").HasMaxLength(200).IsRequired();
            address.Property(a => a.City).HasColumnName("billing_city").HasMaxLength(100).IsRequired();
            address.Property(a => a.State).HasColumnName("billing_state").HasMaxLength(100).IsRequired();
            address.Property(a => a.PostalCode).HasColumnName("billing_postal_code").HasMaxLength(20).IsRequired();
            address.Property(a => a.Country).HasColumnName("billing_country").HasMaxLength(100).IsRequired();
        });

        // OrderItem.OrderId is its own mapped column (order_id_ref), separate from the owning
        // FK EF generates for the relationship itself (order_id). This duplication only exists
        // because OrderItem's constructor requires orderId explicitly for EF's constructor-binding
        // materialization to work; the two columns always hold the same value.
        builder.OwnsMany(o => o.OrderItems, item =>
        {
            item.ToTable("order_items");

            item.WithOwner().HasForeignKey("OwningOrderId");
            item.Property<OrderId>("OwningOrderId")
                .HasColumnName("order_id")
                .HasConversion(
                    orderId => orderId.Value,
                    value => OrderId.From(value));

            item.HasKey(i => i.Id);
            item.Property(i => i.Id).HasColumnName("id").ValueGeneratedNever();

            item.Property(i => i.OrderId)
                .HasColumnName("order_id_ref")
                .HasConversion(
                    orderId => orderId.Value,
                    value => OrderId.From(value))
                .IsRequired();

            item.Property(i => i.ProductId)
                .HasColumnName("product_id")
                .HasConversion(
                    productId => productId.Value,
                    value => ProductId.From(value))
                .IsRequired();

            item.OwnsOne(i => i.Price, price =>
            {
                price.Property(p => p.Currency).HasColumnName("price_currency").HasMaxLength(3).IsRequired();
                price.Property(p => p.Quantity).HasColumnName("price_amount").HasColumnType("numeric(18,2)").IsRequired();
            });
        });

        builder.Navigation(o => o.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(o => o.CustomerId)
            .HasDatabaseName("ix_orders_customer_id");
    }
}