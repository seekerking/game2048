namespace Models.Common
{
    public class Config
    {
        public string Connection { get; set; }

        public string JwtSecretKey { get; set; }

        public string WXCEncryptionKey { get; set; }

        public string AndroidEncryptionKey { get; set; }

        public string IOSEncryptionKey { get; set; }

        public int LimitInviteReward { get; set; } = 5;

        public int WatchAddReward { get; set; } = 2000;

        public string DefaultUserIcon { get; set; } = string.Empty;

    }
}