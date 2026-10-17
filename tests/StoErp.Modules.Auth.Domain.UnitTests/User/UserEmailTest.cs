using System;
using StoErp.Modules.Auth.Domain.User;

namespace StoErp.Modules.Auth.Domain.UnitTests.User;

public class UserEmailTest
{

    
    [Fact]
    public void UserEmail_ShouldNotAcceptEmptyEmail()
    {
        // Arrange
        var emptyEmail = string.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Email.From(emptyEmail));
        Assert.Equal("Email cannot be empty (Parameter 'value')", exception.Message);
    }
}
