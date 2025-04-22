# Task Management (Gerenciamento de Tarefas)

Este reposit�rio cont�m um projeto com uma solu��o de gerenciamento de tarefas.  

## �ndice

- [Descri��o do Projeto](#descri��o-do-projeto)
- [Funcionalidades](#funcionalidades)
- [Arquitetura](#arquitetura)
- [Testes](#testes)
- [Instru��es](#instru��es)
- [Questionamento](#questionamento)
- [Melhorias](#melhorias)


---

## Descri��o do Projeto

Este projeto foi desenvolvido para demonstrar uma aplica��o focada no backend para o gerenciamento de tarefas, disponibilizando todas as funcionalidades em forma de API.  
Este projeto utiliza padr�es de arquitetura como DDD (Domain-Driven Design) visando uma separa��o clara entre as camadas de apresenta��o, dom�nio e infraestrutura.  

Todo desenvolvimento foi organizado com issues e os commits associados pelo projeto github:
[Github Project](https://github.com/users/vldmatos/projects/2/views/1)

## Funcionalidades

1. **Listagem de Projetos** - listar todos os projetos do usu�rio
2. **Visualiza��o de Tarefas** - visualizar todas as tarefas de um projeto espec�fico
3. **Cria��o de Projetos** - criar um novo projeto
4. **Cria��o de Tarefas** - adicionar uma nova tarefa a um projeto
5. **Atualiza��o de Tarefas** - atualizar o status ou detalhes de uma tarefa
6. **Remo��o de Tarefas** - remover uma tarefa de um projeto

## Arquitetura

Abaixo um desenho da arquitetura do projeto
![001](assets/001.png)

Toda Stack com suporte a container e orquestra��o com Aspire.
![003](assets/003.png)

### Projetos das arquitetura
Projetos que compoem a arquitetura da solu��o:
- **Aspire Orchestration**: Respons�vel pela orchestra��o dos projetos e containers.
- **Docker**: Plataforma de containers para execu��o dos projetos e containers de suporte.
- **.Net 9 Minimal API**: Projeto de API.
- **.Net 9 Blazor App**: Web para utilizar a API.
- **.Net 9 Worker Services**: Projeto de Infraestrutura para aplicar as migra��es de banco de dados.
- **Container PostgresSQL**: Container com banco de dados de projetos e volume local.

### Componentes
Este projeto utiliza alguns componentes para facilitar o desenvolvimento e a manuten��o:
- **EF Core**: ORM para manipula��o de contexto de dados.
- **FluentValidation**: Para valida��o de dados das entidades.
- **Coverlet**: Para gera��o de dados de cobertura de testes.
- **Scalar**: Para documenta��o de APIs.
- **Tailwindcss**: Para o projeto Web.
- **OpenTelemetry**: Para monitoramento e rastreamento de chamadas entre os servi�os.

### Tools
Alguns ferramentas n�o sao obrigat�rias, mas foram utilizadas no desenvolvimento:
- **EF Tools**: Para gera��o de migra��es e manipula��o do banco de dados.
- **Dotnet Report Generator**: Para gera��o de relat�rios de cobertura de testes.

### Pre-requisitos T�cnicos
- .Net 9 SDK e Runtime
- Visual Studio 2022 ou VS Code
- Docker Desktop
- Git

### Detalhes da Implementa��o
1. **Utiliza��o de Endpoints protegidos:**
    - A API utiliza autentica��o JWT para proteger os endpoints.
    - Existem dois endpoints que simulam os logins com usu�rios regular e manager, estes endpoints retornam o token JWT.
        - /account/login/regular
        - /account/login/manager

2. **Tratamento de Exce��es:**
    - A solu��o tem uma captura de exce��es global.
    - Toda exce��o de regra � retornada no padr�o problem details.

3. **Mapeamento de Endpoints Automaticos:**
    - Foi implementada uma interface de endpoint para mapear os endpoints automaticamente.
    
4. **Implementa��o de valida��es e regras na camada de dominio:**
    - A camada de dominio centraliza as regras de negocio e valida��es, para facilitar na manuten��o e testes.

5. **Implementa��o RateLimits:**
    - Os endpoints tem limites de requisi��es, para prote��o do back-end.

6. **Os projetos tem suporte a Build Container .NET:**
    - Execu��o de container, sem a necessidade de dockerfile.
    - Execu��o de build e execu��o de containers com o comando:
    ```bash
    dotnet publish --os linux --arch x64 /t:PublishContainer
    ```

7. **Migrations to Database:**
    - O projeto de Worker Services tem a responsabilidade de aplicar as migra��es no banco de dados.
    - Para gerar novas migra��es, caso modifique entidades ou estrutura de tabelas executar:
    ```bash
    dotnet ef migrations add XXX --project source/Configurations/Configurations.csproj --startup-project source/Infrastructure/Infrastructure.csproj --output-dir Data/Migrations
    ```

8. **O projeto Web Utiliza tailwindcss**
    - o css final ja foi gerado para aplica��o.
    - caso seja realizada alguma altera��o e precise gerar novamente o output executar o comando na pasta do projeto web:
    ```bash
    npx @tailwindcss/cli -i ./wwwroot/css/web.css -o ./wwwroot/css/web.output.css --watch --minify
    ```

9. **Endpoints Health:**
    - As aplica��es Web e API tem endpoints de health para verificar a integridade do sistema.
    - Podem ser utilizados por ferramentas de monitoramento
    - Endpoint:
        - /health
        - /alive
        - /health/detailed
    
## Testes
Este projeto possui testes unit�rios para garantir a qualidade do c�digo. 
Voce pode instalar a ferramenta de gera��o de relat�rios de cobertura de testes, caso n�o tenha instalado, se desejar com o comando:
```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
```
Para executar os testes com cobertura de c�digo, utilize o seguinte comando:
```bash
dotnet test --collect:"XPlat Code Coverage"
```
Isso ir� gerar o xml com dados de cobertura de teste.  
Ap�s isso, voc� pode gerar um relat�rio HTML com o seguinte comando:
```bash
reportgenerator "-reports:source\Tests\TestResults\[FolderGeneratedCoverlet]\coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html
```

***Para apenas executar os testes utilize o seguinte comando:***
```bash
dotnet test
```
![004](assets/004.png)

**Os relatorios foram gerados previamente, basta acessar a pagina:**
/coveragereport/index.html
![002](assets/002.png)

## Requisitos

1. **Prioridades de Tarefas:**
    - Cada tarefa deve ter uma prioridade atribu�da (baixa, m�dia, alta).
    - N�o � permitido alterar a prioridade de uma tarefa depois que ela foi criada.

2. **Restri��es de Remo��o de Projetos:**
    - Um projeto n�o pode ser removido se ainda houver tarefas pendentes associadas a ele.
    - Caso o usu�rio tente remover um projeto com tarefas pendentes, a API deve retornar um erro e sugerir a conclus�o ou remo��o das tarefas primeiro.

3. **Hist�rico de Atualiza��es:**
    - Cada vez que uma tarefa for atualizada (status, detalhes, etc.), a API deve registrar um hist�rico de altera��es para a tarefa.
    - O hist�rico de altera��es deve incluir informa��es sobre o que foi modificado, a data da modifica��o e o usu�rio que fez a modifica��o.

4. **Limite de Tarefas por Projeto:**
    - Cada projeto tem um limite m�ximo de 20 tarefas. Tentar adicionar mais tarefas do que o limite deve resultar em um erro.

5. **Relat�rios de Desempenho:**
    - A API deve fornecer endpoints para gerar relat�rios de desempenho, como o n�mero m�dio de tarefas conclu�das por usu�rio nos �ltimos 30 dias.
    - Os relat�rios devem ser acess�veis apenas por usu�rios com uma fun��o espec�fica de "gerente".

6. **Coment�rios nas Tarefas:**
    - Os usu�rios podem adicionar coment�rios a uma tarefa para fornecer informa��es adicionais.
    - Os coment�rios devem ser registrados no hist�rico de altera��es da tarefa.

## Intru��es

1. Clone este reposit�rio:
```bash
git clone https://github.com/vldmatos/Task-Management.git
```

2. Navegue at� o diret�rio do projeto:
```bash
cd Task-Management
```

3. Compile a solu��o:
```bash
dotnet build
```

4. Adicione a user-secret para gera�ao do token JWT no projeto API:
```bash
dotnet user-secrets init
dotnet user-secrets set "JWT_KEY" "9d51ba817b68463f8bf5d4be6e9ec8fd"
```

5. Abra a Solu��o do projeto, execute o projeto **Aspire**, aguarde a instala��o das imagens e execu��o dos containers.

6. Sera aberta toda a stack do projeto.
Com aspire voce poder� ver os logs e traces das aplica��es.  

7. O projeto API tem um link para docs Scalar, os endpoints podem ser executados diretamente da interface.

8. O banco de dados tem a interface do PG Admin para visualizar os dados do banco.

9. Execute o projeto Client para testar a aplica��o que consome as APIs.

10. Caso queira executar os enpoints diretamente, realize a chamada dos endpoints de simula��o de autentica��o para obter o token JWT.
```bash
curl -X GET http://localhost:5180/account/login/regular
curl -X GET http://localhost:5180/account/login/manager
```

11. Para demais endpoints envie o token JWT no header da requisi��o.
**Authorization: Bearer {token}**
```bash
curl -X GET http://localhost:5180/api/projects \
-H "Authorization: Bearer {token_jwt}" \
-H "Content-Type: application/json"
```


## Questionamento  

Esta se��o � destinada a registrar perguntas e pontos de esclarecimento para o Product Owner (PO) sobre futuras implementa��es ou melhorias no projeto.  

### Perguntas Abertas  
1. Existe a necessidade de implementar um sistema de notifica��es para os usu�rios sobre prazos ou atualiza��es de tarefas?  
2. H� planos para integrar o sistema com outras ferramentas de gerenciamento, como Jira ou Trello?  
3. Qual � a prioridade para a implementa��o de relat�rios personalizados al�m dos j� especificados?  
4. Existe a necessidade de escalar esta API para usu�rios de v�rios pa�ses?  
5. Alguma funcionalidade adicional � esperada para o gerenciamento de permiss�es de usu�rios?  


## Melhorias 

Esta se��o � destinada para sugerir melhorias a solu��o proposta.

### Sugest�es de Melhorias Funcionais
1. Sugest�o de aplicar uso de inteligencia artificial para analisar tarefas que podem ser dividas ou melhor detalhadas.
2. Implementar um sistema de arquivamento para projetos e tarefas conclu�das.  
3. Criar um dashboard com m�tricas em tempo real para os gerentes.  
4. Permitir a exporta��o de relat�rios em formatos como PDF ou Excel.  
5. Adicionar suporte a notifica��es push para atualiza��es importantes.
6. Melhorar o projeto Web, UX/UI e resposividade.

### Sugest�es de Melhorias T�cnicas
1. **Refatora��o de C�digo:**
   - Revisar e modularizar m�todos longos para melhorar a legibilidade e manuten��o.
   - Adotar um uso maior de interfaces para desacoplar depend�ncias e facilitar testes unit�rios.

2. **Melhoria na Arquitetura:**
   - Implementar uso de DTOs para trafegar menos informa��es.

3. **Otimiza��o de Desempenho:**
   - Implementar caching em endpoints de leitura com alta frequ�ncia de acesso, utilizando ferramentas como Redis.
   - Revisar consultas ao banco de dados para evitar N+1 queries e melhorar a efici�ncia.

4. **Monitoramento e Logs:**
   - Configurar alertas autom�ticos para identificar falhas ou degrada��o de servi�os.

5. **Seguran�a:**
   - Implementar autentica��o e autoriza��o baseadas em pol�ticas para maior controle de acesso.
   - Melhorar os padr�es de RateLimits implementados.

6. **Testes Integra��o e Carga:**
   - Incluir testes de integra��o para cen�rios cr�ticos.
   - Configurar testes de carga para avaliar o comportamento do sistema sob alta demanda.

7. **Pipeline de CI/CD:**
   - Automatizar a execu��o de testes e valida��o de c�digo no pipeline de CI/CD.
   - Adicionar etapas de verifica��o de seguran�a, como an�lise de vulnerabilidades em depend�ncias.

9. **Escalabilidade:**
   - Revisar a arquitetura para suportar balanceamento de carga horizontal em servi�os cr�ticos.
   - Adicionar suporte a filas de mensagens (ex.: RabbitMQ ou Azure Service Bus) para processar tarefas ass�ncronas caso necess�rio.
