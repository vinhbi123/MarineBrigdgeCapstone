using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Payments;
using ShipCapstone.Domain.Models.Settings;

namespace ShipCapstone.Application.Services.Implements;

public class PaymentService : IPaymentService
{
    private readonly PayOSSettings _payOSSettings;
    private readonly PayOS _payOS;

    public PaymentService(IOptions<PayOSSettings> payOsSettings)
    {
        _payOSSettings = payOsSettings.Value ?? throw new ArgumentNullException(nameof(payOsSettings));
        _payOS = new PayOS(_payOSSettings.ClientId, _payOSSettings.ApiKey, _payOSSettings.ChecksumKey) ?? throw new ArgumentNullException(nameof(_payOS));
    }
    public async Task<CreatePaymentResult> CreatePaymentUrl(CreatePaymentRequest request)
    {
        CreatePaymentResult url = null;
        var expiredAt = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds();
        var description = "";
        int amount = 0;
        List<ItemData> items = new List<ItemData>();
        if (request.Type == EPaymentType.Supplier)
        {
            var order = request.PaymentObject as Order 
                        ?? throw new InvalidCastException("PaymentObject không phải là Order");
            foreach (var item in order.OrderItems)
            {
                items.Add(new ItemData((item.ProductVariant.Name + " " + item.ProductOptionName).Trim(),
                    item.Quantity, (int)item.Price));
            }
            amount = (int)order.TotalAmount;
        }
        else
        {
            var booking = request.PaymentObject as Booking 
                          ?? throw new InvalidCastException("PaymentObject không phải là Booking");
            items = null;
            amount = (int)booking.TotalAmount;
        }
        description = $"Thanh toan {request.TransactionCode}";
        var signatureData = new Dictionary<string, object>
        {
            { "amount", amount },
            { "cancelUrl", _payOSSettings.ReturnUrlFail },
            { "description", description },
            { "expiredAt", expiredAt },
            { "orderCode", request.TransactionCode },
            { "returnUrl", _payOSSettings.ReturnUrl }
        };
        var data = string.Join("&", signatureData.Select(p => $"{p.Key}={p.Value}"));
        var signature = ComputeHmacSha256(data, _payOSSettings.ChecksumKey);

        var paymentData = new PaymentData(
            orderCode: request.TransactionCode,
            amount: amount,
            description: description,
            items: items,
            cancelUrl: _payOSSettings.ReturnUrlFail,
            returnUrl: _payOSSettings.ReturnUrl,
            signature: signature,
            buyerName: request.Account.FullName,
            buyerPhone: request.Account.PhoneNumber,
            buyerAddress: request.Address,
            expiredAt: (int)expiredAt
        );
        url = await _payOS.createPaymentLink(paymentData);
        return url;
    }

    public string CreateUrlSepay(CreatePaymentSePayRequest request)
    {
        string qrLink = $"https://qr.sepay.vn/img?acc={request.BankNo}&bank={request.BankName}&amount={request.Revenue}&des={Uri.EscapeDataString(request.Description)}&template=compact&download=DOWNLOAD";

        return qrLink;
    }

    private string? ComputeHmacSha256(string data, string checksumKey)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(checksumKey)))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}