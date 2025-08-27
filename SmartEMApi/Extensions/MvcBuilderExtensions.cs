using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartEM.Application.Utils;
using System.Text.Json.Serialization;

namespace SmartEMApi.Extensions;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder AddCustomJsonOptions(this IMvcBuilder builder) =>
        builder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    public static IMvcBuilder AddApiBehaviorConfiguration(this IMvcBuilder builder) =>
        builder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(ms => ms.Value is { Errors.Count: > 0, ValidationState: ModelValidationState.Invalid })
                    .SelectMany(ms => ms.Value!.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                var response = OperationResponse.FailedResponse(StatusCode.BadRequest)
                    .AddErrors(errors);

                return new BadRequestObjectResult(response);
            };
        });
}
