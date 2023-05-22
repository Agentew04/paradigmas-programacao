# Paradigmas de Programação

## Trabalho 1 - Biblioteca Virtual


### Comentários

Para este trabalho eu quis diminuir o acoplamento da interface visual e a
lógica de negócios, então as principais classes são a `Library`, representando
a entidade da biblioteca, e a classe `Menu`, que gerencia a entrada e saída
de dados para o usuário.

Também optei por utilizar **MySQL** para todos os dados, não apenas os Livros.
O gerenciamento é feito através da `CollectionBase<T>` e suas classes derivadas:

* `BookCollection`
* `UserCollection`
* `LoanCollection`
* `ReservationCollection`

Também tentei ao máximo usar JavaDoc para descrever parâmetros e funções,
porém nem todas elas estão documentadas devido a falta de tempo.

O projeto foi feito usando *IntelliJ IDEA*, e o `.jar` do conector *MySQL*
está presente na pasta para conveniência.


### Descrição original

Em Java, crie um programa para controle de uma biblioteca que atenda aos seguintes requisitos mínimos:

* Os dados de livro devem ser armazenados em uma base de dados relacional (postgres, mysql, sqlserver etc.)

* Os dados de outras entidades podem ser armazenados em listas em memória (quem desejar, pode armazená-los no banco também).

* Deve ser possível cadastrar livros: incluir livros, alterar livros, excluir livros, listar livros, buscar livros pelo ISBN, por parte do título e pela editora.

    * Cada livro deve conter informações acerca de: autores, edição, editora, nome, ano.

* Deve ser possível cadastrar usuários: incluir usuários, excluir usuários, listar usuários, buscar usuários por parte do nome e pelo login.

* Deve ser possível efetuar empréstimos: emprestar livros, renovar empréstimo, devolver livros, listar empréstimos, buscar empréstimos por livro (ISBN e título), por exemplar de livro e por usuário.

* Deve ser possível efetuar reservas de livros: solicitar reserva de livro, cancelar reserva de livro.

* Deve ser possível pagar multas pendentes.

* Os alunos podem retirar até 3 livros, por uma semana. Os professores podem retirar até 5 livros, por 15 dias.

* Os empréstimos são renovados pelo mesmo período permitido para o usuário em questão, não sendo permitidos, no entanto, renovações de livros que estão com o exemplar reservado por algum usuário.

* Usuários com multas só podem ter um único livro emprestado.

* Não é permitido aos usuários com multas reservarem livros.

* A multa é de R$ 1,00 por dia (útil ou não útil)
