/*TASK RP. PRINTSHOP*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.JsonTemplates.Out;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TowaStandard;
using Microsoft.AspNetCore.SignalR;
using Odyssey2Backend.Alert;

//                                                          //AUTHOR: Towa (CLGA-Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (AQG-Andrea Quiroz).
//                                                          //DATE: March 06, 2020.

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //  var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
    //                                                      //  String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
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
        private readonly IHubContext<ConnectionHub> iHubContext;

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        public ProcessController(
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
        [HttpPost("[action]")]
        public IActionResult DeleteTypeOrTemplate(
            //                                              //PURPOSE:
            //                                              //Delete Type or template of a process.

            //                                              //URL: http://localhost/Odyssey2/Process/
            //                                              //      DeleteTypeOrTemplate
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkEleetOrEleele":1,
            //                                              //          "boolIsEleet":true
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Delete a Type or template of a process.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Dynamic object that contains all necessary data.
            [FromBody] JsonElement jsonTypeOrTemp
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Data invalid, Json Invalid.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonTypeOrTemp) &&
                //                                          //Verify if the data is not null or empty.
                jsonTypeOrTemp.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonTypeOrTemp.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonTypeOrTemp.TryGetProperty("boolIsEleet", out json) &&
                (((int)jsonTypeOrTemp.GetProperty("boolIsEleet").ValueKind == 5) ||
                ((int)jsonTypeOrTemp.GetProperty("boolIsEleet").ValueKind == 6))
                )
            {
                int intPkEleetOrEleele = jsonTypeOrTemp.GetProperty("intPkEleetOrEleele").GetInt32();
                bool boolIsEleet = jsonTypeOrTemp.GetProperty("boolIsEleet").GetBoolean();

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data";
                if (
                    intPkEleetOrEleele > 0 &&
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
                                int intPkProcess;
                                ProProcess.subDeleteIO(intPkEleetOrEleele, boolIsEleet, ps, context, out intPkProcess,
                                    ref intStatus, ref strUserMessage, ref strDevMessage);
                                obj = intPkProcess;

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
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage, obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult Add(
            //                                              //PURPOSE:
            //                                              //Add a new process to the printshop.

            //                                              //URL: http://localhost/Odyssey2/Process
            //                                              //      /Add
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkType":2,
            //                                              //          "strProcessName":"CuttingA"
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add a process to the printshop.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Dynamic object that contains all necessary data.
            [FromBody] JsonElement jsonProcess
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonProcess) &&
                //                                          //Verify if the data is not null or empty.
                jsonProcess.TryGetProperty("intPkType", out json) &&
                (int)jsonProcess.GetProperty("intPkType").ValueKind == 4 &&
                jsonProcess.TryGetProperty("strProcessName", out json) &&
                (int)jsonProcess.GetProperty("strProcessName").ValueKind == 3
                )
            {
                //                                          //Get all the values.
                int intTypePk = jsonProcess.GetProperty("intPkType").GetInt32();
                String strName = jsonProcess.GetProperty("strProcessName").GetString();

                //                                          //Find process.
                EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(intTypePk);

                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "It is not a Process or printshop does not exists.";
                if (
                    (et != null) &&
                    (et.strResOrPro == EtElementTypeAbstract.strProcess) &&
                    (ps != null)
                    )
                {
                    try
                    {
                        ProtypProcessType protyp = (ProtypProcessType)et;
                        //                                          //Add the process.
                        ProProcess.subAdd(strName, protyp, strPrintshopId, ref intStatus, ref strUserMessage,
                            ref strDevMessage);
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
            IActionResult aresult = Ok(respjson1);

            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddNewCustomProcess(
           //                                              //PURPOSE:
           //                                              //Add a new custom process type.

           //                                              //URL: http://localhost/Odyssey2/Process
           //                                              //      /AddNewCustomProcess.
           //                                              //Method: POST.
           //                                              //Use a JSON like this:
           //                                              //      {
           //                                              //          "strProcessName : "MyProcess",
           //                                              //      }

           //                                              //DESCRIPTION:
           //                                              //Add a custom process type from a printshop.

           //                                              //RETURNS:
           //                                              //      200 - Ok(intPk).
           //                                              //      400 - "Something is wrong."
           //                                              //      401 - BadRequest("The custom type already exists.").
           //                                              //      401 - BadRequest("The name of the custom type can 
           //                                              //              not start with XJDF.").

           //                                              //Dynamic object that contains a Json string with all data.
           [FromBody] dynamic jsonProtyp
           )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonProtyp) &&
                //                                          //Verify if the data is not null or empty.
                jsonProtyp.TryGetProperty("strProcessName", out json) &&
                (int)jsonProtyp.GetProperty("strProcessName").ValueKind == 3
                )
            {
                //                                          //Get properties.
                String strCustomProcessType = jsonProtyp.GetProperty("strProcessName").GetString();
               
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Printshop not found.";
                if (
                    ps != null
                    )
                {
                    try
                    {
                        int intPkPrintshop = ps.intPk;
                        //                                  //Add the process.               
                        int intPk;
                        ProProcess.subAddProcessCustomType(strPrintshopId, intPkPrintshop, strCustomProcessType,
                            out intPk, ref intStatus, ref strUserMessage, ref strDevMessage);
                        obj = intPk;
                    }
                    catch (Exception ex)
                    {
                        //                                  //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }                    
                }
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, "", obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //------------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult DeleteProcessFromPrintshop(
           //                                               //PURPOSE:
           //                                               //Delete a process from a printshop.                                        

           //                                               //URL: http://localhost/Odyssey2/Process
           //                                               //      /DeleteProcessFromPrintshop
           //                                               //Method: POST.
           //                                               //Use a JSON like this:
           //                                               //      {
           //                                               //          "intPkProcess" : 3
           //                                               //      }

           //                                               //DESCRIPTION:
           //                                               //Delete a process from Printshop.
           //                                               //Printshop is not needed, because the process has a type,
           //                                               //      and the is related to the printshop. 
           //                                               //E.g. The process type 366 is the cutting process type for
           //                                               //      printshop 1.
           //                                               //The process to be deleted comes from 366 type.

           //                                               //RETURNS:
           //                                               //      200 - Ok

           //                                               //Receive the pk of the process.

           [FromBody] JsonElement resData
           )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, resData) &&
                resData.TryGetProperty("intPkProcess", out json) &&
                (int)resData.GetProperty("intPkProcess").ValueKind == 4
                )
            {
                //                                          //Get the pk of the process to delete.
                int intPkProcess = resData.GetProperty("intPkProcess").GetInt32();

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
                    try
                    {
                        //                                          //Method to delete the process.
                        ProtypProcessType.subDeleteProcessFromPrintshop(intPkProcess, ps, ref intStatus,
                            ref strDevMessage, ref strUserMessage);
                    }
                    catch (Exception ex)
                    {
                        //                                      //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }                    
                }
            }
            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //------------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult EditName(
           //                                               //PURPOSE:
           //                                               //Renames a process.                                        
           //                                               //URL: http://localhost/Odyssey2/Process
           //                                               //      /EditName
           //                                               //Method: POST.
           //                                               //Use a JSON like this:
           //                                               //      {
           //                                               //          "intPkProcess" : 3,
           //                                               //          "strProcessName": ""
           //                                               //      }

           //                                               //DESCRIPTION:
           //                                               //Renames a process or custom process.

           //                                               //RETURNS:
           //                                               //      200 - Ok

           //                                               //Receives JSON with data.
           [FromBody] JsonElement jsonProcess
           )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, jsonProcess) &&
                jsonProcess.TryGetProperty("intPkProcess", out json) &&
                (int)jsonProcess.GetProperty("intPkProcess").ValueKind == 4 &&
                jsonProcess.TryGetProperty("strProcessName", out json) &&
                (int)jsonProcess.GetProperty("strProcessName").ValueKind == 3
                )
            {
                //                                          //Get the pk of the process to edit.
                int intPkProcess = jsonProcess.GetProperty("intPkProcess").GetInt32();
                //                                          //Get new name.
                String strProcessName = jsonProcess.GetProperty("strProcessName").GetString();

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
                    try
                    {
                        //                                      //Method to rename the process.
                        ProProcess.subEditName(intPkProcess, strProcessName, ps, ref intStatus, ref strUserMessage,
                            ref strDevMessage);
                    }
                    catch (Exception ex)
                    {
                        //                                  //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                }                                   
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult InputOrOutputIsDispensable(
            //                                              //PURPOSE:
            //                                              //Get true if the io is dispensable, it means that could be 
            //                                              //      deleted.

            //                                              //URL: http://localhost/Odyssey2/Process
            //                                              //      //InputOrOutputIsDispensable?
            //                                              //      intPk=1&&intPkEleetOrEleele=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get true if the intPkEleetOrEleele has not links or the 
            //                                              //      process is not used in workflows with estimates.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int intPkEleetOrEleele,
            bool boolIsEleet
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            object obj = null;
            if (
                intPkEleetOrEleele > 0
                )
            {
                //                                          //Get the printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                try
                {
                    bool boolIOIsDispensable;
                    ProProcess.subIOIsDispensable(intPkEleetOrEleele, boolIsEleet, strPrintshopId, this.configuration,
                        this.iHubContext, out boolIOIsDispensable, ref intStatus, ref strDevMessage, ref strUserMessage);
                    obj = boolIOIsDispensable;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult IsModifiable(
            //                                              //PURPOSE:
            //                                              //Get true if the process is not in a wf with estimates.

            //                                              //URL: http://localhost/Odyssey2/Process
            //                                              //      /IsModifiable?intPkProcess=1&&
            //                                              //      strInputOrOutput=Input

            //                                              //Method: GET.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkProcess" : 1,
            //                                              //          "intnPkType" : 5,
            //                                              //          "intnPkTemplate" : null,
            //                                              //          "strInputOrOutput" : "Input"    
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Get true if the process is in at least one workflow with 
            //                                              //      estimates and the modification is for an input.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonIO
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonIO) &&
                //                                          //Verify if the data is not null or empty.
                jsonIO.TryGetProperty("intPkProcess", out json) &&
                (int)jsonIO.GetProperty("intPkProcess").ValueKind == 4 &&
                jsonIO.TryGetProperty("strInputOrOutput", out json) &&
                (int)jsonIO.GetProperty("strInputOrOutput").ValueKind == 3
                )
            {
                int intPkProcess = jsonIO.GetProperty("intPkProcess").GetInt32();
                String strInputOrOutput = jsonIO.GetProperty("strInputOrOutput").GetString();

                int? intnPkType = null;
                if (
                    jsonIO.TryGetProperty("intnPkType", out json) &&
                    (int)jsonIO.GetProperty("intnPkType").ValueKind == 4
                    )
                {
                    intnPkType = jsonIO.GetProperty("intnPkType").GetInt32();
                }

                int? intnPkTemplate = null;
                if (
                    jsonIO.TryGetProperty("intnPkTemplate", out json) &&
                    (int)jsonIO.GetProperty("intnPkTemplate").ValueKind == 4
                    )
                {
                    intnPkTemplate = jsonIO.GetProperty("intnPkTemplate").GetInt32();
                }

                try
                {
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;

                    bool boolIsModifiable;
                    ProProcess.subProcessModifiable(intPkProcess, strInputOrOutput, intnPkType, intnPkTemplate,
                        strPrintshopId, this.configuration, this.iHubContext,  out boolIsModifiable, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = boolIsModifiable;
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
        public IActionResult GetProcessesOfAProcessType(
            //                                              //PURPOSE:
            //                                              //Get all processes of a process type.

            //                                              //URL: http://localhost/Odyssey2/Process
            //                                              //      /GetProcesses

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all processes of a process type.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            //                                              //Pk process type.
            int intPkProcessType
            )
        {
            int intStatus = 400;
            String strDevMessage = "Invalid data.";
            String strUserMessage = "Something is wrong.";
            Object obj = null;

            if (
                intPkProcessType > 0
                )
            {
                intStatus = 300;
                strDevMessage = "Process type not exist in database.";
                strUserMessage = "";
                //                                          //Valid the process type.
                EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(intPkProcessType);
                ProtypProcessType protyp = ((et != null) && (et.strResOrPro == EtElementTypeAbstract.strProcess)) ?
                    (ProtypProcessType)et : null;

                if (
                    protyp != null
                    )
                {
                    intStatus = 200;
                    strUserMessage = "";
                    strDevMessage = "";
                    bool boolIsXJDF = true;

                    if (
                        protyp.strCustomTypeId == EtElementTypeAbstract.strProCustomType
                        )
                    {
                        //                                  //This process is a custom process.
                        boolIsXJDF = false;
                    }

                    try
                    {
                        //                                      //Get the dictionary of processes of a process type.
                        Dictionary<int, ProProcess> dicproProcess = protyp.dicpro;

                        //                                      //List to add the process.
                        List<Proelejson3ProcessElementJson3> darrproele = new List<Proelejson3ProcessElementJson3>();
                        foreach (KeyValuePair<int, ProProcess> pro in dicproProcess)
                        {
                            Proelejson3ProcessElementJson3 proele = new Proelejson3ProcessElementJson3(pro.Value.intPk,
                                pro.Value.strName, boolIsXJDF);
                            darrproele.Add(proele);
                        }
                        obj = darrproele;
                    }
                    catch (Exception ex)
                    {
                        //                                      //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
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
        [HttpGet("[action]/")]
        public IActionResult GetPrintshopProcesses(
            //                                              //PURPOSE:
            //                                              //Get all the processes associated with a printshop.

            //                                              //URL: http://localhost/Odyssey2/Process
            //                                              //      /GetPrintshopProcesses
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all the processes from a printshop

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;

            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            if (
               //                                           //Verify if the printshop is not null.
               !String.IsNullOrEmpty(strPrintshopId)
               )
            {
                try
                {
                    //                                          //Method to bring the processes.
                    List<Proelejson1ProcessElementJson1> darrprojson;
                    ProProcess.subGetPrintshopProcesses(strPrintshopId, out darrprojson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = darrprojson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }

            //                                              //Return a list of jsons.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage, obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult IsDispensable(
            //                                              //PURPOSE:
            //                                              //Get true if the resource has calculations associated or  
            //                                              //      links, get false if the resource has not  
            //                                              //      calculations associated.

            //                                              //URL: http://localhost/Odyssey2/Process
            //                                              //      /IsDispensable?intPk=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get a boolean to know if the resource with that Pk has or 
            //                                              //      not a calculation associated and/or links.

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
                ProProcess pro = ProProcess.proFromDB(intPk);

                intStatus = 401;
                strDevMessage = "Process not found.";
                if (
                    pro != null
                    )
                {
                    try
                    {
                        bool boolIsDispensable = pro.boolIsDispensable(ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                        obj = boolIsDispensable;
                    }
                    catch (Exception ex)
                    {
                        //                                      //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }                    
                }
            }

            //                                              //Generate the response.
            Respjson1ResponceJson1 respjson = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult PeriodIsAddable(
            //                                              //PURPOSE:
            //                                              //Verify if the times for the period do not overlapping with
            //                                              //      the rules of the printshop and are times after the 
            //                                              //      end of the periods for past processes.

            //                                              //URL: http://localhost/Odyssey2/Process
            //                                              //      /PeriodIsAddable

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get a boolean to know if the period is addable.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            [FromBody] JsonElement jsonProcess
            )
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify data.
                !Object.ReferenceEquals(null, jsonProcess) &&
                jsonProcess.TryGetProperty("intJobId", out json) &&
                (int)jsonProcess.GetProperty("intJobId").ValueKind == 4 &&
                jsonProcess.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonProcess.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonProcess.TryGetProperty("intPkCalculation", out json) &&
                (int)jsonProcess.GetProperty("intPkCalculation").ValueKind == 4 &&
                jsonProcess.TryGetProperty("strStartDate", out json) &&
                (int)jsonProcess.GetProperty("strStartDate").ValueKind == 3 &&
                jsonProcess.TryGetProperty("strStartTime", out json) &&
                (int)jsonProcess.GetProperty("strStartTime").ValueKind == 3 &&
                jsonProcess.TryGetProperty("strEndDate", out json) &&
                (int)jsonProcess.GetProperty("strEndDate").ValueKind == 3 &&
                jsonProcess.TryGetProperty("strEndTime", out json) &&
                (int)jsonProcess.GetProperty("strEndTime").ValueKind == 3
                )
            {
                //                                          //Get data.
                int intJobId = jsonProcess.GetProperty("intJobId").GetInt32();
                int intPkProcessInWorkflow = jsonProcess.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkCalculation = jsonProcess.GetProperty("intPkCalculation").GetInt32();
                String strStartDate = jsonProcess.GetProperty("strStartDate").GetString();
                String strStartTime = jsonProcess.GetProperty("strStartTime").GetString();
                String strEndDate = jsonProcess.GetProperty("strEndDate").GetString();
                String strEndTime = jsonProcess.GetProperty("strEndTime").GetString();

                int? intnPkPeriod = null;
                if (
                    jsonProcess.TryGetProperty("intnPkPeriod", out json) &&
                    (int)jsonProcess.GetProperty("intnPkPeriod").ValueKind == 4
                    )
                {
                    intnPkPeriod = jsonProcess.GetProperty("intnPkPeriod").GetInt32();
                }

                int? intnContactId = null;
                if (
                    jsonProcess.TryGetProperty("intnContactId", out json) &&
                    (int)jsonProcess.GetProperty("intnContactId").ValueKind == 4
                    )
                {
                    intnContactId = jsonProcess.GetProperty("intnContactId").GetInt32();
                }

                try
                {
                    //                                          //Method that verify if the period is addable.
                    PerisaddablejsonPeriodIsAddableJson perisaddablejson;
                    ProProcess.subPeriodIsAddable(strPrintshopId, intJobId, intnPkPeriod, intPkProcessInWorkflow,
                        intPkCalculation, intnContactId, strStartDate, strStartTime, strEndDate, strEndTime, false,
                        this.configuration, out perisaddablejson, ref intStatus, ref strDevMessage, ref strUserMessage);
                    obj = perisaddablejson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }
            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetEndOfPeriod(
            //                                              //PURPOSE:
            //                                              //Get the end of the period according with the given date 
            //                                              //      and the time for that job.

            //                                              //URL: http://localhost/Odyssey2/Process
            //                                              //      /GetEndForPeriod

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the end of the period.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            [FromBody] JsonElement jsonProcess
            )
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify data.
                !Object.ReferenceEquals(null, jsonProcess) &&
                jsonProcess.TryGetProperty("intJobId", out json) &&
                (int)jsonProcess.GetProperty("intJobId").ValueKind == 4 &&
                jsonProcess.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonProcess.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonProcess.TryGetProperty("intPkCalculation", out json) &&
                (int)jsonProcess.GetProperty("intPkCalculation").ValueKind == 4 &&
                jsonProcess.TryGetProperty("strStartDate", out json) &&
                (int)jsonProcess.GetProperty("strStartDate").ValueKind == 3 &&
                jsonProcess.TryGetProperty("strStartTime", out json) &&
                (int)jsonProcess.GetProperty("strStartTime").ValueKind == 3
                )
            {
                //                                          //Get data.
                int intJobId = jsonProcess.GetProperty("intJobId").GetInt32();
                int intPkProcessInWorkflow = jsonProcess.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkCalculation = jsonProcess.GetProperty("intPkCalculation").GetInt32();
                String strStartDate = jsonProcess.GetProperty("strStartDate").GetString();
                String strStartTime = jsonProcess.GetProperty("strStartTime").GetString();

                try
                {
                    EndperjsonEndOfPeriodJson endperjson;
                    //                                          //Method that verify if the period is addable.
                    ProProcess.subGetEndOfPeriod(strPrintshopId, intJobId, intPkProcessInWorkflow, intPkCalculation,
                        strStartDate, strStartTime, this.configuration, out endperjson, ref intStatus, ref strDevMessage,
                        ref strUserMessage);
                    obj = endperjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }
            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------

        [HttpPost("[action]")]
        public IActionResult SetPeriod(
            //                                              //PURPOSE:
            //                                              //Verify if the times for the period do not overlapping with
            //                                              //      the rules of the printshop and are times after the 
            //                                              //      end of the periods for past processes and if all 
            //                                              //      results ok then add the period.

            //                                              //URL: http://localhost/Odyssey2/Process
            //                                              //      /SetPeriod

            //                                              //Method: POST.

            //                                              //DESCRIPTION:
            //                                              //Set a period to the process in workflow for a job.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            [FromBody] JsonElement jsonProcess
            )
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify data.
                !Object.ReferenceEquals(null, jsonProcess) &&
                jsonProcess.TryGetProperty("intJobId", out json) &&
                (int)jsonProcess.GetProperty("intJobId").ValueKind == 4 &&
                jsonProcess.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonProcess.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonProcess.TryGetProperty("intPkCalculation", out json) &&
                (int)jsonProcess.GetProperty("intPkCalculation").ValueKind == 4 &&
                jsonProcess.TryGetProperty("strStartDate", out json) &&
                (int)jsonProcess.GetProperty("strStartDate").ValueKind == 3 &&
                jsonProcess.TryGetProperty("strStartTime", out json) &&
                (int)jsonProcess.GetProperty("strStartTime").ValueKind == 3 &&
                jsonProcess.TryGetProperty("strEndDate", out json) &&
                (int)jsonProcess.GetProperty("strEndDate").ValueKind == 3 &&
                jsonProcess.TryGetProperty("strEndTime", out json) &&
                (int)jsonProcess.GetProperty("strEndTime").ValueKind == 3 &&
                jsonProcess.TryGetProperty("intMinsBeforeDelete", out json) &&
                (int)jsonProcess.GetProperty("intMinsBeforeDelete").ValueKind == 4 
                )
            {
                //                                          //Get data.
                int intJobId = jsonProcess.GetProperty("intJobId").GetInt32();
                int intPkProcessInWorkflow = jsonProcess.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkCalculation = jsonProcess.GetProperty("intPkCalculation").GetInt32();
                String strStartDate = jsonProcess.GetProperty("strStartDate").GetString();
                String strStartTime = jsonProcess.GetProperty("strStartTime").GetString();
                String strEndDate = jsonProcess.GetProperty("strEndDate").GetString();
                String strEndTime = jsonProcess.GetProperty("strEndTime").GetString();
                int intMinsBeforeDelete = jsonProcess.GetProperty("intMinsBeforeDelete").GetInt32();

                int? intnPkPeriod = null;
                if (
                    jsonProcess.TryGetProperty("intnPkPeriod", out json) &&
                    (int)jsonProcess.GetProperty("intnPkPeriod").ValueKind == 4
                    )
                {
                    intnPkPeriod = jsonProcess.GetProperty("intnPkPeriod").GetInt32();
                }

                String strPassword = null;
                if (
                    jsonProcess.TryGetProperty("strPassword", out json) &&
                    (int)jsonProcess.GetProperty("strPassword").ValueKind == 3
                    )
                {
                    strPassword = jsonProcess.GetProperty("strPassword").GetString();
                }

                int? intnContactId = null;
                if (
                    jsonProcess.TryGetProperty("intnContactId", out json) &&
                    (int)jsonProcess.GetProperty("intnContactId").ValueKind == 4
                    )
                {
                    intnContactId = jsonProcess.GetProperty("intnContactId").GetInt32();
                }

                try
                {
                    CalperjsonCalculationPeriodJson calperjson;
                    //                                          //Method that verify if the period is addable.
                    ProProcess.subSetPeriod(intnPkPeriod, strPassword, strPrintshopId, intJobId, intPkProcessInWorkflow,
                        intPkCalculation, intnContactId, strStartDate, strStartTime, strEndDate, strEndTime, false,
                        intMinsBeforeDelete, this.configuration, out calperjson, ref intStatus, ref strDevMessage,
                        ref strUserMessage);
                    obj = calperjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }                
            }
            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult DeletePeriod(
            //                                              //PURPOSE:
            //                                              //Delete a process's period.

            //                                              //URL: http://localhost/Odyssey2/Process
            //                                              //      /DeletePeriod
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkPeriod":12,
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Delete a process's period from db.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Dynamic JSON which contains all necessary data.
            [FromBody] JsonElement jsonPeriod
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid Data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, jsonPeriod) &&
                jsonPeriod.TryGetProperty("intPkPeriod", out json) &&
                (int)jsonPeriod.GetProperty("intPkPeriod").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                int intPkPeriod = jsonPeriod.GetProperty("intPkPeriod").GetInt32();

                try
                {
                    //                                              //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;

                    String strLastSunday;
                    String strEstimatedDate;
                    //                                          //Method to delete a period.
                    ProProcess.subDeletePeriod(intPkPeriod, strPrintshopId, this.configuration, this.iHubContext,
                        out strLastSunday, out strEstimatedDate, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = new { strLastSunday = strLastSunday, strEstimatedDate = strEstimatedDate } ;
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
    }

    //==================================================================================================================
}
/*END-TASK*/
