using Estudos.CleanArchitecture.Modular.API.Presenters.Modules.Signature.ACL.ResetSignature;
using Estudos.CleanArchitecture.Modular.API.Presenters.Modules.Signature.MigrateSignature;
using Estudos.CleanArchitecture.Modular.API.Presenters.Modules.Signature.ResetSignature;
using Estudos.CleanArchitecture.Modular.CrossCutting.IOC;
using Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Application.UseCases.ResetSignatures;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.MigrateSignature;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.ResetSignature;

namespace Estudos.CleanArchitecture.Modular.API.Presenters;

internal static class DependencyInjection
{
    internal static IServiceCollection AddPresenters(this IServiceCollection services)
    {
        return services
           .AddModuleSignature()
           .AddModuleSignatureACL();
    }

    private static IServiceCollection AddModuleSignature(this IServiceCollection services)
    {
        services.AddPresenter<IMigrateSignatureOutputUseCase, MigrateSignatureOutputPresenter>();
        services.AddPresenter<IResetSignatureOutputUseCase, ResetSignatureOutputPresenter>();

        return services;
    }

    private static IServiceCollection AddModuleSignatureACL(this IServiceCollection services)
    {
        services.AddPresenter<IResetSignatureAclOutputUseCase, ResetSignatureAclOutputPresenter>();

        return services;
    }
}