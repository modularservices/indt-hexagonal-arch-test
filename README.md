# Teste INDT - Microsserviços .NET com arquitetura hexagonal

## Overview

O cenário proposto foi ter 2 microsserviços, 1 de propostas de seguros e outro, responsável por fazer as contratações quando a proposta estiver aprovada.

## O que o projeto contempla

- Domain-Driven Design (Propostas e Contratações sao subdomínios do dominio Seguros e PropostaService, ContratacaoService seriam bounded contexts)<br>
- Principios SOLID<br>
- REST API + Event-Driven usando RabbitMQ<br>
- Clean Code

## Fluxo:

1. A proposta é criada via endpoint REST em PropostaService com status 'Em Análise'.
2. Outro endpoint permite atualizar o status da proposta para 'Aprovada'.
3. Ao fazer a aprovação, é enviada mensagem com id da proposta a uma fila do RabbitMQ.
4. ContratacaoService consome a mensagem dessa fila.
5. Ao consumir a mensagem, ele persiste em banco próprio o id da proposta e data da contratação.
6. Faz chamada no endpoint de PropostaService pra mudar o status da proposta para 'Contratada'.

<br>

![Hexagonal](docs/images/arquitetura.png)

## Ferramentas

Docker<br>
SQL Server containerizado<br>
RabbitMQ containerizado

## Instruções Gerais

1o: subir containers (docker-compose.yaml na raiz do repo).

```docker compose up -d```

<br>

**Build das aplicações**

Entrar nas pastas das 2 solutions e

```dotnet build -c Release```

<br>

**Execução**

PropostaService: entrar na pasta Indt.Teste.PropostaService\src\Indt.Teste.PropostaService.Api

ContratacaoService: entrar na pasta Indt.Teste.ContratacaoService\src\Indt.Teste.ContratacaoService.Api

```dotnet run```

<br>

**Sequência**

Rodar 1o PropostaService. Ao rodar a aplicação pela primeira vez, ela irá conectar ao SQL Server e fazer o setup inicial (criar bancos, tabelas, seed). De toda forma, os scripts foram deixados em /db-scripts do repo.

Se estiver em ambiente de desenvolvimento, os bancos serão criados com sufixo Dev.
Ao rodar testes de integração em PropostaService, o banco terá o sufixo Test.

Atenção aos appsettings.json para conexões.

### PropostaService

![PropostaServiceConfigs](docs/images/proposta-service-configs.png)

### ContratacaoService

![ContratacaoServiceConfigs](docs/images/contratacao-service-configs.png)
