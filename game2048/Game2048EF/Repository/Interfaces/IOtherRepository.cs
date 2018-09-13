using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.Dtos;
using Models.ViewModels.Common;
using Models.ViewModels.ParamModel;
using Models.ViewModels.RpsModel;

namespace Game2048EF.Repository.Interfaces
{
  public  interface IOtherRepository
  {
      RpsData<string> GetInviteCode(ParamUserBase param);
      Task<RpsData<RpsInvited>> GetInvitedUsers(ParamUserBase param);
      Task<RpsData<RpsAddWatch>> UploadAdInfo(ParamUserBase param);
      Task<RpsData<IEnumerable<RewardDataDto>>> GetInvitedReward(ParamGetInviteReward param);
      Task<RpsData<IEnumerable<RpsPoster>>> GetUserPoster(ParamUserBase param);
     Task<RpsData<IEnumerable<RpsPosterDetail>>> GetUserPosterDetail(ParamUserPosterDetail param);
      Task<RpsData<RpsUserGetPoster>> GetPoster(ParamGetPoster param);
  }
}
