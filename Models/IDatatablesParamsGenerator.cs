using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IDatatablesParamsGenerator
    {
        DatatablesParams GetParams(HttpRequest request);
    }
}
