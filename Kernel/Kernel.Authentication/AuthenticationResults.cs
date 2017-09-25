namespace Kernel.Authentication
{
    public enum AuthenticationResults
	{
		// all good
		Success,
		// active, but never logged in with before
		//SuccessFirstLogin,
		// The user's password has expired or is a temp/system generated password
		//SuccessMustChangePassword,
		// username and password are incorrect
		FailInvalidCredentials,
		// username is incorrect
		FailInvalidUsername,
		// password is incorrect
		FailInvalidPassword,
		// locked - usually from 3x login failure
		//FailLocked,
		// admin has temporarily suspended the account
		//FailSuspended,
		// the acount has been terminated and can no longer be used
		//FailTerminated,
		// the account is awaiting manager approval
		//FailAwaitingApproval,
		// the user is already singed in under a different session
		//AlreadySignedIn,
	}
}