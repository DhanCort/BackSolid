using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using Odyssey2Backend.JsonTypes;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TowaStandard;

namespace Odyssey2Backend.PrintShop
{
    public static class HttpTools<Tjson> where Tjson : TjsonTJson
    {
        //-------------------------------------------------------------------------------------------------------------
        /*STATIC VARIABLES*/

        static HttpClient client = new HttpClient();

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task PostAsyncToEndPoint(
            string data,
            string strURL
            )
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(strURL, content);

            HttpStatusCode status = response.StatusCode;
            
            String strResponse = await response.Content.ReadAsStringAsync();
            
        }

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task<List<Tjson>> GetListAsyncToEndPoint(
            string strURL
            )
        {
            HttpResponseMessage response = new HttpResponseMessage();
            List<Tjson> result = new List<Tjson>();
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync(strURL);
               
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<List<Tjson>>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task<List<Tjson>> GetListAsyncWithBodyToEndPoint(
            object obj,
            string strURL
            )
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(strURL),
                Content = new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json")
            };

            List<Tjson> result = new List<Tjson>();
            try
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (
                    response.IsSuccessStatusCode
                    )
                {
                    result = await response.Content.ReadAsAsync<List<Tjson>>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //Returns null if there is no access to the service.
                result = default;
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static async Task<Tjson> GetOneAsyncToEndPoint(
            string strURL
            )
        {
            HttpResponseMessage response = new HttpResponseMessage();
            Tjson result = default(Tjson);
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync(strURL);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<Tjson>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = default;
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static async Task<String> GetStringAsyncToEndPoint(
            string strURL
            )
        {
            HttpResponseMessage response = new HttpResponseMessage();
            String result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync(strURL);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<String>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = default;
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static async Task<bool?> GetBoolAsyncToEndPoint(
            string strURL
            )
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool? result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync(strURL);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<bool>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = default;
            }
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task<List<Tjson>> GetListAsyncPrintshopOrders(
            string strURL,
            JobidsjsonJobIdsJson jobidsjson
            )
        {
            HttpResponseMessage response = new HttpResponseMessage();
            List<Tjson> result = new List<Tjson>();
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(strURL),
                    Content = new StringContent(JsonSerializer.Serialize(jobidsjson), Encoding.UTF8,
                        "application/json")
                };

                response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<List<Tjson>>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task<String> PostAddCustomer(
            String strURL_I,
            int intPrintshopId_I,
            String strFirstName_I,
            String strLastName_I,
            String strEmail_I,
            String strPassword_I
            )
        {
            //                                              //Info to send to wisnet.
            Object objNewCustomer = new 
            {
                intPrintshopId = intPrintshopId_I,
                strFirstName = strFirstName_I,
                strLastName = strLastName_I,
                strEmail = strEmail_I,
                strPassword = strPassword_I
            };

            String result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonSerializer.Serialize(objNewCustomer),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(strURL_I, content);

                if (
                    response.IsSuccessStatusCode
                    )
                {
                    result = (await response.Content.ReadAsAsync<int>()).ToString();
                }
                else
                {
                    result = await response.Content.ReadAsAsync<String>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }

            return result;
        }

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task<String> Task_PostJobEmailToPrintbuyer(
            string strURL,
            JobemailtopbjsoninJobEmailToPrintbuyerJsonInternal jobemailtopbjsonin,
            String strWisnetToken_I
            )
        {
            String result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", strWisnetToken_I);

                HttpContent content = new StringContent(JsonSerializer.Serialize(jobemailtopbjsonin),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(strURL, content);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }

            return result;
        }

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task<String> Task_PostOrderEmailToPrintbuyer(
            string strURL_I,
            OrderemailtopbjsoninOrderEmailToPrintbuyerJsonInternal orderemailtopbjsonin_I,
            String strWisnetToken_I
            )
        {
            String result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", strWisnetToken_I);

                HttpContent content = new StringContent(JsonSerializer.Serialize(orderemailtopbjsonin_I),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(strURL_I, content);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }

            return result;
        }

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task<String> PostEstimatePricesAsyncToEndPoint(
            string strURL,
            SndestjsoninSendEstimateJsonInternal sndestjsoninSendEstimate,
            IConfiguration configuration
            )
        {
            String result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonSerializer.Serialize(sndestjsoninSendEstimate),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(strURL, content);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task<String> PostJobPriceAsyncToEndPoint(
            string strURL,
            //SndestjsoninSendEstimateJsonInternal sndestjsoninSendEstimate,
            int intJobId_I,
            PsPrintShop ps_I,
            int intContactId_I,
            int intOrderId_I,
            double numJobPrice_I
            //IConfiguration configuration
            )
        {
            String result = null;
            try
            {
                //                                          //Model to send the job's price to wisnet.
                Object objJobPrice = new
                {
                    intJobId = intJobId_I,
                    strPrintshopId = ps_I.strPrintshopId,
                    intContactId = intContactId_I,
                    intOrderId = intOrderId_I,
                    numJobPrice = numJobPrice_I
                };

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonSerializer.Serialize(objJobPrice),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(strURL, content);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task<int> PostAddEstimateAsyncToEndPoint(
            int intContactId_I,
            int intQuantity_I,
            string strURL,
            String strPrintshopId_I,
            String strName_I
            )
        {
            int result = 0;
            try
            {
                Object objPrintshop = new
                {
                    intPrintshopId = strPrintshopId_I,
                    strName = strName_I,
                    intContactId = intContactId_I,
                    intQuantity = intQuantity_I
                };

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonSerializer.Serialize(objPrintshop),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(strURL, content);

                if (response.IsSuccessStatusCode)
                {
                    String strJobID = await response.Content.ReadAsStringAsync();
                    if (
                        strJobID.IsParsableToInt()
                        )
                    {
                        result = strJobID.ParseToInt();
                    }
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = 0;
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static async Task<String> PostEstimateToOrderAsyncToEndPoint(
        string strURL,
        ToorderjsoninToOrderJsonInternal toorderjson_I
        )
        {
            String result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonSerializer.Serialize(toorderjson_I),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(strURL, content);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static async Task<String> PostEstimateToRejectedAsyncToEndPoint(
        string strURL,
        SendtowisjsoninSendToWisnetJsonInternal torejectedjson_I,
        IConfiguration configuration
        )
        {
            String result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonSerializer.Serialize(torejectedjson_I),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(strURL, content);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static async Task<String> PostUpdateJobInProgressToCompleteOnWisnetAsyncToEndPoint(
            //                                              //Update Job to Completed on wisnet.

            string strURL,
            SendtowisjsoninSendToWisnetJsonInternal torejectedjson_I,
            IConfiguration configuration
            )
        {
            String result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonSerializer.Serialize(torejectedjson_I),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(strURL, content);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task<DeljobsjsonDeletedJobsJson> GetListAsyncJobsDeleted(
            string strURL,
            int[] darrintJobId
            )
        {
            HttpResponseMessage response = new HttpResponseMessage();
            DeljobsjsonDeletedJobsJson result = new DeljobsjsonDeletedJobsJson();
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(strURL),
                    Content = new StringContent(JsonSerializer.Serialize(darrintJobId), Encoding.UTF8,
                        "application/json")
                };

                response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    int[] resultT = await response.Content.ReadAsAsync<int[]>();
                    result.darrintJobsDeleted = resultT;
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //Returns null if there is no access to the service
                result = null;
            }
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task<DeljobsjsonDeletedJobsJson> GetListAsyncEstimatesNotRequestedStage(
            string strURL,
            String strPrintshopId_I,
            int[] darrintJobId
            )
        {
            //                                              //Info to send.
            Object objInfo = new
            {
                strPrintshopId = strPrintshopId_I,
                arrintJobsIds = darrintJobId
            };

            HttpResponseMessage response = new HttpResponseMessage();
            DeljobsjsonDeletedJobsJson result = new DeljobsjsonDeletedJobsJson();
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(strURL),
                    Content = new StringContent(JsonSerializer.Serialize(objInfo), Encoding.UTF8,
                        "application/json")
                };

                response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<DeljobsjsonDeletedJobsJson>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static async Task<String> PostUpdateInvoiceToPaidStage(
        String strURL,
        String strPrintshopId_I,
        int intOrderId_I
        )
        {
            String result = null;
            try
            {
                //                                          //To send info to wisnet.
                Object objInvoice = new
                {
                    strPrintshopId = strPrintshopId_I,
                    intOrderId = intOrderId_I
                };

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonSerializer.Serialize(objInvoice),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(strURL, content);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static async Task<String[]> GetWorkInProgressSubStages(
            string strURL
            )
        {
            HttpResponseMessage response = new HttpResponseMessage();
            String[] result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync(strURL);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<String[]>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = default;
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static async Task<String> GetJobSubStage(
            string strURL_I
            )
        {
            HttpResponseMessage response = new HttpResponseMessage();
            String result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync(strURL_I);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<String>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = default;
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static async Task<String> PostUpdateWorkInProgressStatus(
            String strURL_I,
            int intPrintshopId_I,
            int intJobId_I,
            String strStatus_I,
            int intContactId_I
            )
        {
            String result = null;
            try
            {
                //                                          //To send info to wisnet.
                Object objUpdateStage = new
                {
                    intPrintshopId = intPrintshopId_I,
                    intJobId = intJobId_I,
                    strStatus = strStatus_I,
                    intContactId = intContactId_I
                };

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonSerializer.Serialize(objUpdateStage), Encoding.UTF8, 
                    "application/json");

                HttpResponseMessage response = await client.PostAsync(strURL_I, content);
                if (
                    response.IsSuccessStatusCode
                    )
                {
                    result = "200|" + await response.Content.ReadAsAsync<String>();
                }
                else
                {
                    result = "400|" + await response.Content.ReadAsAsync<String>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //Returns null if there is no access to the service.
                result = null;
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static async Task<double?> GetJobWisnetPrice(
            string strURL_I
            )
        {
            HttpResponseMessage response = new HttpResponseMessage();
            double? result = -1;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync(strURL_I);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<double?>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = -1;
            }
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------- 
        public static async Task<bool?> boolOrderCompleted(

            string strURL_I
            )
        {
            //                                  
            HttpResponseMessage response = new HttpResponseMessage();
            bool? result = null;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = await client.GetAsync(strURL_I);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<bool?>();
                }
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                //                                          //returns null if there is no access to the service
                result = null;
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
    }
}
