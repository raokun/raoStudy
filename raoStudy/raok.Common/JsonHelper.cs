using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Raok.Common {
    public static class JsonHelper {
        public readonly static JavaScriptEncoder Encoder=JavaScriptEncoder.Create(UnicodeRanges.All);
        //  https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonnamingpolicy?view=net-7.0
        public static JsonSerializerOptions CreateJsonSerializerOptions(bool sameCase=false) {
            JsonSerializerOptions options = new JsonSerializerOptions() { Encoder= Encoder };
            if (sameCase) {
                options.DictionaryKeyPolicy=JsonNamingPolicy.CamelCase;
                options.PropertyNamingPolicy=JsonNamingPolicy.CamelCase;
            }
            options.Converters.Add(new DateTimeJsonConverter());
            return options;
        }
        public static string ToJson(this object obj) {
            JsonSerializerOptions options= CreateJsonSerializerOptions(false);
            return JsonSerializer.Serialize(obj,obj.GetType(),options);
        }

        public static T? ParseJson<T>(this string json) {
            if (string.IsNullOrWhiteSpace(json)) {
                return default(T?);
            }
            var opt=CreateJsonSerializerOptions();
            return JsonSerializer.Deserialize<T>(json, opt);
        }

    }
}
