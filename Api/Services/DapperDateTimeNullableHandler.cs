using Dapper;
using System.Data;

namespace Api.Services;

public class DapperDateTimeNullableHandler : SqlMapper.TypeHandler<DateTime?>
{
    public override DateTime? Parse(object value)
    {
        if (value == null || value is DBNull)
            return null;

        if (value is DateTime dt)
            return dt;

        if (value is string str && string.IsNullOrWhiteSpace(str))
            return null;

        // Intentar parsear como DateTime
        if (DateTime.TryParse(value.ToString(), out var result))
            return result;

        return null;
    }

    public override void SetValue(IDbDataParameter parameter, DateTime? value)
    {
        parameter.Value = value ?? (object)DBNull.Value;
    }
}
