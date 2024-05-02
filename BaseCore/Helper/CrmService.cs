using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Helper
{
    public interface ICrmService
    {
        Task<KipaaResultApi> KipaToken(long price, int month, string order);
        Task ConfirmKipaToken(string payment_token, string reciept_number);
        Task<KipaaResultApi> ConfirmKipaToken2(string payment_token, string reciept_number);
        Task UpdateUserAsync(user entity, bool add = false);
        Task<List<user>> GetUserUserAsync(string name);
        Task<string> Users(string name);
    }
    public class CrmService : ICrmService
    {
        private HttpClient _httpClient;
        private HttpClient _httpClient1;
        const string urlcrm = "http://zookaar.ir";
        const string accountuser = "admin" + ":" + "fazel9450";
        public CrmService(HttpClient httpClient, HttpClient httpClient1)
        {
            _httpClient = httpClient;
            _httpClient1 = httpClient1;
        }

        #region Kippa
        public async Task<KipaaResultApi> KipaToken(long price, int month, string order)
        {
            var obj = new
            {
                amount = price * 10,
                // callback_url= "https://localhost:3200/home/ConfirmKipaa?OrderId=" + order,
                callback_url = "https://zookaar.com/home/ConfirmKipaa?OrderId=" + order,
                debt = new int[] { month },
                min_credit = price * 10,
            };


            var data = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            var client1 = "https://api.kipaa.ir/ipg/v1/supplier/request_payment_token";
            var request1 = new HttpRequestMessage(HttpMethod.Post, client1);
            request1.Content = data;
            request1.Headers.Add("cache-control", "no-cache");
            request1.Headers.Add("Authorization", "Bearer 48419fb2-6b2a-4c07-b899-b37d9bed32a3");
            using var requestRun1 = await _httpClient1.SendAsync(request1);
            var result1 = await requestRun1.Content.ReadAsStringAsync();
            var result = await requestRun1.Content.ReadFromJsonAsync<KipaaResultApi>();
            return result;
        }

        public async Task ConfirmKipaToken(string payment_token, string reciept_number)
        {

            var body = @"{
                             " + "\n" +
                                         @"   ""payment_token"": """ + payment_token + @""",
                             " + "\n" +
                                         @"   ""reciept_number"":""" + reciept_number + @"""
                             " + "\n" +
            @"}";


            /////verify////////
            var data = new StringContent(body, Encoding.UTF8, "application/json");

            var client1 = "https://api.kipaa.ir/ipg/v1/supplier/verify_transaction";
            var request1 = new HttpRequestMessage(HttpMethod.Post, client1);
            request1.Content = data;
            request1.Headers.Add("cache-control", "no-cache");
            request1.Headers.Add("Authorization", "Bearer 48419fb2-6b2a-4c07-b899-b37d9bed32a3");
            using var requestRun1 = await _httpClient.SendAsync(request1);
            var requestRun1ss = await requestRun1.Content.ReadAsStringAsync();


        }

        public async Task<KipaaResultApi> ConfirmKipaToken2(string payment_token, string reciept_number)
        {
            var body = @"{
                             " + "\n" +
                                                  @"   ""payment_token"": """ + payment_token + @""",
                             " + "\n" +
                                                  @"   ""reciept_number"":""" + reciept_number + @"""
                             " + "\n" +
                     @"}";


            /////verify////////
            var data = new StringContent(body, Encoding.UTF8, "application/json");

            var client1 = "https://api.kipaa.ir/ipg/v1/supplier/confirm_transaction";
            var request1 = new HttpRequestMessage(HttpMethod.Post, client1);
            request1.Content = data;
            request1.Headers.Add("cache-control", "no-cache");
            request1.Headers.Add("Authorization", "Bearer 48419fb2-6b2a-4c07-b899-b37d9bed32a3");
            using var requestRun = await _httpClient.SendAsync(request1);
            var requestRun1zss = await requestRun.Content.ReadAsStringAsync();

            var result = await requestRun.Content.ReadFromJsonAsync<KipaaResultApi>();
            return result;
        }

        #endregion

        public async Task UpdateUserAsync(user entity, bool add)
        {
            var byteArray = Encoding.ASCII.GetBytes(accountuser);



            if (!add)
            {
                if (string.IsNullOrEmpty(entity.sales_stage))
                {
                    entity.sales_stage = "تماس برقرار نشده";
                 
                }
                var client1 = urlcrm + "/modules/ParsVT/ws/API/V2/vtiger/default/update";
                var obj = new PotentialsCrm() { element = entity, elementType = "Potentials" };
                var data = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                var request1 = new HttpRequestMessage(HttpMethod.Post, client1);
                request1.Headers.Add("cache-control", "no-cache");
                request1.Headers.Add("authorization", "Basic " + Convert.ToBase64String(byteArray)); request1.Headers.Add("elementType", "Potentials");
                request1.Content = data;
                using var requestRun1 = await _httpClient.SendAsync(request1);

            }
            else
            {

                if (string.IsNullOrEmpty(entity.sales_stage))
                {
                    entity.sales_stage = "تماس برقرار نشده";

                }
                
                var obj = new PotentialsCrm() { element = entity, elementType = "Potentials" };
                var data = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                var client1 = urlcrm + "/modules/ParsVT/ws/API/V2/vtiger/default/create";
                var request1 = new HttpRequestMessage(HttpMethod.Post, client1);
                request1.Content = data;
                request1.Headers.Add("cache-control", "no-cache");
                request1.Headers.Add("authorization", "Basic " + Convert.ToBase64String(byteArray));
                using var requestRun1 = await _httpClient.SendAsync(request1);
                var sssss = await requestRun1.Content.ReadAsStringAsync();
            }


        }

        public async Task<string> Accounts()
        {
            var byteArray = Encoding.ASCII.GetBytes(accountuser);
            var office = "ندارد";
            var officecrmid = "11x5982";
            var officelist = new List<OfficeCrm>();
            var client1 = urlcrm + "/modules/ParsVT/ws/API/V2/vtiger/default/query?query=Select * from Accounts;";

            var request1 = new HttpRequestMessage(HttpMethod.Post, client1);
            request1.Headers.Add("cache-control", "no-cache");
            request1.Headers.Add("content-type", "application/json");
            request1.Headers.Add("authorization", "Basic " + Convert.ToBase64String(byteArray));
            request1.Headers.Add("elementType", "Potentials");
            using var requestRun1 = await _httpClient.SendAsync(request1);
            var response = await requestRun1.Content.ReadFromJsonAsync<List<OfficeCrm>>();
            officelist = response;

            if (officelist.Count > 0)
            {
                var getoff = officelist.Where(z => z.accountname == office.Trim()).ToList();
                if (getoff.Count() != 0)
                {
                    officecrmid = officelist.Where(z => z.accountname == office.Trim()).FirstOrDefault().id;

                }
            }

            return officecrmid;

        }



        public async Task<string> Users(string name)
        {
            var byteArray = Encoding.ASCII.GetBytes(accountuser);
            var officecrmid = "19x1";
            var officelist = new List<UserCrm>();
            var client1 = urlcrm + "/modules/ParsVT/ws/API/V2/vtiger/default/query?query=Select * from Users where last_name='" + name + "';";

            var request1 = new HttpRequestMessage(HttpMethod.Get, client1);
            request1.Headers.Add("cache-control", "no-cache");
            request1.Headers.Add("authorization", "Basic " + Convert.ToBase64String(byteArray));
            request1.Headers.Add("elementType", "Potentials");
            using var requestRun1 = await _httpClient.SendAsync(request1);
            var response1 = await requestRun1.Content.ReadAsStringAsync();
            var response = await requestRun1.Content.ReadFromJsonAsync<RestViewModelUserCrm>();
            officelist = response.result;

            if (officelist.Count > 0)
            {
                var getoff = officelist.Where(z => z.last_name == name.Trim()).ToList();
                if (getoff.Count() > 0)
                {
                    officecrmid = getoff.FirstOrDefault().id;

                }
            }

            return officecrmid;

        }


        public async Task<List<user>> GetUserUserAsync(string name)
        {
            var byteArray = Encoding.ASCII.GetBytes(accountuser);
            var sss = Convert.ToBase64String(byteArray);

            var client = urlcrm + "/modules/ParsVT/ws/API/V2/vtiger/default/query?query=Select * from Potentials where cf_875=" + name + ";";
            var request = new HttpRequestMessage(HttpMethod.Get, client);
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("authorization", "Basic " + Convert.ToBase64String(byteArray));
            using var requestRun = await _httpClient.SendAsync(request);
            var response = await requestRun.Content.ReadFromJsonAsync<RestViewModeluserCrm>();
            if (response.success)
            {
                return response.result;
            }
            return null; ;
        }


    }
    public class RestViewModelOfficeCrm
    {
        public bool success { get; set; }
        public List<OfficeCrm> result { get; set; }
    }
    public class RestViewModelUserCrm
    {
        public bool success { get; set; }
        public List<UserCrm> result { get; set; }
    }
    public class OfficeCrm
    {
        public string id { get; set; }
        public string accountname { get; set; }
    }
    public class UserCrm
    {
        public string id { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
    }

    public class RestViewModelProdcuteCrm
    {
        public bool success { get; set; }
        public List<ProdcuteCrm> result { get; set; }
    }
    public class ProdcuteCrm
    {
        public string id { get; set; }
        public string website { get; set; }
        public string productname { get; set; }
        public string cf_pcf_bcf_943 { get; set; }
        public string qtyinstock { get; set; }
        public string unit_price { get; set; }
        public string usageunit { get; set; }
        public string assigned_user_id { get; set; } = "19x75";
        public string vendor_id { get; set; } = "2x5873";
    }
    public class RestViewModelFactorCrm
    {
        public bool success { get; set; }
        public List<FactorCrm> result { get; set; }
    }
    public class FactorCrm
    {
        public string subject { get; set; }
        public string potential_id { get; set; }
        public string account_id { get; set; }
        public string contact_id { get; set; }
        public string bill_street { get; set; }
        public string bill_code { get; set; }
        public string bill_city { get; set; }
        public string bill_country { get; set; }
        public string ship_street { get; set; }
        public string ship_code { get; set; }
        public string ship_city { get; set; }
        public string ship_country { get; set; }
        public string taxtype { get; set; } = "individual";
        public string adjustment { get; set; }
        public string discount_amount_final { get; set; }
        public string assigned_user_id { get; set; } = "19x75";
        public string quotestage { get; set; } = "Created";
        public string invoicestatus { get; set; } = "Created";
        public string invoicedate { get; set; } = DateTime.Now.ToString();
        public string validtill { get; set; } = DateTime.Now.ToString();
        public List<items> items { get; set; }
    }
    public class items
    {
        public string hdnProductId { get; set; }
        public string qty { get; set; }
        public string listPrice { get; set; }

    }
    public class RestViewModeluseronlyCrm
    {
        public bool success { get; set; }
        public user result { get; set; }
    }
    public class RestViewModeluserCrm
    {
        public bool success { get; set; }
        public List<user> result { get; set; }
    }
    public class PotentialsCrm
    {
        public string elementType { get; set; }
        public user element { get; set; }
    }
    public class KipaaResultApi
    {
        public KipaaResultContent Content { get; set; }
        public bool Success { get; set; }

        public long CallingID { get; set; }
        public string Message { get; set; }
    }

    public class KipaaResultContent
    {
        public string payment_token { get; set; }
        public string payment_url { get; set; }

    }
    public class user
    {
        public string id { get; set; }

        /// <summary>
        /// name customer
        /// </summary>
        public string potentialname { get; set; }

        public string cf_875 { get; set; }//شماره مشتری
        public string cf_1243 { get; set; }//مبلغ کیپا


        /// <summary>
        /// Code Naitionnal
        /// </summary>
        public string cf_968 { get; set; }

        /// <summary>
        /// postal 
        /// </summary>
        public string cf_pcf_irp_1070 { get; set; }

        /// <summary>
        /// شماره سناسنامه
        /// </summary>
        public string cf_1111 { get; set; }
        /// <summary>
        /// province 
        /// ایران - تهران - تهران
        /// </summary>
        public string cf_1109 { get; set; }

        /// <summary>
        /// fathername
        /// </summary>
        public string cf_1199 { get; set; }
        /// <summary>
        /// تلفن
        /// </summary>
        public string cf_1203 { get; set; }
        /// <summary>
        /// ۱6- استان- انتخاب نشده
        /// </summary>
        public string cf_1211 { get; set; }
        /// <summary>
        /// ۱6- شهر- انتخاب نشده
        /// </summary>
        public string cf_1213 { get; set; }




        /// <summary>
        /// name product
        /// </summary>
        public string cf_918 { get; set; }


        /// <summary>
        /// ۲- مبلغ درخواستی تسهیلات خرید کالا-درج نشده
        /// </summary>
        public string cf_1207 { get; set; }



        /// <summary>
        /// 
        //	هزینه عملیات 
        /// </summary>
        public string cf_1137 { get; set; }
        /// <summary>
        /// 
        //	مبلغ پیش پرداخت 
        /// </summary>
        public string cf_1135 { get; set; }


        /// <summary>
        /// مبلغ نقدفاکتور  احتساب ارزش افزوده 
        /// </summary>
        public string amount { get; set; }

        /// <summary>
        /// مبلغ نقدفاکتور بدون احتساب ارزش افزوده 
        /// </summary>
        //public string cf_1231 { get; set; }

        public string cf_pcf_icf_1052 { get; set; }





        /// <summary>
        /// نحوه اشنایی
        /// </summary>

        public string cf_1229 { get; set; }

        /// <summary>
        /// گام
        /// </summary>
        public string cf_1133 { get; set; }
        /// <summary>
        /// ماه
        /// </summary>
        public string cf_1048 { get; set; }
        ///// <summary>
        ///// درخواست وام
        ///// </summary>
        //public string cf_1175 { get; set; }
        /// <summary>
        /// پرداخت شده
        /// </summary>
        public string cf_1241 { get; set; }

        /// <summary>
        /// /مرحله فروش =
        /// </summary>
        public string sales_stage { get; set; }

        /// <summary>
        /// لیزینگ
        /// </summary>
        public string cf_932 { get; set; }

        /// <summary>
        /// دسته محصول یا صنعت چندتایی 
        /// </summary>
        public string cf_904 { get; set; }

        /// <summary>
        /// نماینده
        /// </summary>
        public string cf_1197 { get; set; }


        public string assigned_user_id { get; set; } = "19x1";

    }
}
