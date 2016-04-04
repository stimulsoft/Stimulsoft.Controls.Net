#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports 									            }
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base
{
    public class StiAssemblies
    {
        public const string Monitor = "Monitor";
        public const string Navigator = "Navigator";
        public const string Navigator_Web = "Navigator.Web";

        public const string RunMe_Test = "RunMe.Test";

        public const string TestBrowser = "TestBrowser";

        public const string Server_Console = "Server.Console";

        public const string Stimulsoft_Client = "Stimulsoft.Client";
        public const string Stimulsoft_Client_Designer = "Stimulsoft.Client.Designer";
        public const string Stimulsoft_Client_Web = "Stimulsoft.Client.Web";

        public const string Stimulsoft_Report_Check = "Stimulsoft.Report.Check";
        public const string Stimulsoft_Report_Comparer_Wpf = "Stimulsoft.Report.Comparer.Wpf";
        public const string Stimulsoft_Report_Mobile = "Stimulsoft.Report.Mobile";
        public const string Stimulsoft_Report_MobileDesign = "Stimulsoft.Report.MobileDesign";
        public const string Stimulsoft_Report_Wpf = "Stimulsoft.Report.Wpf";
        public const string Stimulsoft_Report_Design_Wpf = "Stimulsoft.Report.WpfDesign";
        
        public const string Stimulsoft_Server = "Stimulsoft.Server";
        public const string Stimulsoft_Server_Agent = "Stimulsoft.Server.Agent";
        public const string Stimulsoft_Server_Accounts = "Stimulsoft.Server.Accounts";
        public const string Stimulsoft_Server_Accounts_Objects = "Stimulsoft.Server.Accounts.Objects";
        public const string Stimulsoft_Server_Business = "Stimulsoft.Server.Business";
        public const string Stimulsoft_Server_Business_Objects = "Stimulsoft.Server.Business.Objects";
        public const string Stimulsoft_Server_Check = "Stimulsoft.Server.Check";
        public const string Stimulsoft_Server_Cloud_Worker = "Stimulsoft.Server.Cloud.Worker";
        public const string Stimulsoft_Server_Config = "Stimulsoft.Server.Config";
        public const string Stimulsoft_Server_Connect = "Stimulsoft.Server.Connect";
        public const string Stimulsoft_Server_Connect_Test = "Stimulsoft.Server.Connect.Test";
        public const string Stimulsoft_Server_Controller = "Stimulsoft.Server.Controller";
        public const string Stimulsoft_Server_Dashboards = "Stimulsoft.Server.Dashboards";
        public const string Stimulsoft_Server_Dashboards_Objects = "Stimulsoft.Server.Dashboards.Objects";
        public const string Stimulsoft_Server_Data = "Stimulsoft.Server.Data";
        public const string Stimulsoft_Server_Data_Objects = "Stimulsoft.Server.Data.Objects";
        public const string Stimulsoft_Server_Developers = "Stimulsoft.Server.Developers";
        public const string Stimulsoft_Server_Developers_Objects = "Stimulsoft.Server.Developers.Objects";
        public const string Stimulsoft_Server_Dropbox = "Stimulsoft.Server.Dropbox";
        public const string Stimulsoft_Server_Email = "Stimulsoft.Server.Email";
        public const string Stimulsoft_Server_Monitor = "Stimulsoft.Server.Monitor";
        public const string Stimulsoft_Server_Monitor_Objects = "Stimulsoft.Server.Monitor.Objects";
        public const string Stimulsoft_Server_OneDrive = "Stimulsoft.Server.OneDrive";
        public const string Stimulsoft_Server_Reports = "Stimulsoft.Server.Reports";
        public const string Stimulsoft_Server_Reports_Objects = "Stimulsoft.Server.Reports.Objects";
        public const string Stimulsoft_Server_Schedulers = "Stimulsoft.Server.Schedulers";
        public const string Stimulsoft_Server_Schedulers_Objects = "Stimulsoft.Server.Schedulers.Objects";
        public const string Stimulsoft_Server_Signals = "Stimulsoft.Server.Signals";
        public const string Stimulsoft_Server_Signals_Objects = "Stimulsoft.Server.Signals.Objects";
        public const string Stimulsoft_Server_Storage = "Stimulsoft.Server.Storage";
        public const string Stimulsoft_Server_Test = "Stimulsoft.Server.Test";
        public const string Admin_Console = "AdminConsole";
    }
}
