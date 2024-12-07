using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectPet.Framework.EFExtensions;
public static class EFExtensions
{
    public static PropertyBuilder<Type> JsonVOConverter<Type>(this PropertyBuilder<Type> builder)
    {
        return builder.HasConversion(
                push => JsonSerializer.Serialize(push, JsonSerializerOptions.Default),
                pull => JsonSerializer.Deserialize<Type>(pull, JsonSerializerOptions.Default)!);
    }

    public static PropertyBuilder<IReadOnlyList<Type>> JsonVOCollectionConverter<Type>(this PropertyBuilder<IReadOnlyList<Type>> builder)
    {
        return builder.HasConversion(
                push => JsonSerializer.Serialize(push, JsonSerializerOptions.Default),
                pull => JsonSerializer.Deserialize<IReadOnlyList<Type>>(pull, JsonSerializerOptions.Default)!,
                new ValueComparer<IReadOnlyList<Type>>
                (
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                ));
    }
}
