#region Copyright (C) 2003-2015 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
{																	}
{	Copyright (C) 2003-2015 Stimulsoft     							}
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
{	TRADE SECRETS OF Stimulsoft										}
{																	}
{	CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON		}
{	ADDITIONAL RESTRICTIONS.										}
{																	}
{*******************************************************************}
*/
#endregion Copyright (C) 2003-2015 Stimulsoft

using System.Runtime.CompilerServices;

namespace Stimulsoft.Base
{
    public class StiPublicKey
    {
        private const string KeyReports = "00240000048000009400000006020000002400005253413100040000010001003bad30e3275cab0470144f7f98457555375744d5e3c5cd451fe72f4b2459d629b0c409f0d6d4af1ec4a01065a73ff0debd63d7392c703d42a8d2c5dcaed1236f1d384ab74d722c159258b58a07060ff74c3c02329f4e8244ff39ee7c113814c1b1965b8c2ccfeb505e31ed16f314937be078c0b81b7f11d815f45316e02e9cd7";
        private const string KeyServer = "0024000004800000940000000602000000240000525341310004000001000100f1fb6d24a930093776409d1222ebc02c34a7b28c2d12e7dd8fdbbaefc55fcb38ad27ac65bc356b2182ce44d7807ed33b262b33195ca215de36c167a2f5fa96b36241f6e6e459d138b79c575e7f0c771ccb1bb2f732550d15bb55a2653778da49adf886bf7bad9baf86397dabd5c52c4b31366da9741fe01571a9658f2c6e08c5";

        public const string Monitor = "Monitor, PublicKey=" + KeyReports;
        public const string Navigator = "Navigator, PublicKey=" + KeyReports;
        public const string Navigator_Web = "Navigator.Web, PublicKey=" + KeyReports;
        
        public const string RunMe_Test = "RunMe.Test, PublicKey=" + KeyReports;

        public const string Stimulsoft_Client = "Stimulsoft.Client, PublicKey=" + KeyReports;
        public const string Stimulsoft_Client_Designer = "Stimulsoft.Client.Designer, PublicKey=" + KeyReports;
        public const string Stimulsoft_Client_Web = "Stimulsoft.Client.Web, PublicKey=" + KeyReports;

        public const string Stimulsoft_Report_Check = "Stimulsoft.Report.Check, PublicKey=" + KeyReports;
        public const string Stimulsoft_Report_Comparer_Wpf = "Stimulsoft.Report.Comparer.Wpf, PublicKey=" + KeyReports;
        public const string Stimulsoft_Report_Mobile = "Stimulsoft.Report.Mobile, PublicKey=" + KeyReports;
        public const string Stimulsoft_Report_MobileDesign = "Stimulsoft.Report.MobileDesign, PublicKey=" + KeyReports;
        public const string Stimulsoft_Report_Wpf = "Stimulsoft.Report.Wpf, PublicKey=" + KeyReports;
        
        public const string Stimulsoft_Server = "Stimulsoft.Server, PublicKey=" + KeyReports;
        public const string Stimulsoft_Server_Agent = "Stimulsoft.Server.Agent, PublicKey=" + KeyReports;
        public const string Stimulsoft_Server_Connect = "Stimulsoft.Server.Connect, PublicKey=" + KeyReports;
        public const string Stimulsoft_Server_Connect_Test = "Stimulsoft.Server.Connect.Test, PublicKey=" + KeyReports;
        public const string Stimulsoft_Server_Controller = "Stimulsoft.Server.Controller, PublicKey=" + KeyReports;
        public const string Stimulsoft_Server_Test = "Stimulsoft.Server.Test, PublicKey=" + KeyReports;
    }
}