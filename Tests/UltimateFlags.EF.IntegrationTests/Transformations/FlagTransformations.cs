using Reqnroll;
using UltimateUtils.Extensions;

namespace UltimateFlags.EF.IntegrationTests.Transformations;

[Binding]
public class FlagTransformations
{
    // [StepArgumentTransformation]
    // public static IEnumerable<Flag> TransformFlags(DataTable flagsTable)
    // {
    //     return flagsTable.CreateSet<Flag>();
    // }

    [StepArgumentTransformation]
    public static Guid? TransformNullableGuid(string value)
    {
        if (value.IsNullOrWhiteSpace() || value == "<null>")
            return null;

        if (Guid.TryParse(value, out Guid ret))
            return ret;

        throw new ArgumentException($"Invalid Guid format, {value}");
    }
}
