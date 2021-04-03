using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FromLocalsToLocals.Utilities.Helpers
{
    public static class MapMethods
    {
        public static async Task<Tuple<double, double>> ConvertAddressToLocationAsync(string address)
        {
            Console.WriteLine(address);
            try
            {
                var uri = Config.Host + "forward?access_key=" + Config.MapApiKey + "&query=" + address +
                          "&limit=1&output=json";

                var client = new HttpClient();
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                var o = JObject.Parse(responseBody);

                var data = Tuple.Create((double) o.SelectToken("data[0].latitude"),
                    (double) o.SelectToken("data[0].longitude"));

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