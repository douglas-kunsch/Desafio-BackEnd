namespace MotoRent.Api.Endpoints
{
	public static class RootEndpoints
	{
		public static void MapRootEndpoints(this IEndpointRouteBuilder app)
		{
			app.MapGet("/", GetRoot)
			   .WithTags("root");
		}

		private static IResult GetRoot()
		{
			string html = """
            <html>
                <head>
                    <meta charset="utf-8">
                    <title>MotoRent API</title>
                </head>
                <body style="font-family: sans-serif; text-align: center; margin-top: 50px;">
                    <h1>MotoRent API</h1>
                    <p>Bem-vindo à API da MotoRent.</p>
                    <p><a href="/swagger" style="font-size: 18px;">Acesse a documentação Swagger aqui</a></p>
                </body>
            </html>
            """;

			return Results.Content(html, "text/html; charset=utf-8");
		}
	}

}
