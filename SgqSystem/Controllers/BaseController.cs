using Dominio;
using DTO;
using DTO.DTO;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Helper;
using System.Collections;


namespace SgqSystem.Controllers
{
    [HandleController]
    public class BaseController : Controller
    {
        public BaseController()
        {
           
            ViewBag.UrlDataCollect = GlobalConfig.urlAppColleta;

        }

      

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];
            if (languageCookie != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCookie.Value);
            }
            else
            {
                if (GlobalConfig.Brasil)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
                }
                else if (GlobalConfig.Eua)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
                }
            }

           ViewBag.Resources = Resources.Resource.ResourceManager.GetResourceSet(
                Thread.CurrentThread.CurrentUICulture, true, false).Cast<DictionaryEntry>();

            base.Initialize(requestContext);
        }

        public void CreateCookieFromUserDTO(UserDTO isAuthorized)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("webControlCookie");
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddMinutes(60);
                HttpContext.Response.Cookies.Set(cookie);
            }
            else
            {
                //create a cookie
                HttpCookie myCookie = new HttpCookie("webControlCookie");

                //Add key-values in the cookie
                myCookie.Values.Add("userId", isAuthorized.Id.ToString());
                myCookie.Values.Add("userName", isAuthorized.Name);
                myCookie.Values.Add("CompanyId", isAuthorized.ParCompany_Id.GetValueOrDefault().ToString());

                if (isAuthorized.AlterDate != null)
                {
                    myCookie.Values.Add("alterDate", isAuthorized.AlterDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                }
                else
                {
                    myCookie.Values.Add("alterDate", "");
                }

                myCookie.Values.Add("addDate", isAuthorized.AddDate.ToString("dd/MM/yyyy"));

                if (isAuthorized.Role != null)
                    myCookie.Values.Add("roles", isAuthorized.Role.Replace(';', ',').ToString());//"admin, teste, operacional, 3666,344, 43434,...."
                else
                    myCookie.Values.Add("roles", "");

                if (isAuthorized.ParCompanyXUserSgq != null)
                    if (isAuthorized.ParCompanyXUserSgq.Any(r => r.Role != null))
                        myCookie.Values.Add("rolesCompany", string.Join(",", isAuthorized.ParCompanyXUserSgq.Select(n => n.Role).Distinct().ToArray()));
                    else
                        myCookie.Values.Add("rolesCompany", "");

                //set cookie expiry date-time. Made it to last for next 12 hours.
                myCookie.Expires = DateTime.Now.AddMinutes(60);

                //Most important, write the cookie to client.
                Response.Cookies.Add(myCookie);
            }
        }

        public static void NotifyNewComment(int commentId)
        {

          

            //// Prepare Postal classes to work outside of ASP.NET request
            //var viewsPath = Path.GetFullPath(HostingEnvironment.MapPath(@"~/Views/Emails"));
            //var engines = new ViewEngineCollection();
            //engines.Add(new FileSystemRazorViewEngine(viewsPath));

            //var emailService = new EmailService(engines);

            // Get comment and send a notification.
            using (var db = new SgqDbDevEntities())
            {
                //var comment = db.Comments.Find(commentId);

                //var email = new NewCommentEmail
                //{
                //    To = "yourmail@example.com",
                //    UserName = comment.UserName,
                //    Comment = comment.Text
                //};

                //emailService.Send(email);
            }
        }


    }

}