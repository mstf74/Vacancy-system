namespace Vacancy_system.Helpers
{
    public static class CreateVaidationErrorDetails
    {
        public static HttpValidationProblemDetails CreateVaidationDetails(
            string code,
            string Message
        )
        {
            var error = new Dictionary<string, string[]> { { code, new[] { Message } } };
            return new HttpValidationProblemDetails(error);
        }
    }
}
