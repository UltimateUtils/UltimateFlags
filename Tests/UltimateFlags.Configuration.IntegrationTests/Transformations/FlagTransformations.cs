using Reqnroll;
using UltimateUtils.Extensions;

namespace UltimateFlags.Configuration.IntegrationTests.Transformations;

[Binding]
public class FlagTransformations
{
    [StepArgumentTransformation]
    public static Guid? TransformNullableGuid(string value)
    {
        if (value.IsNullOrWhiteSpace() || value == "<null>")
            return null;

        if (Guid.TryParse(value, out Guid ret))
            return ret;

        throw new ArgumentException($"Invalid Guid format, {value}");
    }

    [StepArgumentTransformation]
    public IEnumerable<Guid?> TransformTableToGuidList(DataTable dataTable)
    {
        return
            dataTable
                .Rows
                .Select(
                    row =>
                    {
                        if (row[0] == null || row[0].IsNullOrWhiteSpace() || row[0] == "<null>")
                        {
                            return null;
                        }

                        return (Guid?)Guid.Parse(row[0]);
                    });
    }
}
