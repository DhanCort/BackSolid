/*TASK RP. WISNET*/
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.Job;
using Odyssey2Backend.JsonTypes;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using Odyssey2Backend.Wisnet;
using System.Collections.Generic;
using Odyssey2Backend.Customer;
using System.Text.Json;
using TowaStandard;
using System.IO;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.XJDF;
using Odyssey2Backend.Alert;
using Microsoft.AspNetCore.SignalR;
using Odyssey2Backend.Utilities;

//                                                          //AUTHOR: Towa (IUGS - Ivan Guzman).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: December 18, 2020. 

namespace Odyssey2Backend.Controllers
{    
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class WisnetController : Controller
    {
        //                                                  //Controller associated to the API to deliver to Wisnet.
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
        private readonly IHubContext<ConnectionHub> iHubContext;

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        public WisnetController(
            IConfiguration iConfiguration_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            this.configuration = iConfiguration_I;
            this.iHubContext = iHubContext_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Token(
            //                                              //PURPOSE:
            //                                              //Deliver the token to give Wisnet authirization.

            //                                              //URL: http://localhost/Odyssey2/Wisnet
            //                                              //      /Token

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //The Token this method delivers is used by Wisnet to request
            //                                              //      information from Odyssey 2.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            )
        {
            int intStatus;
            String strUserMessage;
            String strDevMessage;
            TokenjsonTokenJson tokenjson;
            WisnetWisnet.subToken(this.configuration, out tokenjson, out intStatus, out strUserMessage, out strDevMessage);

            Object obj = tokenjson;
            
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage, obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Wisnet")]
        public IActionResult GetPrice(
            //                                              //PURPOSE:
            //                                              //Get the price for a job to deliver to Wisnet.

            //                                              //URL: http://localhost/Odyssey2/Workflow/GetForPrice?
            //                                              //      strPrintshopId_I = 13832 &
            //                                              //      intProductKey_I = 271930 &
            //                                              //      intQuantity_I = 250 &
            //                                              //      intnJobId_I = 577581

            //                                              //Method: POST.

            //                                              //DESCRIPTION:
            //                                              //The price will be displayed by Wisnet in the control center.

            //                                              //RETURNS:
            //                                              //      200 - Ok            
            String strPrintshopId_I,
            int intProductKey_I,
            int intQuantity_I,
            int? intnJobId_I,
            [FromBody] List<AttrjsonAttributeJson> darrattrjson_I
            )
        {
            //                                              //Get the printshop Entity.
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

            //                                              //Job json.
            JobjsonJobJson jobjsonJob;

            //                                              //Jobjsonentity.
            JobjsonentityJobJsonEntityDB jobjsonentity;

            //
            int intPkWorkflow;

            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            if (
                //                                          //Validate all the input data.
                (ps != null) &&
                (intProductKey_I > 0) &&
                (intQuantity_I >= 0) &&
                ModelState.IsValid &&
                JobJob.boolIsThereATemporalJobForAPrintshop(intnJobId_I, strPrintshopId_I, intProductKey_I,
                    intQuantity_I, darrattrjson_I, out jobjsonentity, out jobjsonJob, this.configuration,
                    ref strUserMessage, ref strDevMessage) &&
                ProdtypProductType.boolIsThereADefaultWorkflow(ps.intPk, jobjsonJob.intnProductKey, out intPkWorkflow,
                    ref strUserMessage, ref strDevMessage)
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                try
                {
                    //                                      //Set price to 0, if the quantity is 0 return this price.
                    WisprijsonWisnetPriceJson wisprijson = new WisprijsonWisnetPriceJson(0);

                    intStatus = 200;
                    strUserMessage = "success";
                    strDevMessage = "";
                    if (
                        intQuantity_I > 0
                        )
                    {
                        //                                      //Get all the Job info. 
                        WfjjsonWorkflowJobJson wfjjson = ProdtypProductType.wfjjsonGet(jobjsonJob.intJobId,
                            intPkWorkflow, ps, this.configuration, this.iHubContext, ref intStatus, ref strUserMessage,
                            ref strDevMessage, jobjsonJob);

                        //                                  //Back can crash due to default product's workflow is not
                        //                                  //      ready.
                        wisprijson.numJobPrice = wfjjson.numJobPrice;
                        

                        //                                  //Delete resources setted atm for this job and workflow.
                        ProdtypProductType.subDeleteResourceSetAutomaticallyForAGivenJobAndWorkflow(jobjsonJob.intJobId,
                            intPkWorkflow);
                    }

                    //                                      //When a price was sent to wisnet, set workflow as 
                    //                                      //      "pricing" wich means the WF was use to give a price
                    //                                      //      to wisnet.
                    if (
                        wisprijson.numJobPrice >= 0
                        )
                    {
                        //                                  //Establish the connection.
                        Odyssey2Context contex = new Odyssey2Context();

                        //                                  //Get workflow.
                        WfentityWorkflowEntityDB wfentity = contex.Workflow.FirstOrDefault(wf =>
                            wf.intPk == intPkWorkflow);

                        if (
                            wfentity != null && wfentity.boolPricing == false
                            )
                        {
                            wfentity.boolPricing = true;
                            contex.Workflow.Update(wfentity);
                            contex.SaveChanges();
                        }
                    }

                    obj = wisprijson;

                    //                                      //Delete jobjsonentity.
                    JobJob.subDeleteTemporalJob(jobjsonentity);
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    //                                      //Delete jobjsonentity.
                    JobJob.subDeleteTemporalJob(jobjsonentity);

                    //                                      //Delete resources setted atm for this job and workflow.
                    ProdtypProductType.subDeleteResourceSetAutomaticallyForAGivenJobAndWorkflow(jobjsonJob.intJobId,
                        intPkWorkflow);
                }
            }
            else
            {
                StreamWriter systextwriterLog;

                //                                          //To save file.
                PathX syspathLogFiles = DirectoryX.GetCurrent().GetPath().AddName("Z_LogFiles");

                if (
                    //                                      //Directory does not exist yet
                    !Directory.Exists(syspathLogFiles.FullPath)
                    )
                {
                    //                                      //Create directory
                    syspathLogFiles = new PathX(Directory.CreateDirectory(syspathLogFiles.FullPath).FullPath());
                }

                String strCurrentDate = ZonedTimeTools.ztimeNow.Date.ToString();

                PathX syspathFileForErrorLog = syspathLogFiles.AddName(strCurrentDate + " - Wisnet error log "
                    + ".txt");

                if (
                    //                                      //The File does not exist yet
                    !System.IO.File.Exists(syspathFileForErrorLog.FullPath)
                    )
                {
                    //                                      //Generate file
                    FileInfo sysfileNew = new FileInfo(syspathFileForErrorLog.FullPath);
                    systextwriterLog = sysfileNew.CreateText();
                }
                else
                {
                    //                                      //Add text to file
                    systextwriterLog = new StreamWriter(syspathFileForErrorLog.FullPath, true);
                }

                //                                          //Write first line in log
                String strCurrentTime = ZonedTimeTools.ztimeNow.Time.ToString();
                String strTextErrorInfoToLog = String.Format("{0}", strCurrentDate + " " + strCurrentTime);

                //                                          //Write error message
                systextwriterLog.WriteLine("Date: {0}", strTextErrorInfoToLog);
                systextwriterLog.WriteLine("ModelState.IsValid: {0}", ModelState.IsValid);
                systextwriterLog.WriteLine("darrattrjson_I: ");
                systextwriterLog.WriteLine("    {0}", darrattrjson_I == null ? "null" : JsonSerializer.Serialize(darrattrjson_I));
                systextwriterLog.WriteLine("strPrintshopId_I: {0}", strPrintshopId_I);
                systextwriterLog.WriteLine("intProductKey_I: {0}", intProductKey_I + "");
                systextwriterLog.WriteLine("intQuantity_I: {0}", intQuantity_I + "");
                systextwriterLog.WriteLine("----------------------------------------------------------------------" +
                    "---------------------------------------------------------\n");
                systextwriterLog.Dispose();
                systextwriterLog.Close();
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
