namespace SmartEM.Application.Utils.ErrorTemplates;

/// <summary>
/// Provides common standardized validation messages.
/// </summary>
public static class ValidationMessages
{
    // FluentValidation placeholders that are automatically replaced with actual property names and values at runtime.
    private const string PropertyNamePlaceholder = "{PropertyName}";
    private const string ComparisonValuePlaceholder = "{ComparisonValue}";
    private const string MinLengthPlaceholder = "{MinLength}";
    private const string MaxLengthPlaceholder = "{MaxLength}";

    /// <summary>
    /// Message for required fields that cannot be null or empty.
    /// </summary>
    public const string NotNullOrEmpty
        = $"{PropertyNamePlaceholder} must be provided.";

    /// <summary>
    /// Message for numeric values that must exceed a minimum value.
    /// </summary>
    public const string ValueMustBeGreaterThan
        = $"{PropertyNamePlaceholder} must be greater than {ComparisonValuePlaceholder}.";

    /// <summary>
    /// Message when a collection must not be empty.
    /// </summary>
    public const string CollectionCannotBeEmpty
        = $"{PropertyNamePlaceholder} must contain at least one item.";

    /// <summary>
    /// Message when a collection contains empty values.
    /// </summary>
    public const string CollectionCannotContainEmptyValues
        = $"{PropertyNamePlaceholder} cannot contain empty values.";

    /// <summary>
    /// Message when a property has an invalid value.
    /// </summary>
    public const string MustBeValid
        = $"{PropertyNamePlaceholder} is not valid.";

    /// <summary>
    /// Message for format-specific validation.
    /// </summary>
    /// <param name="expectedFormat">The expected format</param>
    /// <returns>Formatted validation message</returns>
    public static string MustBeValidFormat(string expectedFormat)
        => $"{PropertyNamePlaceholder} must be in a valid {expectedFormat} format.";

    /// <summary>
    /// Message for optional fields that must be valid when provided.
    /// </summary>
    public const string MustBeValidIfProvided
        = $"{PropertyNamePlaceholder} must be valid if provided.";

    /// <summary>
    /// Message for invalid enum values.
    /// </summary>
    public const string MustBeValidEnumValue
        = $"{PropertyNamePlaceholder} must be a valid enum value.";

    /// <summary>
    /// Message for strings that must exceed minimum length.
    /// </summary>
    public const string StringLengthMustBeMoreThan
        = $"{PropertyNamePlaceholder} must be more than {MinLengthPlaceholder} characters.";

    /// <summary>
    /// Message for strings that must not exceed maximum length.
    /// </summary>
    public const string StringLengthMustBeLessThan
        = $"{PropertyNamePlaceholder} must be less than {MaxLengthPlaceholder} characters.";

    /// <summary>
    /// Message for strings that must be within a specific length range.
    /// </summary>
    public const string StringLengthMustBeBetween
        = $"{PropertyNamePlaceholder} must be between {MinLengthPlaceholder} and {MaxLengthPlaceholder} characters.";

    /// <summary>
    /// Message for duplicate values in a collection.
    /// </summary>
    /// <param name="propertyName">Name of the property with duplicate values</param>
    /// <param name="value">The duplicate value</param>
    /// <returns>Formatted validation message</returns>
    public static string CollectionValuesMustBeUnique(string propertyName, object value)
        => $"Duplicate {propertyName} '{value}' found in collection.";

    /// <summary>
    /// Message for duplicate values in a nested collection.
    /// </summary>
    /// <param name="propertyName">Name of the property with duplicate values</param>
    /// <param name="value">The duplicate value</param>
    /// <param name="parentName">Name of the parent property</param>
    /// <param name="parentValue">Value of the parent property</param>
    /// <returns>Formatted validation message</returns>
    public static string CollectionValuesMustBeUnique(string propertyName, object value, string parentName, object parentValue)
        => $"Duplicate {propertyName} '{value}' found in  {parentName} '{parentValue}' in this collection.";

    /// <summary>
    /// Message for duplicate values in a collection.
    /// </summary>
    /// <param name="propertyName">Name of the property with duplicate values</param>
    /// <returns>Formatted validation message</returns>
    public static string CollectionValuesMustBeUnique(string propertyName)
        => $"Duplicate {propertyName} values found in  {PropertyNamePlaceholder} collection.";
}
