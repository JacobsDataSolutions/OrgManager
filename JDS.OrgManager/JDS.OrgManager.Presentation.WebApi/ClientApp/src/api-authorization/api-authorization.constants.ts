// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
export const ApplicationName = "OrgManager";

export const ReturnUrlType = "returnUrl";

export const QueryParameterNames = {
  ReturnUrl: ReturnUrlType,
  Message: "message"
};

export const LogoutActions = {
  LogoutCallback: "logout-callback",
  Logout: "logout",
  LoggedOut: "logged-out"
};

export const LoginActions = {
  Login: "login",
  LoginCallback: "login-callback",
  LoginFailed: "login-failed",
  Profile: "profile",
  Register: "register"
};

let applicationPaths: ApplicationPathsType = {
  DefaultLoginRedirectPath: "/",
  ApiAuthorizationClientConfigurationUrl: `/_configuration/${ApplicationName}`,
  Login: `authentication/${LoginActions.Login}`,
  LoginFailed: `authentication/${LoginActions.LoginFailed}`,
  LoginCallback: `authentication/${LoginActions.LoginCallback}`,
  Register: `authentication/${LoginActions.Register}`,
  Profile: `authentication/${LoginActions.Profile}`,
  LogOut: `authentication/${LogoutActions.Logout}`,
  LoggedOut: `authentication/${LogoutActions.LoggedOut}`,
  LogOutCallback: `authentication/${LogoutActions.LogoutCallback}`,
  LoginPathComponents: [],
  LoginFailedPathComponents: [],
  LoginCallbackPathComponents: [],
  RegisterPathComponents: [],
  ProfilePathComponents: [],
  LogOutPathComponents: [],
  LoggedOutPathComponents: [],
  LogOutCallbackPathComponents: [],
  IdentityRegisterPath: "/Identity/Account/Register",
  IdentityManagePath: "/Identity/Account/Manage"
};

applicationPaths = {
  ...applicationPaths,
  LoginPathComponents: applicationPaths.Login.split("/"),
  LoginFailedPathComponents: applicationPaths.LoginFailed.split("/"),
  RegisterPathComponents: applicationPaths.Register.split("/"),
  ProfilePathComponents: applicationPaths.Profile.split("/"),
  LogOutPathComponents: applicationPaths.LogOut.split("/"),
  LoggedOutPathComponents: applicationPaths.LoggedOut.split("/"),
  LogOutCallbackPathComponents: applicationPaths.LogOutCallback.split("/")
};

interface ApplicationPathsType {
  readonly DefaultLoginRedirectPath: string;
  readonly ApiAuthorizationClientConfigurationUrl: string;
  readonly Login: string;
  readonly LoginFailed: string;
  readonly LoginCallback: string;
  readonly Register: string;
  readonly Profile: string;
  readonly LogOut: string;
  readonly LoggedOut: string;
  readonly LogOutCallback: string;
  readonly LoginPathComponents: string[];
  readonly LoginFailedPathComponents: string[];
  readonly LoginCallbackPathComponents: string[];
  readonly RegisterPathComponents: string[];
  readonly ProfilePathComponents: string[];
  readonly LogOutPathComponents: string[];
  readonly LoggedOutPathComponents: string[];
  readonly LogOutCallbackPathComponents: string[];
  readonly IdentityRegisterPath: string;
  readonly IdentityManagePath: string;
}

export const ApplicationPaths: ApplicationPathsType = applicationPaths;
