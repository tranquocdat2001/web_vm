using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class EnumHelper
    {
        private static EnumHelper _instance;
        private static readonly object ObjectLock = new object();

        protected EnumHelper() { }

        public static EnumHelper Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (ObjectLock)
                    {
                        if (null == _instance)
                        {
                            _instance = new EnumHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        [Serializable]
        public class Enums
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public IEnumerable<Enums> ConvertEnumToList<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                return null;
            }
            List<Enums> lstEnum = new List<Enums>();
            List<T> lstBusinessType = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            foreach (var bussinessType in lstBusinessType)
            {
                Enums enumModel = new Enums
                {
                    Id = (int)Enum.Parse(typeof(T), bussinessType.ToString(), true),
                    Name = StringUtils.GetEnumDescription((Enum)Enum.Parse(typeof(T), bussinessType.ToString(), true))
                };
                lstEnum.Add(enumModel);
            }

            return lstEnum;
        }
    }
}
