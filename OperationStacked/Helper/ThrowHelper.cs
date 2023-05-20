using Serilog;

public static class ThrowHelper
{
    public static void ThrowArgumentNullException(string paramName)
    {
        Log.Error(new ArgumentNullException(paramName), "ArgumentNullException occurred for parameter {ParamName}", paramName);
        throw new ArgumentNullException(paramName);
    }
}