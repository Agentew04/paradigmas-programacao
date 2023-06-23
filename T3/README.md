# Paradigmas de Programação

## Trabalho 3 - Bolsa de Valores - Final

### Comentários

O terceiro trabalho consiste basicamente de criar um _gerenciador
de carteiras de investimentos_ para uma bolsa de valores fictícia.
Não foi necessário simular em tempo real uma bolsa, apenas
a valorização de ativos de uma carteira com base em dados
pré-definidos no banco de dados.

Ao contrário dos trabalhos anteriores, neste eu tive liberdade
de escolher uma linguagem de programação. Assim eu escolhi 
a que eu tenho mais afinidade, que é **C#**.

Eu implementei uma geração dinâmica de consultas SQL com **atributos**
e um extensivo uso de métodos de **reflexão**.

O banco de dados utilizado foi o MariaDB porém usando o
conector [MySql](https://www.nuget.org/packages/MySqlConnector) disponível 
no NuGet.

### Descrição original

Crie uma base de dados de ativos financeiros (cujos dados não precisam ser populados pelo usuário final - podem ser inseridos manualmente no cliente do banco de dados). Cada ativo é descrito por um ticker (NASD11, HGLG11, AAPL34, VALE3) e pelo nome da empresa. Os ativos podem ser de diferentes tipos. Para este trabalho, os tipos pré-definidos são: ações, BDRs, ETFs e fundos imobiliários. Para cada dia, cada ativo deverá ter uma cotação (valor) na base de dados, representada pelo valor de fechamento do ativo no dia. Desta forma, crie três tabelas: ativos. tipos e cotações, para armazenar os dados necessários.

Crie também um programa, na linguagem orientada a objetos de sua escolha, que permita a um usuário criar uma carteira de investimentos. Nesta carteira, ele pode incluir ou excluir ativos (compra e venda), informando o ticker do ativo, a quantidade e o preço pago por ação. A carteira de investimentos deve ser armazenada em um banco de dados. O sistema deve usar tratamento de exceções para aquelas mais comuns que possam ocorrer na execução do sistema.

Deve ser possível também:

1. Para um determinado período escolhido pelo usuário, mostrar a rentabilidade total da carteira, os percentuais de ganho/perda de cada ativo na carteira, o valor de ganho/perda e o preço médio de aquisição de cada ativo.,

2. Permitir também mostrar a rentabilidade por tipo de ativo, o percentual em cada tipo, comparando a rentabilidade obtida com um benchmark (a base de dados deve ser usada para armazenar as cotações diárias dos benchmarks usados - seja nas tabelas existente, seja criando nova(s) tabela(s)):

- Ações: Benchmark: IBOV

- BDRs: BDRX

- Fundos imobiliários: IFIX

- ETFs: Nacionais: IBOV, Internacionais: SP500 (SPX).