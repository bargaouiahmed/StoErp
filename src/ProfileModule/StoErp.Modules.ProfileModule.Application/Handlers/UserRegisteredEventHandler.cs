
using StoErp.Modules.ProfileModule.Application.Persistence;
using StoErp.Modules.ProfileModule.Application.Persistence.Repositories;
using StoErp.Modules.ProfileModule.Domain.ProfileEntity;
using StoErp.Shared.Application.Events;
using StoErp.Shared.Contracts.AuthModuleContracts;

namespace StoErp.Modules.ProfileModule.Application.Handlers;

public class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
{   
    private readonly IProfileWriteRepository _profileWriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UserRegisteredEventHandler(IProfileWriteRepository profileWriteRepository, IUnitOfWork unitOfWork)
    {
        _profileWriteRepository = profileWriteRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task HandleAsync(UserRegisteredEvent eventMessage, CancellationToken cancellationToken = default)
    {   
        if(await _profileWriteRepository.ExistsByUserIdAsync(UserId.From(eventMessage.UserId), cancellationToken))
        {
            return;
        }
        var profile = Profile.Create(
            UserId.From(eventMessage.UserId),
            Firstname.From(eventMessage.Firstname),
            Lastname.From(eventMessage.Lastname),
            eventMessage.DateOfBirth,
            LanguagePreference.From(eventMessage.UserLanguagePreference)
        );

        _profileWriteRepository.Add(profile);
        await _unitOfWork.CommitAsync(cancellationToken);
        
    }
}
