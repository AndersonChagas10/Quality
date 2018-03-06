using AutoMapper;
using Dominio;
using DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class FirstConfigController : Controller
    {

        //private IBaseDomain< _defectDomain;

        //public FirstConfigController(IDefectDomain defectDomain)
        //{
        //    _defectDomain = defectDomain;
        //}

        // GET: FirstConfig
        public ActionResult Index()
        {
            List<SelectListItem> listaProjetos = new List<SelectListItem>();
            listaProjetos.Add(new SelectListItem() { Text = "BRASIL", Value = "1" });
            listaProjetos.Add(new SelectListItem() { Text = "EUA", Value = "2" });
            listaProjetos.Add(new SelectListItem() { Text = "YTOARA", Value = "3" });

            ViewBag.ActiveIn = new SelectList(listaProjetos, "Value", "Text");

            return View();
        }


        // GET: FirstConfig
        [HttpPost]
        public ActionResult Index(FirstConfigDTO cfg)
        {

            using (var db = new ADOFactory.Factory("DbContextSgqEUA"))/*Caso nao configurado, procura config no DB*/
            {
                var identityId = " \n SELECT CAST(scope_identity() AS int)";

                #region ParCompany
                cfg.parCompanyDto.AddDate = DateTime.Now;
                var query = $@"insert into ParCompany
                    (Name, Description, AddDate, IsActive, Initials)
                    values
                    ('{cfg.parCompanyDto.Name}','{cfg.parCompanyDto.Description}','{cfg.userSgqDto.AddDate}',1,'{cfg.parCompanyDto.Initials}');
                    { identityId }";

                using (SqlCommand cmd = new SqlCommand(query, db.connection))
                {
                    cfg.parCompanyDto.Id = (int)cmd.ExecuteScalar();
                }
                #endregion

                #region UserSGQ
                cfg.userSgqDto.AddDate = DateTime.Now;
                cfg.userSgqDto.Password = Guard.EncryptStringAES(cfg.userSgqDto.Password);
                cfg.userSgqDto.ParCompany_Id = cfg.parCompanyDto.Id;
                query =
                    $@"insert into UserSgq 
                    (Name, Password, AddDate, Role, FullName, Email, Phone, ParCompany_Id, PasswordDate)
                    values
                    ('{cfg.userSgqDto.Name}','{cfg.userSgqDto.Password}','{cfg.userSgqDto.AddDate}','admin','{cfg.userSgqDto.FullName}',
'{cfg.userSgqDto.Email}','{cfg.userSgqDto.Phone}','{cfg.userSgqDto.ParCompany_Id}','{cfg.userSgqDto.AddDate}')
                    { identityId }";

                using (SqlCommand cmd = new SqlCommand(query, db.connection))
                {
                    cfg.userSgqDto.Id = (int)cmd.ExecuteScalar();
                }
                #endregion

                #region Vinculo ParCompany
                query =
                    $@"insert into ParCompanyXUserSgq
                    (UserSgq_Id, ParCompany_Id, Role)
                    values
                    ('{cfg.userSgqDto.Id}','{cfg.parCompanyDto.Id}','admin');
                    { identityId }";

                using (SqlCommand cmd = new SqlCommand(query, db.connection))
                {
                    cmd.ExecuteScalar();
                }
                #endregion


                //var resultUser = db.InsertUpdateData(cfg.userSgqDto);

                var result = db.InsertUpdateData(cfg.SgqConfig);
                GlobalConfig.ConfigWebSystem(result);
            }

            return RedirectToAction("Index", "Home");
        }

        public class FirstConfigDTO{
            public SgqConfig SgqConfig { get; set; }
            /*Wizard Inicial*/
            public UserSgq userSgqDto { get; set; }
            public ParCompany parCompanyDto { get; set; }
        }
    }
}