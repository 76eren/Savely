namespace Application.Auth.DTOs;

public sealed class AuthResult
{
    private AuthResult(bool succeeded, AuthTokensDto? tokens, string? errorCode, string? errorDescription)
    {
        Succeeded = succeeded;
        Tokens = tokens;
        ErrorCode = errorCode;
        ErrorDescription = errorDescription;
    }

    public bool Succeeded { get; }
    public AuthTokensDto? Tokens { get; }
    public string? ErrorCode { get; }
    public string? ErrorDescription { get; }

    public static AuthResult Success(AuthTokensDto tokens)
    {
        return new AuthResult(true, tokens, null, null);
    }

    public static AuthResult Failure(string errorCode, string errorDescription)
    {
        return new AuthResult(false, null, errorCode, errorDescription);
    }
}

