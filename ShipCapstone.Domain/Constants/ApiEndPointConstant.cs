namespace ShipCapstone.Domain.Constants;

public class ApiEndPointConstant
{
    public const string RootEndPoint = "/api";
    public const string ApiVersion = "/v1";
    public const string ApiEndpoint = RootEndPoint + ApiVersion;

    public static class Authentication
    {
        public const string AuthenticationEndpoint = ApiEndpoint + "/auth";
        public const string Login = AuthenticationEndpoint + "/login";
        public const string Register = AuthenticationEndpoint + "/register";
        public const string Otp = AuthenticationEndpoint + "/otp";
        public const string OAuth = AuthenticationEndpoint + "/oauth";
    }

    public static class Ships
    {
        public const string ShipEndpoint = ApiEndpoint + "/ships";
        public const string ShipById = ShipEndpoint + "/{id}";
    }

    public static class Suppliers
    {
        public const string SupplierEndpoint = ApiEndpoint + "/suppliers";
    }
    public static class Categories
    {
        public const string CategoryEndpoint = ApiEndpoint + "/categories";
        public const string CategoryById = CategoryEndpoint + "/{id}";
    }
    public static class ModifierGroups
    {
        public const string ModifierGroupEndpoint = ApiEndpoint + "/modifier-groups";
        public const string ModifierGroupById = ModifierGroupEndpoint + "/{id}";
        public const string ModifierGroupByIdWithOptions = ModifierGroupById + "/modifier-options";
    }

    public static class ModifierOptions
    {
        public const string ModifierOptionsEndpoint = ApiEndpoint + "/modifier-options";
        public const string ModifierOptionById = ModifierOptionsEndpoint + "/{id}";
    }
}