namespace Models.ViewModels.RpsModel
{


    public class RpsLiteUser
    {
        public int Id { get; set; }

        public string OpenId { get; set; }

        public string UnionId { get; set; }

        public int OpenType { get; set; }

        public int GameType { get; set; }

        public string NickName { get; set; }

        public string Icon { get; set; }
    }
    public class RpsUser : RpsLiteUser
    {

        public string Token { get; set; }
    }
}
