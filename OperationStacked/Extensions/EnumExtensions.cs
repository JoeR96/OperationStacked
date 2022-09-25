namespace OperationStacked.Extensions
{
    public static class EnumExtensions
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
