using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesApp.Services
{
    public class TransientService : ITransientService
    {
        public int Value { get; set; }
    }
}
