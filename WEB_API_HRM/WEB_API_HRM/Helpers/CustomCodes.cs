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
        public const int NotFound = 1010;
        public const int Exists = 1011;
        public const int DuplicateEmail = 1012;
        public const int DuplicatePhoneNumber = 1013;
        public const int DuplicateIdentification = 1014;
        public const int InvalidPhoneNumber = 1015;
        public const int InvalidIdentification = 1014;
        public const int BranchNotFound = 1015;
        public const int DepartmentNotFound = 1016;
        public const int JobtitleNotFound = 1017;
        public const int RankNotFound = 1018;
        public const int PositionNotFound = 1019;
        public const int ManagerNotFound = 1020;
        public const int JobTypeNotFound = 1021;
        public const int EmployeeNotFound = 1022;
        public const int DuplicatePersonel = 1023;
        public const int DuplicateContract = 1024;
    }
}