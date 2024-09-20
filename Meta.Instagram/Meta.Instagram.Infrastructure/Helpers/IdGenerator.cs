namespace Meta.Instagram.Infrastructure.Helpers
{
    public static class IdGenerator
    {
        private const string Characters = "abcdefghijklmnopqrstuvwxyz0123456789";
        private static readonly Random Random = new();

        public static string GenerateAccountId()
        {
            return GenerateIdWithPrefix("acc_");
        }

        public static string GenerateProfileId()
        {
            return GenerateIdWithPrefix("prf_");
        }

        private static string GenerateIdWithPrefix(string prefix)
        {
            var randomChars = new string(Enumerable.Repeat(Characters, 10)
                                                   .Select(s => s[Random.Next(s.Length)])
                                                   .ToArray());
            return $"{prefix}{randomChars}";
        }
    }
}
