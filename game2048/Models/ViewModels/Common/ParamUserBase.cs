using System.ComponentModel.DataAnnotations;
using Models.Common;
using Models.Enum;

namespace Models.ViewModels.Common
{
  public  class ParamUserBase
    {
        public int Id { get; set; }
        public string OpenId { get; set; } = string.Empty;
        public string UnionId { get; set; } = string.Empty;
        public OpenTypeEnum OpenType { get; set; }
        public GameTypeEnum GameType { get; set; }
        public long TimeOffset = 0L;
        public string Sign { get; set; }

        public ValidationResult Validate(ParamUserBase param)
        {
            if (param.OpenId != this.OpenId || param.GameType != this.GameType || param.Id != this.Id || param.OpenType != this.OpenType || param.UnionId != this.UnionId||param.TimeOffset != this.TimeOffset)
            {
                return new ValidationResult("数据异常", new[] { nameof(OpenId) });
            }

            if (Id == 0&&OpenType!= OpenTypeEnum.系统默认&&string.IsNullOrEmpty(OpenId))
            {
                return new ValidationResult("数据异常", new[] { nameof(OpenId) });
            }

            return null;
        }

        public string SignKey
        {
            get
            {
                string key = string.Empty;
                switch (GameType)
                {
                    case GameTypeEnum.小程序: key = ConfigHelper.StaticConfig.WXCEncryptionKey;break;
                    case GameTypeEnum.Android: key = ConfigHelper.StaticConfig.AndroidEncryptionKey;break;
                    case GameTypeEnum.IOS: key = ConfigHelper.StaticConfig.IOSEncryptionKey;break;
                    default: key = ConfigHelper.StaticConfig.WXCEncryptionKey;break;
                }
                return key;
            }
        }

    }
}
