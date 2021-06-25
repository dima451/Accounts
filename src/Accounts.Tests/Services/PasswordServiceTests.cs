using System.Threading.Tasks;
using Accounts.Services;
using Xunit;

namespace Accounts.Tests.Services
{
    public class PasswordServiceTests
    {
        [Fact]
        public void EncryptDecryptTest()
        {
            var passwordService = new PasswordService();
            string pass = "123456789qwerty";

            var encrypted =  passwordService.Encrypt(pass);

            var result = passwordService.Decrypt(encrypted);

            Assert.Equal(pass, result);

        }

        [Theory]
        [InlineData("123")]
        [InlineData("!Qq1")]
        public async Task ValidationBadTest(string password)
        {
            var passwordService = new PasswordService();

            var result = await passwordService.ValidateAsync(password);

            Assert.False(result.Succeeded);
        }

        [Theory]
        [InlineData("@123Qe")]
        [InlineData("!@#qweQWE123")]
        public async Task ValidationOkTest(string password)
        {
            var passwordService = new PasswordService();

            var result = await passwordService.ValidateAsync(password);
            
            Assert.True(result.Succeeded);
        }
    }
}