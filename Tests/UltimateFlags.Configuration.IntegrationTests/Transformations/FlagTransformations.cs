using Reqnroll;
using UltimateUtils.Extensions;

namespace UltimateFlags.Configuration.IntegrationTests.Transformations;

[Binding]
public class FlagTransformations
{
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
