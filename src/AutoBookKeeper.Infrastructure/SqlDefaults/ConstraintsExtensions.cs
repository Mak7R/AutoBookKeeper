using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoBookKeeper.Infrastructure.SqlDefaults;

public static class ConstraintsExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Only for Postgres</remarks>
    /// <param name="builder"></param>
    /// <param name="propertyName"></param>
    /// <param name="minLength"></param>
    /// <returns></returns>
    public static CheckConstraintBuilder HasMinLengthCheckConstraint(this TableBuilder builder, string propertyName, int minLength)
    {
        return builder.HasCheckConstraint($"CK_MinLength_{propertyName}", $"LENGTH(\"{propertyName}\") >= {minLength}");
    }
}