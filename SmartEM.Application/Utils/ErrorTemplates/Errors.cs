namespace SmartEM.Application.Utils.ErrorTemplates;

/// <summary>
/// Provides standardized error message formatting for application entities.
/// </summary>
public static class Errors
{
    private static string FormatPropertyValue(string propertyName, object value)
        => $"{propertyName} \"{value}\"";

    private static string JoinProperties(IEnumerable<(string PropertyName, object Value)> properties, Conjunction conjunction)
    {
        var items = properties.ToArray();
        if (items.Length == 0) return string.Empty;

        if (items.Length == 1)
            return FormatPropertyValue(items[0].PropertyName, items[0].Value);

        return string.Join(", ", items.Take(items.Length - 1)
            .Select(i => FormatPropertyValue(i.PropertyName, i.Value)))
            + $" {conjunction.ToString().ToLowerInvariant()} {FormatPropertyValue(items.Last().PropertyName, items.Last().Value)}";
    }

    private static string GetEntityName<T>() => typeof(T).Name;

    /// <summary>
    /// Generates a "not found" error message for an entity with a specific property value.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="value">The value of the property.</param>
    /// <returns>A formatted error message.</returns>
    public static string ReturnNotFound<T>(string propertyName, object value)
        => $"{GetEntityName<T>()} with {FormatPropertyValue(propertyName, value)} not found.";

    /// <summary>
    /// Generates a "not found" error message for an entity with multiple property values.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="conjunction">The conjunction used to join properties (e.g., "and" or "or").</param>
    /// <param name="properties">The properties and their values.</param>
    /// <returns>A formatted error message.</returns>
    public static string ReturnNotFound<T>(Conjunction conjunction, params (string PropertyName, object Value)[] properties)
        => $"{GetEntityName<T>()} with {JoinProperties(properties, conjunction)} not found.";

    /// <summary>
    /// Generates a "not found" error message when multiple entity IDs are missing.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="values">A collection of missing identifiers.</param>
    /// <returns>A formatted error message.</returns>
    public static string ReturnNotFoundManyIds<T, TId>(string propertyName, IEnumerable<TId> values)
    {
        var formattedValues = string.Join(", ", values.Select(v => $"\"{v}\""));
        return $"No {GetEntityName<T>()} found with {propertyName}s: {formattedValues}.";
    }

    /// <summary>
    /// Generates an error message for a failed commit operation.
    /// </summary>
    /// <param name="operation">The commit operation that failed.</param>
    /// <returns>A formatted error message.</returns>
    public static string ReturnCommitFailed(CommitOperation operation)
        => $"Unable to {operation.ToString().ToLowerInvariant()} the requested changes";

    /// <summary>
    /// Generates an error message for a failed commit operation on a specific entity.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="operation">The commit operation that failed.</param>
    /// <returns>A formatted error message.</returns>
    public static string ReturnCommitFailed<T>(CommitOperation operation)
        => $"Unable to {operation.ToString().ToLowerInvariant()} {GetEntityName<T>()}.";

    /// <summary>
    /// Generates an "already exists" error message for an entity with multiple property values.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="conjunction">The conjunction used to join properties.</param>
    /// <param name="properties">The properties and their values.</param>
    /// <returns>A formatted error message.</returns>
    public static string ReturnAlreadyExists<T>(Conjunction conjunction, params (string PropertyName, object Value)[] properties)
        => $"{GetEntityName<T>()} with {JoinProperties(properties, conjunction)} already exists.";

    /// <summary>
    /// Generates an "already exists" error message for an entity with a specific property value.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="value">The value of the property.</param>
    /// <returns>A formatted error message.</returns>
    public static string ReturnAlreadyExists<T>(string propertyName, object value)
        => $"{GetEntityName<T>()} with {FormatPropertyValue(propertyName, value)} already exists.";

    /// <summary>
    /// Generates an error message for a failed file retrieval operation.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string ReturnFileRetrievalFailed(string fileName)
        => $"Failed to retrieve file \"{fileName}\". Please upload.";

    /// <summary>
    /// Generates an error message for a failed file upload operation.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string ReturnFileUploadFailed(string fileName)
        => $"Failed to upload file \"{fileName}\". Please try again.";

    /// <summary>
    /// Generates an error message for a failed data extraction from a file.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string ReturnFileDataExtractionFailed(string fileName)
        => $"Failed to extract data from file \"{fileName}\". Please upload.";

    /// <summary>
    /// Generates an error message indicating that a unit source is system and cannot be modified.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="values"></param>
    /// <param name="operation"></param>
    /// <returns></returns>
    public static string ReturnUnitSourceIsSystem<T, TId>(string propertyName, IEnumerable<TId> values, CommitOperation operation)
    {
        var formattedValues = string.Join(", ", values.Select(v => $"\"{v}\""));
        return $"Cannot {operation.ToString().ToLowerInvariant()} {GetEntityName<T>()} with {propertyName}s: {formattedValues} because Unit Source is System.";
    }

    public static string ReturnInvalidExcelFileFormat(string fileName, List<string> applicationDefinedSheetNames)
    {
        return $"The file \"{fileName}\" is not in the expected format. " +
            $"Please ensure it contains the following sheets: {string.Join(", ", applicationDefinedSheetNames)}" +
            " or use the download template button.";
    }
}
