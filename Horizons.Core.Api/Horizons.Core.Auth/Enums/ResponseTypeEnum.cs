﻿namespace Horizons.Core.Auth.Enums
{
    public enum ResponseTypeEnum
    {
        InvalidLogin,
        NotFound,
        EmailAlreadyExists,
        EmailNotConfirmed,
        EmailConfirmed,
        IncorrectPassword,
        LoginSuccess,
        LoginFailed,
        PasswordResetSuccess,
        PasswordResetFailed,
        PasswordResetTokenSent,
        PasswordResetTokenNotSent,
        PasswordResetTokenInvalid,
        PasswordResetTokenGenerated,
        InvalidPassword,
        InvalidToken,
        UserNotFound,
        UserCreated,
        UserNotCreated,
        UserUpdated,
        UserNotUpdated,
        UserDeleted,
        UserAlreadyExists,
        UserAddedToRole,
        UserNotAddedToRole,
        RoleCreated,
        RoleNotCreated,
        RoleAlreadyExists,
        RoleNotFound,
        RoleUpdated,
        RoleNotUpdated,
        RoleDeleted,
        PermissionCreated,
        PermissionAlreadyExists,
        PermissionNotFound,
        PermissionUpdated,
        PermissionNotUpdated,
        PermissionDeleted,
        InvalidModel
    }
}
