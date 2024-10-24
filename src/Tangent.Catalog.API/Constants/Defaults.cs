namespace Tangent.Catalog.API.Constants;

public class Defaults
{
    public const string API_VERSION = "v1";
    public const string HEADER_VERSION_NAME = "x-api-version";
    public const string INCORRECT_VERSION_ERROR_MESSAGE = "x-api-version is missing or incorrect";
    public const string INTERNAL_SERVER_ERROR_MESSAGE = "An internal server error occured.";

    public const string NOT_FOUNT_ERROR_MESSAGE = "No entry was found for the provided ID";
    public const string NOT_AUTHORIZED_ERROR_MESSAGE = "Not Authorized: Unable to find valid Subscriber for supplied auth token";
}
