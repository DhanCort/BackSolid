/*TASK RP.USER*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.App;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: April 13, 2020. 

namespace Odyssey2Backend.Controllers
{
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class UserController : Controller
    {
        //                                                  //Controller associated with the actions for a process 
        //                                                  //      type.
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

        public UserController(
            IConfiguration iConfiguration_I
            )
        {
            this.configuration = iConfiguration_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult Login(
            //                                              //PURPOSE:
            //                                              //Verify the credentials (email and pass) if they are valid
            //                                              //      return a json with a token for the authentication.

            //                                              //URL: http://localhost/Odyssey2/User/Login
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "strEmail":"something@someweb.com",
            //                                              //          "strPassword":"Pass123"
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Get the user from Wisnet if it exists there and compare 
            //                                              //      the pass and if it is active. If everything is ok
            //                                              //      create a token and return the token, user name, user
            //                                              //      last name and printshop name.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Dynamic object that contains all necessary data.
            [FromBody] JsonElement jsonUser
            )
        {
            int intStatus = 400 ;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonUser) &&
                //                                          //Verify if the data is not null or empty.
                jsonUser.TryGetProperty("strEmail", out json) &&
                (int)jsonUser.GetProperty("strEmail").ValueKind == 3 &&
                jsonUser.TryGetProperty("strPassword", out json) &&
                (int)jsonUser.GetProperty("strPassword").ValueKind == 3 &&
                jsonUser.TryGetProperty("strPrintshopId", out json) &&
                (int)jsonUser.GetProperty("strPrintshopId").ValueKind == 3
                )
            {
                String strEmail = jsonUser.GetProperty("strEmail").GetString();
                String strPassword = jsonUser.GetProperty("strPassword").GetString();
                String strPrintshopId = jsonUser.GetProperty("strPrintshopId").GetString();

                try
                {
                    LogjsonLoginJson logjson;
                    UserUser.subLogin(strEmail, strPrintshopId, strPassword, this.configuration, out logjson, 
                        ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = logjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage, obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("[action]")]
        public IActionResult ChangePrintshop(
            //                                              //PURPOSE:
            //                                              //Verify that is an admin token and generate a new token 
            //                                              //      with the new printshop.

            //                                              //URL: http://localhost/Odyssey2/User/ChangePrintshop
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "strPrintshopId":"13832"
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Get the token, verify that is from an admin and generate
            //                                              //      a new one according to the new printshop.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all necessary data.
            [FromBody] JsonElement jsonPrintshop
            )
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "boolIsSuperAdmin");
            bool boolIsSuperAdmin = idClaim.Value.ParseToBool();

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Is not an admin user.";
            Object obj = null;
            if (
                boolIsSuperAdmin
                )
            {
                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data.";
                JsonElement json;
                if (
                    //                                      //Verify if the object is not null.
                    !Object.ReferenceEquals(null, jsonPrintshop) &&
                    //                                      //Verify if the data is not null or empty.
                    jsonPrintshop.TryGetProperty("strPrintshopId", out json) &&
                    (int)jsonPrintshop.GetProperty("strPrintshopId").ValueKind == 3
                    )
                {
                    String strPrintshopId = jsonPrintshop.GetProperty("strPrintshopId").GetString();

                    try
                    {
                        UserUser.subChangePrintshop(strPrintshopId, this.configuration, ref intStatus,
                            ref strUserMessage, ref strDevMessage, ref obj);
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
        public IActionResult GetPrintshops(
            //                                              //PURPOSE:
            //                                              //Get the printshops where the given email is a user.

            //                                              //URL: http://localhost/Odyssey2/User/GetPrintshops
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the users from Wisnet where the email is the given 
            //                                              //      email.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            String strEmail
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            if (
                //                                          //Verify the given data.
                !String.IsNullOrEmpty(strEmail)
                )
            {
                try
                {
                    UserpsjsonUserPrintshopsJson userpsjson;
                    UserUser.subGetPrintshops(strEmail, this.configuration, out userpsjson, ref intStatus, 
                        ref strUserMessage, ref strDevMessage);
                    obj = userpsjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage, obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/