using System.Numerics;
using System;

namespace Bidfood.Common
{
    public static class Constants
    {
        // log message
        public const string InvalidUserRequestErrorLog = "Invalid user detail";
        public const string UserRegistrationSuccessInfoLog = "User detail has been successfully stored into json file";
        public const string UnhandledExceptionErrorLog = "Unhandled exception occured while user registration";
        public const string FileProcessUnhandledExceptionErrorLog = "Unhandled exception occured while storing user details into json file";

        // error message
        public const string InvalidUserRequestErrorMessage = "Please enter valid user detail";
        public const string UnhandledExceptionMessage = "Unhandled exception occurred, please try again later";

        //error code
        public const string InvalidUserRequestErrorCode = "UserRegistration-Error001";
        public const string FileProcessErrorCode = "UserRegistration-Error002";
        public const string UnhandledExceptionErrorCode = "UserRegistration-Error003";
    }
}
