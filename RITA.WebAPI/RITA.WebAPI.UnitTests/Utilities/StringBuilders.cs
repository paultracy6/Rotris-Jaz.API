namespace RITA.WebAPI.UnitTests.Utilities;

public static class StringBuilders
{
    public static string BuildStringForLength(int stringLength)
    {
        var testString = string.Empty;

        for (int i = 0; i < stringLength; i++)
        {
            testString += "X";
        }

        return testString;
    }
}