# Changelog

## 1.5.0

### Novidades
- **Tema claro/escuro** — `AcrTheme` (`Light`, `Dark` ou personalizado com `AcrTheme.Light with { Primary = ... }`).
  Trocar `AcrTheme.Current` atualiza todas as janelas abertas e preserva cores personalizadas.
- **Novos componentes**: `AcrDatePicker`, `AcrNumericUpDown`, `AcrTabControl`, `AcrToolTip`.
- **Toast com ação**: `NotificationHelper.ShowWithAction("Item excluído", "Desfazer", () => ...)`.
- **AcrModal**: escurece o resto da tela (`DimBackground`, `OverlayOpacity`, `OverlayColor`, `ShowModal`); Esc/Enter.
- **AcrTextBox**: novo visual `TextBoxStyle` (Outlined, Underline, Classic), `BorderRadius`, `HorizontalPadding`,
  `VerticalPadding`, cores de borda e borda de erro (`HasError`).
- **AcrButton**: variantes (Primary, Secondary, Outline, Ghost, Danger, Success) e `IsLoading`.
- **Notificações**: título, duração, pausa ao passar o mouse, barra de tempo, `NotificationHelper.Success/Error/Warning/Info`.
- **AcrDataGridView**: destaque da linha no hover, `EmptyText`, double buffering.
- **AcrCard**: `AccentColor`, `TitleColor`, `BorderWidth`, `HoverEffect`, `CardClick`.
- **AcrDropdownMenu**: teclado, separadores, itens desabilitados, rolagem, sombra, evento `Closed`.
- **AcrAccordion**: animação, `CollapseSiblings`, cores do cabeçalho, teclado, `Expand/Collapse/Toggle`.
- **Suporte a DPI alto**: medidas internas escalam com o zoom do Windows (125%, 150%...).
- **Designer**: ícones na Toolbox e evento padrão no duplo clique em todos os controles.
- `AcrFonts`: fontes compartilhadas (sem criar uma `Font` por controle).

### Correções
- `LabelHelper`: não trava quando o controle ainda não tem pai; mensagens acompanham o controle
  (mover/ocultar/remover) e não se empilham mais.
- `AcrTextBox`: a fonte definida no Designer não é mais sobrescrita.
- Notificações: animação de entrada corrigida; aparecem no monitor da janela ativa.
- `AcrCard`: definir `Title` não apaga mais o `Padding` superior.

### Infra
- Build e testes automáticos no GitHub Actions; projeto de testes xUnit (`AcrValidationHelper`).

### Observações de compatibilidade
- `AcrColors` agora expõe propriedades (leem o tema atual) em vez de campos `readonly`: recompile os projetos que usam a biblioteca.
- `NotificationType.Success` foi adicionado; `Sucess` continua funcionando.
