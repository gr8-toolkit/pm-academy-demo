using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.VidewModels;

namespace WebApp.Handlers
{
    public class GetContactsRequest : IRequest<IEnumerable<ContactVm>>
    {
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
