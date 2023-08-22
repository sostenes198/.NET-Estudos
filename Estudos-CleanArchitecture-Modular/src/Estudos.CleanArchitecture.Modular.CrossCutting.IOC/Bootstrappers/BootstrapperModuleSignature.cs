using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.MigrateSignature;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.ResetSignature;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Domain.Signatures.Repositories;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Infrastructure.Repositories.Signatures;

namespace Estudos.CleanArchitecture.Modular.CrossCutting.IOC.Bootstrappers;

internal static class BootstrapperModuleSignature
{
    internal static IServiceCollection InitializeModuleSignature(this IServiceCollection services)
    {
        return services
           .AddApplication()
           .AddInfrastructure();
    }

    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.TryAddScoped<IUseCase<MigrateSignatureUseCaseInput, IMigrateSignatureOutputUseCase>, MigrateSignatureUseCase>();
        services.TryAddScoped<IUseCase<ResetSignatureUseCaseInput, IResetSignatureOutputUseCase>, ResetSignatureUseCase>();

        services.AddSingleton<IValidator<MigrateSignatureUseCaseInput>, MigrateSignatureUseCaseInputValidator>();
        services.AddSingleton<IValidator<ResetSignatureUseCaseInput>, ResetSignatureUseCaseInputValidator>();

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.TryAddScoped<ISignatureRepository, SignatureRepository>();

        return services;
    }
}