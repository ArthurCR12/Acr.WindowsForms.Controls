namespace Acr.WindowsForms.Controls.Interfaces;

public interface IAcrValidatableControl
{
    bool BlockLeave { get; set; }
    bool RequiredField { get; set; }
    string WarningMessageRequiredField { get; set; }

    /// <summary>
    /// Retorna se o controle está "vazio"
    /// </summary>
    bool IsControlEmpty { get; }

    /// <summary>
    /// Mostra a mensagem de erro (Normalmente vai usar o LabelHelper)
    /// </summary>
    void ShowRequiredFieldError();

    /// <summary>
    /// Remove a mensagem de erro
    /// </summary>
    void ClearError();
}
