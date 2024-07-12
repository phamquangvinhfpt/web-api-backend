using System.Collections.ObjectModel;
using Core.Enums;

namespace Core.Auth.Permissions
{
    public static class Action
    {
        public const string View = nameof(View);
        public const string Search = nameof(Search);
        public const string Create = nameof(Create);
        public const string Update = nameof(Update);
        public const string Delete = nameof(Delete);
        public const string Export = nameof(Export);
        public const string Generate = nameof(Generate);
        public const string Clean = nameof(Clean);
        public const string UpgradeSubscription = nameof(UpgradeSubscription);
        public const string Upload = nameof(Upload);
    }

    public static class Resource
    {
        public const string Dashboard = nameof(Dashboard);
        public const string Hangfire = nameof(Hangfire);
        public const string Users = nameof(Users);
        public const string UserRoles = nameof(UserRoles);
        public const string Roles = nameof(Roles);
        public const string RoleClaims = nameof(RoleClaims);
        public const string UserClaims = nameof(UserClaims);
        public const string Clinics = nameof(Clinics);
        public const string ClinicDetails = nameof(ClinicDetails);
        public const string DentistDetails = nameof(DentistDetails);
        public const string DentalRecords = nameof(DentalRecords);
        public const string FollowUpAppointments = nameof(FollowUpAppointments);
        public const string MedicalRecords = nameof(MedicalRecords);
        public const string Prescriptions = nameof(Prescriptions);
        public const string Appointments = nameof(Appointments);
        public const string Files = nameof(Files);
        public const string AuditLogs = nameof(AuditLogs);
    }

    public static class Permissions
    {
        private static readonly Permission[] _all = new Permission[]
        {
            new("View Dashboard", Action.View, Resource.Dashboard),
            new("View Hangfire", Action.View, Resource.Hangfire),

            // USERS
            new("View Users", Action.View, Resource.Users, Roles.SuperAdmin),
            new("Search Users", Action.Search, Resource.Users, Roles.SuperAdmin),
            new("Create Users", Action.Create, Resource.Users, Roles.SuperAdmin),
            new("Update Users", Action.Update, Resource.Users, Roles.SuperAdmin),
            new("Delete Users", Action.Delete, Resource.Users, Roles.SuperAdmin),
            new("Export Users", Action.Export, Resource.Users, Roles.SuperAdmin),

            // ROLES
            new("View UserRoles", Action.View, Resource.UserRoles, Roles.SuperAdmin),
            new("Update UserRoles", Action.Update, Resource.UserRoles, Roles.SuperAdmin),
            new("View Roles", Action.View, Resource.Roles, Roles.SuperAdmin),
            new("Create Roles", Action.Create, Resource.Roles, Roles.SuperAdmin),
            new("Update Roles", Action.Update, Resource.Roles, Roles.SuperAdmin),
            new("Delete Roles", Action.Delete, Resource.Roles, Roles.SuperAdmin),
            new("Create RoleClaims", Action.Create, Resource.RoleClaims, Roles.SuperAdmin),
            new("Delete RoleClaims", Action.Delete, Resource.RoleClaims, Roles.SuperAdmin),
            new("Create UserClaims", Action.Create, Resource.UserClaims, Roles.SuperAdmin),
            new("Delete UserClaims", Action.Delete, Resource.UserClaims, Roles.SuperAdmin),
            new("View UserClaims", Action.View, Resource.UserClaims, Roles.SuperAdmin),
            new("Update UserClaims", Action.Update, Resource.UserClaims, Roles.SuperAdmin),

            // CLINICS
            new("View Clinics", Action.View, Resource.Clinics),
            new("Create Clinics", Action.Create, Resource.Clinics, Roles.ClinicOwner),
            new("Update Clinics", Action.Update, Resource.Clinics, Roles.ClinicOwner),
            new("Delete Clinics", Action.Delete, Resource.Clinics, Roles.ClinicOwner),

            // CLINIC DETAILS
            new("View ClinicDetails", Action.View, Resource.ClinicDetails),
            new("Create ClinicDetails", Action.Create, Resource.ClinicDetails, Roles.ClinicOwner),
            new("Update ClinicDetails", Action.Update, Resource.ClinicDetails, Roles.ClinicOwner),
            new("Delete ClinicDetails", Action.Delete, Resource.ClinicDetails, Roles.ClinicOwner),

            // DENTIST DETAILS
            new("View DentistDetails", Action.View, Resource.DentistDetails),
            new("Create DentistDetails", Action.Create, Resource.DentistDetails, Roles.ClinicOwner),
            new("Update DentistDetails", Action.Update, Resource.DentistDetails, Roles.ClinicOwner),
            new("Delete DentistDetails", Action.Delete, Resource.DentistDetails, Roles.ClinicOwner),

            // DENTAL RECORDS
            new("View DentalRecords", Action.View, Resource.DentalRecords),
            new("Create DentalRecords", Action.Create, Resource.DentalRecords, Roles.Dentist),
            new("Update DentalRecords", Action.Update, Resource.DentalRecords, Roles.Dentist),
            new("Delete DentalRecords", Action.Delete, Resource.DentalRecords, Roles.Dentist),

            // FOLLOW UP APPOINTMENTS
            new("View FollowUpAppointments", Action.View, Resource.FollowUpAppointments),
            new("Create FollowUpAppointments", Action.Create, Resource.FollowUpAppointments, Roles.Dentist),
            new("Update FollowUpAppointments", Action.Update, Resource.FollowUpAppointments, Roles.Dentist),
            new("Delete FollowUpAppointments", Action.Delete, Resource.FollowUpAppointments, Roles.Dentist),

            // MEDICAL RECORDS
            new("View MedicalRecords", Action.View, Resource.MedicalRecords),
            new("Create MedicalRecords", Action.Create, Resource.MedicalRecords, Roles.Dentist),
            new("Update MedicalRecords", Action.Update, Resource.MedicalRecords, Roles.Dentist),
            new("Delete MedicalRecords", Action.Delete, Resource.MedicalRecords, Roles.Dentist),

            // PRESCRIPTIONS
            new("View Prescriptions", Action.View, Resource.Prescriptions),
            new("Create Prescriptions", Action.Create, Resource.Prescriptions, Roles.Dentist),
            new("Update Prescriptions", Action.Update, Resource.Prescriptions, Roles.Dentist),
            new("Delete Prescriptions", Action.Delete, Resource.Prescriptions, Roles.Dentist),

            // APPOINTMENTS
            new("View Appointments", Action.View, Resource.Appointments),
            new("Create Appointments", Action.Create, Resource.Appointments, Roles.Dentist),
            new("Update Appointments", Action.Update, Resource.Appointments, Roles.Dentist),
            new("Delete Appointments", Action.Delete, Resource.Appointments, Roles.Dentist),

            // FILES
            new("Upload files", Action.Upload, Resource.Files),

            // AUDIT LOGS
            new("View AuditLogs", Action.View, Resource.AuditLogs, Roles.SuperAdmin),
        };

        public static IReadOnlyList<Permission> All { get; } = new ReadOnlyCollection<Permission>(_all);
        public static IReadOnlyList<Permission> SuperAdmin { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.MinimumRole == Roles.SuperAdmin).ToArray());
        public static IReadOnlyList<Permission> ClinicOwner { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.MinimumRole == Roles.ClinicOwner).ToArray());
        public static IReadOnlyList<Permission> Dentist { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.MinimumRole == Roles.Dentist).ToArray());
        public static IReadOnlyList<Permission> Customer { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.MinimumRole > Roles.Customer).ToArray());
        public static IReadOnlyList<Permission> Guest { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.MinimumRole == Roles.Guest).ToArray());
    }

    public record Permission(string Description, string Action, string Resource, Roles MinimumRole = Roles.Guest)
    {
        public string Name => NameFor(Action, Resource);
        public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
    }
}