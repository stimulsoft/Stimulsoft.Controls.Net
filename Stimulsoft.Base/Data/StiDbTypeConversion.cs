using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stimulsoft.Base
{
    public partial class StiDbTypeConversion
    {
        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public static Type GetNetType(System.Data.DbType type)
        {
            switch (type)
            {
                case System.Data.DbType.SByte:
                    return typeof (SByte);

                case System.Data.DbType.Int16:
                    return typeof (Int16);

                case System.Data.DbType.Int32:
                    return typeof (Int32);

                case System.Data.DbType.Int64:
                    return typeof (Int64);

                case System.Data.DbType.Byte:
                    return typeof (Byte);

                case System.Data.DbType.UInt16:
                    return typeof (UInt16);

                case System.Data.DbType.UInt32:
                    return typeof (UInt32);

                case System.Data.DbType.UInt64:
                    return typeof (UInt64);

                case System.Data.DbType.Single:
                    return typeof (Single);

                case System.Data.DbType.Double:
                    return typeof (Double);

                case System.Data.DbType.Decimal:
                case System.Data.DbType.Currency:
                    return typeof (Decimal);

                case System.Data.DbType.Guid:
                    return typeof (Guid);

                case System.Data.DbType.Date:
                case System.Data.DbType.DateTime:
                case System.Data.DbType.DateTime2:
                case System.Data.DbType.Time:
                    return typeof (DateTime);

                case System.Data.DbType.DateTimeOffset:
                    return typeof (DateTimeOffset);

                case System.Data.DbType.Boolean:
                    return typeof (Boolean);

                case System.Data.DbType.Binary:
                    return typeof (byte[]);

                case System.Data.DbType.String:
                case System.Data.DbType.StringFixedLength:
                    return typeof (String);

                default:
                    return typeof (string);
            }
        }
    }
}
