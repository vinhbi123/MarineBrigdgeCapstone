using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ShipCapstone.Application.Common.Validators;

public class CustomAuthorizeAttribute : AuthorizeAttribute
{
    public CustomAuthorizeAttribute(params ERole[] roleEnums)
    {
        var allowedRolesAsString = roleEnums.Select(x => x.GetDescriptionFromEnum());
        Roles = string.Join(",", allowedRolesAsString);
    }
}