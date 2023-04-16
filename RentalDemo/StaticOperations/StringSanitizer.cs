namespace RentalDemo.StaticOperations
{
    public static class StringSanitizer
    {
        public static string SanitizeSqlString(string value)
        {
            value.Replace("'", "''");
            return value;
        }
    }
}
