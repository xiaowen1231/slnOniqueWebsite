namespace prjOniqueWebsite.Models.Infra
{
    public class LoginResult
    {
        public bool IsSuccess { get; private set; }
        public bool IsFail => !IsSuccess;

        public string ErrorMessage { get; private set; }

        public static LoginResult Success() => new LoginResult { IsSuccess = true, ErrorMessage = null };

        public static LoginResult Fail(string errorMessage)
            => new LoginResult { IsSuccess = false, ErrorMessage = errorMessage };
    }
}
