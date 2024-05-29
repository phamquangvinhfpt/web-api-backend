namespace Core.Auth
{
    public static class Permissions
    {
        public static class Guests
        {
            public const string ViewClinicInfo = "Permissions.Guests.ViewClinicInfo";
            public const string ViewSchedule = "Permissions.Guests.ViewSchedule";
            public const string ViewServices = "Permissions.Guests.ViewServices";
            public const string RegisterAccount = "Permissions.Guests.RegisterAccount";
        }

        public static class Customers
        {
            public const string BookAppointment = "Permissions.Customers.BookAppointment";
            public const string ReceiveNotification = "Permissions.Customers.ReceiveNotification";
            public const string BookPeriodicAppointment = "Permissions.Customers.BookPeriodicAppointment";
            public const string ReceiveExamResult = "Permissions.Customers.ReceiveExamResult";
            public const string ChatWithDentist = "Permissions.Customers.ChatWithDentist";
        }

        public static class Dentists
        {
            public const string ViewWeeklySchedule = "Permissions.Dentists.ViewWeeklySchedule";
            public const string ProposePeriodicSchedule = "Permissions.Dentists.ProposePeriodicSchedule";
            public const string SendExamResult = "Permissions.Dentists.SendExamResult";
            public const string ViewPatientHistory = "Permissions.Dentists.ViewPatientHistory";
            public const string ChatWithCustomer = "Permissions.Dentists.ChatWithCustomer";
        }

        public static class ClinicOwners
        {
            public const string RegisterClinicInfo = "Permissions.ClinicOwners.RegisterClinicInfo";
            public const string RegisterDentistInfo = "Permissions.ClinicOwners.RegisterDentistInfo";
            public const string ManageClinicSchedule = "Permissions.ClinicOwners.ManageClinicSchedule";
            public const string ManagePatientInfo = "Permissions.ClinicOwners.ManagePatientInfo";
            public const string ManageDentistInfo = "Permissions.ClinicOwners.ManageDentistInfo";
            public const string ManageConversations = "Permissions.ClinicOwners.ManageConversations";
        }

        public static class SuperAdmin
        {
            public const string ReviewClinicInfo = "Permissions.SuperAdmin.ReviewClinicInfo";
            public const string ReviewDentistInfo = "Permissions.SuperAdmin.ReviewDentistInfo";
            public const string ManageAccounts = "Permissions.SuperAdmin.ManageAccounts";
        }
    }
}