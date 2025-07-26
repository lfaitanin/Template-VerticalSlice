using MediatR;
using Shared.Domain.Entities.Enum;
using Shared.Domain.Entities.Response;
using WebAPI.Features.PreRegistration.Commands;
using WebAPI.Infrastructure.Repository;
using UserPreRegistrationModel = WebAPI.Features.PreRegistration.Models.UserPreRegistration;
using ExcelDataReader;

namespace WebAPI.Features.PreRegistration.Handlers
{
    public class ImportUserPreRegistrationsHandler : IRequestHandler<ImportUserPreRegistrationsCommand, BaseResponse>
    {
        private readonly IEntubaRepository _repository;
        private readonly ILogger<ImportUserPreRegistrationsHandler> _logger;

        public ImportUserPreRegistrationsHandler(IEntubaRepository repository,
            ILogger<ImportUserPreRegistrationsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(ImportUserPreRegistrationsCommand request, CancellationToken cancellationToken)
        {
            List<UserPreRegistrationModel> usersSuccess = [];
            List<UserPreRegistrationModel> usersError = [];

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var stream = request.File.OpenReadStream();
            using var reader = ExcelReaderFactory.CreateReader(stream);

            // Ignore first line (header) if necessary
            reader.Read();
            while (reader.Read())
            {
                var user = new UserPreRegistrationModel();
                try
                {
                    _logger.LogInformation("Start PreRegistration import");

                    user.documentNumber = reader.GetString(0);
                    if (string.IsNullOrEmpty(user.documentNumber))
                        continue;

                    if (await _repository.Exists(user.documentNumber))
                    {
                        usersError.Add(user);
                        continue;
                    }

                    user.name = reader.GetString(1);
                    var profileName = reader.GetString(2); 
                    var school = reader.GetString(3);

                    // Convert profile to ProfileEnum
                    if (Enum.TryParse<ProfileEnum>(profileName, true, out var parsedProfile))
                    {
                        user.profileId = parsedProfile;
                    }
                    else
                    {
                        usersError.Add(user);
                        continue;
                    }

                    //START DISTINGUISHING RELATIONSHIPS BASED ON PROFILE
                    switch (user.profileId)
                    {
                        case ProfileEnum.User:
                            break;
                        case ProfileEnum.Administrator:
                            break;
                        default:
                            break;
                    }
                    usersSuccess.Add(user);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error: {documentNumber}", user?.documentNumber);
                    usersError.Add(user);
                }
            }
            if (usersError.Count > 0)
                return new BaseResponse(false, "Import failed", usersError);

            if (usersSuccess.Count > 0)
                await _repository.AddRangeAsync(usersSuccess);

            _logger.LogInformation("End PreRegistration import");
            return new BaseResponse(true, "Success", usersSuccess);
        }
    }
} 