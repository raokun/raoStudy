using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdService.DomainService {
    public static class IdentityHelper {
        public static string ShowErrors(this IEnumerable<IdentityError> errors) {
            var str = errors.Select(m => $"code={m.Code},message={m.Description}");
            return string.Join("/n", str);
        }
    }
}
