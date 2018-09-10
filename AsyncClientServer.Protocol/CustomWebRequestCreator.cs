using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AsyncClientServer.Protocol
{
    public class CustomWebRequestCreator : WebRequest, IWebRequestCreate
    {
        public CustomWebRequestCreator()
        {
        }

        public CustomWebRequestCreator(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }


        public override RequestCachePolicy CachePolicy { get => base.CachePolicy; set => base.CachePolicy = value; }
        public override string Method { get => base.Method; set => base.Method = value; }

        public override Uri RequestUri => base.RequestUri;

        public override string ConnectionGroupName { get => base.ConnectionGroupName; set => base.ConnectionGroupName = value; }
        public override WebHeaderCollection Headers { get => base.Headers; set => base.Headers = value; }
        public override long ContentLength { get => base.ContentLength; set => base.ContentLength = value; }
        public override string ContentType { get => base.ContentType; set => base.ContentType = value; }
        public override ICredentials Credentials { get => base.Credentials; set => base.Credentials = value; }
        public override bool UseDefaultCredentials { get => base.UseDefaultCredentials; set => base.UseDefaultCredentials = value; }
        public override IWebProxy Proxy { get => base.Proxy; set => base.Proxy = value; }
        public override bool PreAuthenticate { get => base.PreAuthenticate; set => base.PreAuthenticate = value; }
        public override int Timeout { get => base.Timeout; set => base.Timeout = value; }

        public override void Abort()
        {
            base.Abort();
        }

        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            return base.BeginGetRequestStream(callback, state);
        }

        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            return base.BeginGetResponse(callback, state);
        }

        public new WebRequest Create(Uri uri)
        {
            return new CustomWebRequestCreator(new SerializationInfo(typeof(CustomWebRequestCreator), new FormatterConverter()), new StreamingContext(StreamingContextStates.All, uri));
        }

        public override ObjRef CreateObjRef(Type requestedType)
        {
            return base.CreateObjRef(requestedType);
        }

        public override Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return base.EndGetRequestStream(asyncResult);
        }

        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            return base.EndGetResponse(asyncResult);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override Stream GetRequestStream()
        {
            return base.GetRequestStream();
        }

        public override Task<Stream> GetRequestStreamAsync()
        {
            return base.GetRequestStreamAsync();
        }

        public override WebResponse GetResponse()
        {
            return base.GetResponse();
        }

        public override Task<WebResponse> GetResponseAsync()
        {
            return base.GetResponseAsync();
        }

        public override object InitializeLifetimeService()
        {
            return base.InitializeLifetimeService();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            base.GetObjectData(serializationInfo, streamingContext);
        }
    }
}
