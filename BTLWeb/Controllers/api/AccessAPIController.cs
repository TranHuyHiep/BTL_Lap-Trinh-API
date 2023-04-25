using BTLWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using BTLWeb.Models.ViewModels;
using Nancy.Json;

namespace BTLWeb.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessAPIController : ControllerBase
    {
        QlbanMayAnhContext db = new QlbanMayAnhContext();

        [HttpPost("Login")]
        public JsonResult Login([FromBody] TUser user)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                /*var f_password = GetMD5(password);*/
                var data = db.TUsers.Where(s => s.Username.Equals(user.Username) && s.Password.Equals(user.Password)).FirstOrDefault();
                if (data != null)
                {
                    //add session
                    if (data.LoaiUser == 0)
                    {
                        return new JsonResult("admin");
                    }
                    else
                    {
                        return new JsonResult("user");
                    }
                }
            }
            return new JsonResult("false");
        }

        public JsonResult VerifyOTP(string oTPClass)
        {
            string otp ="", userString = "";
            TUser user = new JavaScriptSerializer().Deserialize<TUser>(userString);
            if (ModelState.IsValid)
            {
                if (otp.Equals(oTPClass))
                {
                    user.LoaiUser = 1;
                    db.TUsers.Add(user);
                    db.SaveChanges();
                    //HttpContext.Session.Clear();//remove session
                    /*HttpContext.Session.Remove("OTPMail");
                    HttpContext.Session.Remove("UserCheck");
                    HttpContext.Session.Remove("PasswordCheck");*/
                    return new JsonResult("true");
                }
                else
                {
                    return new JsonResult("OTP incorrect");
                }
            }
            return new JsonResult("false");
        }

        //POST: Register
        [HttpPost("Register")]
        public JsonResult Register([FromBody] TUser user)
        {
            if (ModelState.IsValid)
            {
                var check = db.TUsers.FirstOrDefault(s => s.Username == user.Username);
                if (check == null)
                {
                    var otpmail = RandomNumber(6);
                    /*HttpContext.Session.SetString("OTPMail", otpmail);
                    HttpContext.Session.SetString("UserCheck", user.Username);
                    HttpContext.Session.SetString("PasswordCheck", user.Password);*/
                    SendMail(user.Username, "Xác thực đăng ký", "Mã OTP xác thực của bạn là : " + otpmail);
                    return new JsonResult(otpmail);
                }
                else
                {
                    return new JsonResult("Tài khoản đã tồn tại");
                }
            }
            return new JsonResult("Thông tin không hợp lệ");
        }

        public static string RandomNumber(int numberRD)
        {
            string randomStr = "";
            try
            {

                int[] myIntArray = new int[numberRD];
                int x;
                Random autoRand = new Random();
                for (x = 0; x < numberRD; x++)
                {
                    myIntArray[x] = System.Convert.ToInt32(autoRand.Next(0, 9));
                    randomStr += (myIntArray[x].ToString());
                }
            }
            catch (Exception ex)
            {
                randomStr = "error";
            }
            return randomStr;
        }

        //thư viện gửi mail - có sẵn, thay thông tin của mail chủ mình vào
        public void SendMail(String to, String subject, String content)
        {
            String mailAddress = "hiptrangpt@gmail.com";
            String mailPassword = "qiwsxspjamjelrcb"; // lấy mk ứng dụng từ tài khoản gg 

            MailMessage msg = new MailMessage(new MailAddress(mailAddress), new MailAddress(to));
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = content;

            var client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(mailAddress, mailPassword);
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.Send(msg);
        }
    }
}
