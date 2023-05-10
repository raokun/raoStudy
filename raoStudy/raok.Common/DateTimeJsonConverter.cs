using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Raok.Common {
    public class DateTimeJsonConverter : JsonConverter<DateTime> {
        public readonly string _dateFormatString;
        public DateTimeJsonConverter() {
            _dateFormatString = "yyyy-MM-dd HH:mm:ss";
        }
        public DateTimeJsonConverter(string dateFromString) {
            _dateFormatString = dateFromString;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            string? date = reader.GetString();
            if (date == null) { return default(DateTime); }
            return DateTime.Parse(date);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) {
            //固定用服务器所在的时区，前端如果想适应用户的时区，请自己调整
            writer.WriteStringValue(value.ToString(_dateFormatString));
        }
    }
}
