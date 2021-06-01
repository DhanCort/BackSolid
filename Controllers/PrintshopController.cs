/*TASK RP. ATTRIBUTE PROCESS TYPE*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.Infrastructure;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TowaStandard;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 5, 2020.

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //  var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
    //                                                      //  String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class PrintshopController : Controller
    {
        //                                                  //Controller associated with the actions related to
        //                                                  //      printshop´s configurations.

        //                                                  //The post methods receive a json, for the validations are
        //                                                  //      used the ValueKind property that could have the 
        //                                                  //      following values.
        //                                                  //JsonElement.ValueKind:
        //                                                  //      1 - Json Object
        //                                                  //      2 - Json array
        //                                                  //      3 - Json string
        //                                                  //      4 - Json number
        //                                                  //      5 - Json value true
        //                                                  //      6 - Json value false
        //                                                  //      7 - Json value null
        //                                                  //      0 - There is no value

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        private IConfiguration configuration;

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        public PrintshopController(IConfiguration iConfiguration_I)
        {
            this.configuration = iConfiguration_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult ModifySpecialPassword(
            //                                              //PURPOSE:
            //                                              //Edit the Special password.

            //                                              //URL: http://localhost/Odyssey2/Printshop
            //                                              //      /ModifySpecialPassword.
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //	                                            //          "strCurrentPassword" : "printshop13832",
            //	                                            //          "strNewPassword" : "password",
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Modify the current special password for a printshop.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Dynamic object that contains all necessary data.
            [FromBody] JsonElement jsonSpecial
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonSpecial) &&
                //                                          //Verify that all the parameters are in the Json and they
                //                                          //      are from the correct type.
                jsonSpecial.TryGetProperty("strCurrentPassword", out json) &&
                (int)jsonSpecial.GetProperty("strCurrentPassword").ValueKind == 3 &&
                jsonSpecial.TryGetProperty("strNewPassword", out json) &&
                (int)jsonSpecial.GetProperty("strNewPassword").ValueKind == 3
                )
            {
                //                                          //Get the values an assign them to a variable.
                String strCurrentPassword = jsonSpecial.GetProperty("strCurrentPassword").GetString();
                String strNewPassword = jsonSpecial.GetProperty("strNewPassword").GetString();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Printshop does not exist.";
                if (
                    ps != null
                    )
                {
                    try
                    {
                        PsPrintShop.subModifySpecialPassword(ps, strCurrentPassword, strNewPassword, ref intStatus, ref
                        strUserMessage, ref strDevMessage);
                    }
                    catch (Exception ex)
                    {
                        //                                      //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }                    
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SetReportFilter(
            //                                              //PURPOSE:
            //                                              //add or update a report filter.

            //                                              //URL: http://localhost/Odyssey2/Printshop
            //                                              //      /SetReportFilter 
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //	                                            //          "intnPk":null,
            //                                              //          "strDataSet":"Jobs",
            //                                              //          "strName":"My filter A",
            //                                              //          "filter":{ }
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Create a registr for a reportFilter or update an existing

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Dynamic object that contains all necessary data.
        [FromBody] JsonElement jsonReport
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if json is not null or a value is not empty.
                !((int)jsonReport.ValueKind == 7) &&
                !((int)jsonReport.ValueKind == 0) &&
                //                                          //Verify that all the parameters are in the Json and they
                //                                          //      are from the correct type.
                jsonReport.TryGetProperty("intnPk", out json) &&
                jsonReport.TryGetProperty("strDataSet", out json) &&
                (int)jsonReport.GetProperty("strDataSet").ValueKind == 3 &&
                jsonReport.TryGetProperty("strName", out json) &&
                (int)jsonReport.GetProperty("strName").ValueKind == 3 &&
                jsonReport.TryGetProperty("filter", out json) &&
                (int)jsonReport.GetProperty("filter").ValueKind == 1
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                var idClaimSuperAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsSuperAdmin");
                bool boolSuperAdmin = idClaimSuperAdmin.Value.ParseToBool();

                //                                          //Get the values an assign them to a variable.

                String strDataSet = jsonReport.GetProperty("strDataSet").GetString();
                String strName = jsonReport.GetProperty("strName").GetString();
                String filter = JsonSerializer.Serialize(jsonReport.GetProperty("filter"));

                int? intnPk = null;
                if (
                    (int)jsonReport.GetProperty("intnPk").ValueKind == 4
                    )
                {
                    intnPk = jsonReport.GetProperty("intnPk").GetInt32();
                }

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            intStatus = 401;
                            strUserMessage = "Something is wrong.";
                            strDevMessage = "Filter can not be empty";
                            if (
                                filter.Length > 2
                                )
                            {
                                int intPkReportNew;
                                PsPrintShop.subSetReportFilter(intnPk, boolSuperAdmin, strDataSet, strName, filter, ps, 
                                    out intPkReportNew, context, ref intStatus, ref strUserMessage, ref strDevMessage);

                                obj = intPkReportNew;

                                //                              //Commits all changes made to the database in the current
                                //                              //      transaction.
                                if (
                                    intStatus == 200
                                    )
                                {
                                    dbContextTransaction.Commit();
                                }
                                else
                                {
                                    dbContextTransaction.Rollback();
                                }
                            }                            
                        }
                        catch (Exception ex)
                        {
                            //                              //Discards all changes made to the database in the current
                            //                              //      transaction.
                            dbContextTransaction.Rollback();

                            intStatus = 400;
                            strUserMessage = "Something is wrong.";
                            strDevMessage = ex.Message;
                        }
                    }
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult DeleteReportFilter(
            //                                              //PURPOSE:
            //                                              //Delete a report filter from DB.

            //                                              //URL: http://localhost/Odyssey2/Printshop
            //                                              //      /DeleteReportFilter 
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //	                                            //          "intPk": 5 
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Delete a report filter from DB.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Dynamic object that contains all necessary data.
            [FromBody] JsonElement jsonReport
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if json is not null or a value is not empty.
                !((int)jsonReport.ValueKind == 7) &&
                !((int)jsonReport.ValueKind == 0) &&
                //                                          //Verify that all the parameters are in the Json and they
                //                                          //      are from the correct type.
                jsonReport.TryGetProperty("intPk", out json) &&
                (int)jsonReport.GetProperty("intPk").ValueKind == 4
                )
            {
                //                                          //Get the values an assign them to a variable.
                int intPk = jsonReport.GetProperty("intPk").GetInt32();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                var idClaimSuperAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsSuperAdmin");
                bool boolSuperAdmin = idClaimSuperAdmin.Value.ParseToBool();

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            PsPrintShop.subDeleteReportFilter(intPk, boolSuperAdmin, context, ref intStatus, 
                                ref strUserMessage, ref strDevMessage);

                            //                              //Commits all changes made to the database in the current
                            //                              //      transaction.
                            if (
                                intStatus == 200
                                )
                            {
                                dbContextTransaction.Commit();
                            }
                            else
                            {
                                dbContextTransaction.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            //                              //Discards all changes made to the database in the current
                            //                              //      transaction.
                            dbContextTransaction.Rollback();

                            intStatus = 400;
                            strUserMessage = "Something is wrong.";
                            strDevMessage = ex.Message;
                        }
                    }
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SetOffset(
            //                                              //PURPOSE:
            //                                              //Set a number where order numeration will start.

            //                                              //URL: http://localhost/Odyssey2/Printshop
            //                                              //      /SetOffset 
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //	                                            //          "intOffset": 5 
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Set a number where order numeration will start. The 
            //                                              //      number should be greather or equal to 0.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Dynamic object that contains all necessary data.
            [FromBody] JsonElement jsonOffset
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if json is not null or a value is not empty.
                !((int)jsonOffset.ValueKind == 7) &&
                !((int)jsonOffset.ValueKind == 0) &&
                //                                          //Verify that all the parameters are in the Json and they
                //                                          //      are from the correct type.
                jsonOffset.TryGetProperty("intOffset", out json) &&
                (int)jsonOffset.GetProperty("intOffset").ValueKind == 4
                )
            {
                //                                          //Get the values an assign them to a variable.
                int intOffset = jsonOffset.GetProperty("intOffset").GetInt32();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            PsPrintShop.subSetOffsetNumber(intOffset, ps, context, this.configuration, ref intStatus,
                                ref strUserMessage, ref strDevMessage);

                            //                              //Commits all changes made to the database in the current
                            //                              //      transaction.
                            if (
                                intStatus == 200
                                )
                            {
                                dbContextTransaction.Commit();
                            }
                            else
                            {
                                dbContextTransaction.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            //                              //Discards all changes made to the database in the current
                            //                              //      transaction.
                            dbContextTransaction.Rollback();

                            //                              //Making a log for the exception.
                            Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                        }
                    }
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult UpdateTimeZone(
            //                                              //PURPOSE:
            //                                              //Set a zonetime.

            //                                              //URL: http://localhost/Odyssey2/Printshop
            //                                              //      /UpdateTimeZone 
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //	                                            //          "strTimeZoneId": "US_CENTRAL" 
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Set a zonetime to a printshop.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Dynamic object that contains all necessary data.
            [FromBody] JsonElement jsonTimezone
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if json is not null or a value is not empty.
                !((int)jsonTimezone.ValueKind == 7) &&
                !((int)jsonTimezone.ValueKind == 0) &&
                //                                          //Verify that all the parameters are in the Json and they
                //                                          //      are from the correct type.
                jsonTimezone.TryGetProperty("strTimeZoneId", out json) &&
                (int)jsonTimezone.GetProperty("strTimeZoneId").ValueKind == 3
                )
            {
                //                                          //Get the values an assign them to a variable.
                String strTimezone = jsonTimezone.GetProperty("strTimeZoneId").GetString();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                var idClaimOwner = User.Claims.FirstOrDefault(c => c.Type == "boolIsOwner");
                bool boolIsOwner = idClaimOwner.Value.ParseToBool();

                var idClaimAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsAdmin");
                bool boolIsAdmin = idClaimAdmin.Value.ParseToBool();

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "You do not have privileges to perform this action.";
                if (
                    (boolIsOwner || boolIsAdmin)
                    )
                {
                    try
                    {
                        PsPrintShop.subUpdateTimeZone(strTimezone, ps, ref intStatus, ref strUserMessage,
                            ref strDevMessage);
                    }
                    catch (Exception ex)
                    {
                        //                                  //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetTimesZones(
            //                                              //PURPOSE:
            //                                              //Get the list of TimeZones.

            //                                              //URL: http://localhost/Odyssey2/Printshop/GetTimesZones
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the possible timezone a printshop has to select.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";
            Object obj = null;

            if (
                //                                          //Printshop valid.
                ps != null
                )
            {
                try
                {
                    //                                      //timezones's list to send back.
                    List<TimzonjsonTimesZonesJson> darrtimzonjson = ps.darrTimzonList;

                    //                                      //If there is already a timezoned setted by the printshop,
                    //                                      //      set this timezone as the selected.
                    if (
                        ps.strTimeZone != null

                        )
                    {
                        //                                  //TimeZone selected by printshop.
                        TimzonjsonTimesZonesJson timzonjson = darrtimzonjson.FirstOrDefault(tz =>
                            tz.strTimeZoneId == ps.strTimeZone);
                        timzonjson.boolSelected = true;
                    }

                    obj = darrtimzonjson.OrderBy(timezone => timezone.strTimeZone);
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }            

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetEmployees(
            //                                              //PURPOSE:
            //                                              //Get employees for the printshop.

            //                                              //URL: http://localhost/Odyssey2/Printshop/GetEmployees
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the employees from Wisnet for the printshop.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            bool boolOwnerIncluded
            )
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";
            Object obj = null;

            try
            {
                Empljson2EmployeeJson2 empljson;
                PsPrintShop.subGetEmployees(boolOwnerIncluded, ps, out empljson, ref intStatus, ref strUserMessage, ref strDevMessage);
                obj = empljson;
            }
            catch (Exception ex)
            {
                //                                      //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }
            
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetPrintshopCompanies(
           //                                              //PURPOSE:
           //                                              //Get the companies for a specific printshop.

           //                                              //URL: http://localhost/Odyssey2/Printshop/
           //                                              //     GetPrintshopCompanies
           //                                              //Method: GET.

           //                                              //DESCRIPTION:
           //                                              //Get printhsop's companies.

           //                                              //RETURNS:
           //                                              //      200 - Ok().
           )
        {
            //                                              //Get printshop.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Printshop not found.";
            Object obj = null;
            if (
                ps != null
                )
            {
                try
                {
                    List<ComjsonCompanyJson> darrcomjson;
                    PsPrintShop.subGetPrintshopCompanies(ps, out darrcomjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = darrcomjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }
            
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetPrintshopCategories(
            //                                              //PURPOSE:
            //                                              //Get the companies for a specific printshop.

            //                                              //URL: http://localhost/Odyssey2/Printshop/
            //                                              //     GetPrintshopCategories 
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get printhsop's products' categories.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";
            Object obj = null;

            try
            {
                List<CatejsonCategoryJson> darrcatejson;
                PsPrintShop.subGetPrintshopCategories(strPrintshopId, out darrcatejson, ref intStatus,
                    ref strUserMessage, ref strDevMessage);
                obj = darrcatejson;
            }
            catch (Exception ex)
            {
                //                                      //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }            

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetPrintshopCompanyBranches(
            //                                              //PURPOSE:
            //                                              //Get the branches for a specific company.

            //                                              //URL: http://localhost/Odyssey2/Printshop/
            //                                              //     GetPrintshopCompanyBranches?intCompanyId  
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get company's branches.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int? intnCompanyId
            )
        {
            //                                              //Find printshop.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Printshop not found.";
            Object obj = null;
            if (
                ps != null
                )
            {
                try
                {
                    List<BrajsonBranchJson> darrbrajson;
                    PsPrintShop.subGetPrintshopCompanyBranches(ps, intnCompanyId, out darrbrajson,
                        ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = darrbrajson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetCompanyBranchContacts(
            //                                              //PURPOSE:
            //                                              //Get the contacts for a specific company and branch.

            //                                              //URL: http://localhost/Odyssey2/Printshop/
            //                                              //     GetCompanyBranchContacts?intCompanyId  
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get company's branches.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int? intnCompanyId,
            int? intnBranchId
            )
        {
            //                                              //Get printshop.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                ps != null
                )
            {
                try
                {
                    List<ContjsonContactJson> darrcontjson;
                    PsPrintShop.subGetCompanyBranchContacts(ps, intnCompanyId, intnBranchId, out darrcontjson,
                        ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = darrcontjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetUnreadNotificationsNumber(
            //                                              //PURPOSE:
            //                                              //Get total quantity of unread notifications for a user.

            //                                              //URL: http://localhost/Odyssey2/Printshop/
            //                                              //      GetUnreadNotificationsNumber
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get user's total number of unread notifications.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            //                                              //Get the contact id from token.
            var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
            int intContactId = idClaimContact.Value.ParseToInt();

            //                                              //Get printshop id.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            //                                              //Get printshop.
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";
            Object obj = null;

            try
            {
                obj = PsPrintShop.intGetUnreadNotificationsNumber(ps, intContactId);                
            }
            catch (Exception ex)
            {
                //                                      //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetOneReportFilter(
            //                                              //PURPOSE:
            //                                              //Get one report filter.

            //                                              //URL: http://localhost/Odyssey2/Printshop/GetOneReport
            //                                              //          Filter?intPk=1

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the report filter with the received pk.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPk
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intPk > 0
                )
            {
                try
                {
                    CusrepjsonCustomReportJson cusrepjson = PsPrintShop.cusrepjsonGet(intPk, ref intStatus,
                    ref strUserMessage, ref strDevMessage);
                    obj = cusrepjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetReportFilters(
            //                                              //PURPOSE:
            //                                              //Get custom report filters.

            //                                              //URL: http://localhost/Odyssey2/Printshop/
            //                                              //     GetReportFilters?strDataSet  
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //get custom report filters.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            String strDataSet
            )
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                ps != null
                )
            {
                try
                {
                    RepjsonReportsFiltersJson repjson;
                    PsPrintShop.subGetReportFilters(ps, strDataSet, out repjson, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = repjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }
            
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetOpenPayments(
            //                                              //PURPOSE:
            //                                              //Get all payments that have not been deposited into a 
            //                                              //      bank account.

            //                                              //URL: http://localhost/Odyssey2/Printshop/
            //                                              //     GetOpenPayments 
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all payments that have not been deposited into a 
            //                                              //      bank account.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                ps != null
                )
            {
                try
                {
                    List<OppayjsonOpenPaymentJson> darroppayjson;
                    PsPrintShop.subGetOpenPayments(ps, out darroppayjson, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = darroppayjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }
            
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult WorkInProgressStatus(
            //                                              //PURPOSE:
            //                                              //Get the printshop's substages.

            //                                              //URL: http://localhost/Odyssey2/Printshop/
            //                                              //     WorkInProgressStatus
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get list of substages for a inProgress job.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            )
        {
            //                                              //Find printshop.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Printshop not found.";
            Object obj = null;
            if (
                ps != null
                )
            {
                try
                {
                    obj = PsPrintShop.arrstrGetWorkInProgressStatus(ps, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
