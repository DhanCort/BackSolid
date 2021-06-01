/*TASK RP. PRODUCT TYPE*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

//                                                          //AUTHOR: Towa (VSTD - Victor Torres).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 26, 2019.

namespace Odyssey2Backend.Controllers
{
    //                                                      //To obtain the strPrintshopId from token:
    //                                                      //      var idClaim = User.Claims.FirstOrDefault(c => 
    //                                                      //      c.Type == "strPrintshopId");
    //                                                      //      String strPrintshopId = idClaim.Value;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
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
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpPost("[action]")]
        public IActionResult SetType(
            //                                              //PURPOSE:
            //                                              //Set the attribute productType to the product.

            //                                              //URL: http://localhost/Odyssey2/ProductType
            //                                              //      /SetType
            //                                              //Method: POST.
            //                                              //Use a JSON like this:
            //                                              //      {
            //                                              //          "strType":"Book",
            //                                              //          "intPk":0
            //                                              //      }

            //                                              //DESCRIPTION:
            //                                              //Set the attribute productType with a value from 
            //                                              //      ProductTypeEnum.

            //                                              //RETURNS:
            //                                              //      200 - Ok().

            //                                              //JSON containing all necessary data.
            [FromBody] JsonElement jsonTypeData
            )
        {            
            int intStatus = 400;
            String strUserMessage = "Something is wrong.";
            String strDevMessage = "Invalid data.";
            Object obj = null;
            JsonElement json;
            if (
                //                                          //Verify if the object is not null.
                !Object.ReferenceEquals(null, jsonTypeData) &&
                jsonTypeData.TryGetProperty("intPk", out json) &&
                (int)jsonTypeData.GetProperty("intPk").ValueKind == 4 &&
                jsonTypeData.TryGetProperty("strType", out json) &&
                (int)jsonTypeData.GetProperty("strType").ValueKind == 3
                )
            {
                int intPk = jsonTypeData.GetProperty("intPk").GetInt32();
                String strType = jsonTypeData.GetProperty("strType").GetString();
               
                intStatus = 401;
                strUserMessage = "Something is wrong.";
                strDevMessage = "Invalid data.";
                obj = null;
                if (
                    //                                      //Verify the type is valid.
                    Enum.IsDefined(typeof(ProductTypeEnum), strType)
                    )
                {
                    //                                      //Get the printshop id from token.
                    var idClaim = User.Claims.FirstOrDefault(c => c.Type == "strPrintshopId");
                    String strPrintshopId = idClaim.Value;

                    try
                    {
                        ProdtypProductType.subSetProductType(intPk, strType, strPrintshopId, ref intStatus,
                        ref strUserMessage, ref strDevMessage);
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
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]")]
        public IActionResult GetTypes(
            //                                              //PURPOSE:
            //                                              //Get product types.

            //                                              //URL: http://localhost/Odyssey2/ProductType
            //                                              //      /GetTypes.
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //Get all the types of product types.

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            List<String> darrstr = new List<string>();
            foreach(ProductTypeEnum type in Enum.GetValues(typeof(ProductTypeEnum)))
            {
                darrstr.Add(type.ToString());
            }
            return base.Ok(darrstr);
        }

        //------------------------------------------------------------------------------------------------------------------
    }

    //======================================================================================================================
}
/*END-TASK*/
