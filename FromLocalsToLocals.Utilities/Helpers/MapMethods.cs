using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FromLocalsToLocals.Utilities.Helpers
{
    public static class MapMethods
    {
        public static async Task<Tuple<double,double>> ConvertAddressToLocationAsync(string address)
        {
            Console.WriteLine(address);
            try
            {
                var uri = Config.Host + "/maps/api/geocode/json?address=" + address + "&language=lt&key=" +
                          Config.Google_Api_Key;

                var client = new HttpClient();
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                var o = JObject.Parse(responseBody);

                var data = Tuple.Create((double) o.SelectToken("results[0].geometry.location.lat") , (double) o.SelectToken("results[0].geometry.location.lng"));

                return data;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return null;
        }
    }

}