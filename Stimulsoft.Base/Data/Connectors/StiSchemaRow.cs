#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports  											}
{																	}
{	Copyright (C) 2003-2016 Stimulsoft     							}
{	ALL RIGHTS RESERVED												}
{																	}
{	The entire contents of this file is protected by U.S. and		}
{	International Copyright Laws. Unauthorized reproduction,		}
{	reverse-engineering, and distribution of all or any portion of	}
{	the code contained in this file is strictly prohibited and may	}
{	result in severe civil and criminal penalties and will be		}
{	prosecuted to the maximum extent possible under the law.		}
{																	}
{	RESTRICTIONS													}
{																	}
{	THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES			}
{	ARE CONFIDENTIAL AND PROPRIETARY								}
{	TRADE SECRETS OF STIMULSOFT										}
{																	}
{	CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON		}
{	ADDITIONAL RESTRICTIONS.										}
{																	}
{*******************************************************************}
*/
#endregion Copyright (C) 2003-2016 Stimulsoft

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public class StiSchemaRow
    {
        #region Properties
        public string COLUMN_DATA_TYPE
        {
            get
            {
                return row["COLUMN_DATA_TYPE"] as string;
            }
        }

        public string COLUMN_NAME
        {
            get
            {
                return row["COLUMN_NAME"] as string;
            }
        }

        public string COLUMNNAME
        {
            get
            {
                return row["COLUMNNAME"] as string;
            }
        }

        public string COLUMN_TYPE
        {
            get
            {
                return row["COLUMN_TYPE"] as string;
            }
        }

        public string CONSTRAINT_NAME
        {
            get
            {
                return row["CONSTRAINT_NAME"] as string;
            }
        }

        public string DATA_TYPE
        {
            get
            {
                return row["DATA_TYPE"] as string;
            }
        }

        public int DATA_TYPE_INT
        {
            get
            {
                return row["DATA_TYPE"] is int ? (int)row["DATA_TYPE"] : 0;
            }
        }

        public string DATATYPE
        {
            get
            {
                return row["DATATYPE"] as string;
            }
        }

        public string FIELD_NAME
        {
            get
            {
                return row["FIELD_NAME"] as string;
            }
        }

        public string FK_COLUMN_NAME
        {
            get
            {
                return row["FK_COLUMN_NAME"] as string;
            }
        }

        public string FK_CONSTRAINT_NAME
        {
            get
            {
                return row["FK_CONSTRAINT_NAME"] as string;
            }
        }

        public string FK_NAME
        {
            get
            {
                return row["FK_NAME"] as string;
            }
        }

        public string FK_TABLE_NAME
        {
            get
            {
                return row["FK_TABLE_NAME"] as string;
            }
        }

        public string FOREIGN_COLUMN_NAME
        {
            get
            {
                return row["FOREIGN_COLUMN_NAME"] as string;
            }
        }

        public string FOREIGN_TABLE_NAME
        {
            get
            {
                return row["FOREIGN_TABLE_NAME"] as string;
            }
        }

        public string NAME
        {
            get
            {
                return row["NAME"] as string;
            }
        }

        public string OBJECT_NAME
        {
            get
            {
                return row["OBJECT_NAME"] as string;
            }
        }

        public string OWNER
        {
            get
            {
                return row["OWNER"] as string;
            }
        }

        public string PARAMETER_DIRECTION
        {
            get
            {
                return row["PARAMETER_DIRECTION"] as string;
            }
        }

        public string PARAMETER_DATA_TYPE
        {
            get
            {
                return row["PARAMETER_DATA_TYPE"] as string;
            }
        }

        public string PARAMETER_NAME
        {
            get
            {
                return row["PARAMETER_NAME"] as string;
            }
        }

        public string PK_COLUMN_NAME
        {
            get
            {
                return row["PK_COLUMN_NAME"] as string;
            }
        }

        public string PK_TABLE_NAME
        {
            get
            {
                return row["PK_TABLE_NAME"] as string;
            }
        }

        public string PROCEDURE_CAT
        {
            get
            {
                return row["PROCEDURE_CAT"] as string;
            }
        }

        public string PROCEDURE_NAME
        {
            get
            {
                return row["PROCEDURE_NAME"] as string;
            }
        }

        public string PROCEDURE_SCHEMA
        {
            get
            {
                return row["PROCEDURE_SCHEMA"] as string;
            }
        }

        public string PROCEDURE_SCHEM
        {
            get
            {
                return row["PROCEDURE_SCHEM"] as string;
            }
        }

        public string R_OWNER
        {
            get
            {
                return row["R_OWNER"] as string;
            }
        }

        public string R_PK
        {
            get
            {
                return row["R_PK"] as string;
            }
        }

        public string R_TABLE_NAME
        {
            get
            {
                return row["R_TABLE_NAME"] as string;
            }
        }

        public string REFERENCED_COLUMN_NAME
        {
            get
            {
                return row["REFERENCED_COLUMN_NAME"] as string;
            }
        }

        public string REFERENCED_TABLE_NAME
        {
            get
            {
                return row["REFERENCED_TABLE_NAME"] as string;
            }
        }

        public string REFERENCES_TABLE
        {
            get
            {
                return row["REFERENCES_TABLE"] as string;
            }
        }

        public string REFERENCES_FIELD
        {
            get
            {
                return row["REFERENCES_FIELD"] as string;
            }
        }

        public string ROUTINE_CATALOG
        {
            get
            {
                return row["ROUTINE_CATALOG"] as string;
            }
        }

        public string ROUTINE_NAME
        {
            get
            {
                return row["ROUTINE_NAME"] as string;
            }
        }

        public string ROUTINE_TYPE
        {
            get
            {
                return row["ROUTINE_TYPE"] as string;
            }
        }

        public string ROUTINE_SCHEMA
        {
            get
            {
                return row["ROUTINE_SCHEMA"] as string;
            }
        }

        public string SPECIFIC_NAME
        {
            get
            {
                return row["SPECIFIC_NAME"] as string;
            }
        }

        public string SPECIFIC_SCHEMA
        {
            get
            {
                return row["SPECIFIC_SCHEMA"] as string;
            }
        }

        public string TABLE_NAME
        {
            get
            {
                return row["TABLE_NAME"] as string;
            }
        }

        public string TABLE_SCHEM
        {
            get
            {
                return row["TABLE_SCHEM"] as string;
            }
        }

        public string TABLE_SCHEMA
        {
            get
            {
                return row["TABLE_SCHEMA"] as string;
            }
        }

        public string TABLE_TYPE
        {
            get
            {
                return row["TABLE_TYPE"] as string;
            }
        }

        public string TYPE
        {
            get
            {
                return row["TYPE"] as string;
            }
        }

        public string TYPE_NAME
        {
            get
            {
                return row["TYPE_NAME"] as string;
            }
        }

        public string VIEW_NAME
        {
            get
            {
                return row["VIEW_NAME"] as string;
            }
        }

        public string UQ_COLUMN_NAME
        {
            get
            {
                return row["UQ_COLUMN_NAME"] as string;
            }
        }

        public string UQ_TABLE_NAME
        {
            get
            {
                return row["UQ_TABLE_NAME"] as string;
            }
        }
        #endregion

        #region Methods
        public static IEnumerable<StiSchemaRow> All(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                yield return new StiSchemaRow(row);
            }
        }
        #endregion

        #region Fields
        private DataRow row;
        #endregion

        private StiSchemaRow(DataRow row)
        {
            this.row = row;
        }
    }
}