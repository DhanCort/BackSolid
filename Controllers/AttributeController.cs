/*TASK RP. ATTRIBUTE PROCESS TYPE*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odyssey2Backend.Infrastructure;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Text.Json;

//                                                          //AUTHOR: Towa (VSTD - Victor Torres).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: November 22, 2019.

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //  var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
    //                                                      //  String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    public class AttributeController : Controller
    {
        //                                                  //Controller associated with the actions for an attribute.
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
                
        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetValue(
            //                                              //PURPOSE:
            //                                              //Brings back the information from the element 
            //                                              //      that inherits to, depending on the bools
            //                                              //      values.

            //                                              //URL: http://localhost/Odyssey2/Attribute
            //                                              //      /GetValue?intValuePk=1&boolIsCost=false&
            //                                              //      boolIsAvailability=false&boolIsUnit=true

            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //This service is consume when the user clicks on the
            //                                              //      button Reset on the Resources screen.

            //                                              //RETURNS:
            //                                              //      200 - Ok

            int  intValuePk,
            bool boolIsCost,
            bool boolIsUnit,
            bool boolIsAvailability
            )
        {
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            if (
                intValuePk > 0
                )
            {
                try
                {
                    Inhedatajson1InheritanceDataJson1 inhedatajson1;
                    AttrAttribute.subGetValue(intValuePk, boolIsCost, boolIsAvailability, boolIsUnit,
                        out inhedatajson1, ref intStatus, ref strUserMessage, ref strDevMessage);
                    obj = inhedatajson1;
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
    }

    //==================================================================================================================
}
/*END-TASK*/
