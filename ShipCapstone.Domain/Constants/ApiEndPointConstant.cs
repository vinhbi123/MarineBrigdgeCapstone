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
        public const string Profile = AuthenticationEndpoint + "/profile";
    }

    public static class Ships
    {
        public const string ShipEndpoint = ApiEndpoint + "/ships";
        public const string ShipById = ShipEndpoint + "/{id}";
        public const string CaptainByShipId = ShipEndpoint + "/{id}/captains";
        public const string ReportProblemByShip = ShipEndpoint + "/{id}/report-problems";
    }

    public static class Suppliers
    {
        public const string SupplierEndpoint = ApiEndpoint + "/suppliers";
        public const string SupplierById = SupplierEndpoint + "/{id}";
        public const string SupplierWithProducts = SupplierById + "/products";
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
    public static class Ports
    {
        public const string PortEndpoint = ApiEndpoint + "/ports";
        public const string PortById = PortEndpoint + "/{id}";
    }

    public static class Boatyards
    {
        public const string BoatyardEndpoint = ApiEndpoint + "/boatyards";
        public const string BoatyardById = BoatyardEndpoint + "/{id}";
        public const string BoatyardDetail = BoatyardEndpoint + "/detail";
        public const string BoatyardWithBoatyardServices = BoatyardById + "/boatyard-services";
        public const string BoatyardWithDockSlots = BoatyardById + "/dock-slots";
    }
    
    public static class DockSlots
    {
        public const string DockSlotEndpoint = ApiEndpoint + "/dock-slots";
        public const string DockSlotById = DockSlotEndpoint + "/{id}";
    }
    
    public static class BoatyardServices
    {
        public const string BoatyardServiceEndpoint = ApiEndpoint + "/boatyard-services";
        public const string BoatyardServiceById = BoatyardServiceEndpoint + "/{id}";
    }
    
    public static class Captains
    {
        public const string CaptainEndpoint = ApiEndpoint + "/captains";
    }

    public static class Products
    {
        public const string ProductEndpoint = ApiEndpoint + "/products";
        public const string ProductById = ProductEndpoint + "/{id}";
        public const string ProductWithVariants = ProductById + "/product-variants";
        public const string ProductWithReviews = ProductById + "/reviews";
    }
    
    public static class ProductVariants
    {
        public const string ProductVariantEndpoint = ApiEndpoint + "/product-variants";
        public const string ProductVariantById = ProductVariantEndpoint + "/{id}";
    }

    public static class Payments
    {
        public const string PaymentEndpoint = ApiEndpoint + "/payments";
        public const string HandlePayment = PaymentEndpoint + "/handler";
        public const string HandlerPaymentSepay = PaymentEndpoint + "/handle-sepay";
    }
    public static class Accouts
    {
        public const string AccountEndpoint = ApiEndpoint + "/accounts";
        public const string ChangePassword = AccountEndpoint + "/password";
    }
    public static class Orders
    {
        public const string OrderEndpoint = ApiEndpoint + "/orders";
        public const string OrderById = OrderEndpoint + "/{id}";
    }
    public static class Bookings
    {
        public const string BookingEndPoint = ApiEndpoint + "/bookings";
        public const string BookingById = BookingEndPoint + "/{id}";
    }
    public static class Revenue
    {
        public const string RevenueEndPoint = ApiEndpoint + "/revenues";
    }
    
    public static class ReportProblems
    {
        public const string ReportProblemEndpoint = ApiEndpoint + "/report-problems";
        public const string ReportProblemById = ReportProblemEndpoint + "/{id}";
    }
   
    public static class Transaction
    {
        public const string TransactionEndPoint = ApiEndpoint + "/transactions";
    }

    public static class ProductVariantOptions
    {
        public const string ProductVariantOptionEndpoint = ApiEndpoint + "/product-variant-options";
        public const string ProductVariantOptionById = ProductVariantOptionEndpoint + "/{id}";
    }
    
    public static class ProductImages
    {
        public const string ProductImageEndpoint = ApiEndpoint + "/product-images";
        public const string ProductImageById = ProductImageEndpoint + "/{id}";
    }
}