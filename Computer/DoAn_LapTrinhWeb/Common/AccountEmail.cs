using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.Common
{
    public class AccountEmail
    {
        //vui lòng không đổi mật khẩu mail, néu mật khảu bị thay đổi vui lòng nhắn tin qua zalo:0866658801
        //để dùng google email gửi email reset cho người khác bạn cần phải vô đây "https://www.google.com/settings/security/lesssecureapps" Cho phép ứng dụng kém an toàn: Bật
        public const string Host = "mail8971.maychuemail.com"; //tên mấy chủ nếu bạn dùng gmail thì đổi  "Host = "smtp.gmail.com"
        //tài khoản email của bạn
        public const string UserEmail = "noreply@nvtcomputer.tech";
        public const string UserEmailSupport = "support@nvtcomputer.tech";
        //Mật khẩu Email của bạn
        public const string Password = "123456a@";
        //Tên Email bạn muốn hiển thị khi gửi
        public const string Name = "NVTComputer";
    }
}