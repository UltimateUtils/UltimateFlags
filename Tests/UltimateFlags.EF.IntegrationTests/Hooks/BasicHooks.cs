using Reqnroll;
using Reqnroll.Assist;
using Reqnroll.Assist.ValueRetrievers;

namespace UltimateFlags.EF.IntegrationTests.Hooks;

[Binding]
public class BasicHooks
{
    [BeforeTestRun]
    public static void RegisterNullValueRetriever()
    {
        Service.Instance.ValueRetrievers.Register(new NullValueRetriever("<null>"));
    }

    // [BeforeTestRun]
    // public static void RegisterGuidValueRetriever()
    // {
    //     Service.Instance.ValueRetrievers.Register(new GuidValueRetriever());
    // }
}
