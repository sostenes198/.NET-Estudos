using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;
using Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Application.UseCases.ResetSignatures;
using Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Domain.Signatures.Repositories;
using Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Infrastructure.Repositories.Signatures;

namespace Estudos.CleanArchitecture.Modular.CrossCutting.IOC.Bootstrappers;

internal static class BootstrapperModuleSignatureAcl
{
    internal static IServiceCollection InitializeModuleSignatureACL(this IServiceCollection services)
    {
        return services
           .AddApplication()
           .AddInfrastructure();
    }

    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.TryAddScoped<IUseCase<ResetSignatureAclUseCaseInput, IResetSignatureAclOutputUseCase>, ResetSignatureAclUseCase>();

        services.AddSingleton<IValidator<ResetSignatureAclUseCaseInput>, ResetSignatureAclUseCaseInputValidator>();

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.TryAddScoped<ISignatureRepositoryAcl, SignatureRepositoryAcl>();

        return services;
    }
}