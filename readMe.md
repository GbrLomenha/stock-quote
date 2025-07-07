# Stock Quote Alert Service

Este projeto é uma aplicação de console desenvolvida em C# que monitora a cotação de um ativo da B3 (Bolsa de Valores do Brasil) e envia alertas por e-mail caso o preço caia abaixo de um ponto de compra especificado ou suba acima de um ponto de venda definido.

## Índice

- Funcionalidades
- Tecnologias Utilizadas
- Como Começar
  - Pré-requisitos
  - Instalação
  - Configuração
  - Executando a Aplicação
- Estrutura do Projeto
- Decisões de Design e Boas Práticas
- Funcionalidades Extras
- Melhorias Futuras

## Funcionalidades

- Monitora continuamente um ticker de ação da B3 especificado.
- Envia notificações por e-mail quando o preço da ação cai até ou abaixo do ponto de compra definido.
- Envia notificações por e-mail quando o preço da ação sobe até ou acima do ponto de venda definido.
- Configuração via `AppSettings.json` para destino de e-mail e configurações do servidor SMTP.
- Validação de entrada dos argumentos de linha de comando.
- Envia e-mail de confirmação na configuração para verificar o funcionamento do envio de e-mails.

## Tecnologias Utilizadas

- **C#**: Linguagem principal utilizada.
- **.NET**: Framework para construção da aplicação de console.
- **Brapi API**: Utilizada para buscar cotações de ações, devido ao plano gratuito e cobertura da B3.
- **IHttpClientFactory**: Para gerenciamento eficiente e reutilização de conexões HTTP, evitando `SocketException`.
- **Cliente SMTP**: Para envio das notificações por e-mail.
- **Microsoft.Extensions.Configuration**: Para manipulação das configurações da aplicação a partir do `AppSettings.json`.
- **Microsoft.Extensions.Hosting**: Suporte a injeção de dependência e gerenciamento do host.

# Como Começar

Siga as instruções abaixo para ter o projeto funcionando para testes.

## Arquivo .exe

Baixe o arquivo .exe contido no no ultimo commit da branch master e execute-o em seu computador;

## Clonando Repositório

### Pré-requisitos

- .NET SDK (compatível com .NET 6 ou superior)
- Uma chave de API do Brapi (há um plano gratuito disponível)
- Uma conta de e-mail configurada para envio SMTP (ex: Gmail com senha de app habilitada)

### Instalação

1. **Clone o repositório:**

    ```bash
    git clone https://github.com/GbrLomenha/stock-quote.git
    cd stock-quote
    ```

2. **Restaure os pacotes NuGet:**

    ```bash
    dotnet restore
    ```

### Configuração

Antes de executar a aplicação, configure o arquivo `AppSettings.json` com sua chave de API e dados de e-mail.

Crie um arquivo `AppSettings.json` na raiz do projeto com a seguinte estrutura:

```json
{
    "ApiKey": "BRAPI_API_KEY",
    "EmailConfig": {
        "SmtpServer": "smtp.seu-provedor-email.com",
        "SmtpPort": 587,
        "SmtpUser": "seu-email@exemplo.com",
        "SmtpPassword": "sua-senha-email",
        "From": "seu-email@exemplo.com",
        "To": "destinatario@exemplo.com"
    },
    "VerificationInterval": "30"
}
```

- `ApiKey`: Sua chave de API do Brapi.
- `EmailConfig`:
    - `SmtpServer`: Endereço do servidor SMTP (ex: `smtp.gmail.com` para Gmail).
    - `SmtpPort`: Porta do servidor SMTP (ex: `587` para TLS/STARTTLS).
    - `SmtpUser`: E-mail utilizado para enviar os alertas.
    - `SmtpPassword`: Senha do e-mail remetente. Se usar Gmail, pode ser necessário gerar uma [Senha de App](https://support.google.com/accounts/answer/185833?hl=pt-BR).
    - `From`: E-mail do remetente que aparecerá nos alertas.
    - `To`: E-mail de destino para os alertas.
- `VerificationInterval`: Intervalo em minutos entre as verificações de cotação. Para o plano gratuito do Brapi, recomenda-se um mínimo de 30 minutos devido à frequência de atualização dos dados.

### Executando a Aplicação

A aplicação é um console que recebe três argumentos: o símbolo do ticker, o ponto de compra e o ponto de venda.

```bash
dotnet run <SÍMBOLO> <PONTO_COMPRA> <PONTO_VENDA>
```

**Exemplo:**

```bash
dotnet run PETR4 22.67 22.59
```

Esse comando irá monitorar o ativo `PETR4` e enviar alertas de compra se o preço cair até ou abaixo de R$22,59, e alertas de venda se o preço subir até ou acima de R$22,67.

## Estrutura do Projeto

- **`Program.cs`**: Ponto de entrada da aplicação, responsável por configurar o host, as configurações, a injeção de dependências e iniciar o monitoramento.
- **`Quotation.Models`**: Modelos de dados para respostas da API (`ApiResponse`, `ApiRootResponse`), cotações (`StockQuotation`) e configuração de e-mail (`EmailConfig`).
- **`Quotation.Services`**: Lógica principal da aplicação:
    - **`StockQuoteService.cs`**: Busca cotações na API Brapi e monitora os pontos de preço.
    - **`EmailService.cs`**: Gerencia o envio de e-mails de notificação, incluindo compra, venda e confirmação.
    - **`InputTreatment.cs`**: Classe estática para validação e sanitização dos argumentos de entrada.

## Decisões de Design e Boas Práticas

- **Injeção de Dependência**: Utiliza o DI nativo do .NET para `IHttpClientFactory`, `StockQuoteService` e `EmailService`, promovendo modularidade e testabilidade.
- **IHttpClientFactory**: Gerencia instâncias de `HttpClient`, evitando problemas como exaustão de sockets e melhorando o uso de recursos.
- **Separação de Responsabilidades**: O envio de e-mails está encapsulado em `EmailService`, facilitando a manutenção e reutilização.
- **Decimal para Cálculos Financeiros**: O tipo `decimal` é utilizado para garantir precisão em cálculos monetários.
- **Gerenciamento de Configuração**: As configurações são carregadas do `AppSettings.json` via `IConfiguration`, permitindo fácil modificação sem recompilar.
- **Tratamento de Erros**: Inclui tratamento básico de erros para requisições à API e envio de e-mails, com mensagens informativas no console.
- **Monitoramento Contínuo**: O programa roda em loop contínuo, verificando periodicamente os preços das ações.

## Funcionalidades Extras

- **E-mail de Confirmação na Configuração**: Um e-mail é enviado ao iniciar a aplicação para confirmar que o envio de e-mails está funcionando.
- **Conteúdo de E-mail Formatado**: Notificações por e-mail são formatadas em HTML para melhor leitura e incluem informações relevantes da ação e um ícone (se disponível).
- **Validação de Entrada**: Validação abrangente dos argumentos de linha de comando para garantir uso correto e evitar erros.

## Melhorias Futuras

- **WebSockets para Dados em Tempo Real**: Integrar com uma API que suporte WebSockets para atualização em tempo real das cotações, pois requisições HTTP são menos eficientes para monitoramento contínuo. (Nota: Limitações atuais da API exigem polling via HTTP)
- **Log de Erros Robusto**: Implementar um sistema de logs mais sofisticado (ex: Serilog, NLog) para melhor rastreamento e depuração.
- **Autenticação Segura para E-mail**: Explorar métodos mais seguros de autenticação para envio de e-mails (ex: Codigo gerado e enviado no email de inicialização).
- **Recarregamento Dinâmico de Configuração**: Permitir que o `AppSettings.json` seja recarregado em tempo de execução sem reiniciar a aplicação.
- **Monitoramento de Múltiplos Ativos**: Permitir monitorar vários ativos simultaneamente.
- **Integração com Banco de Dados**: Armazenar histórico de alertas e configurações em um banco de dados.
- **Interface de Usuário**: Desenvolver uma interface gráfica ou web para facilitar a configuração e o monitoramento.
