# Paradigmas de Programação

Este repositório contém as minhas implementações dos trabalhos da disciplina
de **Paradigmas de Programação** da UFSM.

## Trabalho 1

O trabalho 1 consiste da implementação de uma biblioteca virtual usando banco
de dados em Java. 
Mais detalhes sobre a implementação e os requisitos do trabalho original
estão encontrados na [pasta do projeto](./T1/README.md)

Abaixo se encontra a descrição original do trabalho:

<details>
    <summary><b>Descrição</b></summary>

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

</details>

## Trabalho 2


## Trabalho 3


