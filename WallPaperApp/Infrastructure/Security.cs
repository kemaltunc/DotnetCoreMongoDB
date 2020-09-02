using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;

namespace WallPaperApp.Infrastructure {
    public class Security {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly string _key;

        public Security (IDataProtectionProvider dataProtectionProvider, IConfiguration configuration) {
            _dataProtectionProvider = dataProtectionProvider;
            _key = configuration.GetValue<string> ("jwtTokenConfig:secret");
        }

        public string Encrypt (string input) {
            var protector = _dataProtectionProvider.CreateProtector (_key);
            return protector.Protect (input);
        }

        public string Decrypt (string input) {
            var protector = _dataProtectionProvider.CreateProtector (_key);
            return protector.Unprotect (input);
        }
    }
}