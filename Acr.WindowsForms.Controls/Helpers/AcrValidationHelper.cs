using Acr.WindowsForms.Controls.Enums;
using Acr.WindowsForms.Controls.Interfaces;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Acr.WindowsForms.Controls.Helpers;

public static class AcrValidationHelper
{
    public static void ValidateRequired(IAcrValidatableControl control, bool blockLeave, CancelEventArgs? e = null)
    {
        if (!control.RequiredField)
            return;

        if (control.IsControlEmpty)
        {
            control.ShowRequiredFieldError();
            if (blockLeave && e != null)
                e.Cancel = true;
        }
        else control.ClearError();
    }

    /// <summary>
    /// Valida recursivamente todos os controles Acr* (que implementam IAcrValidatableControl)
    /// dentro de um container (Form, Panel, GroupBox, etc).
    /// </summary>
    /// <returns>true se todos os campos obrigatórios estiverem preenchidos.</returns>
    public static bool ValidateForm(Control container, bool blockLeave = false)
    {
        bool isValid = true;

        foreach (Control child in container.Controls)
        {
            if (child is IAcrValidatableControl validatable)
            {
                ValidateRequired(validatable, blockLeave);
                if (validatable.RequiredField && validatable.IsControlEmpty)
                    isValid = false;
            }

            if (child.HasChildren && !ValidateForm(child, blockLeave))
                isValid = false;
        }

        return isValid;
    }

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled);

    public static bool IsValidEmail(string? text)
    {
        return !string.IsNullOrWhiteSpace(text) && EmailRegex.IsMatch(text.Trim());
    }

    public static bool IsValidPhone(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;
        string digits = new(text.Where(char.IsDigit).ToArray());
        return digits.Length is 10 or 11;
    }

    public static bool IsValidPattern(string? text, string pattern)
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(pattern)) return false;
        return Regex.IsMatch(text, pattern);
    }

    public static bool IsValidCpf(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        string digits = new(cpf.Where(char.IsDigit).ToArray());
        if (digits.Length != 11 || digits.Distinct().Count() == 1) return false;

        int[] numbers = digits.Select(c => c - '0').ToArray();

        int CalcDigit(int count)
        {
            int sum = 0;
            for (int i = 0; i < count; i++)
                sum += numbers[i] * (count + 1 - i);

            int remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        return CalcDigit(9) == numbers[9] && CalcDigit(10) == numbers[10];
    }

    public static bool IsValidCnpj(string? cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj)) return false;

        string digits = new(cnpj.Where(char.IsDigit).ToArray());
        if (digits.Length != 14 || digits.Distinct().Count() == 1) return false;

        int[] numbers = digits.Select(c => c - '0').ToArray();
        int[] firstWeights = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] secondWeights = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        int CalcDigit(int[] weights, int count)
        {
            int sum = 0;
            for (int i = 0; i < count; i++)
                sum += numbers[i] * weights[i];

            int remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        return CalcDigit(firstWeights, 12) == numbers[12] && CalcDigit(secondWeights, 13) == numbers[13];
    }

    public static bool IsValidByType(string? text, TextValidationType type, string? customPattern = null)
    {
        return type switch
        {
            TextValidationType.None => true,
            TextValidationType.Email => IsValidEmail(text),
            TextValidationType.Cpf => IsValidCpf(text),
            TextValidationType.Cnpj => IsValidCnpj(text),
            TextValidationType.Phone => IsValidPhone(text),
            TextValidationType.Custom => IsValidPattern(text, customPattern ?? string.Empty),
            _ => true
        };
    }
}
