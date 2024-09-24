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

        public static string GeneratePictureId()
        {
            return GenerateIdWithPrefix("pic_");
        }

        public static string GenerateLikeId()
        {
            return GenerateIdWithPrefix("lik_");
        }

        private static string GenerateIdWithPrefix(string prefix)
        {
            var randomChars = new string(Enumerable.Repeat(Characters, 32)
                                                   .Select(s => s[Random.Next(s.Length)])
                                                   .ToArray());
            return $"{prefix}{randomChars}";
        }
    }
}
