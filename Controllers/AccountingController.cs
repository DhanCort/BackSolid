/*TASK RP. ACCOUNTING*/
using Microsoft.EntityFrameworkCore;
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
using System.Text.Json;
using TowaStandard;
using System.IO;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.Utilities;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: October 30, 2020. 

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //  var idClaim = User.Claims.FirstOrDefault(c => c.Type == 
    //                                                      //      "strPrintshopId");
    //                                                      //  String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class AccountingController : Controller
    {
        //                                                  //Controller associated to the actions related to 
        //                                                  //      accounting.
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

        public AccountingController(
            IConfiguration iConfiguration_I
            )
        {
            this.configuration = iConfiguration_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddAccount(
            //                                              //PURPOSE:
            //                                              //Unlink.

            //                                              //URL: http://localhost/Odyssey2/Accounting
            //                                              //      /AddAccount

            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "strNumber": "2443-BBC",
            //                                              //          "strName": "Processes' costs",
            //                                              //          "intPkType": 32
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Unlink.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonAccount
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonAccount.ValueKind == 7) &&
                !((int)jsonAccount.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonAccount.TryGetProperty("strNumber", out json) &&
                (int)jsonAccount.GetProperty("strNumber").ValueKind == 3 &&
                jsonAccount.TryGetProperty("strName", out json) &&
                (int)jsonAccount.GetProperty("strName").ValueKind == 3 &&
                jsonAccount.TryGetProperty("intPkType", out json) &&
                (int)jsonAccount.GetProperty("intPkType").ValueKind == 4
                )
            {
                String strNumber = jsonAccount.GetProperty("strNumber").GetString();
                strNumber = strNumber.TrimExcel();
                String strName = jsonAccount.GetProperty("strName").GetString();
                strName = strName.TrimExcel();
                int intPkType = jsonAccount.GetProperty("intPkType").GetInt32();

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
                            AccAccounting.subAddAccount(strNumber, strName, intPkType, false, ps, context,
                                ref intStatus, ref strUserMessage, ref strDevMessage);

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

            //                                              //Response for the front web.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult EnableDisable(
            //                                              //PURPOSE:
            //                                              //Set a printshop's account available/unavailable.

            //                                              //URL: http://localhost/Odyssey2/Accounting
            //                                              //      /EnableDisable
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkAccount":1,
            //                                              //          "boolEnabled: true
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Set a printshop's account available/unavailable.
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Json that contains all data.
            [FromBody] JsonElement jsonAccount
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                //                                          //Verify if the object is not null or a value is not empty.
                !((int)jsonAccount.ValueKind == 7) &&
                !((int)jsonAccount.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonAccount.TryGetProperty("intPkAccount", out json) &&
                (int)jsonAccount.GetProperty("intPkAccount").ValueKind == 4 &&
                jsonAccount.TryGetProperty("boolEnabled", out json) &&
                ((int)jsonAccount.GetProperty("boolEnabled").ValueKind == 6 ||
                (int)jsonAccount.GetProperty("boolEnabled").ValueKind == 5)
                )
            {
                int intPkAccount = jsonAccount.GetProperty("intPkAccount").GetInt32();
                bool boolEnabled = jsonAccount.GetProperty("boolEnabled").GetBoolean();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                //                                          //Using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            AccAccounting.subEnableDisable(intPkAccount, boolEnabled, ps, context, ref intStatus,
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
        public IActionResult AddInvoice(
            //                                              //PURPOSE:
            //                                              //Store a printhop's invoice.

            //                                              //URL: http://localhost/Odyssey2/Accounting
            //                                              //      /AddInvoice

            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intOrderId": "4523142",
            //                                              //          "arrintJobsIds": [5515963,36421,44523]
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Save an invoice.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonInvoice
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonInvoice.ValueKind == 7) &&
                !((int)jsonInvoice.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonInvoice.TryGetProperty("intOrderId", out json) &&
                (int)jsonInvoice.GetProperty("intOrderId").ValueKind == 4 &&
                jsonInvoice.TryGetProperty("arrintJobsIds", out json) &&
                (int)jsonInvoice.GetProperty("arrintJobsIds").ValueKind == 2
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                //                                          //Get values.
                int intOrderId = jsonInvoice.GetProperty("intOrderId").GetInt32();

                //                                          //List to add all the jobs id related to the order
                List<int> darrintJobsIds = new List<int>();

                for (int intU = 0; intU < jsonInvoice.GetProperty("arrintJobsIds").GetArrayLength(); intU = intU + 1)
                {
                    //int intJobId = jsonInvoice.GetProperty("arrintJobsIds")[intU].GetInt32();
                    darrintJobsIds.Add(jsonInvoice.GetProperty("arrintJobsIds")[intU].GetInt32());
                }

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            obj = AccAccounting.invjsonAddInvoice(intOrderId, darrintJobsIds, ps, this.configuration,
                                context, ref intStatus, ref strUserMessage, ref strDevMessage);

                            //                              //Commits all changes made to the database in the current
                            //                              //      transaction.
                            if (
                                intStatus == 200
                                )
                            {
                                dbContextTransaction.Commit();
                                AccAccounting.darrintOrderInvoicing_S.Remove(intOrderId);
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

            //                                              //Response for the front web.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult EditInvoice(
            //                                              //PURPOSE:
            //                                              //Update an invoice.

            //                                              //URL: http://localhost/Odyssey2/Accounting
            //                                              //      /EditInvoice

            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          //UPDATE JSON
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Update an invoice

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonEditInvoice
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonEditInvoice.ValueKind == 7) &&
                !((int)jsonEditInvoice.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonEditInvoice.TryGetProperty("intPkInvoice", out json) &&
                (int)jsonEditInvoice.GetProperty("intPkInvoice").ValueKind == 4 &&
                jsonEditInvoice.TryGetProperty("strBilledTo", out json) &&
                (int)jsonEditInvoice.GetProperty("strBilledTo").ValueKind == 3 &&
                jsonEditInvoice.TryGetProperty("boolIsShipped", out json) &&
                ((int)jsonEditInvoice.GetProperty("boolIsShipped").ValueKind == 6 ||
                (int)jsonEditInvoice.GetProperty("boolIsShipped").ValueKind == 5)
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                //                                          //Get the contact id from token.
                var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                int intContactId = idClaimContact.Value.ParseToInt();

                //                                          //Get values from json.
                int intPkInvoice = jsonEditInvoice.GetProperty("intPkInvoice").GetInt32();
                String strBilledTo = jsonEditInvoice.GetProperty("strBilledTo").GetString();
                strBilledTo = strBilledTo.TrimExcel();
                int intOrderId = jsonEditInvoice.GetProperty("intOrderId").GetInt32();
                bool boolIsShipped = jsonEditInvoice.GetProperty("boolIsShipped").GetBoolean();

                //                                          //Get ZipCodes values.
                String strPrintshopZipCode = "";
                if (
                    jsonEditInvoice.TryGetProperty("strPrintshopZipCode", out json) &&
                    (int)jsonEditInvoice.GetProperty("strPrintshopZipCode").ValueKind == 3
                    )
                {
                    strPrintshopZipCode = jsonEditInvoice.GetProperty("strPrintshopZipCode").GetString();
                    strPrintshopZipCode = strPrintshopZipCode.TrimExcel();
                }

                String strShippedToZip = "";
                if (
                    jsonEditInvoice.TryGetProperty("strShippedToZip", out json) &&
                    (int)jsonEditInvoice.GetProperty("strShippedToZip").ValueKind == 3
                    )
                {
                    strShippedToZip = jsonEditInvoice.GetProperty("strShippedToZip").GetString();
                    strShippedToZip = strShippedToZip.TrimExcel();
                }

                //                                          //To easy code.
                //                                          //Zip code to use.
                String strZipCodeToUse = boolIsShipped ? strShippedToZip : strPrintshopZipCode;

                //                                          //List to get the job info array values.
                List <InvjobinfojsonInvoiceJobInformationJson> darrinvjobinfojson =
                    new List<InvjobinfojsonInvoiceJobInformationJson>();

                if (
                    jsonEditInvoice.TryGetProperty("darrinvjobinfojson", out json) &&
                    (int)jsonEditInvoice.GetProperty("darrinvjobinfojson").ValueKind == 2
                    )
                {
                    for (int intU = 0; intU < jsonEditInvoice.GetProperty("darrinvjobinfojson").GetArrayLength();
                            intU = intU + 1)
                    {
                        JsonElement json1 = jsonEditInvoice.GetProperty("darrinvjobinfojson")[intU];

                        int? intnJobId = null;
                        if (
                            json1.TryGetProperty("intnJobId", out json) &&
                            (int)json1.GetProperty("intnJobId").ValueKind == 4
                            )
                        {
                            intnJobId = json1.GetProperty("intnJobId").GetInt32();
                        }

                        //                                  //Get account if exists.
                        int? intnPkAccount = null;
                        if (
                            json1.TryGetProperty("intnPkAccount", out json) &&
                            (int)json1.GetProperty("intnPkAccount").ValueKind == 4
                            )
                        {
                            intnPkAccount = json1.GetProperty("intnPkAccount").GetInt32();
                        }

                        //                                  //Get accMovement if exists.
                        int? intnPkAccountMov = null;
                        if (
                            json1.TryGetProperty("intnPkAccountMov", out json) &&
                            (int)json1.GetProperty("intnPkAccountMov").ValueKind == 4
                            )
                        {
                            intnPkAccountMov = json1.GetProperty("intnPkAccountMov").GetInt32();
                        }

                        //                                  //Create Json.
                        InvjobinfojsonInvoiceJobInformationJson invjobinfojson =
                            new InvjobinfojsonInvoiceJobInformationJson(
                            intnJobId,
                            json1.GetProperty("strJobNumber").GetString(),
                            json1.GetProperty("strName").GetString(),
                            json1.GetProperty("intQuantity").GetInt32(),
                            json1.GetProperty("numPrice").GetDouble(),
                            intnPkAccount,
                            intnPkAccountMov,
                            json1.GetProperty("strAccount").GetString(),
                            json1.GetProperty("boolIsExempt").GetBoolean()
                            );
                        darrinvjobinfojson.Add(invjobinfojson);
                    }
                }
                else
                {
                    darrinvjobinfojson = null;
                }

                //                                          //Serialize complete json.
                String strEditedInvoice = JsonSerializer.Serialize(jsonEditInvoice);

                //                                          //using is to release connection at the end.
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            AccAccounting.subEditInvoice(intPkInvoice, intContactId, darrinvjobinfojson, ps,
                                strEditedInvoice, strBilledTo, intOrderId, strZipCodeToUse, this.configuration,
                                context, ref intStatus, ref strUserMessage, ref strDevMessage);

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

            //                                              //Response for the front web.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SetAccountToProduct(
            //                                              //PURPOSE:
            //                                              //Set an account to a porduct.

            //                                              //URL: http://localhost/Odyssey2/Accounting
            //                                              //      /SetAccountToProduct

            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intOrderId": "4523142",
            //                                              //          "arrintJobsIds": [5515963,36421,44523]
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //set an account to product.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonSetAccountToProduct
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonSetAccountToProduct.ValueKind == 7) &&
                !((int)jsonSetAccountToProduct.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonSetAccountToProduct.TryGetProperty("intPkProduct", out json) &&
                (int)jsonSetAccountToProduct.GetProperty("intPkProduct").ValueKind == 4 &&
                jsonSetAccountToProduct.TryGetProperty("intPkAccount", out json) &&
                (int)jsonSetAccountToProduct.GetProperty("intPkAccount").ValueKind == 4
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                //                                          //Get values from json.
                int intPkProduct = jsonSetAccountToProduct.GetProperty("intPkProduct").GetInt32();
                int intPkAccount = jsonSetAccountToProduct.GetProperty("intPkAccount").GetInt32();

                //                                          //using is to release connection at the end.
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            AccAccounting.subSetAccountToProduct(intPkProduct, intPkAccount, ps, context,
                                ref intStatus, ref strUserMessage, ref strDevMessage);

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

            //                                              //Response for the front web.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]/")]
        public IActionResult UploadTaxesFile(
            //                                              //Upload taxes file.

            [FromBody] JsonElement jsonTaxes
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null or a value is not empty. 
                !((int)jsonTaxes.ValueKind == 7) &&
                !((int)jsonTaxes.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonTaxes.TryGetProperty("taxFile", out json) &&
                (int)jsonTaxes.GetProperty("taxFile").ValueKind == 3 &&
                jsonTaxes.TryGetProperty("strFileExt", out json) &&
                (int)jsonTaxes.GetProperty("strFileExt").ValueKind == 3

                )
            {
                //                                          //Get file extension value from body.
                String strFileExt = jsonTaxes.GetProperty("strFileExt").GetString();

                intStatus = 401;
                strUserMessage = "Only .tab files are allowed.";
                strDevMessage = "Only .tab files are allowed";
                if (
                    strFileExt == ".tab"
                    )
                {
                    try
                    {
                        //                                  //To save file.
                        PathX syspathA1 = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
                        System.IO.File.WriteAllBytes(Path.Combine(syspathA1.FullPath.ToString(), "taxes.tab"),
                            jsonTaxes.GetProperty("taxFile").GetBytesFromBase64());

                        //                                  //Get the taxes file to process it.
                        PathX syspathRead = DirectoryX.GetCurrent().GetPath().AddName("Z_BatchFiles");
                        PathX syspath = syspathRead.AddName("taxes.tab");
                        FileInfo sysfile = FileX.New(syspath);

                        //                                  //Get the array of info from the file.
                        String[] arrTypesData = sysfile.ReadAll();

                        //                                  //Get array of column name.
                        String[] arrstrColumnName = arrTypesData[0].Split("\t");

                        int intNumberOfColumns = 0;
                        int intPostalCodePos = 0;
                        int intUsetaxPos = 0;
                        for (int intI = 0; intI < arrstrColumnName.Length; intI++)
                        {
                            //                              //To get zipcode
                            String[] arrColumnName = arrstrColumnName[intI].Split("\0");
                            String strColumnName = String.Join("", arrColumnName);

                            if (
                                strColumnName.ToLower() == "postalcode"
                                )
                            {
                                intNumberOfColumns++;
                                intPostalCodePos = intI;
                            }

                            if (
                                strColumnName.ToLower() == "usetax"
                                )
                            {
                                intNumberOfColumns++;
                                intUsetaxPos = intI;
                            }
                        }

                        intStatus = 402;
                        strUserMessage = "File not valid.";
                        strDevMessage = "File not valid.";
                        if (
                            intNumberOfColumns == 2
                            )
                        {
                            //                              //Dictionary to store valid zipcode and tax value.
                            Dictionary<String, double> dicZipCodes = new Dictionary<string, double>();

                            for (int intI = 1; intI < arrTypesData.Length - 1; intI++)
                            {
                                String[] arrsplitFirstLine = arrTypesData[intI].Split("\t");

                                //                          //To get zipcode
                                String[] arrZipCode = arrsplitFirstLine[intPostalCodePos].Split("\0");
                                String strZipCode = String.Join("", arrZipCode);

                                //                          //To get tax value.
                                String[] arrTaxValue = arrsplitFirstLine[intUsetaxPos].Split("\0");
                                String strTaxValue = String.Join("", arrTaxValue);
                                double numTaxValue = strTaxValue.ParseToNum();

                                if (
                                    !dicZipCodes.ContainsKey(strZipCode)
                                    )
                                {
                                    dicZipCodes.Add(strZipCode, numTaxValue);
                                }
                                else
                                {
                                    if (
                                        dicZipCodes[strZipCode] < numTaxValue
                                        )
                                    {
                                        //                  //Always keep greather tax value for a zipcode.
                                        dicZipCodes[strZipCode] = numTaxValue;
                                    }
                                }
                            }

                            //                              //using is to release connection at the end.
                            using (Odyssey2Context context = new Odyssey2Context())
                            {
                                //                          //Starts a new transaction.
                                using (var dbContextTransaction = context.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        //                  //Delete all records from the Taxes table
                                        context.Database.ExecuteSqlRaw("Truncate table Taxes");

                                        //                  //Add info to DB.
                                        foreach (KeyValuePair<String, double> zipcode in dicZipCodes)
                                        {
                                            //              //Add register to DB
                                            TaxentityTaxesEntityDB taxentity = new TaxentityTaxesEntityDB
                                            {
                                                strZipCode = zipcode.Key,
                                                numTaxValue = zipcode.Value
                                            };
                                            context.Taxes.Add(taxentity);
                                        }
                                        context.SaveChanges();

                                        intStatus = 200;
                                        strUserMessage = "Tax rates updated with new file.";
                                        strDevMessage = "Success";

                                        //                  //Commits all changes made to the database in the current
                                        //                  //      transaction.
                                        dbContextTransaction.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        //                  //Discards all changes made to the database in the current
                                        //                  //      transaction.
                                        dbContextTransaction.Rollback();

                                        //                  //Making a log for the exception.
                                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, 
                                            ref strDevMessage);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //                              //Delete file.
                            if (
                                System.IO.File.Exists(syspath.FullPath.ToString())
                                )
                            {
                                System.IO.File.Delete(syspath.FullPath.ToString());
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //                                  //Making a log for the exception.
                        Tools.subExceptionHandler(e, ref intStatus, ref strUserMessage, ref strDevMessage);
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
        public IActionResult AddPayment(
            //                                              //PURPOSE:
            //                                              //Add a customer's payment.

            //                                              //URL: http://localhost/Odyssey2/Accounting
            //                                              //      /AddPayment

            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intContactId":5435,
            //                                              //          "strDate":"",
            //                                              //          "intnPkPaymentMethod":1,
            //                                              //          "strReference":"242SD3324",
            //                                              //          "intnPkAccount":2,
            //                                              //          "arrintPkInvoices":
            //                                              //          [
            //                                              //              1,2,3
            //                                              //          ],
            //                                              //          "arrintPkCredits":
            //                                              //          [
            //                                              //              {
            //                                              //                  "intPkCredit":1,
            //                                              //                  "boolIsCreditMemo":true
            //                                              //              },
            //                                              //              {
            //                                              //                  "intPkCredit":1,
            //                                              //                  "boolIsCreditMemo":false
            //                                              //              }
            //                                              //          ],
            //                                              //          "numAmountReceived":500
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add a customer's payment.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonPayment
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonPayment.ValueKind == 7) &&
                !((int)jsonPayment.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonPayment.TryGetProperty("intContactId", out json) &&
                (int)jsonPayment.GetProperty("intContactId").ValueKind == 4 &&
                jsonPayment.TryGetProperty("strDate", out json) &&
                (int)jsonPayment.GetProperty("strDate").ValueKind == 3 &&
                jsonPayment.TryGetProperty("numAmountReceived", out json) &&
                (int)jsonPayment.GetProperty("numAmountReceived").ValueKind == 4
                )
            {
                
                int intContactId = jsonPayment.GetProperty("intContactId").GetInt32();
                String strDate = jsonPayment.GetProperty("strDate").GetString();
                double numAmountReceived = jsonPayment.GetProperty("numAmountReceived").GetDouble();

                //                                          //Get payment method if it exists.
                int? intnPkPaymentMethod = null;
                if (
                    jsonPayment.TryGetProperty("intnPkPaymentMethod", out json) &&
                    (int)jsonPayment.GetProperty("intnPkPaymentMethod").ValueKind == 4
                    )
                {
                    intnPkPaymentMethod = jsonPayment.GetProperty("intnPkPaymentMethod").GetInt32();
                }

                //                                          //Get reference if it exists.
                String strReference = null;
                if (
                    jsonPayment.TryGetProperty("strReference", out json) &&
                    (int)jsonPayment.GetProperty("strReference").ValueKind == 3
                    )
                {
                    strReference = jsonPayment.GetProperty("strReference").GetString();
                    strReference = strReference.TrimExcel();
                }

                //                                          //Get account if it exists.
                int? intnPkAccount = null;
                if (
                    jsonPayment.TryGetProperty("intnPkAccount", out json) &&
                    (int)jsonPayment.GetProperty("intnPkAccount").ValueKind == 4
                    )
                {
                    intnPkAccount = jsonPayment.GetProperty("intnPkAccount").GetInt32();
                }

                //                                          //Get credits if they exist.
                List<Crjson2CreditJson2> darrcrjson2 = new List<Crjson2CreditJson2>();
                if (
                    jsonPayment.TryGetProperty("arrCredits", out json) &&
                    (int)jsonPayment.GetProperty("arrCredits").ValueKind == 2
                    )
                {
                    for (int intI = 0; intI < jsonPayment.GetProperty("arrCredits").GetArrayLength(); intI = intI + 1)
                    {
                        JsonElement json2 = jsonPayment.GetProperty("arrCredits")[intI];

                        int intPkCredit = json2.GetProperty("intPkCredit").GetInt32();
                        bool boolIsCreditMemo = json2.GetProperty("boolIsCreditMemo").GetBoolean();

                        Crjson2CreditJson2 crjson2 = new Crjson2CreditJson2(intPkCredit, boolIsCreditMemo);

                        darrcrjson2.Add(crjson2);
                    }
                }

                //                                          //Get pk invoices if they exist.
                bool boolAllPkInvoiceAreInt = true;
                List<int> darrintPkInvoice = new List<int>();
                if (
                    jsonPayment.TryGetProperty("arrintPkInvoices", out json) &&
                    (int)jsonPayment.GetProperty("arrintPkInvoices").ValueKind == 2
                    )
                {
                    int intI = 0;
                    //                                      //Check if all pk are int.
                    /*WHILE-DO*/
                    while (
                        intI < jsonPayment.GetProperty("arrintPkInvoices").GetArrayLength() && boolAllPkInvoiceAreInt
                        )
                    {
                        JsonElement json1 = jsonPayment.GetProperty("arrintPkInvoices")[intI];

                        int intPkInvoice;
                        if (
                            //                              //Pk is int.
                            json1.TryGetInt32(out intPkInvoice) 
                            )
                        {
                            darrintPkInvoice.Add(intPkInvoice);
                        }
                        else
                        {
                            boolAllPkInvoiceAreInt = false;
                        }

                        intI = intI + 1;
                    }
                }

                if (
                    //                                      //All pk of invoices are int.
                    boolAllPkInvoiceAreInt
                    )
                {
                    //                                      //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                    //                                      //Using is to release connection at the end.
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                List<InvoInvoiceEntityDB> darrinvoentity;
                                AccAccounting.subAddPayment(intContactId, strDate, intnPkPaymentMethod, 
                                    strReference, intnPkAccount, numAmountReceived, ps, context, darrintPkInvoice, 
                                    darrcrjson2, out darrinvoentity, ref intStatus, ref strUserMessage,
                                    ref strDevMessage);

                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
                                if (
                                    intStatus == 200
                                    )
                                {
                                    //                      //Notify to Wisnet those invoice already paid.
                                    List<int> darrintOrdersIdPaid = AccAccounting.subSendToWisnetInvoicesAlreadyPaid(
                                        ps, darrinvoentity, context);
                                    obj = darrintOrdersIdPaid.ToArray();

                                    dbContextTransaction.Commit();
                                }
                                else
                                {
                                    dbContextTransaction.Rollback();
                                }
                            }
                            catch (Exception ex)
                            {
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
                        }
                    }
                }
            }

            //                                              //Response for the front web.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddBankDeposit(
            //                                              //PURPOSE:
            //                                              //Add Bank Deposit to the payments.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /AddBankDeposit
            //                                              //Method: POST.

            //                                              //DESCRIPTION:
            //                                              //Add bank Deposit to the payments.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            [FromBody] JsonElement jsonPayment
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);
            bool boolContinue = true;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonPayment.ValueKind == 7) &&
                !((int)jsonPayment.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonPayment.TryGetProperty("strDate", out json) &&
                (int)jsonPayment.GetProperty("strDate").ValueKind == 3 &&
                jsonPayment.TryGetProperty("intPkAccount", out json) &&
                (int)jsonPayment.GetProperty("intPkAccount").ValueKind == 4 &&
                jsonPayment.TryGetProperty("arrintPkPayment", out json) &&
                (int)jsonPayment.GetProperty("arrintPkPayment").ValueKind == 2
                )
            {
                int intPkAccount = jsonPayment.GetProperty("intPkAccount").GetInt32();
                String strDate = jsonPayment.GetProperty("strDate").GetString();
                boolContinue = intPkAccount > 0 ? true : false;

                List<int> darrintPkPayment = new List<int>();
                if (
                    (int)jsonPayment.GetProperty("arrintPkPayment").ValueKind == 2
                    )
                {
                    for (int intU = 0; (boolContinue &&
                        intU < jsonPayment.GetProperty("arrintPkPayment").GetArrayLength()); intU = intU + 1)
                    {
                        //                                  //Get the payment from the json.
                        int intPkPayment = jsonPayment.GetProperty("arrintPkPayment")[intU].GetInt32();
                        boolContinue = intPkPayment > 0 ? true : false;
                        darrintPkPayment.Add(intPkPayment);
                    }
                }

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data.";
                if (
                    strDate.IsParsableToDate() &&
                    //                                      //Validate that the number is be greather than zero.
                    boolContinue
                    )
                {
                    //                                      //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                BankdepsummjsonBankDepositSummaryJson bankdepsummjson;
                                AccAccounting.subAddBankDeposit(ps, strDate, intPkAccount, darrintPkPayment, 
                                    out bankdepsummjson, context, ref intStatus, ref strUserMessage,
                                    ref strDevMessage);

                                obj = bankdepsummjson;
                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
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
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
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
        public IActionResult AddCreditMemo(
            //                                              //PURPOSE:
            //                                              //Assigned a credit memo to a customer.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /AddCreditMemo
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intContactId": 35345,
            //                                              //          "strCustomerFullName": "Cesar Cigarroa",
            //                                              //          "strDate":"2020-12-10",
            //                                              //          "strBilledTo": "My direction",
            //                                              //          "strDescription":"Burned paper",
            //                                              //          "intPkRevenueAccount":2,
            //                                              //          "numAmount": 500,
            //                                              //          "boolIsExempt": true
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Assigned a credit memo to a customer.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            [FromBody] JsonElement jsonMemo
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonMemo.ValueKind == 7) &&
                !((int)jsonMemo.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonMemo.TryGetProperty("intContactId", out json) &&
                (int)jsonMemo.GetProperty("intContactId").ValueKind == 4 &&
                jsonMemo.TryGetProperty("intPkRevenueAccount", out json) &&
                (int)jsonMemo.GetProperty("intPkRevenueAccount").ValueKind == 4 &&
                jsonMemo.TryGetProperty("strCustomerFullName", out json) &&
                (int)jsonMemo.GetProperty("strCustomerFullName").ValueKind == 3 &&
                jsonMemo.TryGetProperty("strDate", out json) &&
                (int)jsonMemo.GetProperty("strDate").ValueKind == 3 &&
                jsonMemo.TryGetProperty("strBilledTo", out json) &&
                (int)jsonMemo.GetProperty("strBilledTo").ValueKind == 3 &&
                jsonMemo.TryGetProperty("strDescription", out json) &&
                (int)jsonMemo.GetProperty("strDescription").ValueKind == 3 &&
                jsonMemo.TryGetProperty("numAmount", out json) &&
                (int)jsonMemo.GetProperty("numAmount").ValueKind == 4 &&
                jsonMemo.TryGetProperty("boolIsExempt", out json) &&
                ((int)jsonMemo.GetProperty("boolIsExempt").ValueKind == 6 ||
                (int)jsonMemo.GetProperty("boolIsExempt").ValueKind == 5)
                )
            {
                //                                          //Get Properties.
                int intContactId = jsonMemo.GetProperty("intContactId").GetInt32();
                int intPkRevenueAccount = jsonMemo.GetProperty("intPkRevenueAccount").GetInt32();
                String strCustomerFullName = jsonMemo.GetProperty("strCustomerFullName").GetString();
                strCustomerFullName = strCustomerFullName.TrimExcel();
                String strDate = jsonMemo.GetProperty("strDate").GetString();
                String strBilledTo = jsonMemo.GetProperty("strBilledTo").GetString();
                strBilledTo = strBilledTo.TrimExcel();
                String strDescription = jsonMemo.GetProperty("strDescription").GetString();
                strDescription = strDescription.TrimExcel();
                double numAmount = jsonMemo.GetProperty("numAmount").GetDouble();
                bool boolIsExempt = jsonMemo.GetProperty("boolIsExempt").GetBoolean();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data.";
                if (
                    ps != null
                    )
                {
                    //                                      //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                CredmemjsonCreditMemoJson credmemjson;
                                AccAccounting.subAddCreditMemo(intContactId, strCustomerFullName, strDate, strBilledTo, 
                                    strDescription, intPkRevenueAccount, numAmount, boolIsExempt, ps, out credmemjson, 
                                    context, ref intStatus, ref strUserMessage, ref strDevMessage);
                                obj = credmemjson;

                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
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
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
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
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetAllAccounts(
            //                                              //PURPOSE:
            //                                              //Get all printshop's accounts.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetAllAccounts
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all printshop's accounts.

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
                    obj = AccAccounting.darraccjsonGetAllAccounts(ps, ref intStatus, ref strUserMessage, 
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
        [HttpGet("[action]")]
        public IActionResult GetAllAccountsExpenseAvailable(
            //                                              //PURPOSE:
            //                                              //Get all printshop's accounts expense available.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetAllAccountsExpenseAvailable
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //et all printshop's accounts expense available.

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
                    List<Accjson3AccountJson3> darraccexpjson;
                    AccAccounting.subGetAllAccountsExpenseAvailable(ps, out darraccexpjson, ref intStatus, 
                        ref strUserMessage, ref strDevMessage);
                    obj = darraccexpjson;
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
        public IActionResult GetAllAccountsRevenueAvailable(
            //                                              //PURPOSE:
            //                                              //Get all printshop's accounts revenue available.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetAllAccountsRevenueAvailable
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all printshop's accounts revenue available.

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
                    List<Accjson3AccountJson3> darraccrevjson;
                    AccAccounting.subGetAllAccountsRevenueAvailable(ps, out darraccrevjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = darraccrevjson;
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
        public IActionResult GetAccountTypes(
            //                                              //PURPOSE:
            //                                              //Get all account types.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetAccountTypes
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all account types.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            try
            {
                obj = AccAccounting.darracctypjsonGetAccountTypes(ref intStatus, ref strUserMessage, ref strDevMessage);
            }
            catch (Exception ex)
            {
                //                                          //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }
            
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetPrintshopOrders(
            //                                              //PURPOSE:
            //                                              //Get orders for a printshop.

            //                                              //URL: http://localhost/Odyssey2/Accounting/
            //                                              //     GetPrintshopOrders  
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the orders of a printshop only if all their jobs
            //                                              //      are completed.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
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
                    List<PsorderjsonPrintshopOrderJson> darrpsorderjson =
                    AccAccounting.darrpsorderjsonGetPrintshopOrders(ps, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = darrpsorderjson;
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
        public IActionResult GetInvoice(
            //                                              //PURPOSE:
            //                                              //Get all account types.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetInvoice?intPkInvoice=3
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all account types.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            int intPkInvoice
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            try
            {
                obj = AccAccounting.strGetInvoice(intPkInvoice, ref intStatus, ref strUserMessage, ref strDevMessage);
            }
            catch (Exception ex)
            {
                //                                          //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }
            
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetJobMovements(
            //                                              //PURPOSE:
            //                                              //Get all job's movements.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetJobMovements?intJobId=56675
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all job's movements.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            int intJobId
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            try
            {
                JobmovsjsonJobMovementsJson jobmovsjson;
                AccAccounting.subGetJobMovements(intJobId, strPrintshopId, this.configuration, out jobmovsjson, 
                    ref intStatus, ref strUserMessage, ref strDevMessage);
                obj = jobmovsjson;
            }
            catch (Exception ex)
            {
                //                                          //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetAccountsWithMovementsInAPeriod(
            //                                              //PURPOSE:
            //                                              //Get all accounts that had movements in a period of time.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetAccountsWithMovementsInAPeriod
            //                                              //Method: GET.

            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "strStartDate": "2020-11-13",
            //                                              //          "strStartTime": "15:50:00",
            //                                              //          "strEndDate": "2020-11-13",
            //                                              //          "strEndTime": "15:59:00",
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Get all accounts that had movements in a period of time.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            [FromBody] JsonElement jsonMovements
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonMovements.ValueKind == 7) &&
                !((int)jsonMovements.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonMovements.TryGetProperty("strStartDate", out json) &&
                (int)jsonMovements.GetProperty("strStartTime").ValueKind == 3 &&
                jsonMovements.TryGetProperty("strEndDate", out json) &&
                (int)jsonMovements.GetProperty("strEndTime").ValueKind == 3
                )
            {
                //                                          //Get data from json.
                String strStartDate = jsonMovements.GetProperty("strStartDate").GetString();
                String strStartTime = jsonMovements.GetProperty("strStartTime").GetString();
                String strEndDate = jsonMovements.GetProperty("strEndDate").GetString();
                String strEndTime = jsonMovements.GetProperty("strEndTime").GetString();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                if (
                    ps != null
                    )
                {
                    try
                    {
                        List<Accjson2AccountJson2> darraccjson2;
                        AccAccounting.subGetAccountsWithMovementsInAPeriod(strStartDate, strStartTime, strEndDate,
                            strEndTime, ps, out darraccjson2, ref intStatus, ref strUserMessage, ref strDevMessage);
                        obj = darraccjson2;
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
        [HttpGet("[action]")]
        public IActionResult GetInvoices(
            //                                              //PURPOSE:
            //                                              //Get all printshop's invoices.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetInvoices?intContactId
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all printshop's invoices.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int intContactId
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
                ps != null
                )
            {
                try
                {
                    obj = AccAccounting.darrinvojsonGetInvoices(intContactId, ps, ref intStatus, ref strUserMessage, 
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
        [HttpGet("[action]")]
        public IActionResult GetAccountMovement(
            //                                              //PURPOSE:
            //                                              //Get movements for an especific account.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetAccountsWithMovementsInAPeriod
            //                                              //Method: GET.

            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "strStartDate": "2020-11-13",
            //                                              //          "strStartTime": "15:50:00",
            //                                              //          "strEndDate": "2020-11-13",
            //                                              //          "strEndTime": "15:59:00",
            //                                              //          "intPkAccount": 1,
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Get movements for an especific account, in a period of 
            //                                              //      time.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            [FromBody] JsonElement jsonAccMovements
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonAccMovements.ValueKind == 7) &&
                !((int)jsonAccMovements.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonAccMovements.TryGetProperty("strStartDate", out json) &&
                (int)jsonAccMovements.GetProperty("strStartDate").ValueKind == 3 &&
                jsonAccMovements.TryGetProperty("strStartTime", out json) &&
                (int)jsonAccMovements.GetProperty("strStartTime").ValueKind == 3 &&
                jsonAccMovements.TryGetProperty("strEndDate", out json) &&
                (int)jsonAccMovements.GetProperty("strEndDate").ValueKind == 3 &&
                jsonAccMovements.TryGetProperty("strEndTime", out json) &&
                (int)jsonAccMovements.GetProperty("strEndTime").ValueKind == 3 &&
                jsonAccMovements.TryGetProperty("intPkAccount", out json) &&
                (int)jsonAccMovements.GetProperty("intPkAccount").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                String strStartDate = jsonAccMovements.GetProperty("strStartDate").GetString();
                String strStartTime = jsonAccMovements.GetProperty("strStartTime").GetString();
                String strEndDate = jsonAccMovements.GetProperty("strEndDate").GetString();
                String strEndTime = jsonAccMovements.GetProperty("strEndTime").GetString();
                int intPkAccount = jsonAccMovements.GetProperty("intPkAccount").GetInt32();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                if (
                    ps != null
                    )
                {
                    try
                    {
                        obj = AccAccounting.AccdetjsonGetAccountMovements(strStartDate, strStartTime, strEndDate,
                        strEndTime, ps, intPkAccount, this.configuration, ref intStatus, ref strUserMessage, ref strDevMessage);
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
        [HttpGet("[action]")]
        public IActionResult GetPaymentMethods(
            //                                              //PURPOSE:
            //                                              //Get all the payment methods

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetPaymentMethods
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all the payment methods

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            try
            {
                PymtmtjsonPaymentMethodJson[] arrpymtmtjson;
                AccAccounting.subGetPaymentMethods(out arrpymtmtjson, ref intStatus, ref strUserMessage, 
                    ref strDevMessage);
                obj = arrpymtmtjson;
            }
            catch (Exception ex)
            {
                //                                          //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetBankAccounts(
            //                                              //PURPOSE:
            //                                              //Get all printshop's bank accounts available.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetBankAccounts?boolUndepositedFunds=true
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all printshop's bank accounts.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            bool boolUndepositedFunds
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
                    List<Accjson3AccountJson3> darraccjson;
                    AccAccounting.subGetBankAccounts(boolUndepositedFunds, ps, out darraccjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = darraccjson;
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
        public IActionResult GetBankDepositsInARange(
            //                                              //PURPOSE:
            //                                              //Get account's bank deposits in a range of time.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetBankDepositsInARange
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get account's bank deposits in a range of time.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            [FromBody] JsonElement jsonBankDeposits
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonBankDeposits.ValueKind == 7) &&
                !((int)jsonBankDeposits.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonBankDeposits.TryGetProperty("strStartDate", out json) &&
                (int)jsonBankDeposits.GetProperty("strStartDate").ValueKind == 3 &&
                jsonBankDeposits.TryGetProperty("strEndDate", out json) &&
                (int)jsonBankDeposits.GetProperty("strEndDate").ValueKind == 3 &&
                jsonBankDeposits.TryGetProperty("intPkBankAccount", out json) &&
                (int)jsonBankDeposits.GetProperty("intPkBankAccount").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                String strStartDate = jsonBankDeposits.GetProperty("strStartDate").GetString();
                String strEndDate = jsonBankDeposits.GetProperty("strEndDate").GetString();
                int intPkBankAccount = jsonBankDeposits.GetProperty("intPkBankAccount").GetInt32();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data.";
                if (
                    //                                      //Verify if the printshop is not null.
                    ps != null && intPkBankAccount > 0
                    )
                {
                    try
                    {
                        List<BankdepjsonBankDepositJson> darrbankdepjson;
                        AccAccounting.subGetBankDepositsInARange(strStartDate, strEndDate, intPkBankAccount, ps, 
                            out darrbankdepjson, ref intStatus, ref strUserMessage, ref strDevMessage);
                        obj = darrbankdepjson;
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
        [HttpGet("[action]")]
        public IActionResult GetBankDepositSummary(
            //                                              //PURPOSE:
            //                                              //Get information about the deposit's payments.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetBankDepositSummary?intPkBankDeposit=1
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get information about the deposit's payments.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int intPkBankDeposit
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
                    //                                      //Establish connection.
                    Odyssey2Context context = new Odyssey2Context();

                    BankdepsummjsonBankDepositSummaryJson bankdepsummjson;
                    AccAccounting.subGetBankDepositSummary(intPkBankDeposit, ps, out bankdepsummjson, context, 
                        ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = bankdepsummjson;
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
        public IActionResult GetCustomersBalances(
            //                                              //PURPOSE:
            //                                              //Get balances of the customers

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetCustomersBalances?strBalanceStatus = 'ALL'
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get information about the deposit's payments.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //{
            //                                              //    "intContactId":1212,
            //                                              //    "strFullName": "Juan Perez",
            //                                              //    "numBalance": 23323.23
            //                                              //},
            //                                              //{
            //                                              //    "intContactId":121223,
            //                                              //    "strFullName": "María Perez",
            //                                              //    "numBalance": 23323.23
            //                                              //}

            String strBalanceStatus
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
                //                                          //Verify the stringi is not be null.
                !String.IsNullOrEmpty(strBalanceStatus)
                )
            {
                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            List<BalcusjsonBalanceCustomerJson> darrbalcusjson;
                            AccAccounting.subGetCustomersBalances(ps, strBalanceStatus, context, out darrbalcusjson,
                                ref intStatus, ref strUserMessage, ref strDevMessage);

                            obj = darrbalcusjson;

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
        [HttpGet("[action]")]
        public IActionResult GetCreditMemo(
            //                                              //PURPOSE:
            //                                              //Get information about one credit memo.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetCreditMemo?intPkCreditMemo=1
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get information about one credit memo.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int intPkCreditMemo
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
                    CredmemjsonCreditMemoJson credmemjson;
                    AccAccounting.subGetCreditMemo(intPkCreditMemo, ps, out credmemjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = credmemjson;
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
        public IActionResult GetAccountBalance(
            //                                              //PURPOSE:
            //                                              //Get balance of the account bank.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetAccountBalance?intPk=1
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get balance of the account type bank.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int intPk
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
                //                                          //Verify if the printshop is not null and pk grater than zero.
                ps != null &&
                intPk > 0
                )
            {
                try
                {
                    double? numnBalanceAccount;
                    AccAccounting.subGetAccountBalance(intPk, ps, out numnBalanceAccount, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = numnBalanceAccount;
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
        public IActionResult GetContactBillingAddress(
            //                                              //PURPOSE:
            //                                              //Get contact's billing address.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetContactBillingAddress?intContactId=453422
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get contact's billing address.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int intContactId
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            try
            {
                Contaddjson2ContactAddressJson2 custaddjson2;
                AccAccounting.subGetContactBillingAddress(intContactId, strPrintshopId, out custaddjson2, ref intStatus,
                    ref strUserMessage, ref strDevMessage);
                obj = custaddjson2;
            }
            catch (Exception ex)
            {
                //                                          //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }
            
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetStatement(
            //                                              //PURPOSE:
            //                                              //Get information about statement

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetStatement
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get information about one statement.

            //                                              //Json input.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            [FromBody] JsonElement jsonStatement
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
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonStatement.ValueKind == 7) &&
                !((int)jsonStatement.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonStatement.TryGetProperty("strType", out json) &&
                (int)jsonStatement.GetProperty("strType").ValueKind == 3 &&
                jsonStatement.TryGetProperty("intContactId", out json) &&
                (int)jsonStatement.GetProperty("intContactId").ValueKind == 4
                )
            {
                String strType = jsonStatement.GetProperty("strType").GetString();
                int intContactId = jsonStatement.GetProperty("intContactId").GetInt32();

                //                                          //StartDate.
                String strStartDate = null;
                if (
                    jsonStatement.TryGetProperty("strStartDate", out json) &&
                    (int)jsonStatement.GetProperty("strStartDate").ValueKind == 3
                    )
                {
                    strStartDate = jsonStatement.GetProperty("strStartDate").GetString();
                }

                //                                          //EndDate.
                String strEndDate = null;
                if (
                    jsonStatement.TryGetProperty("strEndDate", out json) &&
                    (int)jsonStatement.GetProperty("strStartDate").ValueKind == 3
                    )
                {
                    strEndDate = jsonStatement.GetProperty("strEndDate").GetString();
                }

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data.";
                if (
                    intContactId > 0
                    )
                {
                    try
                    {
                        StmjsonStatementJson stmjsonStatemet;
                        AccAccounting.subGetStatement(ps, strType, strStartDate, strEndDate, intContactId,
                            out stmjsonStatemet, ref intStatus, ref strUserMessage, ref strDevMessage);
                        obj = stmjsonStatemet;
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
        [HttpGet("[action]")]
        public IActionResult GetAccount(
            //                                              //PURPOSE:
            //                                              //Get data of an account

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /GetAccount?intPk=2
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get data of an account.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            int intPk
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
                //                                          //Verify if the printshop is not null and pk grater than zero.
                ps != null && intPk > 0
                )
            {
                try
                {
                    Accjson4AccountJson4 accjson4;
                    AccAccounting.subGetAccount(intPk, ps.intPk, out accjson4, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = accjson4;
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
        [HttpPost("[action]")]
        public IActionResult UpdateAccount(
            //                                              //PURPOSE:
            //                                              //Update an Account.

            //                                              //URL: http://localhost:5001/Odyssey2/Accounting
            //                                              //      /UpdateAccount
            //                                              //Method: POST.

            //                                              //DESCRIPTION:
            //                                              //Update an account.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            [FromBody] JsonElement jsonPayment
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !((int)jsonPayment.ValueKind == 7) &&
                !((int)jsonPayment.ValueKind == 0) &&
                //                                          //Verify if the data is not null or empty.
                jsonPayment.TryGetProperty("intPk", out json) &&
                (int)jsonPayment.GetProperty("intPk").ValueKind == 4 &&
                jsonPayment.TryGetProperty("strName", out json) &&
                (int)jsonPayment.GetProperty("strName").ValueKind == 3 &&
                jsonPayment.TryGetProperty("strNumber", out json) &&
                (int)jsonPayment.GetProperty("strNumber").ValueKind == 3
                )
            {
                int intPk = jsonPayment.GetProperty("intPk").GetInt32();
                String strName = jsonPayment.GetProperty("strName").GetString();
                String strNumber = jsonPayment.GetProperty("strNumber").GetString();

                //                                              //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data.";
                if (
                    intPk > 0 &&
                    !String.IsNullOrEmpty(strName) &&
                    !String.IsNullOrEmpty(strNumber)
                    )
                {
                    //                                      //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                  //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                AccAccounting.subUpdateAccount(ps, intPk, strName, strNumber,
                                    context, ref intStatus, ref strUserMessage,
                                    ref strDevMessage);

                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
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
                                //                          //Discards all changes made to the database in the current
                                //                          //      transaction.
                                dbContextTransaction.Rollback();

                                //                          //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }
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
    }

    //==================================================================================================================
}
/*END-TASK*/
