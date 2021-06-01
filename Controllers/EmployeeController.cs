/*TASK RP.EMPLOYEE*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.Alert;
using Odyssey2Backend.Job;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.JsonTypes.Out;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 22, 2020. 

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //  var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
    //                                                      //  String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class EmployeeController : Controller
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
        private readonly IWebHostEnvironment hostingEnvironment;

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        public EmployeeController(
            IConfiguration iConfiguration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            IWebHostEnvironment iHostingEnvironment_I
            )
        {
            this.configuration = iConfiguration_I;
            this.iHubContext = iHubContext_I;
            this.hostingEnvironment = iHostingEnvironment_I;
        }


        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.


        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SetFinalStart(
            //                                              //PURPOSE:
            //                                              //Set final start date and time.

            //                                              //URL: http://localhost/Odyssey2/Employee
            //                                              //      /SetFinalStartDateAndTimeToPeriod
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkPeriod": 00,
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Set final start date and time to a process o resource
            //                                              //      period.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Dynamic json that contains all necessary data.
            [FromBody] JsonElement jsonPeriod
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

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

                //                                              //Get the contact id from token.
                var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                int intContactId = idClaimContact.Value.ParseToInt();

                try
                {
                    bool boolAskEmailNeedsToBeSent = false;
                    //                                          //Method to add period.
                    EmplEmployee.subSetFinalStart(intPkPeriod, strPrintshopId, intContactId, this.configuration,
                        this.iHubContext, ref boolAskEmailNeedsToBeSent, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = boolAskEmailNeedsToBeSent;
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
        [HttpPost("[action]")]
        public IActionResult SetFinalEnd(
            //                                              //PURPOSE:
            //                                              //Set final end date and time.

            //                                              //URL: http://localhost/Odyssey2/Employee
            //                                              //      /SetFinalEnd
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkPeriod": 00,
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Set final end date and time to a process o resource
            //                                              //      period.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Dynamic json that contains all necessary data.
            [FromBody] JsonElement jsonPeriod
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

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

                //                                              //Get the contact id from token.
                var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                int intContactId = idClaimContact.Value.ParseToInt();

                try
                {
                    bool boolAskEmailNeedsToBeSent = false;
                    //                                          //Method to add period.
                    EmplEmployee.subSetFinalEnd(intPkPeriod, strPrintshopId, intContactId, this.configuration,
                        this.iHubContext, ref boolAskEmailNeedsToBeSent, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = boolAskEmailNeedsToBeSent;
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
        [HttpPost("[action]")]
        public IActionResult SetTask(
            //                                              //PURPOSE:
            //                                              //Add or edit a task.

            //                                              //URL: http://localhost/Odyssey2/Employee
            //                                              //      /SetTask
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intnPkTask":12,
            //                                              //          "strDescription":"Llamar a Edgar...",
            //                                              //          "strStartDate":"2020-07-30",
            //                                              //          "strStartTime":"16:30:00",
            //                                              //          "strEndDate":"2020-07-30",
            //                                              //          "strEndTime":"17:00:00",
            //                                              //          "intMinutesForNotification":0,
            //                                              //          "boolIsNotifiedable":false,
            //                                              //          "intnContactId: 534534
            //                                              //      }
            //
            //                                              //DESCRIPTION:
            //                                              //Add or edit a task.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Dynamic json that contains all necessary data.
            [FromBody] JsonElement jsonTask
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid Data.";
            Object obj = null;
            JsonElement json;
            if (
                !Object.ReferenceEquals(null, jsonTask) &&
                jsonTask.TryGetProperty("strDescription", out json) &&
                (int)jsonTask.GetProperty("strDescription").ValueKind == 3 &&
                jsonTask.TryGetProperty("strStartDate", out json) &&
                (int)jsonTask.GetProperty("strStartDate").ValueKind == 3 &&
                jsonTask.TryGetProperty("strStartTime", out json) &&
                (int)jsonTask.GetProperty("strStartTime").ValueKind == 3 &&
                jsonTask.TryGetProperty("strEndDate", out json) &&
                (int)jsonTask.GetProperty("strEndDate").ValueKind == 3 &&
                jsonTask.TryGetProperty("strEndTime", out json) &&
                (int)jsonTask.GetProperty("strEndTime").ValueKind == 3 &&
                jsonTask.TryGetProperty("intMinutesForNotification", out json) &&
                (int)jsonTask.GetProperty("intMinutesForNotification").ValueKind == 4 &&
                jsonTask.TryGetProperty("boolIsNotifiedable", out json) &&
                ((int)jsonTask.GetProperty("boolIsNotifiedable").ValueKind == 5 ||
                (int)jsonTask.GetProperty("boolIsNotifiedable").ValueKind == 6)
                )
            {
                //                                          //Get data from json.
                String strDescription = jsonTask.GetProperty("strDescription").GetString();
                String strStartDate = jsonTask.GetProperty("strStartDate").GetString();
                String strStartTime = jsonTask.GetProperty("strStartTime").GetString();
                String strEndDate = jsonTask.GetProperty("strEndDate").GetString();
                String strEndTime = jsonTask.GetProperty("strEndTime").GetString();
                int intMinutesForNotification = jsonTask.GetProperty("intMinutesForNotification").GetInt32();
                bool boolIsNotifiedable = jsonTask.GetProperty("boolIsNotifiedable").GetBoolean();

                //                                          //Get the printshop id from token.
                var idClaimPrintshop = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaimPrintshop.Value;

                //                                          //Get the contact id from token.
                var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                int intContactId = idClaimContact.Value.ParseToInt();

                //                                          //Set the task, if it comes, get it from the json.
                int? intnPkTask = null;
                if (
                    jsonTask.TryGetProperty("intnPkTask", out json) &&
                    (int)jsonTask.GetProperty("intnPkTask").ValueKind == 4
                    )
                {
                    intnPkTask = jsonTask.GetProperty("intnPkTask").GetInt32();
                }

                //                                          //Customer Id.
                int? intnCustomerId = null;
                if (
                    jsonTask.TryGetProperty("intnContactId", out json) &&
                    (int)jsonTask.GetProperty("intnContactId").ValueKind == 4
                    )
                {
                    intnCustomerId = jsonTask.GetProperty("intnContactId").GetInt32();
                }

                try
                {
                    //                                          //Method to add task.
                    EmplEmployee.subSetTask(intnPkTask, strDescription, strStartDate, strStartTime, strEndDate, strEndTime,
                        intContactId, strPrintshopId, intMinutesForNotification, boolIsNotifiedable, intnCustomerId,
                        this.iHubContext, ref intStatus, ref strUserMessage, ref strDevMessage);
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
        [HttpPost("[action]")]
        public IActionResult DeleteTask(
            //                                              //PURPOSE:
            //                                              //Delete one task from DB.

            //                                              //URL: http://localhost/Odyssey2/Employee
            //                                              //      /DeleteTask
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intnPkTask": 1,
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Delete one task from DB.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Dynamic json that contains all necessary data.
            [FromBody] JsonElement jsonTask
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid Data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null.
                !Object.ReferenceEquals(null, jsonTask) &&
                jsonTask.TryGetProperty("intnPkTask", out json) &&
                (int)jsonTask.GetProperty("intnPkTask").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                //                                          //Not null variable but frontend prefers this name.
                int intPkTask = jsonTask.GetProperty("intnPkTask").GetInt32();

                if (
                    intPkTask > 0
                    )
                {
                    //                                      //Get the printshop id from token.
                    var idClaimPrintshop = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaimPrintshop.Value;

                    //                                      //Get the contact id from token.
                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    int intContactId = idClaimContact.Value.ParseToInt();

                    try
                    {
                        //                                      //Method to delete a task.
                        EmplEmployee.subDeleteTask(this.iHubContext, intPkTask, strPrintshopId, intContactId,
                            this.configuration, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                    catch (Exception ex)
                    {
                        //                                      //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                }
            }

            //                                              //Response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    obj);
            IActionResult aresult = base.Ok(respjson1);

            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult CompleteTask(
            //                                              //PURPOSE:
            //                                              //Complete one task from DB.

            //                                              //URL: http://localhost/Odyssey2/Employee
            //                                              //      /CompleteTask
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intnPkTask": 1,
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Complete one task from DB.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Dynamic json that contains all necessary data.
            [FromBody] JsonElement jsonTask
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid Data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null.
                !Object.ReferenceEquals(null, jsonTask) &&
                jsonTask.TryGetProperty("intnPkTask", out json) &&
                (int)jsonTask.GetProperty("intnPkTask").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                //                                          //Not null variable but frontend prefers this name.
                int intPkTask = jsonTask.GetProperty("intnPkTask").GetInt32();

                if (
                    intPkTask > 0
                    )
                {
                    //                                      //Get the printshop id from token.
                    var idClaimPrintshop = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaimPrintshop.Value;

                    //                                      //Get the contact id from token.
                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    int intContactId = idClaimContact.Value.ParseToInt();

                    try
                    {
                        //                                      //Method to complete a task.
                        EmplEmployee.subCompleteTask(this.iHubContext, intPkTask, strPrintshopId, intContactId,
                            this.configuration, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                    catch (Exception ex)
                    {
                        //                                      //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }                    
                }
            }

            //                                              //Response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    obj);
            IActionResult aresult = base.Ok(respjson1);

            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SetRole(
            //                                              //PURPOSE:
            //                                              //Add or remove role from an employee.

            //                                              //URL: http://localhost/Odyssey2/Employee
            //                                              //      /SetRole
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intContactId: 354345,
            //                                              //          "boolnIsSupervisor: true
            //                                              //          "boolnIsAccountant: false
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add or remove role from an employee.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Dynamic json that contains all necessary data.
            [FromBody] JsonElement jsonSupervisor
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid Data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null.
                !Object.ReferenceEquals(null, jsonSupervisor) &&
                jsonSupervisor.TryGetProperty("intContactId", out json) &&
                (int)jsonSupervisor.GetProperty("intContactId").ValueKind == 4)
            {
                //                                          //Get the printshop id from token.
                var idClaimPrintshop = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaimPrintshop.Value;

                var idClaimOwner = User.Claims.FirstOrDefault(c => c.Type == "boolIsOwner");
                bool boolIsOwner = idClaimOwner.Value.ParseToBool();

                var idClaimAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsAdmin");
                bool boolIsAdmin = idClaimAdmin.Value.ParseToBool();

                //                                          //Get data from json.
                int intContactId = jsonSupervisor.GetProperty("intContactId").GetInt32();

                bool? boolnIsSupervisor = null;
                if (
                    jsonSupervisor.TryGetProperty("boolnIsSupervisor", out json) &&
                    ((int)jsonSupervisor.GetProperty("boolnIsSupervisor").ValueKind == 5 ||
                    (int)jsonSupervisor.GetProperty("boolnIsSupervisor").ValueKind == 6)
                    )
                {
                    boolnIsSupervisor = jsonSupervisor.GetProperty("boolnIsSupervisor").GetBoolean();
                }

                bool? boolnIsAccountant = null;
                if (
                    jsonSupervisor.TryGetProperty("boolnIsAccountant", out json) &&
                    ((int)jsonSupervisor.GetProperty("boolnIsAccountant").ValueKind == 5 ||
                    (int)jsonSupervisor.GetProperty("boolnIsAccountant").ValueKind == 6)
                    )
                {
                    boolnIsAccountant = jsonSupervisor.GetProperty("boolnIsAccountant").GetBoolean();
                }

                if (
                    intContactId > 0
                    )
                {
                    intStatus = 401;
                    strUserMessage = "Something is wrong.";
                    strDevMessage = "You are not an owner or admin user.";
                    if (
                        boolIsAdmin || 
                        boolIsOwner
                        )
                    {
                        intStatus = 402;
                        strUserMessage = "Something is wrong.";
                        strDevMessage = "Data entries is not valid.";
                        if (
                        //                                  //Data valid entries.

                        //                                  //Set or unset supervisor.
                            (
                                (boolnIsSupervisor == true || (boolnIsSupervisor == false) &&
                                boolnIsAccountant == null)
                            ) 
                            ||
                            //                              //Set or unset accountable.
                            (
                                (boolnIsAccountant == true || (boolnIsAccountant == false) &&
                                boolnIsSupervisor == null)
                            )
                        )
                        {
                            try
                            {
                                //                              //Method to add or remove an employee as a supervisor.
                                EmplEmployee.subSetRole(strPrintshopId, intContactId, boolnIsSupervisor,
                                    boolnIsAccountant, ref intStatus, ref strUserMessage,
                                    ref strDevMessage);
                            }
                            catch (Exception ex)
                            {
                                //                                      //Making a log for the exception.
                                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                            }                            
                        }
                    }
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
        public IActionResult GetDay(
            //                                              //PURPOSE:
            //                                              //Get the calendar of the employee.

            //                                              //URL: http://localhost/Odyssey2/Employee/GetDay?
            //                                              //      intnContactId=1030575&strDay="2020-08-03"

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the employee calendar on a specific day.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            String strDay,
            int? intnContactId
            )
        {
            //                                              //Get the printshop id from token.
            var idClaimPrintshop = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaimPrintshop.Value;

            var idClaimOwner = User.Claims.FirstOrDefault(c => c.Type == "boolIsOwner");
            bool boolIsOwner = idClaimOwner.Value.ParseToBool();

            var idClaimAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsAdmin");
            bool boolIsAdmin = idClaimAdmin.Value.ParseToBool();

            //                                              //Get the contact id from token.
            var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
            int intContactId = idClaimContact.Value.ParseToInt();

            if (
                intnContactId != null
                )
            {
                intContactId = (int)intnContactId;
            }

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                strDay.IsParsableToDate()
                )
            {
                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Is an owner user.";
                if (
                    (intnContactId == null) ||
                    (intnContactId != null && (boolIsAdmin || boolIsOwner))
                    )
                {
                    try
                    {
                        Dayjson2DayJson2 dayjson2;
                        //                                      //Method that gets the calendar.
                        EmplEmployee.subGetDay(strPrintshopId, intContactId, strDay, this.configuration, out dayjson2,
                            ref intStatus, ref strDevMessage, ref strUserMessage);
                        obj = dayjson2;
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

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetAlerts(
            //                                              //PURPOSE:
            //                                              //Get employee's alerts.

            //                                              //URL: http://localhost/Odyssey2/Employee/GetAlerts

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all alerts.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            )
        {
            //                                              //Get the printshop id from token.
            var idClaimPrintshop = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaimPrintshop.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            //                                              //Get the contact id from token.
            var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
            int intContactId = idClaimContact.Value.ParseToInt();

            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";
            Object obj = null;

            using (Odyssey2Context context = new Odyssey2Context())
            {
                //                                  //Starts a new transaction.
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        AlertjsonAlertJson[] arralertjson;
                        //                                              //Method that gets the calendar.
                        EmplEmployee.subGetAlerts(intContactId, strPrintshopId, ps, this.configuration,
                            out arralertjson, context, ref intStatus, ref strDevMessage, ref strUserMessage);
                        obj = arralertjson;

                        //                          //Commits all changes made to the database in the current
                        //                          //      transaction.
                        if (
                            intStatus == 200
                            )
                        {
                            dbContextTransaction.Commit();
                            strUserMessage = "";
                            strDevMessage = "";
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        //                                      //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);

                        //                          //Discards all changes made to the database in the current
                        //                          //      transaction.
                        dbContextTransaction.Rollback();

                        intStatus = 407;
                        strUserMessage = "Something is wrong.";
                        strDevMessage = ex.Message;
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
        [HttpGet("[action]")]
        public IActionResult GetAllTasks(
            //                                              //PURPOSE:
            //                                              //Get an employee's uncompleted tasks.

            //                                              //URL: http://localhost/Odyssey2/Employee/GetAllTasks?
            //                                              //      intnPkTask=124124

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get uncompleted tasks from DB.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int? intnContactId
            )
        {
            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";
            Object obj = null;
            //                                              //Get the printshop id from token.
            var idClaimPrintshop = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaimPrintshop.Value;

            int intContactId = 0;

            if (
                intnContactId != null
                )
            {
                intContactId = (int)intnContactId;
            }
            else
            {
                //                                          //Get the contact id from token.
                var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                intContactId = idClaimContact.Value.ParseToInt();
            }

            try
            {
                PerortaskjsonPeriodOrTaskJson[] arrperortaskjson;
                //                                          //Method that gets the task.
                EmplEmployee.subGetAllTasks(intContactId, strPrintshopId, this.configuration, out arrperortaskjson,
                    ref intStatus, ref strDevMessage, ref strUserMessage);
                obj = arrperortaskjson;
            }
            catch (Exception ex)
            {
                //                                          //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }

            //                                              //Response to the frontend.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);

            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetTask(
            //                                              //PURPOSE:
            //                                              //Get employee's task.

            //                                              //URL: http://localhost/Odyssey2/Employee/GetTask?
            //                                              //      intPkTask=25

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get one task from DB.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int intPkTask
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intPkTask > 0
                )
            {
                //                                          //Get the printshop id from token.
                var idClaimPrintshop = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaimPrintshop.Value;

                //                                          //Get the contact id from token.
                var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                int intContactId = idClaimContact.Value.ParseToInt();

                try
                {
                    TaskjsonTaskJson taskjson;
                    //                                          //Method that gets the task.
                    EmplEmployee.subGetTask(intPkTask, strPrintshopId, intContactId, this.configuration, out taskjson,
                        ref intStatus, ref strDevMessage, ref strUserMessage);
                    obj = taskjson;
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
        public IActionResult GetOverdueTasks(
            //                                              //PURPOSE:
            //                                              //Get overdue tasks.

            //                                              //URL: http://localhost/Odyssey2/Employee/GetOverdueTasks?
            //                                              //      intnContactId=123312

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get overdue tasks from an employee.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int? intnContactId
            )
        {
            //                                              //Get the printshop id from token.
            var idClaimPrintshop = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaimPrintshop.Value;

            var idClaimOwner = User.Claims.FirstOrDefault(c => c.Type == "boolIsOwner");
            bool boolIsOwner = idClaimOwner.Value.ParseToBool();

            var idClaimAdmin = User.Claims.FirstOrDefault(c => c.Type == "boolIsAdmin");
            bool boolIsAdmin = idClaimAdmin.Value.ParseToBool();

            //                                              //Get the contact id from token.
            var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
            int intContactId = idClaimContact.Value.ParseToInt();

            if (
                intnContactId != null
                )
            {
                intContactId = (int)intnContactId;
            }

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Is not an owner user.";
            Object obj = null;
            if (
                (intnContactId == null) ||
                (intnContactId != null && (boolIsAdmin || boolIsOwner))
                )
            {
                try
                {
                    Taskjson2TaskJson2[] arrtaskjson2;
                    //                                          //Method that gets the overdue tasks.
                    EmplEmployee.subGetOverdueTasks(strPrintshopId, intContactId, this.configuration, out arrtaskjson2,
                        ref intStatus, ref strDevMessage, ref strUserMessage);
                    obj = arrtaskjson2;
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
    }

    //==================================================================================================================
}
/*END-TASK*/
