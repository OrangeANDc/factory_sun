using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using SunwaysFactoryProgram.DBModel;
using SunwaysFactoryProgram.Model;
using SupportProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.StaticSource
{


    public static class HttpApi
    {
        private static string normalApi = "https://www.sunways-portal.com/";
        public static string burninDataUrl = normalApi + "monitor/hbaseApi/inverter/getSingleAgeing";
        public static string configDataUrl = normalApi + "monitor/iotApi/deviceSetting/getConfigdataPO";

        private readonly static string _addOutBoundUrl = "http://192.168.30.95:8081/website/warranty/batchSaveInformation";
        private readonly static string _UpdateOutBoundUrl = "http://192.168.30.95:8081/website/warranty/updateProductWarranty";
        public static string HttpPostBurnData(string sn)
        {
            string startDate = (DateTime.Now).AddMinutes(-20.0).ToString("yyyy-MM-dd HH:mm:ss");
            string endDate = (DateTime.Now).AddMinutes(10.0).ToString("yyyy-MM-dd HH:mm:ss");
            int dataCount = 1;
            string queryString = "{\"inverterSN\":\"" + sn + "\",\"startTime\":\"" + startDate +
               "\",\"endTime\":\"" + endDate + "\",\"pageNum\":\"" + dataCount.ToString() + "\",\"pageSize\":\"" + dataCount.ToString() +"\"}";
            /*string queryString = "{\"sn\":\"" + sn + "\",\"query_field\":\"" + ResourceName.BurnNameList + "\",\"start_date\":\"" + startDate +
                "\",\"end_date\":\"" + endDate + "\",\"count\":" + dataCount.ToString() + ",\"desc\":\"true\"}";*/
            try
            {
                var options = new RestClientOptions(burninDataUrl);
                var client = new RestClient(options);
                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("Authorization", "1qaz@WSX3edc");
                request.AddBody(queryString);

                RestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.Content; 
                }
                else
                    return "";
                
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return string.Empty;
            }

        }

        public static string HttpPostAllData(string sn, DateTime endDate, int hour)
        {
            DateTime dateTime = endDate.AddHours((double)-hour);
            int dataCount = hour * 60 * 2;
            string queryString = GetQueryString(sn, dateTime.ToString("yyy-MM-dd HH:mm:ss"), endDate.ToString("yyy-MM-dd HH:mm:ss"), dataCount);
            try
            {
                var options = new RestClientOptions(burninDataUrl);
                var client = new RestClient(options);
                var request = new RestRequest("", Method.Post);


                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "1qaz@WSX3edc");
                request.AddBody(queryString);

                RestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.Content;
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return string.Empty;
            }

        }

        public static string HttpGetConfigData(string sn)
        {
            try
            {
                var options = new RestClientOptions(configDataUrl + "?sn=" + sn);
                var client = new RestClient(options);
                var request = new RestRequest();


                request.AddHeader("Content-Type", "application/json");

                RestResponse response = client.Execute(request);
                if (response.Content == null)
                    return "";
                return response.Content;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return string.Empty;
            }

        }


        private static string GetQueryString(string sn, string startDate, string endDate, int dataCount)
        {
            string queryString = "{\"inverterSN\":\"" + sn + "\",\"startTime\":\"" + startDate +
              "\",\"endTime\":\"" + endDate + "\",\"pageNum\":\"" + "1" + "\",\"pageSize\":\"" + dataCount.ToString() + "\"}";
            return queryString;
        }



        //
        public static string HttpPostAddOutBound(List<ST_OutBound>  outBounds)
        {

            try
            {
                var options = new RestClientOptions(_addOutBoundUrl);
                var client = new RestClient(options);
                var request = new RestRequest("", Method.Post);

                var serializerSettings  =new JsonSerializerSettings() 
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateFormatString = "yyyy-MM-dd HH:mm:ss"
                };

                request.AddHeader("Content-Type", "application/json");
                var json = JsonConvert.SerializeObject(outBounds, serializerSettings);
                request.AddJsonBody(json);

                RestResponse response = client.Execute(request);
                if (response.Content == null)
                    return "";
                return response.Content;
            }
            catch (Exception ex)
            {
                
                Log.Error(ex.Message);
                return string.Empty;
            }
        }

        public static string HttpGetUpdateOutBound(string InverterSN = "", string OrderID = "", string PalletNum ="")
        {

            try
            {
                string body = $"?InverterSN={InverterSN}&OrderID={OrderID}&PalletNum={PalletNum}";
                var options = new RestClientOptions(_UpdateOutBoundUrl + body);

                var client = new RestClient(options);
                var request = new RestRequest();


                request.AddHeader("Content-Type", "application/json");

                RestResponse response = client.Execute(request);
                if (response.Content == null)
                    return "";
                return response.Content;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return string.Empty;
            }
        }
    }

}
