
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Application.Ports.Persistence.Projections;
using StoErp.Modules.Auth.Application.Ports.Security;
using StoErp.Modules.Auth.Domain.RefreshToken;

namespace StoErp.Modules.Auth.Application.Commands.Login;

public sealed class LoginService : ILoginService
{
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenWriteRepository _refreshTokenWriteRepository;
    private readonly IRefreshTokenHasher _refreshTokenHasher;
    private readonly TokenLifeTimeOptions _tokenLifeTimeOptions;
    private readonly IUnitOfWork _unitOfWork;
    public LoginService(
                            IAccessTokenGenerator accessTokenGenerator,
                            IRefreshTokenGenerator refreshTokenGenerator,
                            IRefreshTokenWriteRepository refreshTokenWriteRepository,
                            IPasswordHasher passwordHasher,
                            IRefreshTokenHasher refreshTokenHasher,
                            TokenLifeTimeOptions tokenLifeTimeOptions,
                            IUnitOfWork unitOfWork
                            )
    {
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenWriteRepository = refreshTokenWriteRepository;
        _passwordHasher = passwordHasher;
        _refreshTokenHasher = refreshTokenHasher;
        _tokenLifeTimeOptions = tokenLifeTimeOptions;
        _unitOfWork = unitOfWork;

    }
    public async Task<LoginResult> LoginAsync(UserLoginData user, string password, CancellationToken cancellationToken = default)
    {
        var isPasswordValid = _passwordHasher.Verify(password, user.PasswordHash);


        if (!isPasswordValid)
        {
            throw new InvalidOperationException("Invalid password.");
        }
        if (!user.Verified)
        {
            throw new InvalidOperationException("User account is not verified.");
        }
        if (user.DeletedAt.HasValue)
        {
            throw new InvalidOperationException("User account is deleted.");
        }
        var accessToken = _accessTokenGenerator.GenerateAccessToken(user, _tokenLifeTimeOptions.AccessTokenLifeTime, cancellationToken);
        var refreshTokenSecret = _refreshTokenGenerator.Generate();
        var refreshTokenId = RefreshTokenId.From(Guid.CreateVersion7());
        var refreshTokenHash = RefreshTokenHash.From(_refreshTokenHasher.Hash(refreshTokenSecret.Value).Value);
        var refreshToken = RefreshToken.Create(refreshTokenId, user.UserId, refreshTokenHash, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.Add(_tokenLifeTimeOptions.RefreshTokenLifeTime));
        _refreshTokenWriteRepository.AddRefreshTokenToUser(user.UserId, refreshToken, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        var loginResult = new LoginResult(accessToken.AccessToken, refreshTokenSecret.Value, accessToken.ExpiresAt, refreshToken.ExpiresAt);
        return loginResult;
    }
}
