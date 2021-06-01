/*TASK RP.RESOURCE*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Text.Json;
using TowaStandard;
using System.Linq;
using Odyssey2Backend.PrintShop;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.DB_Odyssey2;
using Microsoft.AspNetCore.SignalR;
using Odyssey2Backend.Alert;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: January 20, 2020.

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //  var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
    //                                                      //  String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class ResourceController : Controller
    {
        //                                                  //Controller associated with the actions for Resource 
        //                                                  //      element.
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

        public ResourceController(
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
        public IActionResult Add(
            //                                              //PURPOSE:
            //                                              //Add a new resource.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /Add
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkType":2,
            //                                              //          "strResourceName":"Tinta",
            //                                              //          "strUnit":"l",
            //                                              //          "intnPkInherited": null,
            //                                              //          "boolIsTemplate": true,
            //                                              //          "arrattr":
            //                                              //           [
            //                                              //                {
            //                                              //                    "strAscendant":"3|6|9",
            //                                              //                    "strValue":"Valor1",
            //                                              //                    "intnInheritedValuePk":15,
            //                                              //                    "boolChangeable":false
            //                                              //                },
            //                                              //                {
            //                                              //                    "strAscendant":"4|8|12",
            //                                              //                    "strValue":"Valor2",
            //                                              //                    "intnInheritedValuePk":20,
            //                                              //                    "boolChangeable":true
            //                                              //                }
            //                                              //           ],
            //                                              //          "inhe":
            //                                              //              {
            //                                              //              "unit":
            //                                              //                  {
            //                                              //                      "strValue": "cm",
            //                                              //                      "boolnIsInherited": true,
            //                                              //                      "boolnIsChangeable": false
            //                                              //                  }
            //                                              //              "cost":
            //                                              //                  {
            //                                              //                      "numnCost": 13445,
            //                                              //                      "numnQuantity": 1425,
            //                                              //                      "numnMin": 2,
            //                                              //                      "numnBlock": 5,
            //                                              //                      "boolnIsInherited": true,
            //                                              //                      "boolnIsChangeable": false
            //                                              //                  }
            //                                              //              "avai":
            //                                              //                  {
            //                                              //                      "boolnIsCalendar": false,
            //                                              //                      "boolnIsInherited": true,
            //                                              //                      "boolnIsChangeable": false
            //                                              //                  }
            //                                              //              },
            //                                              //              "boolIsDecimal": true
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add a resource according with a resource type, 
            //                                              //      giving values to the attributes.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Dynamic object that contains all necessary data.
            [FromBody] JsonElement jsonResOrTemplate
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonResOrTemplate) &&
                //                                          //Verify if the data is not null or empty.
                jsonResOrTemplate.TryGetProperty("intPkType", out json) &&
                (int)jsonResOrTemplate.GetProperty("intPkType").ValueKind == 4 &&
                jsonResOrTemplate.TryGetProperty("strResourceName", out json) &&
                (int)jsonResOrTemplate.GetProperty("strResourceName").ValueKind == 3 &&
                jsonResOrTemplate.TryGetProperty("boolIsTemplate", out json) &&
                ((int)jsonResOrTemplate.GetProperty("boolIsTemplate").ValueKind == 5 ||
                (int)jsonResOrTemplate.GetProperty("boolIsTemplate").ValueKind == 6)
                )
            {
                //                                          //Get all the values.
                int intTypePk = jsonResOrTemplate.GetProperty("intPkType").GetInt32();
                String strName = jsonResOrTemplate.GetProperty("strResourceName").GetString();
                bool boolIsTemplate = jsonResOrTemplate.GetProperty("boolIsTemplate").GetBoolean();

                //                                          //Not all resources have a Dad.
                int? intnDad = null;
                if (
                    jsonResOrTemplate.TryGetProperty("intnPkInherited", out json) &&
                    (int)jsonResOrTemplate.GetProperty("intnPkInherited").ValueKind == 4
                    )
                {
                    intnDad = jsonResOrTemplate.GetProperty("intnPkInherited").GetInt32();
                }

                //                                          //No physical res do not have unit.
                String strUnit = null;
                if (
                    jsonResOrTemplate.TryGetProperty("strUnit", out json) &&
                    (int)jsonResOrTemplate.GetProperty("strUnit").ValueKind == 3
                    )
                {
                    strUnit = jsonResOrTemplate.GetProperty("strUnit").GetString();
                }

                double? numnCost = null;
                double? numnQuantity = null;
                double? numnMin = null;
                double? numnBlock = null;
                int? intnPkAccount = null;
                double? numnHourlyRate = null;
                bool? boolnIsCostInherited = null;
                bool? boolnIsCostChangeable = null;
                String strInheritedUnit = null;
                bool? boolnIsUnitInherited = null;
                bool? boolnIsUnitChangeable = null;
                bool? boolnCalendarized = null;
                bool? boolnAvailabilityInherited = null;
                bool? boolnAvailabilityChangeable = null;
                bool? boolnArea = null;
                if (
                    jsonResOrTemplate.TryGetProperty("inhe", out json) &&
                    (int)jsonResOrTemplate.GetProperty("inhe").ValueKind != 7
                    )
                {
                    //                                      //Inheritance data.
                    JsonElement jsonInherited = jsonResOrTemplate.GetProperty("inhe");

                    //                                      //Inherited cost Json.
                    JsonElement jsonInheritedCost = jsonInherited.GetProperty("cost");

                    if (
                        jsonInheritedCost.TryGetProperty("numnCost", out json) &&
                        (int)jsonInheritedCost.GetProperty("numnCost").ValueKind == 4
                        )
                    {
                        numnCost = jsonInheritedCost.GetProperty("numnCost").GetDouble();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("numnQuantity", out json) &&
                        (int)jsonInheritedCost.GetProperty("numnQuantity").ValueKind == 4
                        )
                    {
                        numnQuantity = jsonInheritedCost.GetProperty("numnQuantity").GetDouble();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("numnMin", out json) &&
                        (int)jsonInheritedCost.GetProperty("numnMin").ValueKind == 4
                        )
                    {
                        numnMin = jsonInheritedCost.GetProperty("numnMin").GetDouble();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("numnBlock", out json) &&
                        (int)jsonInheritedCost.GetProperty("numnBlock").ValueKind == 4
                        )
                    {
                        numnBlock = jsonInheritedCost.GetProperty("numnBlock").GetDouble();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("intnPkAccount", out json) &&
                        (int)jsonInheritedCost.GetProperty("intnPkAccount").ValueKind == 4
                        )
                    {
                        intnPkAccount = jsonInheritedCost.GetProperty("intnPkAccount").GetInt32();
                    }
                    if (
                        jsonInheritedCost.TryGetProperty("numnHourlyRate", out json) &&
                        (int)jsonInheritedCost.GetProperty("numnHourlyRate").ValueKind == 4
                        )
                    {
                        numnHourlyRate = jsonInheritedCost.GetProperty("numnHourlyRate").GetDouble();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("boolnArea", out json) &&
                        ((int)jsonInheritedCost.GetProperty("boolnArea").ValueKind == 5 ||
                        (int)jsonInheritedCost.GetProperty("boolnArea").ValueKind == 6)
                        )
                    {
                        boolnArea = jsonInheritedCost.GetProperty("boolnArea").GetBoolean();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("boolnIsInherited", out json) &&
                        ((int)jsonInheritedCost.GetProperty("boolnIsInherited").ValueKind == 5 ||
                        (int)jsonInheritedCost.GetProperty("boolnIsInherited").ValueKind == 6)
                        )
                    {
                        boolnIsCostInherited = jsonInheritedCost.GetProperty("boolnIsInherited").GetBoolean();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("boolnIsChangeable", out json) &&
                        ((int)jsonInheritedCost.GetProperty("boolnIsChangeable").ValueKind == 5 ||
                        (int)jsonInheritedCost.GetProperty("boolnIsChangeable").ValueKind == 6)
                        )
                    {
                        boolnIsCostChangeable = jsonInheritedCost.GetProperty("boolnIsChangeable").GetBoolean();
                    }

                    //                                      //Inherited Unit Json.
                    JsonElement jsonInheritedUnit = jsonInherited.GetProperty("unit");

                    if (
                        jsonInheritedUnit.TryGetProperty("strValue", out json) &&
                        (int)jsonInheritedUnit.GetProperty("strValue").ValueKind == 3
                        )
                    {
                        strInheritedUnit = jsonInheritedUnit.GetProperty("strValue").GetString();
                    }

                    if (
                        jsonInheritedUnit.TryGetProperty("boolnIsInherited", out json) &&
                        ((int)jsonInheritedUnit.GetProperty("boolnIsInherited").ValueKind == 5 ||
                        (int)jsonInheritedUnit.GetProperty("boolnIsInherited").ValueKind == 6)
                        )
                    {
                        boolnIsUnitInherited = jsonInheritedUnit.GetProperty("boolnIsInherited").GetBoolean();
                    }

                    if (
                        jsonInheritedUnit.TryGetProperty("boolnIsChangeable", out json) &&
                        ((int)jsonInheritedUnit.GetProperty("boolnIsChangeable").ValueKind == 5 ||
                        (int)jsonInheritedUnit.GetProperty("boolnIsChangeable").ValueKind == 6)
                        )
                    {
                        boolnIsUnitChangeable = jsonInheritedUnit.GetProperty("boolnIsChangeable").GetBoolean();
                    }

                    //                                      //Inherited Availability Json.
                    JsonElement jsonInheritedAvailability = jsonInherited.GetProperty("avai");

                    if (
                        jsonInheritedAvailability.TryGetProperty("boolnIsCalendar", out json) &&
                        ((int)jsonInheritedAvailability.GetProperty("boolnIsCalendar").ValueKind == 5 ||
                        (int)jsonInheritedAvailability.GetProperty("boolnIsCalendar").ValueKind == 6)
                        )
                    {
                        boolnCalendarized = jsonInheritedAvailability.GetProperty("boolnIsCalendar").GetBoolean();
                    }

                    if (
                        jsonInheritedAvailability.TryGetProperty("boolnIsInherited", out json) &&
                        ((int)jsonInheritedAvailability.GetProperty("boolnIsInherited").ValueKind == 5 ||
                        (int)jsonInheritedAvailability.GetProperty("boolnIsInherited").ValueKind == 6)
                        )
                    {
                        boolnAvailabilityInherited =
                            jsonInheritedAvailability.GetProperty("boolnIsInherited").GetBoolean();
                    }

                    if (
                        jsonInheritedAvailability.TryGetProperty("boolnIsChangeable", out json) &&
                        ((int)jsonInheritedAvailability.GetProperty("boolnIsChangeable").ValueKind == 5 ||
                        (int)jsonInheritedAvailability.GetProperty("boolnIsChangeable").ValueKind == 6)
                        )
                    {
                        boolnAvailabilityChangeable =
                            jsonInheritedAvailability.GetProperty("boolnIsChangeable").GetBoolean();
                    }
                }

                List<Attrjson5AttributeJson5> darrattjson5 = new List<Attrjson5AttributeJson5>();
                if (
                    jsonResOrTemplate.TryGetProperty("arrattr", out json) &&
                    (int)jsonResOrTemplate.GetProperty("arrattr").ValueKind == 2
                    )
                {
                    for (int intU = 0; intU < jsonResOrTemplate.GetProperty("arrattr").GetArrayLength();
                            intU = intU + 1)
                    {
                        JsonElement json1 = jsonResOrTemplate.GetProperty("arrattr")[intU];

                        int? intnInheritedValuePk = null;
                        if (
                            json1.TryGetProperty("intnInheritedValuePk", out json) &&
                            (int)json1.GetProperty("intnInheritedValuePk").ValueKind == 4
                            )
                        {
                            intnInheritedValuePk = json1.GetProperty("intnInheritedValuePk").GetInt32();
                        }

                        //                                  //Create Json.
                        Attrjson5AttributeJson5 attjson5 = new Attrjson5AttributeJson5(
                            json1.GetProperty("strAscendant").GetString(),
                            json1.GetProperty("strValue").GetString(),
                            intnInheritedValuePk,
                            json1.GetProperty("boolChangeable").GetBoolean(),
                            null
                            );
                        darrattjson5.Add(attjson5);
                    }
                }
                else
                {
                    darrattjson5 = null;
                }

                EtElementTypeAbstract et = EtElementTypeAbstract.etFromDB(intTypePk);

                strDevMessage = "Is not a Resource.";
                if (
                    et != null &&
                    et.strResOrPro == EtElementTypeAbstract.strResource
                    )
                {
                    //                                      //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;

                    RestypResourceType restyp = (RestypResourceType)et;

                    //                                      //ref to get the Pk of the values added.
                    Attrjson5AttributeJson5[] arrattjson5 = new Attrjson5AttributeJson5[0];
                    if (
                        darrattjson5 != null
                        )
                    {
                        arrattjson5 = darrattjson5.ToArray();
                    }

                    bool? boolnIsDecimal = null;
                    if (
                        jsonResOrTemplate.TryGetProperty("boolnIsDecimal", out json) &&
                        ((int)jsonResOrTemplate.GetProperty("boolnIsDecimal").ValueKind == 5 ||
                        (int)jsonResOrTemplate.GetProperty("boolnIsDecimal").ValueKind == 6)
                        )
                    {
                        boolnIsDecimal = jsonResOrTemplate.GetProperty("boolnIsDecimal").GetBoolean();
                    }

                    //                                      //If we add a Not Physical resource, boolnIsDecimal should 
                    //                                      //      be null
                    boolnIsDecimal = strUnit != null ? boolnIsDecimal : null;

                    //                                          //using is to release connection at the end
                    using (Odyssey2Context context = new Odyssey2Context())
                    {
                        //                                      //Starts a new transaction.
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                Pejson1PathElementJson1 pejson1;
                                ResResource.subAdd(strName, strUnit, boolnIsDecimal, restyp, strPrintshopId,
                                    boolIsTemplate, intnDad, numnCost, numnQuantity, numnMin, numnBlock, intnPkAccount,
                                    numnHourlyRate, boolnArea, boolnIsCostInherited, boolnIsCostChangeable, strInheritedUnit, 
                                    boolnIsUnitInherited, boolnIsUnitChangeable, boolnCalendarized, 
                                    boolnAvailabilityInherited, boolnAvailabilityChangeable, context, ref arrattjson5, 
                                    ref intStatus, ref strUserMessage, ref strDevMessage, out pejson1);
                                obj = pejson1;

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
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = Ok(respjson1);

            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddCustom(
            //                                              //PURPOSE:
            //                                              //Add custom resource to printshop.

            //                                              //URL: http://localhost/Odyssey2/Resource/AddCustom

            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "strResourceName" : "Papel",
            //                                              //          "strUnit" : "gsm",
            //                                              //          "arrstrAttribute" : color, size
            //                                              //          "arrstrValue"     : red, a4
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add attribute and value custom of 
            //                                              //      printshop.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Dynamic object that contqains a Json string with all data.
            [FromBody] JsonElement jsonResource
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonResource) &&
                //                                          //Verify if the data is not null or empty.
                jsonResource.TryGetProperty("strResourceName", out json) &&
                (int)jsonResource.GetProperty("strResourceName").ValueKind == 3 &&
                jsonResource.TryGetProperty("strUnit", out json) &&
                (int)jsonResource.GetProperty("strUnit").ValueKind == 3
                )
            {
                String strResourceName = jsonResource.GetProperty("strResourceName").GetString();
                strResourceName = strResourceName.TrimExcel();
                String strUnit = jsonResource.GetProperty("strUnit").GetString();
                strUnit = strUnit.TrimExcel();

                String strAttribute = null;
                List<String> darrstrAttribute = new List<String>();
                if (
                    jsonResource.TryGetProperty("arrstrAttribute", out json) &&
                    (int)jsonResource.GetProperty("arrstrAttribute").ValueKind == 2
                    )
                {
                    for (int intU = 0; intU < jsonResource.GetProperty("arrstrAttribute").GetArrayLength();
                        intU = intU + 1)
                    {
                        strAttribute = jsonResource.GetProperty("arrstrAttribute")[intU].GetString();
                        darrstrAttribute.Add(strAttribute.TrimExcel());
                    }
                }

                String strValue = null;
                List<String> darrstrValue = new List<String>();
                if (
                    jsonResource.TryGetProperty("arrstrValue", out json) &&
                    (int)jsonResource.GetProperty("arrstrValue").ValueKind == 2
                    )
                {
                    for (int intU = 0; intU < jsonResource.GetProperty("arrstrValue").GetArrayLength();
                        intU = intU + 1)
                    {
                        strValue = jsonResource.GetProperty("arrstrValue")[intU].GetString();
                        darrstrValue.Add(strValue.TrimExcel());
                    }
                }

                bool? boolnIsDecimal = null;
                if (
                    jsonResource.TryGetProperty("boolnIsDecimal", out json) &&
                    ((int)jsonResource.GetProperty("boolnIsDecimal").ValueKind == 5 ||
                    (int)jsonResource.GetProperty("boolnIsDecimal").ValueKind == 6)
                    )
                {
                    boolnIsDecimal = jsonResource.GetProperty("boolnIsDecimal").GetBoolean();
                }

                //                                          //To obtain the strPrintshopId from token:
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                //                                          //Get the printshop.
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            int intPk;
                            ResResource.subAddCustom(strResourceName, strUnit, boolnIsDecimal,
                                darrstrAttribute.ToArray(), darrstrValue.ToArray(), ps, context , ref intStatus, 
                                ref strUserMessage, ref strDevMessage, out intPk);
                            obj = intPk;

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
        public IActionResult Edit(
            //                                              //PURPOSE:
            //                                              //Edit a resource.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /Edit
            //                                              //Method: POST.

            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkResource": 2,
            //                                              //          "strResourceName": "Tinta mate roja",
            //                                              //          "strUnit": "cm",
            //                                              //          "arrattr": [
            //                                              //              {
            //                                              //                  "strAscendant":"883",
            //                                              //                  "strValue": "Tinta mate roja",
            //                                              //                  "intnPkInheritedValue":2,
            //                                              //                  "boolChangeable":false,
            //                                              //                  "intnPKValueToDeleteToAddANewOne":"3" 
            //                                              //              },
            //                                              //              {
            //                                              //                   "strAscendant": "1488|1548",
            //                                              //                   "strValue": "Matte",
            //                                              //                   "intnPkInheritedValue":null,
            //                                              //                   "boolChangeable": false,
            //                                              //                   "intnPKValueToDeleteToAddANewOne":null
            //                                              //              }
            //                                              //          ],
            //                                              //          "inhe":
            //                                              //              {
            //                                              //              "unit":
            //                                              //                  {
            //                                              //                      "strValue": "cm",
            //                                              //                      "boolnIsInherited": true,
            //                                              //                      "boolnIsChangeable": false
            //                                              //                  }
            //                                              //              "cost":
            //                                              //                  {
            //                                              //                      "numnCost": 13445,
            //                                              //                      "numnQuantity": 1425,
            //                                              //                      "numnMin": 2,
            //                                              //                      "numnBlock": 5,
            //                                              //                      "boolnIsInherited": true,
            //                                              //                      "boolnIsChangeable": false
            //                                              //                  }
            //                                              //              "avai":
            //                                              //                  {
            //                                              //                      "boolnIsCalendar": false,
            //                                              //                      "boolnIsInherited": true,
            //                                              //                      "boolnIsChangeable": false
            //                                              //                  }
            //                                              //              },
            //                                              //              "boolIsDecimal": true
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Update the resource with the new values.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Dynamic object that contains all necessary data.
            [FromBody] JsonElement jsonResOrTemplate
            )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonResOrTemplate) &&
                //                                          //Verify if the data is not null or empty.
                jsonResOrTemplate.TryGetProperty("intPkResource", out json) &&
                (int)jsonResOrTemplate.GetProperty("intPkResource").ValueKind == 4 &&
                jsonResOrTemplate.TryGetProperty("strResourceName", out json) &&
                (int)jsonResOrTemplate.GetProperty("strResourceName").ValueKind == 3 &&
                jsonResOrTemplate.TryGetProperty("strUnit", out json) &&
                (int)jsonResOrTemplate.GetProperty("strUnit").ValueKind == 3
                )
            {
                //                                          //Get all the values.
                int intPkResource = jsonResOrTemplate.GetProperty("intPkResource").GetInt32();
                String strResourceName = jsonResOrTemplate.GetProperty("strResourceName").GetString();
                String strUnit = jsonResOrTemplate.GetProperty("strUnit").GetString();

                double? numnCost = null;
                double? numnQuantity = null;
                double? numnMin = null;
                double? numnBlock = null;
                int? intnPkAccount = null;
                double? numnHourlyRate = null;
                bool? boolnArea = null;
                bool? boolnIsCostInherited = null;
                bool? boolnIsCostChangeable = null;
                String strInheritedUnit = null;
                bool? boolnIsUnitInherited = null;
                bool? boolnIsUnitChangeable = null;
                bool? boolnCalendarized = null;
                bool? boolnAvailabilityInherited = null;
                bool? boolnAvailabilityChangeable = null;
                if (
                    jsonResOrTemplate.TryGetProperty("inhe", out json) &&
                    (int)jsonResOrTemplate.GetProperty("inhe").ValueKind != 7
                    )
                {
                    //                                      //Inheritance data.
                    JsonElement jsonInherited = jsonResOrTemplate.GetProperty("inhe");

                    //                                      //Inherited cost Json.
                    JsonElement jsonInheritedCost = jsonInherited.GetProperty("cost");

                    if (
                       jsonInheritedCost.TryGetProperty("numnCost", out json) &&
                       (int)jsonInheritedCost.GetProperty("numnCost").ValueKind == 4
                       )
                    {
                        numnCost = jsonInheritedCost.GetProperty("numnCost").GetDouble();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("numnQuantity", out json) &&
                        (int)jsonInheritedCost.GetProperty("numnQuantity").ValueKind == 4
                        )
                    {
                        numnQuantity = jsonInheritedCost.GetProperty("numnQuantity").GetDouble();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("numnMin", out json) &&
                        (int)jsonInheritedCost.GetProperty("numnMin").ValueKind == 4
                        )
                    {
                        numnMin = jsonInheritedCost.GetProperty("numnMin").GetDouble();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("numnBlock", out json) &&
                        (int)jsonInheritedCost.GetProperty("numnBlock").ValueKind == 4
                        )
                    {
                        numnBlock = jsonInheritedCost.GetProperty("numnBlock").GetDouble();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("intnPkAccount", out json) &&
                        (int)jsonInheritedCost.GetProperty("intnPkAccount").ValueKind == 4
                        )
                    {
                        intnPkAccount = jsonInheritedCost.GetProperty("intnPkAccount").GetInt32();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("numnHourlyRate", out json) &&
                        (int)jsonInheritedCost.GetProperty("numnHourlyRate").ValueKind == 4
                        )
                    {
                        numnHourlyRate = jsonInheritedCost.GetProperty("numnHourlyRate").GetDouble();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("boolnArea", out json) &&
                        ((int)jsonInheritedCost.GetProperty("boolnArea").ValueKind == 5 ||
                        (int)jsonInheritedCost.GetProperty("boolnArea").ValueKind == 6)
                        )
                    {
                        boolnArea = jsonInheritedCost.GetProperty("boolnArea").GetBoolean();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("boolnIsInherited", out json) &&
                        ((int)jsonInheritedCost.GetProperty("boolnIsInherited").ValueKind == 5 ||
                        (int)jsonInheritedCost.GetProperty("boolnIsInherited").ValueKind == 6)
                        )
                    {
                        boolnIsCostInherited = jsonInheritedCost.GetProperty("boolnIsInherited").GetBoolean();
                    }

                    if (
                        jsonInheritedCost.TryGetProperty("boolnIsChangeable", out json) &&
                        ((int)jsonInheritedCost.GetProperty("boolnIsChangeable").ValueKind == 5 ||
                        (int)jsonInheritedCost.GetProperty("boolnIsChangeable").ValueKind == 6)
                        )
                    {
                        boolnIsCostChangeable = jsonInheritedCost.GetProperty("boolnIsChangeable").GetBoolean();
                    }

                    if (
                        jsonInherited.TryGetProperty("unit", out json) &&
                        (int)jsonInherited.GetProperty("unit").ValueKind != 7
                        )
                    {
                        //                                      //Inherited Unit Json.
                        JsonElement jsonInheritedUnit = jsonInherited.GetProperty("unit");

                        if (
                           jsonInheritedUnit.TryGetProperty("strValue", out json) &&
                           (int)jsonInheritedUnit.GetProperty("strValue").ValueKind == 3
                           )
                        {
                            strInheritedUnit = jsonInheritedUnit.GetProperty("strValue").GetString();
                        }

                        if (
                            jsonInheritedUnit.TryGetProperty("boolnIsInherited", out json) &&
                            ((int)jsonInheritedUnit.GetProperty("boolnIsInherited").ValueKind == 5 ||
                            (int)jsonInheritedUnit.GetProperty("boolnIsInherited").ValueKind == 6)
                            )
                        {
                            boolnIsUnitInherited = jsonInheritedUnit.GetProperty("boolnIsInherited").GetBoolean();
                        }

                        if (
                            jsonInheritedUnit.TryGetProperty("boolnIsChangeable", out json) &&
                            ((int)jsonInheritedUnit.GetProperty("boolnIsChangeable").ValueKind == 5 ||
                            (int)jsonInheritedUnit.GetProperty("boolnIsChangeable").ValueKind == 6)
                            )
                        {
                            boolnIsUnitChangeable = jsonInheritedUnit.GetProperty("boolnIsChangeable").GetBoolean();
                        }
                    }

                    if (
                       jsonInherited.TryGetProperty("avai", out json) &&
                       (int)jsonInherited.GetProperty("avai").ValueKind != 7
                       )
                    {
                        //                                      //Inherited Availability Json.
                        JsonElement jsonInheritedAvailability = jsonInherited.GetProperty("avai");

                        if (
                            jsonInheritedAvailability.TryGetProperty("boolnIsCalendar", out json) &&
                            ((int)jsonInheritedAvailability.GetProperty("boolnIsCalendar").ValueKind == 5 ||
                            (int)jsonInheritedAvailability.GetProperty("boolnIsCalendar").ValueKind == 6)
                            )
                        {
                            boolnCalendarized = jsonInheritedAvailability.GetProperty("boolnIsCalendar").GetBoolean();
                        }

                        if (
                            jsonInheritedAvailability.TryGetProperty("boolnIsInherited", out json) &&
                            ((int)jsonInheritedAvailability.GetProperty("boolnIsInherited").ValueKind == 5 ||
                            (int)jsonInheritedAvailability.GetProperty("boolnIsInherited").ValueKind == 6)
                            )
                        {
                            boolnAvailabilityInherited =
                                jsonInheritedAvailability.GetProperty("boolnIsInherited").GetBoolean();
                        }

                        if (
                            jsonInheritedAvailability.TryGetProperty("boolnIsChangeable", out json) &&
                            ((int)jsonInheritedAvailability.GetProperty("boolnIsChangeable").ValueKind == 5 ||
                            (int)jsonInheritedAvailability.GetProperty("boolnIsChangeable").ValueKind == 6)
                            )
                        {
                            boolnAvailabilityChangeable =
                                jsonInheritedAvailability.GetProperty("boolnIsChangeable").GetBoolean();
                        }
                    }
                }

                List<Attrjson5AttributeJson5> darrattjson5 = new List<Attrjson5AttributeJson5>();
                if (
                    jsonResOrTemplate.TryGetProperty("arrattr", out json) &&
                    (int)jsonResOrTemplate.GetProperty("arrattr").ValueKind == 2
                    )
                {
                    for (int intU = 0; intU < jsonResOrTemplate.GetProperty("arrattr").GetArrayLength(); intU = intU + 1)
                    {
                        JsonElement json1 = jsonResOrTemplate.GetProperty("arrattr")[intU];

                        int? intnPkInheritedValue = null;
                        if (
                            json1.TryGetProperty("intnPkInheritedValue", out json) &&
                            (int)json1.GetProperty("intnPkInheritedValue").ValueKind == 4
                            )
                        {
                            intnPkInheritedValue = json1.GetProperty("intnPkInheritedValue").GetInt32();
                        }

                        int? intnPkValueToDeleteToAddANewOne = null;
                        if (
                            json1.TryGetProperty("intnPkValueToDeleteToAddANewOne", out json) &&
                            (int)json1.GetProperty("intnPkValueToDeleteToAddANewOne").ValueKind == 4
                            )
                        {
                            intnPkValueToDeleteToAddANewOne =
                                json1.GetProperty("intnPkValueToDeleteToAddANewOne").GetInt32();
                        }

                        //                                      //Create Json.
                        Attrjson5AttributeJson5 attjson5 = new Attrjson5AttributeJson5(
                            json1.GetProperty("strAscendant").GetString(),
                            json1.GetProperty("strValue").GetString(),
                            intnPkInheritedValue,
                            json1.GetProperty("boolChangeable").GetBoolean(),
                            intnPkValueToDeleteToAddANewOne
                            );
                        darrattjson5.Add(attjson5);
                    }
                }

                bool? boolnIsDecimal = null;
                if (
                    jsonResOrTemplate.TryGetProperty("boolnIsDecimal", out json) &&
                    ((int)jsonResOrTemplate.GetProperty("boolnIsDecimal").ValueKind == 5 ||
                    (int)jsonResOrTemplate.GetProperty("boolnIsDecimal").ValueKind == 6)
                    )
                {
                    boolnIsDecimal = jsonResOrTemplate.GetProperty("boolnIsDecimal").GetBoolean();
                }

                //                                      //If we add a Not Physical resource, boolnIsDecimal should 
                //                                      //      be null
                boolnIsDecimal = strUnit != null ? boolnIsDecimal : null;

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            Pejson1PathElementJson1 pejson1;
                            ResResource.subEdit(intPkResource, strResourceName, strUnit, boolnIsDecimal, numnCost, 
                                numnQuantity, numnMin, numnBlock, intnPkAccount, numnHourlyRate, boolnArea,
                                boolnIsCostInherited, boolnIsCostChangeable, strInheritedUnit, 
                                boolnIsUnitInherited, boolnIsUnitChangeable, 
                                boolnCalendarized, boolnAvailabilityInherited, boolnAvailabilityChangeable, 
                                darrattjson5.ToArray(), ps, context, ref intStatus, ref strUserMessage, ref strDevMessage,
                                out pejson1);
                            obj = pejson1;

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
        public IActionResult Delete(
            //                                              //PURPOSE:
            //                                              //Delete a resource or a template.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /Delete
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPk":00
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Delete a resource or a template from db
            //                                              //      (and all the relations).

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive the pk of the element.
            [FromBody] JsonElement resData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Something is wrong.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, resData) &&
                resData.TryGetProperty("intPk", out json) &&
                (int)resData.GetProperty("intPk").ValueKind == 4
                )
            {
                //                                          //Get the pk of the resource or template to delete.
                int intPk = resData.GetProperty("intPk").GetInt32();

                //                                          //To obtain the strPrintshopId from token:
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //                                      //Method to delete the resource or template.
                            ResResource.subDelete(intPk, strPrintshopId, this.iHubContext, context, ref intStatus,
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

            //                                              //Response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult DeletePeriod(
            //                                              //PURPOSE:
            //                                              //Delete a resource's period.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /DeletePeriod
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkPeriod":12,
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Delete a resource's period from db.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Dynamic json that contains all necessary data.
            [FromBody] JsonElement resData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid Data.";
            Object obj = null;
            JsonElement json;

            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, resData) &&
                resData.TryGetProperty("intPkPeriod", out json) &&
                (int)resData.GetProperty("intPkPeriod").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                int intPkPeriod = resData.GetProperty("intPkPeriod").GetInt32();

                try
                {
                    //                                      //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;

                    String strLastSunday;
                    String strEstimatedDate;
                    //                                      //Method to delete a period.
                    ResResource.subDeletePeriod(intPkPeriod, strPrintshopId, this.configuration, this.iHubContext, 
                        out strLastSunday, out strEstimatedDate, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = new { strLastSunday = strLastSunday, strEstimatedDate = strEstimatedDate };

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
        public IActionResult AddCost(
            //                                              //PURPOSE:
            //                                              //Add a cost for a resource.

            //                                              //URL: http://localhost/Odyssey2/Resource/AddCost
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //        "intPkResource":5678,
            //                                              //        "numnQuantity":3.75,
            //                                              //        "numnCost":27.50,
            //                                              //        "numnMin":0.5,
            //                                              //        "intnPkAccount: 3
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add or update the cost for a resource in the db when it 
            //                                              //      is not a not physical resource.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive the pk of the element.
            [FromBody] JsonElement jsonCost
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
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, jsonCost) &&
                jsonCost.TryGetProperty("intPkResource", out json) &&
                (int)jsonCost.GetProperty("intPkResource").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                int intPkResource = jsonCost.GetProperty("intPkResource").GetInt32();

                double? numnMin = null;
                if (
                    jsonCost.TryGetProperty("numnMin", out json) &&
                    (int)jsonCost.GetProperty("numnMin").ValueKind == 4
                    )
                {
                    numnMin = jsonCost.GetProperty("numnMin").GetDouble();
                }

                double? numnCost = null;
                if (
                    jsonCost.TryGetProperty("numnCost", out json) &&
                    (int)jsonCost.GetProperty("numnCost").ValueKind == 4
                    )
                {
                    numnCost = jsonCost.GetProperty("numnCost").GetDouble();
                }

                double? numnQuantity = null;
                if (
                    jsonCost.TryGetProperty("numnQuantity", out json) &&
                    (int)jsonCost.GetProperty("numnQuantity").ValueKind == 4
                    )
                {
                    numnQuantity = jsonCost.GetProperty("numnQuantity").GetDouble();
                }

                double? numnBlock = null;
                if (
                    jsonCost.TryGetProperty("numnBlock", out json) &&
                    (int)jsonCost.GetProperty("numnBlock").ValueKind == 4
                    )
                {
                    numnBlock = jsonCost.GetProperty("numnBlock").GetDouble();
                }

                int? intnPkAccount = null;
                if (
                    jsonCost.TryGetProperty("intnPkAccount", out json) &&
                    (int)jsonCost.GetProperty("intnPkAccount").ValueKind == 4
                    )
                {
                    intnPkAccount = jsonCost.GetProperty("intnPkAccount").GetInt32();
                }

                double? numnHourlyRate = null;
                if (
                    jsonCost.TryGetProperty("numnHourlyRate", out json) &&
                    (int)jsonCost.GetProperty("numnHourlyRate").ValueKind == 4
                    )
                {
                    numnHourlyRate = jsonCost.GetProperty("numnHourlyRate").GetDouble();
                }

                bool? boolnArea = null;
                if (
                    jsonCost.TryGetProperty("boolnArea", out json) &&
                    ((int)jsonCost.GetProperty("boolnArea").ValueKind == 5 ||
                    (int)jsonCost.GetProperty("boolnArea").ValueKind == 6)
                    )
                {
                    boolnArea = jsonCost.GetProperty("boolnArea").GetBoolean();
                }

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //                              //Method to add the cost for a resource.
                            ResResource.subAddCost(intPkResource, numnQuantity, numnCost, numnMin, numnBlock,
                                intnPkAccount, numnHourlyRate, false, ps, boolnArea, context, ref intStatus,
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

            //                                              //Response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddTime(
            //                                              //PURPOSE:
            //                                              //Add a time for a resource.

            //                                              //URL: http://localhost/Odyssey2/Resource/AddTime
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //            "intPkResource":5678,
            //                                              //            "numQuantity":3.75,
            //                                              //            "intHours":27.50,
            //                                              //            "intMinutes":5,
            //                                              //            "intSeconds":2,
            //                                              //            "numnMinThickness":0.5,
            //                                              //            "numnMaxThickness":0.5,
            //                                              //            "strThicknessUnit":"mm"
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add or update the time for a resource in the db when it 
            //                                              //      is device, tool or custom.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive the pk of the element.
            [FromBody] JsonElement jsonTIme
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, jsonTIme) &&
                jsonTIme.TryGetProperty("intPkResource", out json) &&
                (int)jsonTIme.GetProperty("intPkResource").ValueKind == 4 &&
                jsonTIme.TryGetProperty("numQuantity", out json) &&
                (int)jsonTIme.GetProperty("numQuantity").ValueKind == 4 &&
                jsonTIme.TryGetProperty("intHours", out json) &&
                (int)jsonTIme.GetProperty("intHours").ValueKind == 4 &&
                jsonTIme.TryGetProperty("intMinutes", out json) &&
                (int)jsonTIme.GetProperty("intMinutes").ValueKind == 4 &&
                jsonTIme.TryGetProperty("intSeconds", out json) &&
                (int)jsonTIme.GetProperty("intSeconds").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                int intPkResource = jsonTIme.GetProperty("intPkResource").GetInt32();
                double numQuantity = jsonTIme.GetProperty("numQuantity").GetDouble();
                int intHours = jsonTIme.GetProperty("intHours").GetInt32();
                int intMinutes = jsonTIme.GetProperty("intMinutes").GetInt32();
                int intSeconds = jsonTIme.GetProperty("intSeconds").GetInt32();                

                double? numnMinThickness = null;
                if (
                    jsonTIme.TryGetProperty("numnMinThickness", out json) &&
                    (int)jsonTIme.GetProperty("numnMinThickness").ValueKind == 4
                    )
                {
                    numnMinThickness = jsonTIme.GetProperty("numnMinThickness").GetDouble().Round(4);
                }

                double? numnMaxThickness = null;
                if (
                    jsonTIme.TryGetProperty("numnMaxThickness", out json) &&
                    (int)jsonTIme.GetProperty("numnMaxThickness").ValueKind == 4
                    )
                {
                    numnMaxThickness = jsonTIme.GetProperty("numnMaxThickness").GetDouble().Round(4);
                }

                String strThicknessUnit = "";
                if (
                    jsonTIme.TryGetProperty("strThicknessUnit", out json) &&
                    (int)jsonTIme.GetProperty("strThicknessUnit").ValueKind == 3
                    )
                {
                    strThicknessUnit = jsonTIme.GetProperty("strThicknessUnit").GetString();
                }

                if (
                    intPkResource > 0 &&
                    numQuantity > 0 &&
                    intHours >= 0 &&
                    intMinutes >= 0 &&
                    intSeconds >= 0 &&
                    (intHours > 0 ||
                    intMinutes > 0 ||
                    intSeconds > 0)
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
                                //                                      //Method to add the time for a resource.
                                ResResource.subAddTime(intPkResource, numQuantity, intHours, intMinutes, intSeconds,
                                    numnMinThickness, numnMaxThickness, strThicknessUnit, context, ref intStatus,
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
            }

            //                                              //Response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult UpdateTime(
            //                                              //PURPOSE:
            //                                              //Update a time for a resource.

            //                                              //URL: http://localhost/Odyssey2/Resource/UpdateTime
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //            "intPkResource":5678,
            //                                              //            "numQuantity":3.75,
            //                                              //            "intHours":27.50,
            //                                              //            "intMinutes":5,
            //                                              //            "intSeconds":2,
            //                                              //            "numnMinThickness":0.5,
            //                                              //            "numnMaxThickness":0.5,
            //                                              //            "strThicknessUnit":"mm"
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add or update the time for a resource in the db when it 
            //                                              //      is device, tool or custom.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive the pk of the element.
            [FromBody] JsonElement jsonTime
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, jsonTime) &&
                jsonTime.TryGetProperty("intPkResource", out json) &&
                (int)jsonTime.GetProperty("intPkResource").ValueKind == 4 &&
                jsonTime.TryGetProperty("numQuantity", out json) &&
                (int)jsonTime.GetProperty("numQuantity").ValueKind == 4 &&
                jsonTime.TryGetProperty("intHours", out json) &&
                (int)jsonTime.GetProperty("intHours").ValueKind == 4 &&
                jsonTime.TryGetProperty("intMinutes", out json) &&
                (int)jsonTime.GetProperty("intMinutes").ValueKind == 4 &&
                jsonTime.TryGetProperty("intSeconds", out json) &&
                (int)jsonTime.GetProperty("intSeconds").ValueKind == 4 &&
                jsonTime.TryGetProperty("intPkTime", out json) &&
                (int)jsonTime.GetProperty("intPkTime").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                int intPkResource = jsonTime.GetProperty("intPkResource").GetInt32();
                double numQuantity = jsonTime.GetProperty("numQuantity").GetDouble();
                int intHours = jsonTime.GetProperty("intHours").GetInt32();
                int intMinutes = jsonTime.GetProperty("intMinutes").GetInt32();
                int intSeconds = jsonTime.GetProperty("intSeconds").GetInt32();
                int intPkTime = jsonTime.GetProperty("intPkTime").GetInt32();

                double? numnMinThickness = null;
                if (
                    jsonTime.TryGetProperty("numnMinThickness", out json) &&
                    (int)jsonTime.GetProperty("numnMinThickness").ValueKind == 4
                    )
                {
                    numnMinThickness = jsonTime.GetProperty("numnMinThickness").GetDouble().Round(4);
                }

                double? numnMaxThickness = null;
                if (
                    jsonTime.TryGetProperty("numnMaxThickness", out json) &&
                    (int)jsonTime.GetProperty("numnMaxThickness").ValueKind == 4
                    )
                {
                    numnMaxThickness = jsonTime.GetProperty("numnMaxThickness").GetDouble().Round(4);
                }

                String strThicknessUnit = "";
                if (
                    jsonTime.TryGetProperty("strThicknessUnit", out json) &&
                    (int)jsonTime.GetProperty("strThicknessUnit").ValueKind == 3
                    )
                {
                    strThicknessUnit = jsonTime.GetProperty("strThicknessUnit").GetString();
                }

                if (
                    intPkResource > 0 &&
                    intPkTime > 0 &&
                    numQuantity > 0 &&
                    intHours >= 0 &&
                    intMinutes >= 0 &&
                    intSeconds >= 0 &&
                    (intHours > 0 ||
                    intMinutes > 0 ||
                    intSeconds > 0)
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
                                //                                      //Method to add the time for a resource.
                                ResResource.subUpdateTime(intPkResource, numQuantity, intHours, intMinutes, intSeconds,
                                    numnMinThickness, numnMaxThickness, strThicknessUnit, intPkTime, context,
                                    ref intStatus, ref strUserMessage, ref strDevMessage);

                                //                              //Commits all changes made to the database in the 
                                //                              //      current transaction.
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
            }

            //                                              //Response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult DeleteTime(
            //                                              //PURPOSE:
            //                                              //Delete a time for a resource

            //                                              //URL: http://localhost/Odyssey2/Resource/DeleteTime
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //            "intPkTime": 1
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Delete a register from time table.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Receive the pk of the time,
            [FromBody] JsonElement jsonTIme
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, jsonTIme) &&
                jsonTIme.TryGetProperty("intPkTime", out json) &&
                (int)jsonTIme.GetProperty("intPkTime").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                int intPkTime = jsonTIme.GetProperty("intPkTime").GetInt32();

                if (
                    intPkTime > 0
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
                                //                                      //Method to add the time for a resource.
                                ResResource.subDeleteTime(intPkTime, context, ref intStatus, ref strUserMessage,
                                    ref strDevMessage);

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
            }

            //                                              //Response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult Availability(
            //                                              //PURPOSE:
            //                                              //Update resource´s availability.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /Availability
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkResource":00,
            //                                              //          "boolnIsCalendar": true,
            //                                              //          "boolIsAvailable" : null 
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Change resource´s availability type and set or change
            //                                              //  resource´s availability (available/not available).

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Dynamic json that contains all necessary data.
            [FromBody] JsonElement resData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid Data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, resData) &&
                resData.TryGetProperty("intPkResource", out json) &&
                (int)resData.GetProperty("intPkResource").ValueKind == 4 &&
                resData.TryGetProperty("boolIsCalendar", out json) &&
                ((int)resData.GetProperty("boolIsCalendar").ValueKind == 5 ||
                (int)resData.GetProperty("boolIsCalendar").ValueKind == 6)
                )
            {
                //                                          //Get data from json.
                int intPkResource = resData.GetProperty("intPkResource").GetInt32();
                bool boolIsCalendar = resData.GetProperty("boolIsCalendar").GetBoolean();

                bool? boolnIsAvailable = null;
                if (
                    resData.TryGetProperty("boolnIsAvailable", out json) &&
                    ((int)resData.GetProperty("boolnIsAvailable").ValueKind == 5 ||
                    (int)resData.GetProperty("boolnIsAvailable").ValueKind == 6)
                    )
                {
                    boolnIsAvailable = resData.GetProperty("boolnIsAvailable").GetBoolean();
                }

                //                                          //Get printshop id from token.
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                String strPrintshopId = idClaim.Value;

                //                                          //Get the contact id from token.
                var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                int intContactId = idClaimContact.Value.ParseToInt();

                //                                          //using is to release connection at the end
                using (Odyssey2Context context = new Odyssey2Context())
                {
                    //                                      //Starts a new transaction.
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //                              //Method to update availability.
                            ResResource.subAvailability(strPrintshopId, intContactId, intPkResource, boolIsCalendar,
                                boolnIsAvailable, this.iHubContext, context, ref intStatus, ref strUserMessage, 
                                ref strDevMessage);

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

            //                                              //Response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddRule(
        //                                              //PURPOSE:
        //                                              //Add a rule for a resource's calendar.

        //                                              //URL: http://localhost/Odyssey2/Resource/AddRule
        //                                              //Method: POST.
        //                                              //Use a JSON like this:
        //                                              //      {
        //                                              //          "boolIsEmployee":true,  
        //                                              //          "intnContactId":23, 
        //                                              //          "intnPkResource":1,
        //                                              //          "strFrecuency":"daily",
        //                                              //          "strStartTime":"16:55:00",
        //                                              //          "strEndTime":"17:30:00",
        //                                              //          "strStartDate":null,
        //                                              //          "strEndDate":null,
        //                                              //          "strRangeStartDate":null
        //                                              //          "strRangeStartTime":null
        //                                              //          "strRangeEndDate":null
        //                                              //          "strRangeEndTime":null
        //                                              //          "arrintDays":null,
        //                                              //          "strDay":null
        //                                              //      }

        //                                              //DESCRIPTION:
        //                                              //Add a rule for a resource's calendar.

        //                                              //RETURNS:
        //                                              //      200 - Ok().

        //                                              //Json with all necessary data.
        [FromBody] JsonElement jsonRule
            )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, jsonRule) &&
                jsonRule.TryGetProperty("strFrecuency", out json) &&
                (int)jsonRule.GetProperty("strFrecuency").ValueKind == 3 &&
                jsonRule.TryGetProperty("strStartTime", out json) &&
                (int)jsonRule.GetProperty("strStartTime").ValueKind == 3 &&
                jsonRule.TryGetProperty("strEndTime", out json) &&
                (int)jsonRule.GetProperty("strEndTime").ValueKind == 3 &&
                jsonRule.TryGetProperty("boolIsEmployee", out json) &&
                ((int)jsonRule.GetProperty("boolIsEmployee").ValueKind == 5 ||
                (int)jsonRule.GetProperty("boolIsEmployee").ValueKind == 6)
                )
            {
                //                                          //Get data from json.               
                String strFrecuency = jsonRule.GetProperty("strFrecuency").GetString();
                String strStartTime = jsonRule.GetProperty("strStartTime").GetString();
                String strEndTime = jsonRule.GetProperty("strEndTime").GetString();
                bool boolIsEmployee = jsonRule.GetProperty("boolIsEmployee").GetBoolean();

                int? intnPkResource = null;
                if (
                    jsonRule.TryGetProperty("intnPkResource", out json) &&
                    (int)jsonRule.GetProperty("intnPkResource").ValueKind == 4
                    )
                {
                    intnPkResource = jsonRule.GetProperty("intnPkResource").GetInt32();
                }

                int? intnContactId = null;
                if (
                    jsonRule.TryGetProperty("intnContactId", out json) &&
                    (int)jsonRule.GetProperty("intnContactId").ValueKind == 4
                    )
                {
                    intnContactId = jsonRule.GetProperty("intnContactId").GetInt32();
                }

                if (
                    intnContactId == null
                    )
                {
                    //                                              //Get the contact id from token.
                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    intnContactId = idClaimContact.Value.ParseToInt();
                }

                String strStartDate = null;
                if (
                    jsonRule.TryGetProperty("strStartDate", out json) &&
                    (int)jsonRule.GetProperty("strStartDate").ValueKind == 3
                    )
                {
                    strStartDate = jsonRule.GetProperty("strStartDate").GetString();
                }

                String strEndDate = null;
                if (
                    jsonRule.TryGetProperty("strEndDate", out json) &&
                    (int)jsonRule.GetProperty("strEndDate").ValueKind == 3
                    )
                {
                    strEndDate = jsonRule.GetProperty("strEndDate").GetString();
                }

                String strRangeStartDate = null;
                if (
                    jsonRule.TryGetProperty("strRangeStartDate", out json) &&
                    (int)jsonRule.GetProperty("strRangeStartDate").ValueKind == 3
                    )
                {
                    strRangeStartDate = jsonRule.GetProperty("strRangeStartDate").GetString();
                }

                String strRangeStartTime = null;
                if (
                    jsonRule.TryGetProperty("strRangeStartTime", out json) &&
                    (int)jsonRule.GetProperty("strRangeStartTime").ValueKind == 3
                    )
                {
                    strRangeStartTime = jsonRule.GetProperty("strRangeStartTime").GetString();
                }

                String strRangeEndDate = null;
                if (
                    jsonRule.TryGetProperty("strRangeEndDate", out json) &&
                    (int)jsonRule.GetProperty("strRangeEndDate").ValueKind == 3
                    )
                {
                    strRangeEndDate = jsonRule.GetProperty("strRangeEndDate").GetString();
                }

                String strRangeEndTime = null;
                if (
                    jsonRule.TryGetProperty("strRangeEndTime", out json) &&
                    (int)jsonRule.GetProperty("strRangeEndTime").ValueKind == 3
                    )
                {
                    strRangeEndTime = jsonRule.GetProperty("strRangeEndTime").GetString();
                }

                String strDay = null;
                if (
                    jsonRule.TryGetProperty("strDay", out json) &&
                    (int)jsonRule.GetProperty("strDay").ValueKind == 3
                    )
                {
                    strDay = jsonRule.GetProperty("strDay").GetString();
                }

                int[] arrintDays = null;
                if (
                    jsonRule.TryGetProperty("arrintDays", out json) &&
                    (int)jsonRule.GetProperty("arrintDays").ValueKind == 2
                    )
                {
                    //                                      //To easy code.
                    int intLength = jsonRule.GetProperty("arrintDays").GetArrayLength();

                    arrintDays = new int[intLength];
                    for (int intU = 0; intU < intLength; intU = intU + 1)
                    {
                        arrintDays[intU] = jsonRule.GetProperty("arrintDays")[intU].GetInt32();
                    }
                }

                try
                {
                    //                                          //Method to add the rule for a resource.
                    ResResource.subAddRule(boolIsEmployee, intnContactId, intnPkResource, strPrintshopId, strFrecuency,
                        strStartTime, strEndTime, strStartDate, strEndDate, strRangeStartDate, strRangeStartTime,
                        strRangeEndDate, strRangeEndTime, arrintDays, strDay, this.configuration, this.iHubContext,
                        ref intStatus, ref strUserMessage, ref strDevMessage);
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
        public IActionResult DeleteRule(
            //                                              //PURPOSE:
            //                                              //Delete a rule.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /DeleteRule
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkRule":26,
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Delete a rule from Rule table.
            //                                              //RETURNS:
            //                                              //      200 - Ok().
            //                                              //      400 - BadRequest().

            //                                              //Dynamic json that contains all necessary data.
            [FromBody] JsonElement resData
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid Data.";
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, resData) &&
                resData.TryGetProperty("intPkRule", out json) &&
                (int)resData.GetProperty("intPkRule").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                int intPkRule = resData.GetProperty("intPkRule").GetInt32();

                try
                {
                    //                                      //Method to delete a rule.
                    ResResource.subDeleteRule(intPkRule, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }
            //                                              //Response.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                    null);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult AddPeriod(
            //                                              //PURPOSE:
            //                                              //Add a period to a specific resource.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /AddPeriod
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkResource": 00,
            //                                              //          "intnContactId":180194,
            //                                              //          "strStartDate": "2020-04-24",
            //                                              //          "strStartTime": "11:00:00",
            //                                              //          "strEndDate": "2020-04-25",
            //                                              //          "strEndTime": "12:00:00",
            //                                              //          "intJobId": 00,
            //                                              //          "intPkProcessInWorkflow": 00,
            //                                              //          "intPkEleetOrEleele": 00,
            //                                              //          "boolIsEleet": false,
            //                                              //          "intMinsBeforeDelete": 3 
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Add a period to a specific resource.

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
                jsonPeriod.TryGetProperty("intPkResource", out json) &&
                (int)jsonPeriod.GetProperty("intPkResource").ValueKind == 4 &&
                jsonPeriod.TryGetProperty("strStartDate", out json) &&
                (int)jsonPeriod.GetProperty("strStartDate").ValueKind == 3 &&
                jsonPeriod.TryGetProperty("strStartTime", out json) &&
                (int)jsonPeriod.GetProperty("strStartTime").ValueKind == 3 &&
                jsonPeriod.TryGetProperty("strEndDate", out json) &&
                (int)jsonPeriod.GetProperty("strEndDate").ValueKind == 3 &&
                jsonPeriod.TryGetProperty("strEndTime", out json) &&
                (int)jsonPeriod.GetProperty("strEndTime").ValueKind == 3 &&
                 jsonPeriod.TryGetProperty("intJobId", out json) &&
                (int)jsonPeriod.GetProperty("intJobId").ValueKind == 4 &&
                jsonPeriod.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonPeriod.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                jsonPeriod.TryGetProperty("intPkEleetOrEleele", out json) &&
                (int)jsonPeriod.GetProperty("intPkEleetOrEleele").ValueKind == 4 &&
                jsonPeriod.TryGetProperty("boolIsEleet", out json) &&
                (((int)jsonPeriod.GetProperty("boolIsEleet").ValueKind == 5) ||
                ((int)jsonPeriod.GetProperty("boolIsEleet").ValueKind == 6)) &&
                jsonPeriod.TryGetProperty("intMinsBeforeDelete", out json) &&
                (int)jsonPeriod.GetProperty("intMinsBeforeDelete").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                int intPkResource = jsonPeriod.GetProperty("intPkResource").GetInt32();
                String strStartDate = jsonPeriod.GetProperty("strStartDate").GetString();
                String strStartTime = jsonPeriod.GetProperty("strStartTime").GetString();
                String strEndDate = jsonPeriod.GetProperty("strEndDate").GetString();
                String strEndTime = jsonPeriod.GetProperty("strEndTime").GetString();
                int intJobId = jsonPeriod.GetProperty("intJobId").GetInt32();
                int intPkProcessInWorkflow = jsonPeriod.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intPkEleetOrEleele = jsonPeriod.GetProperty("intPkEleetOrEleele").GetInt32();
                bool boolIsEleet = jsonPeriod.GetProperty("boolIsEleet").GetBoolean();
                int intMinsBeforeDelete = jsonPeriod.GetProperty("intMinsBeforeDelete").GetInt32();

                String strPassword = null;
                if (
                    jsonPeriod.TryGetProperty("strPassword", out json) &&
                    (int)jsonPeriod.GetProperty("strPassword").ValueKind == 3
                    )
                {
                    strPassword = jsonPeriod.GetProperty("strPassword").GetString();
                }

                int? intnContactId = null;
                if (
                    jsonPeriod.TryGetProperty("intnContactId", out json) &&
                    (int)jsonPeriod.GetProperty("intnContactId").ValueKind == 4
                    )
                {
                    intnContactId = jsonPeriod.GetProperty("intnContactId").GetInt32();
                }

                try
                {
                    String strLastSunday;
                    int intPkPeriod;
                    String strEstimatedDate;
                    //                                      //Method to add period.
                    ResResource.subAddPeriod(strPrintshopId, intPkResource, intnContactId, strPassword, strStartDate,
                        strStartTime, strEndDate, strEndTime, intJobId, intPkProcessInWorkflow, intPkEleetOrEleele,
                        boolIsEleet, false, intMinsBeforeDelete, this.configuration, this.iHubContext, out strLastSunday, 
                        out intPkPeriod, out strEstimatedDate, ref intStatus, ref strUserMessage, ref strDevMessage);
                    //                                      //Create generic object
                    obj = new { strLastSunday = strLastSunday, intPkPeriod = intPkPeriod, strEstimatedDate = strEstimatedDate};
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
        [HttpGet("[action]")]
        public IActionResult PeriodIsAddable(
            //                                              //Verify if the times for the period do not overlapping with
            //                                              //      the rules of the printshop and are times after the 
            //                                              //      end of the periods for past processes.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /PeriodisAddable
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "intPkResource": 00,
            //|                                             //          "intnPkPeriod": null,
            //                                              //          "strStartDate": "2020-04-24",
            //                                              //          "strStartTime": "11:00:00",
            //                                              //          "strEndDate": "2020-04-25",
            //                                              //          "strEndTime": "12:00:00",
            //                                              //          "intJobId": 00,
            //                                              //          "intPkProcessInWorkflow": 00,
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Get a boolean to know if the period is addable.

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
                jsonPeriod.TryGetProperty("intPkResource", out json) &&
                (int)jsonPeriod.GetProperty("intPkResource").ValueKind == 4 &&
                jsonPeriod.TryGetProperty("strStartDate", out json) &&
                (int)jsonPeriod.GetProperty("strStartDate").ValueKind == 3 &&
                jsonPeriod.TryGetProperty("strStartTime", out json) &&
                (int)jsonPeriod.GetProperty("strStartTime").ValueKind == 3 &&
                jsonPeriod.TryGetProperty("strEndDate", out json) &&
                (int)jsonPeriod.GetProperty("strEndDate").ValueKind == 3 &&
                jsonPeriod.TryGetProperty("strEndTime", out json) &&
                (int)jsonPeriod.GetProperty("strEndTime").ValueKind == 3 &&
                 jsonPeriod.TryGetProperty("intJobId", out json) &&
                (int)jsonPeriod.GetProperty("intJobId").ValueKind == 4 &&
                jsonPeriod.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)jsonPeriod.GetProperty("intPkProcessInWorkflow").ValueKind == 4
                )
            {
                //                                          //Get data from json.
                int intPkResource = jsonPeriod.GetProperty("intPkResource").GetInt32();
                String strStartDate = jsonPeriod.GetProperty("strStartDate").GetString();
                String strStartTime = jsonPeriod.GetProperty("strStartTime").GetString();
                String strEndDate = jsonPeriod.GetProperty("strEndDate").GetString();
                String strEndTime = jsonPeriod.GetProperty("strEndTime").GetString();
                int intJobId = jsonPeriod.GetProperty("intJobId").GetInt32();
                int intPkProcessInWorkflow = jsonPeriod.GetProperty("intPkProcessInWorkflow").GetInt32();

                int? intnPkPeriod = null;
                if (
                    jsonPeriod.TryGetProperty("intnPkPeriod", out json) &&
                    (int)jsonPeriod.GetProperty("intnPkPeriod").ValueKind == 4
                    )
                {
                    intnPkPeriod = jsonPeriod.GetProperty("intnPkPeriod").GetInt32();
                }

                int? intnContactId = null;
                if (
                    jsonPeriod.TryGetProperty("intnContactId", out json) &&
                    (int)jsonPeriod.GetProperty("intnContactId").ValueKind == 4
                    )
                {
                    intnContactId = jsonPeriod.GetProperty("intnContactId").GetInt32();
                }

                try
                {
                    //                                      //Method that verify if the period is addable.
                    Perisaddablejson2PeriodIsAddableJson2 perisaddablejson;
                    ResResource.subPeriodIsAddable(strPrintshopId, intnPkPeriod, intPkResource, intnContactId, strStartDate,
                        strStartTime, strEndDate, strEndTime, intJobId, intPkProcessInWorkflow, false, this.configuration,
                        out perisaddablejson, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = perisaddablejson;
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
        public IActionResult ModifyPeriod(
            //                                              //PURPOSE:
            //                                              //Edit a period.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /ModifyPeriod
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //              "intPkPeriod": 2,
            //                                              //              "intPkResource":21,
            //                                              //              "intJobId":156250,
            //                                              //              "strPassword": "password",
            //                                              //              "strStartDate": "2020-04-28",
            //                                              //              "strStartTime": "15:30:00",
            //                                              //              "strEndDate": "2020-04-28",
            //                                              //              "strEndTime": "15:30:00",
            //                                              //              "intPkProcessInWorkflow": 2,
            //                                              //              "intMinsBeforeDelete": 3
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Edit a resource's period.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //Dynamic json that contains all necessary data.
            [FromBody] JsonElement perData
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, perData) &&
                //                                          //Verify if the data is not null or empty.
                perData.TryGetProperty("intPkPeriod", out json) &&
                (int)perData.GetProperty("intPkPeriod").ValueKind == 4 &&
                perData.TryGetProperty("intPkResource", out json) &&
                (int)perData.GetProperty("intPkResource").ValueKind == 4 &&
                perData.TryGetProperty("intJobId", out json) &&
                (int)perData.GetProperty("intJobId").ValueKind == 4 &&
                perData.TryGetProperty("strStartDate", out json) &&
                (int)perData.GetProperty("strStartDate").ValueKind == 3 &&
                perData.TryGetProperty("strStartTime", out json) &&
                (int)perData.GetProperty("strStartTime").ValueKind == 3 &&
                perData.TryGetProperty("strEndDate", out json) &&
                (int)perData.GetProperty("strEndDate").ValueKind == 3 &&
                perData.TryGetProperty("strEndTime", out json) &&
                (int)perData.GetProperty("strEndTime").ValueKind == 3 &&
                perData.TryGetProperty("intPkProcessInWorkflow", out json) &&
                (int)perData.GetProperty("intPkProcessInWorkflow").ValueKind == 4 &&
                perData.TryGetProperty("intMinsBeforeDelete", out json) &&
                (int)perData.GetProperty("intMinsBeforeDelete").ValueKind == 4
                )
            {
                //                                          //Get all the values.
                int intPkPeriod = perData.GetProperty("intPkPeriod").GetInt32();
                int intPkResource = perData.GetProperty("intPkResource").GetInt32();
                int intJobId = perData.GetProperty("intJobId").GetInt32();
                String strStartDate = perData.GetProperty("strStartDate").GetString();
                String strStartTime = perData.GetProperty("strStartTime").GetString();
                String strEndDate = perData.GetProperty("strEndDate").GetString();
                String strEndTime = perData.GetProperty("strEndTime").GetString();
                int intPkProcessInWorkflow = perData.GetProperty("intPkProcessInWorkflow").GetInt32();
                int intMinsBeforeDelete = perData.GetProperty("intMinsBeforeDelete").GetInt32();

                String strPassword = null;
                if (
                    perData.TryGetProperty("strPassword", out json) &&
                    (int)perData.GetProperty("strPassword").ValueKind == 3
                    )
                {
                    strPassword = perData.GetProperty("strPassword").GetString();
                }

                int? intnContactId = null;
                if (
                    perData.TryGetProperty("intnContactId", out json) &&
                    (int)perData.GetProperty("intnContactId").ValueKind == 4
                    )
                {
                    intnContactId = perData.GetProperty("intnContactId").GetInt32();
                }

                try
                {
                    String strLastSunday;
                    String strEstimatedDate;
                    ResResource.subModifyPeriod(strPrintshopId, intPkPeriod, intPkResource, intnContactId, intJobId,
                        strPassword, strStartDate, strStartTime, strEndDate, strEndTime, intPkProcessInWorkflow,
                        false, intMinsBeforeDelete, this.configuration, this.iHubContext, out strLastSunday, 
                        out strEstimatedDate, ref intStatus, ref strUserMessage, ref strDevMessage);

                    obj = new { strLastSunday = strLastSunday, strEstimatedDate = strEstimatedDate };
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
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetPrintshopResources(
            //                                              //PURPOSE:
            //                                              //Get resources associate to printshop.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /GetPrintshopResources?intnPkType=138
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all resource element.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            int? intnPkType
            )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;

            if (
               //                                          //Verify if the printshop is not null.
               ps != null
               )
            {
                try
                {
                    List<ResjsonResourceJson> darrresjson;
                    ResResource.subGetPrintshopResources(ps, intnPkType, out darrresjson, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = darrresjson;
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
        public IActionResult GetAscendantsValuesResource(
            //                                              //PURPOSE:
            //                                              //Get resources associate to printshop.

            //                                              //URL: http://localhost/Odyssey2/Resources
            //                                              //      /GetAscendantsValuesResource?intPkType=2s

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all Ascendants'name elements and values.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkType
            )
        {
            IActionResult aresult = BadRequest("Invalid data");

            if (
                intPkType > 0
                )
            {
                try
                {
                    EleascjsonElementAscendantJson[] arreleascjson = ResResource.arreleascjsonGet(intPkType);

                    aresult = Ok(arreleascjson);
                }
                catch (Exception ex)
                {
                    //                                      //TO FIX. Hacer que este servicio devuelva jsonresponse.
                    int intStatus = 0;
                    String strUserMessage = "", strDevMessage = "";

                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            //                                              //Return a list of jsons.
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetInheritanceData(
            //                                              //PURPOSE:
            //                                              //Get inheritance data.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /GetInheritanceData?intPkTemplateOrResource=138
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //It can be used to get the data for the father (tem)
            //                                              //      when adding a new res or tem.
            //                                              //Also it can be used to get data for the sun (res or tem)
            //                                              //      when editing.
            //                                              //RETURNS:
            //                                              //      200 - Ok().
            int intPkTemplateOrResource
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
               //                                          //Verify if the printshop is not null.
               intPkTemplateOrResource > 0
               )
            {
                try
                {
                    InhedatajsonInheritanceDataJson inhedatajson;
                    ResResource.subGetInheritanceData(intPkTemplateOrResource, out inhedatajson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = inhedatajson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            //                                              //Return a list of jsons.
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                obj);
            IActionResult aresult = base.Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult IsDispensable(
            //                                              //PURPOSE:
            //                                              //Get true if the resource has calculations associated, get 
            //                                              //      false if the resource has not calculations 
            //                                              //      associated.

            //                                              //URL: http://localhost/Odyssey2/Resources
            //                                              //      /IsDispensable?intPk=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get a boolean to know if the resource with that Pk has or 
            //                                              //      not a calculation associated.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPk
            )
        {
            int intStatus = 400;
            String strUserMessage = "Someting wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intPk > 0
                )
            {
                ResResource resortem = ResResource.resFromDB(intPk, false);
                if (
                    resortem == null
                    )
                {
                    resortem = ResResource.resFromDB(intPk, true);
                }

                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Resource not found.";
                if (
                    resortem != null
                    )
                {
                    try
                    {
                        bool boolIsDispensable = resortem.boolIsDispensable(ref intStatus, ref strUserMessage,
                            ref strDevMessage);
                        obj = boolIsDispensable;
                    }
                    catch (Exception ex)
                    {
                        //                                  //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                }
            }

            //                                              //Generate the response.
            Respjson1ResponceJson1 respjson = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage, obj);
            IActionResult aresult = base.Ok(respjson);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetAttributesAndValues(
            //                                              //PURPOSE:
            //                                              //

            //                                              //URL: http://localhost/Odyssey2/Resources
            //                                              //      /GetAttributesAndValues?intPk=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPk
            )
        {
            IActionResult aresult = BadRequest("Invalid data");

            if (
                intPk > 0
                )
            {
                ResResource res = ResResource.resFromDB(intPk, false);
                if (
                    res == null
                    )
                {
                    res = ResResource.resFromDB(intPk, true);
                }

                aresult = BadRequest("Resource not found.");
                if (
                    res != null
                    )
                {
                    try
                    {
                        List<AttrvaljsonAttributeValueJson> darrattrval = res.darrattrvalGet();
                        aresult = base.Ok(darrattrval);
                    }
                    catch (Exception ex)
                    {
                        //                                  //FIX IT. Hacer que este servicio devuelva jsonresponse.
                        int intStatus = 0;
                        String strUserMessage = "", strDevMessage = "";

                        //                                  //Making a log for the exception.
                        Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                    }
                }
            }

            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetAscendantsPkAndValue(
            //                                              //PURPOSE:
            //                                              //

            //                                              //URL: http://localhost/Odyssey2/Resources
            //                                              //      /GetAscendantsPkAndValue?intPk=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intResourcePk,
            int intAttributePk
            )
        {
            IActionResult aresult = BadRequest("Invalid data");

            if (
                (intResourcePk > 0) &&
                (intAttributePk > 0)
                )
            {
                ResResource res = ResResource.resFromDB(intResourcePk, false);
                if (
                    res == null
                    )
                {
                    res = ResResource.resFromDB(intResourcePk, true);
                }
                aresult = BadRequest("Resource not found.");
                if (
                    res != null
                    )
                {
                    aresult = BadRequest("Value attribute not found.");
                    Odyssey2Context context = new Odyssey2Context();
                    Attrjson3AttributeJson3 attrjson3 = res.attrjson3Get(intAttributePk, context);
                    if (
                        attrjson3 != null
                        )
                    {
                        aresult = base.Ok(attrjson3);
                    }
                }
            }

            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetAttributesTemplatesAndResources(
            //                                              //PURPOSE:
            //                                              //

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /GetAttributesTemplatesAndResources?intPk=363
            //                                              //      ?boolIsType=true

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok

            //                                              //intPkType or intPkTemplate
            int intPk,
            bool boolIsType
            )
        {
            int intStatus = 400;
            String strUserMessage = "Invalid data.";
            String strDevMessage = "";
            Object obj = null;

            if (
                intPk > 0
                )
            {
                try
                {
                    TyportemjsonTypeOrTemplateJson typortemjson;
                    ResResource.subGetAttributesTemplatesAndResources(intPk, boolIsType, out typortemjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = typortemjson;
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage, obj);

            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetTypeOrTemplateAllResources(
            //                                              //PURPOSE:
            //                                              //

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /GetTypeOrTemplateAllResources?intPk=2&
            //                                              //      boolIsType=true

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all resource of a template or a type
            //                                              //      from a pkELementTypeElementType or 
            //                                              //      ElementTypeElement.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            //                                              //Receive the pkELementTypeElementType or 
            //                                              //      ElementTypeElement.
            int intPkEleetOrEleele,
            //                                              //Receive boolIsElementTypeElementType.
            bool boolIsEleet,
            //                                              //Receive intPkProcessInWorkflow.
            int intPkProcessInWorkflow,
            int? intnJobId
            )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Invalid data";
            String strDevMessage = "";
            Object obj = null;
            if (
                intPkEleetOrEleele > 0
                )
            {
                try
                {
                    List<TyportempresjsonTypeOrTemplateResourceJson> darrtyportempresjson;

                    ResResource.subGetTypeOrTemplateAllResources(intPkEleetOrEleele, boolIsEleet, intPkProcessInWorkflow,
                        intnJobId, strPrintshopId, out darrtyportempresjson, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = darrtyportempresjson;
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
        public IActionResult GetData(
            //                                              //PURPOSE:
            //                                              //Returns data to be edited.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /GetData?intPk=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPk
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Data invalid.";

            Resjson1ResourceJson1 resjson1 = null;

            if (
                intPk > 0
                )
            {
                try
                {
                    ResResource.subGetData(intPk, out resjson1, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
                catch (Exception ex)
                {
                    //                                      //Making a log for the exception.
                    Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
                }
            }
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                resjson1);

            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------

        [HttpGet("[action]")]
        public IActionResult GetUnitsMeasurement(
           //                                              //PURPOSE:
           //                                              //

           //                                              //URL: http://localhost/Odyssey2/Resource/
           //                                              //GetUnitsMeasurement
           //                                              //     

           //                                              //Method: GET.

           //                                              //DESCRIPTION:
           //                                              //

           //                                              //RETURNS:
           //                                              //      200 - Ok
           )
        {

            int intStatus = 400;
            String strUserMessage = "Invalid data";
            String strDevMessage = "Invalid data";

            List<String> darrstrUnitsMeasurement = null;

            try
            {
                ResResource.GetUnitsMeasurement(out darrstrUnitsMeasurement, ref intStatus, ref strUserMessage,
                    ref strDevMessage);
            }
            catch (Exception ex)
            {
                //                                          //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }

            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
                darrstrUnitsMeasurement);

            IActionResult aresult = Ok(respjson1);
            return aresult;
        }

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetCostData(
            //                                              //PURPOSE:
            //                                              //Get the cost and unit of measurement for a given resource.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /GetCostData?intPk=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the cost and unit of measurement for a given resource.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkResource
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intPkResource > 0
                )
            {
                try
                {
                    CostjsonCostJson costjson;
                    ResResource.subGetCostAndUnitOfMeasurement(intPkResource, out costjson, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = costjson;
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
        public IActionResult GetTimeData(
            //                                              //PURPOSE:
            //                                              //Get the time and unit of measurement for a given resource.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /GetTimeData?intPk=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the time and unit of measurement for a given resource.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkTime
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intPkTime > 0
                )
            {
                try
                {
                    TimejsonTimeJson timejson;
                    ResResource.subGetTimeAndUnitOfMeasurement(intPkTime, out timejson, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = timejson;
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
        public IActionResult GetTimes(
            //                                              //PURPOSE:
            //                                              //Get times for a given resource.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /GetTimes?intPkResource=2

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the time and unit of measurement for a given resource.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkResource
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intPkResource > 0
                )
            {
                try
                {
                    List<TimejsonTimeJson> darrtimejson;
                    ResResource.subGetTimes(intPkResource, out darrtimejson, ref intStatus, ref strUserMessage,
                        ref strDevMessage);
                    obj = darrtimejson;
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
        public IActionResult GetWeek(
            //                                              //PURPOSE:
            //                                              //Get the unit of measurement for a given resource.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /GetWeek?intPkResource=2&strDate=2020-04-26

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the unit of measurement for a given resource.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkResource,
            String strDate
            )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            if (
                (intPkResource > 0) &&
                !String.IsNullOrEmpty(strDate) &&
                strDate.IsParsableToDate()
                )
            {
                try
                {
                    DayjsonDayJson[] arrdayjson;
                    ResResource.subGetWeek(intPkResource, strDate, strPrintshopId, this.configuration, out arrdayjson,
                        ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = arrdayjson;
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
        public IActionResult GetDay(
            //                                              //PURPOSE:
            //                                              //Get Rules and period from one day.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /GetDay?intPkResource=2&strDate=2020-04-26

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get Rules and period from one day

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkResource,
            String strDate
            )
        {
            //                                          //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            if (
                (intPkResource > 0) &&
                !String.IsNullOrEmpty(strDate) &&
                strDate.IsParsableToDate()
                )
            {
                try
                {
                    DayjsonDayJson dayjson;
                    ResResource.subGetDay(intPkResource, strDate, strPrintshopId, this.configuration, out dayjson,
                        ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = dayjson;
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
        public IActionResult RuleIsAddable(
            //                                              //PURPOSE:
            //                                              //Return a bool in true if the rule do not overlap with 
            //                                              //      periods.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /RuleIsAddable

            //                                              //Method: GET.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "boolIsEmployee":true,  
            //                                              //          "intnContactId":23, 
            //                                              //          "intnPkResource":1,
            //                                              //          "strFrecuency":"daily",
            //                                              //          "strStartTime":"16:55:00",
            //                                              //          "strEndTime":"17:30:00",
            //                                              //          "strStartDate":null,
            //                                              //          "strEndDate":null,
            //                                              //          "strRangeStartDate":null
            //                                              //          "strRangeStartTime":null
            //                                              //          "strRangeEndDate":null
            //                                              //          "strRangeEndTime":null
            //                                              //          "arrintDays":null,
            //                                              //          "strDay":null
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Get the periods to delete if the rule is added.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            [FromBody] JsonElement jsonRule
                )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if pk is not null and greater that 0.
                !Object.ReferenceEquals(null, jsonRule) &&
                jsonRule.TryGetProperty("strFrecuency", out json) &&
                (int)jsonRule.GetProperty("strFrecuency").ValueKind == 3 &&
                jsonRule.TryGetProperty("strStartTime", out json) &&
                (int)jsonRule.GetProperty("strStartTime").ValueKind == 3 &&
                jsonRule.TryGetProperty("strEndTime", out json) &&
                (int)jsonRule.GetProperty("strEndTime").ValueKind == 3 &&
                jsonRule.TryGetProperty("boolIsEmployee", out json) &&
                ((int)jsonRule.GetProperty("boolIsEmployee").ValueKind == 5 ||
                (int)jsonRule.GetProperty("boolIsEmployee").ValueKind == 6)
                )
            {
                //                                          //Get data from json.                
                String strFrecuency = jsonRule.GetProperty("strFrecuency").GetString();
                String strStartTime = jsonRule.GetProperty("strStartTime").GetString();
                String strEndTime = jsonRule.GetProperty("strEndTime").GetString();
                bool boolIsEmployee = jsonRule.GetProperty("boolIsEmployee").GetBoolean();

                int? intnPkResource = null;
                if (
                    jsonRule.TryGetProperty("intnPkResource", out json) &&
                    (int)jsonRule.GetProperty("intnPkResource").ValueKind == 4
                    )
                {
                    intnPkResource = jsonRule.GetProperty("intnPkResource").GetInt32();
                }

                int? intnContactId = null;
                if (
                    jsonRule.TryGetProperty("intnContactId", out json) &&
                    (int)jsonRule.GetProperty("intnContactId").ValueKind == 4
                    )
                {
                    intnContactId = jsonRule.GetProperty("intnContactId").GetInt32();
                }

                if (
                    intnContactId == null
                    )
                {
                    //                                      //Get the contact id from token.
                    var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                    intnContactId = idClaimContact.Value.ParseToInt();
                }

                String strStartDate = null;
                if (
                    jsonRule.TryGetProperty("strStartDate", out json) &&
                    (int)jsonRule.GetProperty("strStartDate").ValueKind == 3
                    )
                {
                    strStartDate = jsonRule.GetProperty("strStartDate").GetString();
                }

                String strEndDate = null;
                if (
                    jsonRule.TryGetProperty("strEndDate", out json) &&
                    (int)jsonRule.GetProperty("strEndDate").ValueKind == 3
                    )
                {
                    strEndDate = jsonRule.GetProperty("strEndDate").GetString();
                }

                String strRangeStartDate = null;
                if (
                    jsonRule.TryGetProperty("strRangeStartDate", out json) &&
                    (int)jsonRule.GetProperty("strRangeStartDate").ValueKind == 3
                    )
                {
                    strRangeStartDate = jsonRule.GetProperty("strRangeStartDate").GetString();
                }

                String strRangeStartTime = null;
                if (
                    jsonRule.TryGetProperty("strRangeStartTime", out json) &&
                    (int)jsonRule.GetProperty("strRangeStartTime").ValueKind == 3
                    )
                {
                    strRangeStartTime = jsonRule.GetProperty("strRangeStartTime").GetString();
                }

                String strRangeEndDate = null;
                if (
                    jsonRule.TryGetProperty("strRangeEndDate", out json) &&
                    (int)jsonRule.GetProperty("strRangeEndDate").ValueKind == 3
                    )
                {
                    strRangeEndDate = jsonRule.GetProperty("strRangeEndDate").GetString();
                }

                String strRangeEndTime = null;
                if (
                    jsonRule.TryGetProperty("strRangeEndTime", out json) &&
                    (int)jsonRule.GetProperty("strRangeEndTime").ValueKind == 3
                    )
                {
                    strRangeEndTime = jsonRule.GetProperty("strRangeEndTime").GetString();
                }

                String strDay = null;
                if (
                    jsonRule.TryGetProperty("strDay", out json) &&
                    (int)jsonRule.GetProperty("strDay").ValueKind == 3
                    )
                {
                    strDay = jsonRule.GetProperty("strDay").GetString();
                }

                int[] arrintDays = null;
                if (
                    jsonRule.TryGetProperty("arrintDays", out json) &&
                    (int)jsonRule.GetProperty("arrintDays").ValueKind == 2
                    )
                {
                    //                                      //To easy code.
                    int intLength = jsonRule.GetProperty("arrintDays").GetArrayLength();

                    arrintDays = new int[intLength];
                    for (int intU = 0; intU < intLength; intU = intU + 1)
                    {
                        arrintDays[intU] = jsonRule.GetProperty("arrintDays")[intU].GetInt32();
                    }
                }

                try
                {
                    bool boolRuleIsAddable;
                    //                                      //Method to bring the periods to delete if the rule is 
                    //                                      //      added.
                    ResResource.subRuleIsAddable(boolIsEmployee, intnContactId, intnPkResource, strPrintshopId, strFrecuency,
                        strStartTime, strEndTime, strStartDate, strEndDate, strRangeStartDate, strRangeStartTime, strRangeEndDate,
                        strRangeEndTime, arrintDays, strDay, this.configuration, out boolRuleIsAddable, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
                    obj = boolRuleIsAddable;
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
        [HttpGet("[action]")]
        public IActionResult GetRules(
            //                                              //PURPOSE:
            //                                              //Get rules from resource, printshop or employee.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      /GetRules?intPkResource=2&boolIsEmployee=false&
            //                                              //      intnContactId=78542

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all rules of a resource, rules of a printshop or
            //                                              //      rules or a employee.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int? intnPkResource,
            bool boolIsEmployee,
            int? intnContactId
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            if (
                intnContactId == null &&
                boolIsEmployee == true
                )
            {
                //                                          //Get the contact id from token.
                var idClaimContact = User.Claims.FirstOrDefault(c => c.Type == "intContactId");
                intnContactId = idClaimContact.Value.ParseToInt();
            }

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            RuljsonRuleJson[] arrruljsonRules;
            if (
                //                                          //User gets printshop's rules.
                (intnPkResource == null &&
                boolIsEmployee == false &&
                intnContactId == null) ||
                //                                          //User gets resource's rules.
                (intnPkResource > 0 &&
                boolIsEmployee == false &&
                intnContactId == null) ||
                //                                          //User gets employee's rules.
                (intnPkResource == null &&
                boolIsEmployee == true &&
                intnContactId > 0)
                )
            {
                try
                {
                    arrruljsonRules = ResResource.arrruljsonGetRules(intnPkResource, ps, boolIsEmployee,
                        intnContactId, this.configuration, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = arrruljsonRules;
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
        public IActionResult GetPeriod(
            //                                              //PURPOSE:
            //                                              //Get a specific period.

            //                                              //URL: http://localhost/Odyssey2/Resource/
            //                                              //   GetPeriod?intPkPeriod=21
            //                                              //     

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkPeriod
            )
        {
            //                                              //Get the printshop id from token.
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data";
            Object obj = null;
            if (
                intPkPeriod > 0 &&
                ps != null
                )
            {
                try
                {
                    PerjsonPeriodJson perjson;
                    ResResource.subGetPeriod(intPkPeriod, ps, out perjson, ref intStatus, ref strUserMessage,
                            ref strDevMessage);
                    obj = perjson;
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
        public IActionResult GetAvailableTimes(
            //                                              //PURPOSE:
            //                                              //Get the unit of measurement for a given resource.

            //                                              //URL: http://localhost/Odyssey2/Resource
            //                                              //      

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get the unit of measurement for a given resource.

            //                                              //RETURNS:
            //                                              //      200 - Ok
            int intPkResource,
            int intJobId,
            int intPkProcessInWorkflow,
            int intPkEleetOrEleele,
            bool boolIsEleet
            )
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
            String strPrintshopId = idClaim.Value;

            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;

            if (
                (intPkResource > 0) &&
                (intJobId > 0) &&
                (intPkProcessInWorkflow > 0) &&
                (intPkEleetOrEleele > 0)
                )
            {
                try
                {
                    List<PerentityPeriodEntityDB> darrperentityNotUsed = new List<PerentityPeriodEntityDB>();
                    ZonedTime ztimeBaseNow = ZonedTimeTools.NewZonedTime(Date.Now(ZonedTimeTools.timezone),
                            Time.Now(ZonedTimeTools.timezone));
                    int intOffsetTimeMinuteAfterDateNow = 2;

                    TimesjsonTimesJson timesjson;
                    long longMilisecondNeeded = 0;
                    ResResource.subGetAvailableTime(strPrintshopId, intPkResource, intJobId, intPkProcessInWorkflow,
                        intPkEleetOrEleele, boolIsEleet, ztimeBaseNow, intOffsetTimeMinuteAfterDateNow,
                        darrperentityNotUsed, this.configuration, null, out timesjson, ref intStatus,
                        ref strUserMessage, ref strDevMessage, ref longMilisecondNeeded);
                    obj = timesjson;
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
        public IActionResult IsAddable(
            //                                              //PURPOSE:
            //                                              //Get true if the array of attributes does not contain one 
            //                                              //      or more dimensions attributes or the array contains
            //                                              //      all the dimensions attributes.

            //                                              //URL: http://localhost/Odyssey2/Resource/IsAddable  

            //                                              //Method: GET.
            //                                              //Use a JSON like this:


            //                                              //DESCRIPTION:
            //                                              //Search the dimensions attributes if it contains none or 
            //                                              //      all of them return true, other case return false.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            [FromBody] JsonElement jsonAttributes
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if jsonAttributes is not null or is not empty.
                !((int)jsonAttributes.ValueKind == 7) &&
                !((int)jsonAttributes.ValueKind == 0) &&
                jsonAttributes.TryGetProperty("intPkType", out json) &&
                ((int)jsonAttributes.GetProperty("intPkType").ValueKind == 4) &&
                jsonAttributes.TryGetProperty("arrattr", out json)
                )
            {
                int intPkType = jsonAttributes.GetProperty("intPkType").GetInt32();

                //                                          //Get the array as a known json.
                List<Attrjson5AttributeJson5> darrattjson5 = new List<Attrjson5AttributeJson5>();
                if (
                    //                                      //Array is not null.
                    (int)jsonAttributes.GetProperty("arrattr").ValueKind == 2
                    )
                {
                    for (int intA = 0; intA < jsonAttributes.GetProperty("arrattr").GetArrayLength(); intA = intA + 1)
                    {
                        JsonElement json1 = jsonAttributes.GetProperty("arrattr")[intA];

                        int? intnInheritedValuePk = null;
                        if (
                            json1.TryGetProperty("intnInheritedValuePk", out json) &&
                            (int)json1.GetProperty("intnInheritedValuePk").ValueKind == 4
                            )
                        {
                            intnInheritedValuePk = json1.GetProperty("intnInheritedValuePk").GetInt32();
                        }

                        //                                  //Create Json.
                        Attrjson5AttributeJson5 attjson5 = new Attrjson5AttributeJson5(
                            json1.GetProperty("strAscendant").GetString(), json1.GetProperty("strValue").GetString(),
                            intnInheritedValuePk, json1.GetProperty("boolChangeable").GetBoolean(), null);
                        darrattjson5.Add(attjson5);
                    }
                }

                try
                {
                    bool boolIsAddable = ResResource.boolIsAddableAlKindOfResources(intPkType, darrattjson5, ref intStatus, 
                        ref strUserMessage, ref strDevMessage);
                    obj = boolIsAddable;
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
        public IActionResult GetThicknessUnits(
           //                                              //PURPOSE:
           //                                              //Return the ThicknessUnits

           //                                              //URL: http://localhost/Odyssey2/Resource/GetThicknessUnits  

           //                                              //Method: GET.
           //                                              //Use a JSON like this:


           //                                              //DESCRIPTION:
           //                                              //Return the ThicknessUnits

           //                                              //RETURNS:
           //                                              //      200 - Ok
           )
        {
            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "";
            Object obj = null;

            Odyssey2Context context = new Odyssey2Context();

            try
            {
                obj = from enumentity in context.Enumeration
                      where enumentity.strEnumName == "ThicknessUnit"
                      select enumentity.strEnumValue;
            }
            catch (Exception ex)
            {
                //                                          //Making a log for the exception.
                Tools.subExceptionHandler(ex, ref intStatus, ref strUserMessage, ref strDevMessage);
            }

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
