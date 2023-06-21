namespace BolsaValores.Storage; 

/// <summary>
/// Interface que define métodos básicos para a manipulação
/// de qualquer tipo de dado.
/// </summary>
/// <typeparam name="T">O tipo de dado a ser manipulado</typeparam>
public interface ICrud<T> {
    
    /// <summary>
    /// Adiciona apenas um elemento à coleção.
    /// </summary>
    /// <param name="t">O elemento que será adicionado</param>
    void Add(T t);
    
    /// <summary>
    /// Adiciona muitos elementos ao banco de dados.
    /// </summary>
    /// <param name="t">Uma lista com todos elementos a serem adicionados</param>
    void Add(IEnumerable<T> t);
    
    /// <summary>
    /// Seleciona todos os itens de uma coleção.
    /// </summary>
    /// <returns>Todos os itens da coleção</returns>
    IEnumerable<T> Select();
    
    /// <summary>
    /// Seleciona muitos elementos da coleção com base numa condição.
    /// </summary>
    /// <param name="predicate">A condição a ser satisfeita para selecionar um item</param>
    /// <returns>Os itens que foram selecionados</returns>
    IEnumerable<T> Select(Func<T,bool> predicate);
    
    /// <summary>
    /// Substitui qualquer item que satisfaz <paramref name="predicate"/> com
    /// o item <paramref name="t"/>. Recomendado que a função condição seja
    /// satisfeita apenas para um item, senão o item será duplicado.
    /// </summary>
    /// <param name="predicate">A condição para um item ser atualizado</param>
    /// <param name="t">O item que será colocado no lugar</param>
    /// <returns>Quantos itens foram atualizados</returns>
    int Update(Func<T,bool> predicate, T t);
    
    /// <summary>
    /// Atualiza os itens que satisfazem <paramref name="predicate"/> de acordo
    /// com a função <paramref name="updater"/>;
    /// </summary>
    /// <param name="predicate">A condição para itens serem atualizados.</param>
    /// <param name="updater">A função que atualiza os itens</param>
    /// <returns>Quantos itens foram modificados</returns>
    int Update(Func<T,bool> predicate, Func<T,T> updater);

    /// <summary>
    /// Remove um elemento da coleção 
    /// </summary>
    /// <param name="t">O elemento que vai ser removido</param>
    /// <returns>A quantidade de linhas afetadas</returns>
    int Remove(T t);
    
    /// <summary>
    /// Remove muitos elementos satisfazem o predicado.
    /// </summary>
    /// <param name="predicate">A condicação para exclusão</param>
    /// <returns>Quantos elementos foram removidos</returns>
    int Remove(Func<T,bool> predicate);
}