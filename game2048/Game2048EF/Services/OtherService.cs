using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Game2048EF.Repository.Interfaces;
using Game2048EF.Services.Interfaces;
using Models.Dtos;
using Models.ViewModels.Common;
using Models.ViewModels.ParamModel;
using Models.ViewModels.RpsModel;

namespace Game2048EF.Services
{
 public   class OtherService:IOtherService
 {
     private readonly IOtherRepository _otherRepository;

     public OtherService(IOtherRepository otherRepository)
     {
         _otherRepository = otherRepository;
     }

        public async Task<RpsData<RpsAddWatch>> UploadAdInfo(ParamUserBase param)
        {
            return await _otherRepository.UploadAdInfo(param);
        }

        public RpsData<string> GetInviteCode(ParamUserBase param)
        {
            return _otherRepository.GetInviteCode(param);
        }

        public async Task<RpsData<RpsInvited>> GetInvitedUsers(ParamUserBase param)
        {
            return await _otherRepository.GetInvitedUsers(param);
        }

        public async Task<RpsData<IEnumerable<RewardDataDto>>> GetInvitedReward(ParamGetInviteReward param)
        {
            return await _otherRepository.GetInvitedReward(param);
        }

        public async Task<RpsData<IEnumerable<RpsPoster>>> GetUserPoster(ParamUserBase param)
        {
            return await _otherRepository.GetUserPoster(param);
        }

        public async Task<RpsData<IEnumerable<RpsPosterDetail>>> GetUserPosterDetail(ParamUserPosterDetail param)
        {
            return await _otherRepository.GetUserPosterDetail(param);
        }

        public async Task<RpsData<RpsUserGetPoster>> GetPoster(ParamGetPoster param)
        {
            return await _otherRepository.GetPoster(param);
        }
    }
}
