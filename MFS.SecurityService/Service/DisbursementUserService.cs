using MFS.SecurityService.Models;
using MFS.SecurityService.Models.Utility;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
    public interface IDisbursementUserService : IBaseService<DisbursementUser>
    {
        AuthDisbursementUser login(LoginModel model);
        AuthDisbursementUser DisbursementUserLogIn(LoginModel model);
        string GetTransAmtLimit(string createUser);
		object IsProceedToController(List<string> userInfos);
		object GetAppUserListDdl();
        object GetAllDisbursementUserList(string roleName);
        PasswordPolicy GetPasswordPolicy();
        object CheckDisbursementUserAlreadyExist(string username);
    }

    public class DisbursementUserService : BaseService<DisbursementUser>, IDisbursementUserService
    {
        public IDisbursementUserRepository disbursementUserRepository;
        public IFeatureService featureService;
        public DisbursementUserService(IDisbursementUserRepository _usersRepo, IFeatureService _featureService)
        {
            disbursementUserRepository = _usersRepo;
            featureService = _featureService;
        }

        public AuthDisbursementUser login(LoginModel model)
        {
            DisbursementUser user = validateLogin(model);
            return BuildAuthUserModel(user);
        }

        public AuthDisbursementUser DisbursementUserLogIn(LoginModel model)
        {
            DisbursementUser user = validateLogin(model);
            return BuildAuthUserModel(user);
        }

        private DisbursementUser validateLogin(LoginModel model)
        {
            StringBuilderService stringBuilderService = new StringBuilderService();

            return disbursementUserRepository.validateLogin(model.UserName, stringBuilderService.GenerateSha1Hash(model.Password));
        }

        private AuthDisbursementUser BuildAuthUserModel(DisbursementUser model)
        {
            AuthDisbursementUser authUserModel = new AuthDisbursementUser();

            authUserModel.User = model;
            if (authUserModel.User.Is_validated)
            {
                authUserModel.IsAuthenticated = true;
				if(authUserModel.User.Pstatus == "Y")
				{
					authUserModel.FeatureList = featureService.GetAuthFeatureList(authUserModel.User.Id);
				}
				else
				{
					authUserModel.FeatureList = new List<dynamic>();
				}
                authUserModel.BearerToken = Guid.NewGuid().ToString();
            }
            else
            {
                authUserModel.IsAuthenticated = false;
            }

            return authUserModel;
        }

        public string GetTransAmtLimit(string createUser)
        {
            try
            {
                return disbursementUserRepository.GetTransAmtLimit(createUser);
            }
            catch (Exception)
            {

                throw;
            }
        }

		public object IsProceedToController(List<string> userInfos)
		{
			var userId = userInfos[0];
			var roleId = userInfos[1];
			var userInfo = (Tuple<string,string>) disbursementUserRepository.IsProceedToController(userInfos);
			if(userInfo.Item1 != roleId || userInfo.Item2 == "Y")
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public object GetAppUserListDdl()
		{
			return disbursementUserRepository.GetAppUserListDdl();
		}

        public object GetAllDisbursementUserList(string roleName)
        {
            return disbursementUserRepository.GetAllDisbursementUserList(roleName);
        }

        public PasswordPolicy GetPasswordPolicy()
        {
            try
            {
                return disbursementUserRepository.GetPasswordPolicy();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public object CheckDisbursementUserAlreadyExist(string username)
        {
            return disbursementUserRepository.CheckDisbursementUserAlreadyExist(username);
        }

    }
}
