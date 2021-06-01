/*TASK RP. PRINTSHOP*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Linq;
using TowaStandard;

//                                                          //AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (AQG-Andrea Quiroz).
//                                                          //DATE: April 25, 2020.

namespace Odyssey2Backend.Controllers
{
    //==================================================================================================================
    [Route("Odyssey2/[controller]")]
    [ApiController]
    public class TempDefaultDataController : ControllerBase
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        [HttpGet("[action]/")]
        public IActionResult Add(
            //                                              //PURPOSE:
            //                                              //Add Initial data to ps.

            //                                              //URL: http://localhost/Odyssey2/TempInitialDataController
            //                                              //      /Add
            //                                              //Method: GET.

            //                                              //DESCRIPTION:
            //                                              //

            //                                              //RETURNS:
            //                                              //      200 - Ok().
            )
        {
            int intStatus = 200;
            String strUserMessage = "";
            String strDevMessage = "Add Initial data to ps, will be automatically.";
            Object obj = null;

            String strPrintshopId = "13832";
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId);

            ps.AddDefaultData();

            obj = new {

            };
            
            Respjson1ResponceJson1 respjson1 = new Respjson1ResponceJson1(intStatus, strUserMessage, strDevMessage,
               obj);
            IActionResult aresult = base.Ok(respjson1);
            
            return aresult;
        }

        //------------------------------------------------------------------------------------------------------------------
    }

    //======================================================================================================================
}
/*END-TASK*/
