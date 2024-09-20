using Auth0.ManagementApi.Models;
using AutoMapper;
using Meta.Instagram.Bussines.Services;
using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;
using Meta.Instagram.Infrastructure.Entities;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Repositories;
using Meta.Instagram.Infrastructure.Services;
using Moq;

namespace Meta.Instagram.UnitTests.Tests
{
    public class AccountsTests
    {
        private readonly Mock<IAuthenticationService> _mockAuth0Service;
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IMapper> _mockMapper;

        public AccountsTests()
        {
            _mockAuth0Service = new Mock<IAuthenticationService>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        #region POST Account

        [Fact]
        public async Task PostAccountAsync_ShouldReturnAccountContract_WhenSuccessful()
        {
            // Arrange
            var request = new CreateAccountRequest { Phone = "1234567890" };
            var auth0User = new User();
            var domainAccount = new Account { Phone = request.Phone };
            var createdAccount = new Account { /* Initialize properties */ };
            var accountContract = new AccountContract { /* Initialize properties */ };

            _mockAuth0Service
                .Setup(x => x.CreateAuth0UserAsync(request))
                .ReturnsAsync(auth0User);
            _mockMapper
                .Setup(x => x.Map<Account>(auth0User))
                .Returns(domainAccount);
            _mockAccountRepository
                .Setup(x => x.CreateAccountAsync(domainAccount))
                .ReturnsAsync(createdAccount);
            _mockMapper
                .Setup(x => x.Map<AccountContract>(createdAccount))
                .Returns(accountContract);

            // Act
            var service = CreateTestService();
            var result = await service.PostAccountAsync(request);

            // Assert
            Assert.Equal(accountContract, result);
            _mockAuth0Service.Verify(x => x.CreateAuth0UserAsync(request), Times.Once);
            _mockMapper.Verify(x => x.Map<Account>(auth0User), Times.Once);
            _mockAccountRepository.Verify(x => x.CreateAccountAsync(domainAccount), Times.Once);
            _mockMapper.Verify(x => x.Map<AccountContract>(createdAccount), Times.Once);
        }

        [Fact]
        public async Task PostAccountAsync_ShouldThrowAuthenticationException_WhenAuth0Fails()
        {
            // Arrange
            var request = new CreateAccountRequest();
            _mockAuth0Service
                .Setup(x => x.CreateAuth0UserAsync(request))
                .ThrowsAsync(new AuthenticationException("Auth0 error"));

            // Act & Assert
            var service = CreateTestService();
            await Assert.ThrowsAsync<AuthenticationException>(() => service.PostAccountAsync(request));
        }

        [Fact]
        public async Task PostAccountAsync_ShouldThrowDatabaseException_WhenDatabaseFails()
        {
            // Arrange
            var request = new CreateAccountRequest();
            var auth0User = new User();
            _mockAuth0Service
                .Setup(x => x.CreateAuth0UserAsync(request))
                .ReturnsAsync(auth0User);
            _mockMapper
                .Setup(x => x.Map<Account>(auth0User))
                .Returns(new Account());
            _mockAccountRepository
                .Setup(x => x.CreateAccountAsync(It.IsAny<Account>()))
                .ThrowsAsync(new DatabaseException("Database error"));

            // Act & Assert
            var service = CreateTestService();
            await Assert.ThrowsAsync<DatabaseException>(() => service.PostAccountAsync(request));
        }

        #endregion

        #region CHANGE PASSWORD Account

        [Fact]
        public async Task ChangeAccountPasswordAsync_ShouldUpdatePassword_WhenAccountExists()
        {
            // Arrange
            var accountId = "123";
            var externalId = "auth0|externalId";
            var newPassword = "NewSecurePassword!";

            var request = new ChangeAccountPasswordRequest
            {
                AccountId = accountId,
                NewPassword = newPassword
            };

            var account = new Account
            {
                AccountId = accountId,
                ExternalId = externalId,
                UpdatedAt = DateTime.Now.AddDays(-1) // Set to some past date
            };

            _mockAccountRepository.Setup(repo => repo.GetAccountAsync(accountId))
                .ReturnsAsync(account);

            _mockAuth0Service.Setup(auth => auth.ChangeAuth0UserPasswordAsync(externalId, newPassword))
                .Returns(Task.CompletedTask);

            _mockAccountRepository.Setup(repo => repo.UpdateAccountAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);

            // Act
            var service = CreateTestService();
            await service.ChangeAccountPasswordAsync(request);

            // Assert
            _mockAccountRepository.Verify(repo => repo.GetAccountAsync(accountId), Times.Once);
            _mockAuth0Service.Verify(auth => auth.ChangeAuth0UserPasswordAsync(externalId, newPassword), Times.Once);
            _mockAccountRepository.Verify(repo => repo.UpdateAccountAsync(It.Is<Account>(a => a.UpdatedAt > DateTime.Now.AddSeconds(-1))), Times.Once);
        }

        [Fact]
        public async Task ChangeAccountPasswordAsync_ShouldThrowNotFoundException_WhenAccountDoesNotExist()
        {
            // Arrange
            var request = new ChangeAccountPasswordRequest
            {
                AccountId = "123",
                NewPassword = "NewSecurePassword!"
            };

            _mockAccountRepository.Setup(repo => repo.GetAccountAsync(request.AccountId))
                .ReturnsAsync((Account)null);

            // Act & Assert
            var service = CreateTestService();
            await Assert.ThrowsAsync<NotFoundException>(() => service.ChangeAccountPasswordAsync(request));

            _mockAccountRepository.Verify(repo => repo.GetAccountAsync(request.AccountId), Times.Once);
            _mockAuth0Service.Verify(auth => auth.ChangeAuth0UserPasswordAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockAccountRepository.Verify(repo => repo.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public async Task ChangeAccountPasswordAsync_ShouldThrowAuthenticationException_WhenAuthenticationFails()
        {
            // Arrange
            var accountId = "123";
            var externalId = "auth0|externalId";
            var newPassword = "NewSecurePassword!";

            var request = new ChangeAccountPasswordRequest
            {
                AccountId = accountId,
                NewPassword = newPassword
            };

            var account = new Account
            {
                AccountId = accountId,
                ExternalId = externalId
            };

            _mockAccountRepository.Setup(repo => repo.GetAccountAsync(accountId))
                .ReturnsAsync(account);

            _mockAuth0Service.Setup(auth => auth.ChangeAuth0UserPasswordAsync(externalId, newPassword))
                .ThrowsAsync(new AuthenticationException("Authentication failed."));

            // Act & Assert
            var service = CreateTestService();
            await Assert.ThrowsAsync<AuthenticationException>(() => service.ChangeAccountPasswordAsync(request));

            _mockAccountRepository.Verify(repo => repo.GetAccountAsync(accountId), Times.Once);
            _mockAuth0Service.Verify(auth => auth.ChangeAuth0UserPasswordAsync(externalId, newPassword), Times.Once);
            _mockAccountRepository.Verify(repo => repo.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);
        }

        #endregion

        #region GET Account

        [Fact]
        public async Task GetAccountAsync_AccountExists_ReturnsMappedAccount()
        {
            // Arrange
            var accountId = "test-id";
            var account = new Account { AccountId = accountId };
            var accountContract = new AccountContract { AccountId = accountId };

            _mockAccountRepository.Setup(repo => repo.GetAccountAsync(accountId))
                .ReturnsAsync(account);
            _mockMapper.Setup(mapper => mapper.Map<AccountContract>(account))
                .Returns(accountContract);

            // Act
            var service = CreateTestService();
            var result = await service.GetAccountAsync(accountId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountContract.AccountId, result.AccountId);

            _mockAccountRepository.Verify(repo => repo.GetAccountAsync(accountId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<AccountContract>(account), Times.Once);
        }

        [Fact]
        public async Task GetAccountAsync_AccountDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var accountId = "non-existent-id";

            _mockAccountRepository.Setup(repo => repo.GetAccountAsync(accountId))
                .ReturnsAsync((Account)null);

            // Act & Assert
            var service = CreateTestService();
            await Assert.ThrowsAsync<NotFoundException>(() => service.GetAccountAsync(accountId));

            _mockAccountRepository.Verify(repo => repo.GetAccountAsync(accountId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<AccountContract>(It.IsAny<Account>()), Times.Never);
        }

        #endregion

        #region DELETE Account

        [Fact]
        public async Task DeleteAccountAsync_AccountExists_DeletesAccountSuccessfully()
        {
            // Arrange
            var accountId = "test-id";
            var account = new Account { AccountId = accountId, ExternalId = "external-id" };

            _mockAccountRepository.Setup(repo => repo.GetAccountAsync(accountId))
                .ReturnsAsync(account);
            _mockAuth0Service.Setup(auth => auth.DeleteAuth0UserAsync(account.ExternalId!))
                .Returns(Task.CompletedTask);
            _mockAccountRepository.Setup(repo => repo.DeleteAccountAsync(account))
                .Returns(Task.CompletedTask);

            // Act
            var service = CreateTestService();
            await service.DeleteAccountAsync(accountId);

            // Assert
            _mockAccountRepository.Verify(repo => repo.GetAccountAsync(accountId), Times.Once);
            _mockAuth0Service.Verify(auth => auth.DeleteAuth0UserAsync(account.ExternalId!), Times.Once);
            _mockAccountRepository.Verify(repo => repo.DeleteAccountAsync(account), Times.Once);
        }

        [Fact]
        public async Task DeleteAccountAsync_AccountDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var accountId = "non-existent-id";

            _mockAccountRepository.Setup(repo => repo.GetAccountAsync(accountId))
                .ReturnsAsync((Account)null);

            // Act & Assert
            var service = CreateTestService();
            await Assert.ThrowsAsync<NotFoundException>(() => service.GetAccountAsync(accountId));

            _mockAccountRepository.Verify(repo => repo.GetAccountAsync(accountId), Times.Once);
            _mockAuth0Service.Verify(auth => auth.DeleteAuth0UserAsync(It.IsAny<string>()), Times.Never);
            _mockAccountRepository.Verify(repo => repo.DeleteAccountAsync(It.IsAny<Account>()), Times.Never);
        }

        #endregion

        #region UPDATE Account

        [Fact]
        public async Task UpdateAccountAsync_AccountExists_UpdatesAccountSuccessfully()
        {
            // Arrange
            var accountId = "test-id";
            var request = new ChanageAccountRequest { PhoneNumber = "123-456-7890" };
            var account = new Account { AccountId = accountId, Phone = "000-000-0000" };
            var updatedAccount = new Account { AccountId = accountId, Phone = request.PhoneNumber, UpdatedAt = DateTime.Now };
            var accountContract = new AccountContract { AccountId = accountId, Phone = request.PhoneNumber };

            _mockAccountRepository.Setup(repo => repo.GetAccountAsync(accountId))
                .ReturnsAsync(account);
            _mockAccountRepository.Setup(repo => repo.UpdateAccountAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);
            _mockMapper.Setup(mapper => mapper.Map<AccountContract>(It.IsAny<Account>()))
                .Returns(accountContract);

            // Act
            var service = CreateTestService();
            var result = await service.UpdateAccountAsync(accountId, request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountContract.AccountId, result.AccountId);
            Assert.Equal(accountContract.Phone, result.Phone);

            _mockAccountRepository.Verify(repo => repo.GetAccountAsync(accountId), Times.Exactly(2)); // Called twice, once before and once after update
            _mockAccountRepository.Verify(repo => repo.UpdateAccountAsync(It.Is<Account>(acc => acc.Phone == request.PhoneNumber)), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<AccountContract>(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAccountAsync_AccountDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var accountId = "non-existent-id";
            var request = new ChanageAccountRequest { PhoneNumber = "123-456-7890" };

            _mockAccountRepository.Setup(repo => repo.GetAccountAsync(accountId))
                .ReturnsAsync((Account)null);

            // Act & Assert
            var service = CreateTestService();
            await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAccountAsync(accountId, request));

            _mockAccountRepository.Verify(repo => repo.GetAccountAsync(accountId), Times.Once);
            _mockAccountRepository.Verify(repo => repo.UpdateAccountAsync(It.IsAny<Account>()), Times.Never);
            _mockMapper.Verify(mapper => mapper.Map<AccountContract>(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAccountAsync_CorrectAccountUpdated()
        {
            // Arrange
            var accountId = "test-id";
            var request = new ChanageAccountRequest { PhoneNumber = "123-456-7890" };
            var account = new Account { AccountId = accountId, Phone = "000-000-0000" };

            _mockAccountRepository.Setup(repo => repo.GetAccountAsync(accountId))
                .ReturnsAsync(account);
            _mockAccountRepository.Setup(repo => repo.UpdateAccountAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);

            // Act
            var service = CreateTestService();
            await service.UpdateAccountAsync(accountId, request);

            // Assert
            _mockAccountRepository.Verify(repo => repo.UpdateAccountAsync(It.Is<Account>(acc =>
                acc.Phone == request.PhoneNumber &&
                acc.UpdatedAt != default(DateTime))), Times.Once);
        }

        #endregion

        private AccountService CreateTestService()
        {
            return new AccountService(_mockAccountRepository.Object, _mockAuth0Service.Object, _mockMapper.Object);
        }
    }
}
