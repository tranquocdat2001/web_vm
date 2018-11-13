using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Utilities.RequestHelper
{
    [DataContract]
    [Serializable]
    public class ResponseData
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Result { get; set; }
    }

    [DataContract]
    [Serializable]
    public class RequestData
    {
        [DataMember]
        public string Input { get; set; }
    }


    [DataContract]
    [Serializable]
    public class ResponseMultiData
    {
        public ResponseMultiData()
        {
            this.ResponseDatas = new List<ResponseData>();
        }

        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public List<ResponseData> ResponseDatas { get; set; } 
    }

    [DataContract]
    [Serializable]
    public class RequestMultiData
    {
        public List<RequestData> RequestDatas { get; set; }
    }
}