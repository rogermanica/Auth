using System;
using AuthApi.Util;

namespace AuthApi.Requests
{
    public class RemoveAccount : Request<Response>
    {
        public int Id { get; }

        public RemoveAccount(int id)
        {
            Id = id;
        }
    }
}