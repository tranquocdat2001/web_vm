using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Enums
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

        #region Product

        public enum ProductStatus
        {
            Hidden = 0,
            Active = 1,
            Deleted = 2
        }

        public enum OrderBy
        {
            Default = 0,
            PriceAsc = 1,
            PriceDesc = 2
        }

        public enum UnitType
        {
            Gam,
            Kg
        }

        #endregion

        #region Article

        public enum ArticleStatus
        {
            Hidden = 0,
            Active = 1,
            Deleted = 2
        }

        #endregion
    }
}
