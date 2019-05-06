﻿using ADOFactory;
using Dominio;
using DTO;
using DTO.DTO;
using SGQDBContext;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{

    [HandleApi()]
    [RoutePrefix("api/ResultLevel3PhotosApi")]
    public class ResultLevel3PhotosApiController : ApiController
    {

        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ResultLevel3PhotosApiController()
        {

        }

        [HttpPost]
        public IHttpActionResult Insert([FromBody] List<Result_Level3_PhotosDTO> Fotos)
        {
            //, int Level1Id, int Level2Id, int Level3Id, int Evaluation, int Sample, string Date
            string quebraProcesso = "98789";

            for (int i = 0; i < Fotos.Count; i++)
            {
                var ResultPhoto = Fotos[i];

                string parCluster_Id_parLevel1_id = ResultPhoto.Level1Id.ToString().Replace(quebraProcesso, "|"); //"Cluster|Indicador"
                string parCluster_Id = parCluster_Id_parLevel1_id.Split('|').Length > 1 ? parCluster_Id_parLevel1_id.Split('|')[0] : "0";
                string parLevel1_Id = parCluster_Id_parLevel1_id.Split('|').Length > 1 ? parCluster_Id_parLevel1_id.Split('|')[1] : parCluster_Id_parLevel1_id.Split('|')[0];

                string parCluster_Id_parLevel2_id = ResultPhoto.Level2Id.ToString().Replace(quebraProcesso, "|");
                string parLevel2_Id = parCluster_Id_parLevel2_id.Split('|').Length > 1 ? parCluster_Id_parLevel2_id.Split('|')[1] : parCluster_Id_parLevel2_id.Split('|')[0];

                ResultPhoto.Level1Id = Convert.ToInt32(parLevel1_Id);
                ResultPhoto.Level2Id = Convert.ToInt32(parLevel2_Id);

                #region Upload Photos
                var bytes = Convert.FromBase64String(ResultPhoto.Photo);

                var basePath = System.Configuration.ConfigurationManager.AppSettings["StorageRoot"] ?? "~";
                if (basePath.Equals("~"))
                {
                    basePath = @AppDomain.CurrentDomain.BaseDirectory;
                }

                var path = Path.Combine(basePath, "photos", parLevel1_Id + parLevel2_Id + DateTime.Now.ToString("yyyyMMddHHssmm")) + new Random().Next(1000,9999) + ".png";
                using (var imageFile = new FileStream(path, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                ResultPhoto.Photo = path;
                ResultPhoto.Photo_Thumbnaills = ResultPhoto.Photo;
                #endregion

                string sqlResulLevel3 = @"SELECT R.Id FROM Result_Level3 R                                     
                            LEFT JOIN CollectionLevel2 C                                      
                            ON R.CollectionLevel2_Id = C.Id                                
                            WHERE                                                         
                            C.EvaluationNumber = @Evaluation AND                                           
                            C.Sample = @Sample AND                                              
                            C.ParLevel1_Id = @Level1Id AND                                       
                            C.ParLevel2_Id = @Level2Id AND                                         
                            R.ParLevel3_Id = @Level3Id AND                                           
                            C.UnitId = @UnitId AND                                           
                            C.Shift = @Shift AND                                           
                            C.Period = @Period AND                                           
                            C.CollectionDate BETWEEN @CollectionDate+' 00:00' AND @CollectionDate+' 23:59'";

                try
                {
                    using (SqlConnection connection = new SqlConnection(conexao))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(sqlResulLevel3, connection))
                        {
                            command.Parameters.Add("@Evaluation", SqlDbType.Int).Value = ResultPhoto.Evaluation;
                            command.Parameters.Add("@Sample", SqlDbType.Int).Value = ResultPhoto.Sample;
                            command.Parameters.Add("@Level1Id", SqlDbType.Int).Value = ResultPhoto.Level1Id;
                            command.Parameters.Add("@Level2Id", SqlDbType.Int).Value = ResultPhoto.Level2Id;
                            command.Parameters.Add("@Level3Id", SqlDbType.Int).Value = ResultPhoto.Level3Id;
                            command.Parameters.Add("@UnitId", SqlDbType.Int).Value = ResultPhoto.UnitId;
                            command.Parameters.Add("@Period", SqlDbType.Int).Value = ResultPhoto.Period;
                            command.Parameters.Add("@Shift", SqlDbType.Int).Value = ResultPhoto.Shift;
                            command.Parameters.Add("@CollectionDate", SqlDbType.NVarChar).Value = DateFormatToAnother(ResultPhoto.ResultDate, "MMddyyyy", "yyyy-MM-dd");
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    for (int j = 0; j < reader.FieldCount; j++)
                                    {
                                        ResultPhoto.Result_Level3_Id = reader.GetInt32(j);
                                    }
                                }
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToClient());
                }

                if (ResultPhoto.Result_Level3_Id == 0)
                {
                    return Ok(new { message = "ResultLevel3Id não encontrado.", count = i+1 });
                }

                string sql = @"INSERT INTO Result_Level3_Photos(Result_Level3_Id, Photo_Thumbnaills, Photo, Latitude, Longitude) 
                            VALUES(@Result_Level3_Id, @Photo_Thumbnaills, @Photo, @Latitude, @Longitude)";

                try
                {
                    using (SqlConnection connection = new SqlConnection(conexao))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.Add("@Result_Level3_Id", SqlDbType.BigInt).Value = ResultPhoto.Result_Level3_Id;
                            command.Parameters.Add("@Photo_Thumbnaills", SqlDbType.NText).Value = ResultPhoto.Photo_Thumbnaills;
                            command.Parameters.Add("@Photo", SqlDbType.NText).Value = ResultPhoto.Photo;
                            command.Parameters.Add("@Latitude", SqlDbType.Float).Value = ResultPhoto.Latitude;
                            command.Parameters.Add("@Longitude", SqlDbType.Float).Value = ResultPhoto.Longitude;

                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToClient());
                }
            }

            return Ok(new { message = "Fotos inseridas com sucesso.", count = Fotos.Count });

        }

        private String DateFormatToAnother(String date, String format, String anotherFormat)
        {
            if (string.IsNullOrEmpty(date))
            {
                return DateTime.MinValue.ToString(anotherFormat);
            }

            if (date.Length == 10)
            {
                date += " 00:00:00";
            }

            DateTime datetime = DateTime.MinValue;

            if (DateTime.TryParseExact(
                    date,
                    format,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out datetime) || DateTime.TryParseExact(
                    date,
                    format.Replace("/", "."),
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out datetime))
            {
                return datetime.ToString(anotherFormat);
            }
            else
            {
                return DateTime.MinValue.ToString(anotherFormat);
            }
        }

    }

}
