using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelReclaim.Application.DTOs;

namespace TravelReclaim.Application.Invoices.Queries;

public record GetInvoiceByIdQuery(Guid Id) : IQuery<InvoiceResponse>;
