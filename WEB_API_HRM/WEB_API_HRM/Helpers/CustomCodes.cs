namespace WEB_API_HRM.Helpers
{
    public class CustomCodes
    {
        public const int UsernameExists = 1002;       // Username đã tồn tại
        public const int InvalidEmail = 1003;         // Email không hợp lệ
        public const int RoleNotFound = 1004;         // Role không tồn tại
        public const int InvalidCredentials = 1005;   // Sai username hoặc password
        public const int RoleExists = 1006;           // Role đã tồn tại
        public const int ModuleNotFound = 1007;       // Module không tồn tại
        public const int ActionNotFound = 1008;       // Action không tồn tại
        public const int UserNotFound = 1009;         // User không tồn tại
        public const int InvalidRequest = 5; 
        public const int InvalidToken = 6;
    }
}