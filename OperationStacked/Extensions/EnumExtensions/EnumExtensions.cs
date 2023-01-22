namespace OperationStacked.Extensions.EnumExtensions
{
    public static class EnumExtensions
    {
        public static T ParseEnum<T>(string value)
            => (T)Enum.Parse(typeof(T), value, true);

    }
}
