using System.ComponentModel;

namespace BO
{
    public class CachedEnum
    {
        public enum CachedTypes
        {
            /// <summary>
            /// NoCache = 0
            /// </summary>
            NoCache = 0,

            /// <summary>
            /// Redis = 1
            /// </summary>
            Redis = 1,

            /// <summary>
            /// IIS = 2
            /// </summary>
            IIS = 2,

            /// <summary>
            /// Memcached = 3
            /// </summary>
            Memcached = 3
        }

        public enum ExpiredDurationInMinute
        {
            /// <summary>
            /// 120 phút
            /// </summary>
            [Description("LongCached")]
            LongCached = 120,
            /// <summary>
            /// 60 phút
            /// </summary>
            [Description("MediumCached")]
            MediumCached = 60,
            /// <summary>
            /// 30 phút
            /// </summary>
            [Description("ShortCached")]
            ShortCached = 30
        }
    }
}
