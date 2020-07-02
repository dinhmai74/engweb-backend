using System.Text.RegularExpressions;
using System;
namespace HollypocketBackend.Utils
{
    public class StringHelper
    {
        private static readonly StringHelper instance = new StringHelper();
        static StringHelper() { } // Make sure it's truly lazy
        private StringHelper() { } // Prevent instantiation outside

        public static StringHelper Instance { get { return instance; } }

        public bool IsEmailString(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            var rex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = rex.Match(s);
            return match.Success;
        }

        public string APIDeleteSuccess { get { return "Delete successfully!"; } }
        public string APIUpdateSuccess { get { return "Update successfully!"; } }
        public string APICreateSuccess { get { return "Create successfully!"; } }
        public string APIInvalidToken { get { return "Invalid token!"; } }
    }
}
