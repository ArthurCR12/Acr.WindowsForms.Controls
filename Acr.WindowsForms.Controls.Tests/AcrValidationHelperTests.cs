using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Helpers;
using Xunit;

namespace Acr.WindowsForms.Controls.Tests;

public class AcrValidationHelperTests
{
    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    [InlineData("111.444.777-35")]
    public void IsValidCpf_ValidNumbers_ReturnsTrue(string cpf) =>
        Assert.True(AcrValidationHelper.IsValidCpf(cpf));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("529.982.247-26")]
    [InlineData("111.111.111-11")]
    [InlineData("1234567890")]
    public void IsValidCpf_InvalidNumbers_ReturnsFalse(string? cpf) =>
        Assert.False(AcrValidationHelper.IsValidCpf(cpf));

    [Theory]
    [InlineData("11.222.333/0001-81")]
    [InlineData("11222333000181")]
    public void IsValidCnpj_ValidNumbers_ReturnsTrue(string cnpj) =>
        Assert.True(AcrValidationHelper.IsValidCnpj(cnpj));

    [Theory]
    [InlineData(null)]
    [InlineData("11.222.333/0001-82")]
    [InlineData("00.000.000/0000-00")]
    [InlineData("1122233300018")]
    public void IsValidCnpj_InvalidNumbers_ReturnsFalse(string? cnpj) =>
        Assert.False(AcrValidationHelper.IsValidCnpj(cnpj));

    [Theory]
    [InlineData("arthur@acr.com", true)]
    [InlineData(" nome.sobrenome@empresa.com.br ", true)]
    [InlineData("sem-arroba.com", false)]
    [InlineData("a@b", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidEmail(string? email, bool expected) =>
        Assert.Equal(expected, AcrValidationHelper.IsValidEmail(email));

    [Theory]
    [InlineData("(11) 98765-4321", true)]
    [InlineData("(11) 3456-7890", true)]
    [InlineData("12345", false)]
    [InlineData(null, false)]
    public void IsValidPhone(string? phone, bool expected) =>
        Assert.Equal(expected, AcrValidationHelper.IsValidPhone(phone));

    [Theory]
    [InlineData("ABC-1234", @"^[A-Z]{3}-\d{4}$", true)]
    [InlineData("abc-1234", @"^[A-Z]{3}-\d{4}$", false)]
    [InlineData("ABC-1234", "", false)]
    public void IsValidPattern(string text, string pattern, bool expected) =>
        Assert.Equal(expected, AcrValidationHelper.IsValidPattern(text, pattern));

    [Theory]
    [InlineData(TextValidationType.None, "qualquer coisa", true)]
    [InlineData(TextValidationType.Cpf, "529.982.247-25", true)]
    [InlineData(TextValidationType.Cnpj, "11.222.333/0001-81", true)]
    [InlineData(TextValidationType.Email, "invalido", false)]
    public void IsValidByType(TextValidationType type, string text, bool expected) =>
        Assert.Equal(expected, AcrValidationHelper.IsValidByType(text, type));
}
