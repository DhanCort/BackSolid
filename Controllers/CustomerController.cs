/*TASK RP. CUSTOMER*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.Job;
using Odyssey2Backend.JsonTypes;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using Odyssey2Backend.PrintShop;
using System.Collections.Generic;
using Odyssey2Backend.Customer;
using Odyssey2Backend.Utilities;
using System.Text.Json;
using TowaStandard;

//                                                          //AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: August 07, 2020. 

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //  var idClaim = User.Claims.FirstOrDefault(c => c.Type == 
    //                                                      //      "strPrintshopId");
    //                                                      //  String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class CustomerController : Controller
    {
        //                                                  //Controller associated with the actions for a Customers.
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

        public CustomerController(
            IConfiguration iConfiguration_I
            )
        {
            this.configuration = iConfiguration_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult Add(
            //                                              //PURPOSE:
            //                                              //Add a new customer

            //                                              //URL: http://localhost/Odyssey2/Customer/Add
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "strFirstName":"Esteban",
            //                                              //          "strLastName": "Rogelio",
            //                                              //          "strEmail":"esteban@nose.com",
            //                                              //          "strPassword":"Towa2019"
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add a new customer from odyssey2.0 to wisnet.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            [FromBody] JsonElement jsonCustomer
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid Data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonCustomer.ValueKind == 7) &&
                !((int)jsonCustomer.ValueKind == 0) &&
                //                                         //Verify the necessary properties.
                jsonCustomer.TryGetProperty("strFirstName", out json) &&
                (int)jsonCustomer.GetProperty("strFirstName").ValueKind == 3 &&
                jsonCustomer.TryGetProperty("strLastName", out json) &&
                (int)jsonCustomer.GetProperty("strLastName").ValueKind == 3 &&
                jsonCustomer.TryGetProperty("strEmail", out json) &&
                (int)jsonCustomer.GetProperty("strEmail").ValueKind == 3 &&
                jsonCustomer.TryGetProperty("strPassword", out json) &&
                (int)jsonCustomer.GetProperty("strPassword").ValueKind == 3
                )
            {
                //                                          //Get data from json.
                String strFirstName = jsonCustomer.GetProperty("strFirstName").GetString();
                String strLastName = jsonCustomer.GetProperty("strLastName").GetString();
                String strEmail = jsonCustomer.GetProperty("strEmail").GetString();
                String strPassword = jsonCustomer.GetProperty("strPassword").GetString();

                //                                          //Clean blanks.
                strFirstName = strFirstName.TrimExcel();
                strLastName = strLastName.TrimExcel();
                strEmail = strEmail.TrimExcel();
                strPassword = strPassword.TrimExcel();

                try
                {
                    //                                          //Method to add period.
                    obj = CusCustomer.subAddCustomer(strFirstName, strLastName, strEmail, strPassword,
                        ps, this.configuration, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            //                                              //Response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetAllForAPrintshop(
            //                                              //PURPOSE:
            //                                              //Get all customers for a printshop.

            //                                              //URL: http://localhost:5001/Odyssey2/Customer
            //                                              //      /GetAllForAPrintshop
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all customers for a printshop.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                //                                          //Verify if the printshop is not null.
                ps != null
                )
            {
                try
                {
                    List<CusjsonCustomerJson> darrcusjson;
                    CusCustomer.subGetAllForAPrintshop(ps, out darrcusjson, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = darrcusjson;
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
        public IActionResult GetCustomers(
            //                                              //PURPOSE:
            //                                              //Get all customers for a printshop.

            //                                              //URL: http://localhost:5001/Odyssey2/Customer
            //                                              //      /GetCustomers
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all customers for a printshop, 
            //                                              //      only contact Id and full name.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                //                                          //Verify if the printshop is not null.
                ps != null
                )
            {
                try
                {
                    Cusjson2CustomerJson2[] arrcusjson2;
                    CusCustomer.subGetCustomers(ps, out arrcusjson2, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = arrcusjson2;
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
        public IActionResult GetCreditMemos(
            //                                              //PURPOSE:
            //                                              //Get all the credit memos from a customer or get all the
            //                                              //       credit memos from the customers of a printshop

            //                                              //URL: http://localhost:5001/Odyssey2/Customer
            //                                              //      /GetCreditMemos?intnContactId=12334
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all the credit memos from a customer

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int? intnContactId
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                //                                          //Verify if the printshop is not null.
                ps != null
                )
            {
                try
                {
                    CrmjsonCreditMemoJson[] arrcrmjson;
                    CusCustomer.subGetCreditMemos(intnContactId, ps, out arrcrmjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = arrcrmjson;
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
        public IActionResult GetOpenInvoices(
            //                                              //PURPOSE:
            //                                              //Get all customers' invoices that have not been paid.

            //                                              //URL: http://localhost:5001/Odyssey2/Customer
            //                                              //      /GetOpenInvoices?intnContactId=5464564&
            //                                              //      intnOrderNumber=4434578
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all customers' invoices that have not been paid.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int? intnContactId,
            int? intnOrderNumber
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                //                                          //Verify if the printshop is not null.
                ps != null
                )
            {
                try
                {
                    OpinvosjsonOpenInvoicesJson opinvosjson;
                    CusCustomer.subGetOpenInvoices(intnContactId, intnOrderNumber, ps, out opinvosjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = opinvosjson;
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
        public IActionResult GetCredits(
            //                                              //PURPOSE:
            //                                              //Get all the credit memos and payments from a customer

            //                                              //URL: http://localhost:5001/Odyssey2/Customer
            //                                              //      /GetCredits?intnContactId=12334&intnOrderNumber=345
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all the unapplied credit memos and payments from a 
            //                                              //      customer

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int? intnContactId,
            int? intnOrderNumber
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                //                                          //Verify if the printshop is not null.
                ps != null
                )
            {
                try
                {
                    CrjsonCreditJson[] arrcrjson;
                    CusCustomer.subGetCredits(intnContactId, intnOrderNumber, ps, out arrcrjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = arrcrjson;
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