using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApp.VidewModels;

namespace WebApp.Handlers
{
    public class GetContactsHandler
    {
        const int minLength = 4;
        const int maxLength = 50;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz 0123456789";

        public Task<IEnumerable<ContactVm>> Handle(GetContactsRequest request, CancellationToken cancellationToken)
        {
            var random = new Random();

            return Task.FromResult(Enumerable.Range(request.Skip, request.Limit).Select(v => new ContactVm
            {
                Id = v + 1000,
                FirstName = new string(Enumerable.Repeat(chars, random.Next(minLength, maxLength)).Select(s => s[random.Next(s.Length)]).ToArray()),
                LastName = new string(Enumerable.Repeat(chars, random.Next(minLength, maxLength)).Select(s => s[random.Next(s.Length)]).ToArray()),
            }));
        }
    }
}
