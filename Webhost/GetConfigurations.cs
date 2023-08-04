using NetCore.Helpers;
using Newtonsoft.Json.Linq;

namespace NetCore.WebHost
{
    public static class AppSettingsProvider
    {
        /// <summary>
        /// get encrypted file，convert to KeyValuePairs
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="needDecryptKeys">decrypt property</param>
        /// <returns>KeyValuePairs list</returns>
        public static List<KeyValuePair<string, string>> GetConfigurations(string path, IEnumerable<string> needDecryptKeys)
        {
            IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile(path, optional: true, reloadOnChange: true)
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .Build();
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();


            foreach (var item in config.AsEnumerable())
            {
                if (string.IsNullOrEmpty(item.Value))
                    continue;

                if (!needDecryptKeys.Contains(item.Key))
                {
                    result.Add(new KeyValuePair<string, string>(item.Key, item.Value));
                }
                else
                {
                    var decStr = AES256.DecryptString(item.Value);
                    var newJson = JObject.Parse(decStr);
                    foreach (var prop in newJson.Properties())
                    {

                        if (newJson.Property(prop.Name) != null)
                        {
                            if (prop.Name == item.Key)
                                result.Add(new KeyValuePair<string, string>(item.Key, prop.Value.ToString()));
                            else
                                result.Add(new KeyValuePair<string, string>($"{item.Key}:{prop.Name}", prop.Value.ToString()));
                        }
                    }
                }
            }

            return result;
        }
    }
}

