using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Interfaces;
using TravelReclaim.Application.Invoices.Commands;
using TravelReclaim.Application.Invoices.Queries;
using TravelReclaim.Application.Reclaims.Commands;
using TravelReclaim.Application.Reclaims.Queries;
using TravelReclaim.Application.Services;
using TravelReclaim.Application.Validators;
using TravelReclaim.Domain.Interfaces;
using TravelReclaim.Infrastructure.Persistence.SqlServer.Repositories;

namespace TravelReclaim.Application.Extensions
{
    public static class ApplicationExtension
    {
        public static void AddApplicationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            // Repositories
            serviceCollection.AddScoped<IInvoiceRepository, InvoiceRepository>();
            serviceCollection.AddScoped<IReclaimRepository, ReclaimRepository>();

            // Services
            serviceCollection.AddScoped<IAuditService, AuditService>();

            // Validation Rules - Strategy pattern
            // OCP: add new rule = one class + one registration
            serviceCollection.RegisterInvoiceValidators(
                typeof(AmountThresholdRule),
                typeof(DuplicateInvoiceRule),
                typeof(InvoiceAgeRule),
                typeof(VatRatioRule)
            );

            // CQRS Handlers - Invoice Commands
            serviceCollection.AddScoped<ICommandHandler<CreateInvoiceCommand, InvoiceResponse>, CreateInvoiceHandler>();

            // CQRS Handlers - Invoice Queries
            serviceCollection.AddScoped<IQueryHandler<GetInvoiceByIdQuery, InvoiceResponse>, GetInvoiceByIdHandler>();
            serviceCollection.AddScoped<IQueryHandler<GetInvoicesPagedQuery, PagedResponse<InvoiceResponse>>, GetInvoicesPagedHandler>();

            // CQRS Handlers — Reclaim Commands
            serviceCollection.AddScoped<ICommandHandler<ProcessReclaimCommand, ProcessReclaimResponse>, ProcessReclaimHandler>();
            serviceCollection.AddScoped<ICommandHandler<ApproveReclaimCommand, ReclaimResponse>, ApproveReclaimHandler>();
            serviceCollection.AddScoped<ICommandHandler<RejectReclaimCommand, ReclaimResponse>, RejectReclaimHandler>();

            // CQRS Handlers — Reclaim Queries
            serviceCollection.AddScoped<IQueryHandler<GetReclaimByIdQuery, ReclaimResponse>, GetReclaimByIdHandler>();
            serviceCollection.AddScoped<IQueryHandler<GetReclaimsPagedQuery, PagedResponse<ReclaimResponse>>, GetReclaimsPagedHandler>();
        }

        private static IServiceCollection RegisterInvoiceValidators(this IServiceCollection services, params Type[] validatorTypes)
        {
            foreach (var type in validatorTypes)
            {
                services.AddScoped(typeof(IValidationRule), type);
            }

            return services;
        }
    }
}