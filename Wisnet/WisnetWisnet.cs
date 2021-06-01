/*TASK RP.WISNET*/
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TowaStandard;

//                                                          //AUTHOR: Towa (IUGS - Ivan Guzman).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: December 18, 2020.

namespace Odyssey2Backend.Wisnet
{
    //==================================================================================================================
    public class WisnetWisnet
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subToken(
            IConfiguration configuration_I,
            out TokenjsonTokenJson tokenjson_O,
            out int intStatus_IO,
            out String strUserMessage_IO,
            out String strDevMessage_IO
            )
        {
            //                                              //Create token.

            //                                              //Get Issuer and Audience.
            String strIssuer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection(
                "WisnetAPI")["Issuer"];

            //                                              //Get signingCredentials.
            String strKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection(
                "WisnetAPI")["Key"];
            SymmetricSecurityKey sskKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(strKey));
            SigningCredentials credCredentials = new SigningCredentials(sskKey, SecurityAlgorithms.HmacSha256);

            //                                              //Get Token expiration.
            DateTime datetimeExpiration = DateTime.Now.AddYears(17);

            //                                              //List that will contain token's variables.
            List<Claim> darrclaims = new List<Claim>();

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: strIssuer,
                audience: strIssuer,
                signingCredentials: credCredentials,
                expires: datetimeExpiration,
                claims: darrclaims
                );
            String strToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            tokenjson_O = new TokenjsonTokenJson(strToken);

            intStatus_IO = 200;
            strUserMessage_IO = "Success.";
            strDevMessage_IO = "";
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
