namespace Tech2023.Web.Shared;

/// <summary>
/// Thrown when a configuration section of the application is not valid and cannot run
/// </summary>
[Serializable]
public class ConfigurationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationException"/> class
    /// </summary>
    public ConfigurationException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationException"/> class with the specified message
    /// </summary>
    /// <param name="message">The message error detailing the exception</param>
    public ConfigurationException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationException"/> class with the specified message and inner exception
    /// </summary>
    /// <param name="message">The message error detailing the exception</param>
    /// <param name="inner">The inner exception of the <see cref="ConfigurationException"/></param>
    public ConfigurationException(string message, Exception inner) : base(message, inner) { }

    /// <inheritdoc/>
    protected ConfigurationException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
