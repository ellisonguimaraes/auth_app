using System;
using AuthApp.Domain.Api;
using AuthApp.Domain.Utils;
using Refit;

namespace AuthApp.Services.HttpClients;

public interface IEgressHttpClient
{
	[Get("/egress")]
	Task<ApiResponse<GenericHttpResponse<PersonApiResponse>>> GetPersonByDocumentAsync([Query][AliasAs("document")] string document, [Query][AliasAs("document_type")] byte documentType);
}

