using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKVideo.Core
{
    public class _12306UrlConfig
    {
        /// <summary>
        /// 登录地址
        /// </summary>
        public const string LoginUrl = "https://kyfw.12306.cn/otn/login/loginAysnSuggest";
        /// <summary>
        /// 校验登录地址
        /// </summary>
        public const string CheckLoginUrl = "https://kyfw.12306.cn/otn/login/checkUser";

        /// <summary>
        /// 初始化cookie地址
        /// </summary>
        public const string LoginCookieUrl = "https://kyfw.12306.cn/otn/login/init";

        /// <summary>
        /// 验证码地址
        /// </summary>
        public const string RandCodeUrl = "https://kyfw.12306.cn/otn/passcodeNew/getPassCodeNew?module=login&rand=sjrand&{0}";
        /// <summary>
        /// 验证码校验地址
        /// </summary>
        public const string CheckRandCodeUrl = "https://kyfw.12306.cn/otn/passcodeNew/checkRandCodeAnsyn";

        /// <summary>
        /// 获取联系人列表地址
        /// </summary>
        public const string GetPassengersUrl = "https://kyfw.12306.cn/otn/confirmPassenger/getPassengerDTOs";
    }
}
