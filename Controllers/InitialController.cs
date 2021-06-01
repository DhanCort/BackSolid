/*TASK RP.INITIAL*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.App;
using Odyssey2Backend.Customer;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using TowaStandard;
using System.Threading.Tasks;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.Job;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 14, 2020. 

namespace Odyssey2Backend.Controllers
{
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class InitialController : Controller
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

        public InitialController(
            IConfiguration iConfiguration_I
            )
        {
            this.configuration = iConfiguration_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult AddUrlToPs(
            //                                              //PURPOSE:
            //                                              //Get every ps from wisnet and update strUrl in DB

            //                                              //URL: http://localhost/Odyssey2/Type/AddInitialData
            //                                              //Method: GET.

            //                                              //DESCRIPTION:

            //                                              //RETURNS:
            )
        {
            Odyssey2Context context = new Odyssey2Context();
            List<PsentityPrintshopEntityDB> darrpsentity = context.Printshop.Where(ps => ps.intPk > 0).ToList();
            foreach (PsentityPrintshopEntityDB psentity in darrpsentity)
            {
                String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                    GetSection("Odyssey2Settings")["urlWisnetApi"];
                Task<PsjsonPrintshopJson> Task_psjsonFromWisnet = HttpTools<PsjsonPrintshopJson>.GetOneAsyncToEndPoint(
                    strUrlWisnet + "/PrintShopData/printshopData/" + psentity.strPrintshopId);
                Task_psjsonFromWisnet.Wait();
                if (
                    Task_psjsonFromWisnet.Result != null
                    )
                {
                    PsjsonPrintshopJson psjsonFromWisnet = Task_psjsonFromWisnet.Result;
                    if (
                        psjsonFromWisnet.strPrintshopId != "-1"
                        )
                    {
                        psentity.strUrl = psjsonFromWisnet.strUrl;
                    }
                }
            }
            context.SaveChanges();

            return Ok();
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult AddInitialData(
            //                                              //PURPOSE:
            //                                              //Load the initial XJDF data.

            //                                              //URL: http://localhost/Odyssey2/Type/AddInitialData
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Create the general type with general attributes, the 
            //                                              //      catalogs and all data necessary to start Odyssey2.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            //                                              //Call the methods that add the necessary data.
            Odyssey2.subAddInitialDataToDb();
            RestypResourceType.subAddInitialDataToDb();
            ProtypProcessType.subAddInitialDataToDb();
            ProdtypProductType.subAddInitialDataToDb();
            InttypIntentType.subAddInitialDataToDb();
            ProtypProcessType.subAddInitialDataOfProcessWithResourcesToDb();
            RestypResourceType.subUpdateInitialClassificationResourceToDb();
            ProtypProcessType.subUpdateInitialClassificationProcessToDb();
            Odyssey2.subAddAdmin();
            Odyssey2.subAddAlertType();
            AccAccounting.subAddAccountTypes();
            AccAccounting.subAddPaymentMethods();

            return Ok();
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddTemplateAndResourceToPrintshopFromFile(
            //                                              //PURPOSE:
            //                                              //Load the template and resource XJDF data.

            //                                              //URL: http://localhost/Odyssey2/Initial/
            //                                              //    AddTemplateAndResourceToPrintshopFromFile
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Add Template a resource from file.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            String strPrintshopId, 
            String strFileName,
            String strAccountNumber
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                !String.IsNullOrEmpty(strPrintshopId) &&
                !String.IsNullOrEmpty(strAccountNumber) &&
                !String.IsNullOrEmpty(strFileName) &&
                strFileName.Contains(".csv")
                )
            {
                //                                          //Get the printshop.
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                strUserMessage = "Printshop not exist.";
                strDevMessage = "Printshop not exist.";
                if (
                    ps != null
                    )
                {
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                //                          //Call the methods that add the necessary data.
                                ResResource.subAddTemplateAndResourceToPrintshopFromFile(ps, strFileName, strAccountNumber, context, 
                                    ref intStatus, ref strUserMessage, ref strDevMessage);

                                //                          //Commits all changes made to the database in the current
                                //                          //      transaction.
                                if (
                                    intStatus == 200
                                    )
                                {
                                    dbContextTransaction.Commit();
                                    return Ok();
                                }
                                else
                                {
                                    dbContextTransaction.Rollback();
                                    return BadRequest();
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
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //------------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]/")]
        public IActionResult BuiltMessageForEstimateAndOrderAlerts(
            //                                              //PURPOSE:
            //                                              //Built the message for the estimate and order alerts.

            //                                              //URL: http://localhost/Odyssey2/Printshop/
            //                                              //     BUiltMessageForEstimateAndOrderAlerts
            //                                              //Method: POST.

            //                                              //DESCRIPTION:
            //                                              //Build the message and update the alerts message.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            //                                          //using is to release connection at the end
            using (Odyssey2Context context = new Odyssey2Context())
            {
                //                                      //Starts a new transaction.
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        JobJob.subBuiltMessageForEstimateAndOrderAlerts(this.configuration, context,
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

                        intStatus = 400;
                        strUserMessage = "Something is wrong.";
                        strDevMessage = ex.Message;
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