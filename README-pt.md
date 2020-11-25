### Selecionar Idioma :   
- [[Inglês]](https://github.com/virgnet/api-modelo-asp.net-core/blob/main/README.md)
- [[Português]](https://github.com/virgnet/api-modelo-asp.net-core/blob/main/README-pt.md)

### Introdução
Projeto modelo para estudos referente a arquitetura backend usando C# em .NET Core

### Tecnologias utilizadas
 - .NET 5.0
 
### Padrões, Princípios E Filosofias
No projeto foi utilizado o padrão de arquitetura DDD.

## Arquitetura
A api contém 4 projetos sendo os 3 primeiros como principais:
  1. ApiRest
  2. Domain
  3. Infraestructure
  4. Shared

#### 1 ApiRest
Projeto responsável pela interface de comunicação, ou seja, por receber as requisições e direcionar a execução.

#### 2 Domain
Projeto responsável pelas entidades.

#### 3 Infraestructure
Projeto responsável pela requisição ao banco de dados.

#### 4 Shared
O Projeto Shared apesar de não ser colocado como principal, ele serve de apoio para todos os outros. 
Este contém três subdiretórios.
  - Commands
  - Notifications
  - Validation
