using Newtonsoft.Json;
using NUnit.Framework;

namespace RITA.WebAPI.UnitTests.Utilities;

public static class ComparerUtility
{
    public static void AreEqual(object? expected, object? actual)
    {
        Assert.AreEqual(
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(expected)),
            JsonConvert.DeserializeObject(JsonConvert.SerializeObject(actual)));
    }
}