using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManagement
{
    public class GlobalData
    {
        public static string firebaseDatabase = "https://property-management-2cf1e-default-rtdb.firebaseio.com/";

        public static string firebaseStorage = "property-management-2cf1e.appspot.com";

        public static string firebaseAuthentication = "AIzaSyA7s0KUOR-IfIkVSe94VpiWtXZnBSchUok";

        public static PropertyItem property { get; set; }
        public static TenantItem tenant { get; set; }
        public static MaintenanceRequest maintenanceRequest { get; set; }
        public static Appointment appointment { get; set; }

    }
}
