
using System;
using System.Security.Principal;
using System.Security.Permissions;
using System.Runtime.InteropServices;


[assembly: SecurityPermissionAttribute(SecurityAction.RequestMinimum, UnmanagedCode = true)]
[assembly: PermissionSetAttribute(SecurityAction.RequestMinimum, Name = "FullTrust")]


namespace AdImpersonate
{
    /// <summary>
    /// Updates Current Thread to run as the specified user
    /// </summary>

    //if we see other DataProviders need AD access to servers, move this class to Platform
    public class ADImpersonate : IDisposable
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        const int LOGON32_PROVIDER_DEFAULT = 0;
        const int LOGON32_LOGON_INTERACTIVE = 2;

        private WindowsIdentity _id;
        private WindowsImpersonationContext _context;
        private IntPtr userToken;

        public ADImpersonate(string domain, string username, string password)
        {
            userToken = IntPtr.Zero;

            // Call LogonUser to obtain a handle to an access token.
            bool returnValue = LogonUser(username, domain, password,
                LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                ref userToken);

            if (!returnValue)
            {
                // Logon failure
                int ret = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(ret);
            }

            _id = new WindowsIdentity(userToken);
            _context = _id.Impersonate();

        }

        public void Dispose()
        {
            _context.Undo(); // Stop impersonating the thread.

            _id.Dispose();
            _context.Dispose();
        }
    }
}
