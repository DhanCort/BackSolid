/*TASK RP.USER*/
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

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 13, 2020.

namespace Odyssey2Backend.App
{
    //==================================================================================================================
    public class UserUser
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subLogin(
            String strEmail_I,
            String strPrintshopId_I,
            String strPassword_I,
            IConfiguration configuration_I,
            out LogjsonLoginJson logjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            logjson_O = null;

            intStatus_IO = 401;
            strUserMessage_IO = "Email cannot be empty.";
            strDevMessage_IO = "";
            if (
                //                                          //The email comes empty.
                strEmail_I != ""
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Password cannot be empty.";
                strDevMessage_IO = "";
                if (
                    //                                      //The password comes empty.
                    strPassword_I != ""
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Printshop cannot be empty.";
                    if (
                        //                                  //The printshop id comes empty.
                        strPrintshopId_I != ""
                        )
                    {
                        //                                  //Get the ps.
                        PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Printshop not found.";
                        if (
                            //                              //Ps exists.
                            ps != null
                            )
                        {
                            //                              //Prepare the strEmail.
                            String strEmail = strEmail_I.ToLower();
                            strEmail = strEmail.TrimStart(' ');
                            strEmail = strEmail.TrimEnd(' ');

                            Odyssey2Context context = new Odyssey2Context();

                            //                              //Find administrator.
                            AdminentityAdministratorEntityDB adminentity = context.Administrator.FirstOrDefault(admin =>
                                admin.strEmail == strEmail);

                            if (
                                //                          //It is not a super administrator.
                                adminentity == null
                                )
                            {
                                //                          //Get user(s) data from Wisnet. 
                                Task<List<Userjson1UserJson1>> Task_userjson1FromWisnet = HttpTools<Userjson1UserJson1>.
                                    GetListAsyncToEndPoint(configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi")
                                    + "/Contacts/GetUserInformation/" + strEmail + "/" + strPrintshopId_I);
                                Task_userjson1FromWisnet.Wait();

                                intStatus_IO = 405;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "Connection lost with Wisnet.";
                                if (
                                    //                      //There is a result.
                                    Task_userjson1FromWisnet.Result != null
                                    )
                                {
                                    //                      //Take the result as a dynamic array of json.
                                    List<Userjson1UserJson1> darruserjson1FromWisnet = new List<Userjson1UserJson1>();
                                    darruserjson1FromWisnet = Task_userjson1FromWisnet.Result;

                                    intStatus_IO = 406;
                                    strUserMessage_IO = "Something is wrong.";
                                    strDevMessage_IO = "Invalid user for that printshop.";
                                    if (
                                        //                  //The array is not empty.
                                        darruserjson1FromWisnet.Count != 0
                                        )
                                    {
                                        intStatus_IO = 407;
                                        strUserMessage_IO = "You are not able to login. Please contact MI4P.";
                                        strDevMessage_IO = "";
                                        if (
                                            //              //The printshop has access
                                            darruserjson1FromWisnet[0].intPrintshopId != 0
                                            )
                                        {
                                            //              //Check if the pass makes match with one of the users.
                                            bool boolIsValid = false;
                                            Userjson1UserJson1 userjson1 = new Userjson1UserJson1();
                                            int intI = 0;

                                            /*UNTIL-DO*/
                                            while (!(
                                                //          //The auxiliar int is the same length as the array.
                                                (intI >= darruserjson1FromWisnet.Count) ||
                                                //          //Pass makes match.
                                                boolIsValid
                                                ))
                                            {
                                                if (
                                                    strPassword_I == darruserjson1FromWisnet[intI].strPassword
                                                    )
                                                {
                                                    boolIsValid = true;
                                                    userjson1 = darruserjson1FromWisnet[intI];
                                                }

                                                intI = intI + 1;
                                            }

                                            intStatus_IO = 408;
                                            strUserMessage_IO = "Something is wrong.";
                                            strDevMessage_IO = "Invalid password.";
                                            if (
                                                boolIsValid
                                                )
                                            {
                                                intStatus_IO = 409;
                                                strUserMessage_IO = "Inactive account. Please contact MI4P.";
                                                strDevMessage_IO = "";
                                                if (
                                                    userjson1.intActive == 1
                                                    )
                                                {
                                                    //      //Find supervisor.
                                                    RolentityRoleEntityDB rolentity =
                                                        context.Role.FirstOrDefault(role =>
                                                        role.intContactId == userjson1.intContactId &&
                                                        role.boolSupervisor &&
                                                        role.intPkPrintshop == ps.intPk);
                                                    bool boolIsSupervisor = rolentity != null;

                                                    //      //Find accountant.
                                                    RolentityRoleEntityDB rolentityAccountant =
                                                        context.Role.FirstOrDefault(role =>
                                                        role.intContactId == userjson1.intContactId &&
                                                        role.boolAccountant &&
                                                        role.intPkPrintshop == ps.intPk);
                                                    bool boolIsAccountant = rolentityAccountant != null;

                                                    //      //Create token.
                                                    String strMI4PK = new ConfigurationBuilder().AddJsonFile(
                                                        "appsettings.json").Build().GetSection("Odyssey2Settings")["MI4PK"];
                                                    SymmetricSecurityKey sskKey = new SymmetricSecurityKey(Encoding.UTF8.
                                                        GetBytes(strMI4PK));
                                                    SigningCredentials credCredentials = new SigningCredentials(sskKey,
                                                        SecurityAlgorithms.HmacSha256);

                                                    //      //Token expiration.
                                                    DateTime datetimeExpiration = DateTime.Now.AddDays(10);

                                                    //      //List that will contain token's variables.
                                                    List<Claim> darrclaims = new List<Claim>();

                                                    bool boolIsAdmin = userjson1.intPrintshopAdmin == 1;
                                                    bool boolIsOwner = userjson1.intPrintshopOwner == 1;

                                                    if (
                                                        boolIsOwner || boolIsAdmin
                                                        )
                                                    {
                                                        boolIsAccountant = true;
                                                    }

                                                    darrclaims.Add(new Claim("strPrintshopId", ps.strPrintshopId));
                                                    darrclaims.Add(new Claim("boolIsSuperAdmin", "False"));
                                                    darrclaims.Add(new Claim("boolIsAdmin", boolIsAdmin.ToString()));
                                                    darrclaims.Add(new Claim("boolIsOwner", boolIsOwner.ToString()));
                                                    darrclaims.Add(new Claim("boolIsSupervisor",
                                                        boolIsSupervisor.ToString()));
                                                    darrclaims.Add(new Claim("boolIsAccountant",
                                                        boolIsAccountant.ToString()));
                                                    darrclaims.Add(new Claim("intContactId", userjson1.intContactId + ""));

                                                    JwtSecurityToken jwtToken = new JwtSecurityToken(
                                                       issuer: strMI4PK,
                                                       audience: "front",
                                                       expires: datetimeExpiration,
                                                       signingCredentials: credCredentials,
                                                       claims: darrclaims
                                                       );
                                                    String strToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                                                    int intUnreadNotificatons = UserUser.intUnreadNotifications(
                                                        userjson1.intContactId, ps.intPk);

                                                    //      //Customer URL
                                                    String strCustomerUrl = "https://" + ps.strUrl +
                                                        PsPrintShop.strComplementToCustomerUrl;

                                                    //      //Url to send a proof.
                                                    String strSendAProofUrl = "https://" + ps.strUrl +
                                                        PsPrintShop.strSendAProofUrl;

                                                    bool boolOffsetNumber = ps.boolOffset;

                                                    logjson_O = new LogjsonLoginJson(strToken, userjson1.strName,
                                                        userjson1.strLastName, ps.strPrintshopId, ps.strPrintshopName,
                                                        boolIsAdmin, boolIsOwner, false, boolIsSupervisor,
                                                        boolIsAccountant, intUnreadNotificatons, userjson1.intContactId,
                                                        strCustomerUrl, strSendAProofUrl, boolOffsetNumber);

                                                    intStatus_IO = 200;
                                                    strUserMessage_IO = "Success.";
                                                    strDevMessage_IO = "";
                                                }
                                            }                                            
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //                          //It is a super administrator.

                                intStatus_IO = 407;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "Invalid password.";
                                if (
                                    adminentity.strPassword == strPassword_I
                                    )
                                {
                                    //                      //Create token.
                                    String strMI4PK = new ConfigurationBuilder().AddJsonFile(
                                        "appsettings.json").Build().GetSection("Odyssey2Settings")["MI4PK"];
                                    SymmetricSecurityKey sskKey = new SymmetricSecurityKey(Encoding.UTF8.
                                        GetBytes(strMI4PK));
                                    SigningCredentials credCredentials = new SigningCredentials(sskKey,
                                        SecurityAlgorithms.HmacSha256);

                                    //                      //Token expiration.
                                    DateTime datetimeExpiration = DateTime.Now.AddDays(10);

                                    //                      //List that will contain token's variables.
                                    List<Claim> darrclaims = new List<Claim>();

                                    darrclaims.Add(new Claim("strPrintshopId", ps.strPrintshopId));
                                    darrclaims.Add(new Claim("boolIsSuperAdmin", "True"));
                                    darrclaims.Add(new Claim("boolIsAdmin", "True"));
                                    darrclaims.Add(new Claim("boolIsOwner", "True"));
                                    darrclaims.Add(new Claim("boolIsSupervisor", "False"));
                                    darrclaims.Add(new Claim("boolIsAccountant", "True"));
                                    darrclaims.Add(new Claim("intContactId", "0"));

                                    JwtSecurityToken jwtToken = new JwtSecurityToken(
                                       issuer: strMI4PK,
                                       audience: "front",
                                       expires: datetimeExpiration,
                                       signingCredentials: credCredentials,
                                       claims: darrclaims
                                       );
                                    String strToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                                    //                      //Url to send a proof.
                                    String strSendAProofUrl = "https://" + ps.strUrl + 
                                        PsPrintShop.strSendAProofUrl;
                                    //                      //Customer URL
                                    String strCustomerUrl = "https://" + ps.strUrl + 
                                        PsPrintShop.strComplementToCustomerUrl;

                                    bool boolOffsetNumber = ps.boolOffset;

                                    logjson_O = new LogjsonLoginJson(strToken, adminentity.strName,
                                        adminentity.strLastName, ps.strPrintshopId, ps.strPrintshopName, true, true, 
                                        true, false, true, 0, 0, strCustomerUrl, strSendAProofUrl, boolOffsetNumber);

                                    intStatus_IO = 200;
                                    strUserMessage_IO = "Success.";
                                    strDevMessage_IO = "";
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static int intUnreadNotifications(
            //                                              //Calculates total number of unread notifications by 
            //                                              //      printshop or by employee.
            //                                              //Deletes old alerts.

            int intContactId_I,
            int intPkPrintshop_I
            )
        {
            int intUnreadNotifications = 0;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find alerts.
            List<AlertentityAlertEntityDB> darralertentity = context.Alert.Where(alert => 
                (alert.intPkPrintshop == intPkPrintshop_I && alert.intnContactId == null) ||
                (alert.intnContactId == intContactId_I)).ToList();

            foreach(AlertentityAlertEntityDB alertentity in darralertentity)
            {
                //                                          //Read or unread alerts.
                String strReadBy = (alertentity.strReadBy == null) ? "" : alertentity.strReadBy;
                if (
                    !strReadBy.Contains(intContactId_I + "")
                    )
                {
                    intUnreadNotifications = intUnreadNotifications + 1;
                }

                if (
                    //                                      //Alert related to tasks.
                    alertentity.intnPkTask != null
                    )
                {
                    //                                      //Find task.
                    TaskentityTaskEntityDB taskentity = context.Task.FirstOrDefault(task => 
                        task.intPk == alertentity.intnPkTask);

                    if (
                        taskentity != null
                        )
                    {
                        //                                  //Task's start time.
                        String strTaskStartTime = taskentity.strStartHour + ":" + taskentity.strStartMinute + ":" +
                           taskentity.strStartSecond;
                        //                                  //Task's start date and time.
                        ZonedTime ztimeTaskStartDate = ZonedTimeTools.NewZonedTime(taskentity.strStartDate.ParseToDate(),
                                strTaskStartTime.ParseToTime());

                        //                                  //Time now.
                        ZonedTime ztimeDateNow = ZonedTimeTools.NewZonedTime(Date.Now(ZonedTimeTools.timezone),
                                Time.Now(ZonedTimeTools.timezone));

                        if (
                            //                              //Task is in the past.
                            ztimeTaskStartDate < ztimeDateNow
                            )
                        {
                            if (
                                //                          //Alert was not read.
                                !strReadBy.Contains(intContactId_I + "")
                               )
                            {
                                intUnreadNotifications = intUnreadNotifications - 1;
                            }

                            //                              //Remove alert.
                            context.Alert.Remove(alertentity);
                            context.SaveChanges();
                        }
                    }
                }
            }

            return intUnreadNotifications;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subChangePrintshop(
            String strPrintshopId_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO,
            ref Object obj_IO
            )
        {
            //                                              //Get the ps.
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Printshop not found.";
            if (
                //                                          //Ps exists.
                ps != null
                )
            {
                //                                          //Create token.
                String strMI4PK = new ConfigurationBuilder().AddJsonFile(
                    "appsettings.json").Build().GetSection("Odyssey2Settings")["MI4PK"];
                SymmetricSecurityKey sskKey = new SymmetricSecurityKey(Encoding.UTF8.
                    GetBytes(strMI4PK));
                SigningCredentials credCredentials = new SigningCredentials(sskKey,
                    SecurityAlgorithms.HmacSha256);

                DateTime datetimeExpiration = DateTime.Now.AddDays(10);

                List<Claim> darrclaims = new List<Claim>();

                darrclaims.Add(new Claim("strPrintshopId", ps.strPrintshopId));
                darrclaims.Add(new Claim("boolIsSuperAdmin", "True"));
                darrclaims.Add(new Claim("boolIsAdmin", "True"));
                darrclaims.Add(new Claim("boolIsOwner", "True"));
                darrclaims.Add(new Claim("intContactId", "0"));

                JwtSecurityToken jwtToken = new JwtSecurityToken(
                   issuer: strMI4PK,
                   audience: "front",
                   expires: datetimeExpiration,
                   signingCredentials: credCredentials,
                   claims: darrclaims
                   );
                String strToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                String strPrintshopName = ps.strPrintshopName;

                obj_IO = new { strToken, strPrintshopName };

                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPrintshops(
            String strEmail_I,
            IConfiguration configuration_I,
            out UserpsjsonUserPrintshopsJson userpsjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            List<Psjson1PrinthsopJson1> darrpsjson1 = new List<Psjson1PrinthsopJson1>();
            //                                              //Prepare the strEmail.
            String strEmail = strEmail_I.ToLower();
            strEmail = strEmail.TrimStart(' ');
            strEmail = strEmail.TrimEnd(' ');

            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find if it is a administrator.
            AdminentityAdministratorEntityDB adminentity = context.Administrator.FirstOrDefault(admin =>
                admin.strEmail == strEmail);

            bool boolIsAdmin = true;
            if (
                //                                          //It is a administrator.
                adminentity == null
                )
            {
                //                                          //Get user(s) data from Wisnet. 
                Task<List<Psjson1PrinthsopJson1>> Task_psjson1FromWisnet = HttpTools<Psjson1PrinthsopJson1>.
                    GetListAsyncToEndPoint(configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi")
                    + "/Contacts/GetPrintshops/" + strEmail);
                Task_psjson1FromWisnet.Wait();

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Connection lost with Wisnet.";
                if (
                    //                                      //There is a result.
                    Task_psjson1FromWisnet.Result != null
                    )
                {
                    //                                      //Take the result as a dynamic array of json.
                    darrpsjson1 = Task_psjson1FromWisnet.Result;

                    intStatus_IO = 402;
                    strUserMessage_IO = "Invalid email.";
                    strDevMessage_IO = "";
                    if (
                        //                                  //Email founded.
                        darrpsjson1.Count > 0
                        )
                    {
                        intStatus_IO = 200;
                        strUserMessage_IO = "Success.";
                    }
                }

                boolIsAdmin = false;
            }
            else
            {
                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
            }

            userpsjson_O = new UserpsjsonUserPrintshopsJson(darrpsjson1.ToArray(), boolIsAdmin);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
