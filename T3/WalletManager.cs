using BolsaValores.Exceptions;

namespace BolsaValores; 

public class WalletManager {
    private readonly List<Wallet> _wallets = new();
    
    public void AddWallet(string login) {
        if (string.IsNullOrWhiteSpace(login))
            throw new InvalidWalletNameException("O login não pode ser vazio ou ser apenas espaços!");
        
        if (_wallets.Any(x => x.Login == login)) {
            throw new WalletExistsException($"Uma carteira com o login {login} já existe!");
        }

        Wallet w = new Wallet() {
            Login = login
        };
        _wallets.Add(w);
    }

    public Wallet GetWallet(string login) {
        Wallet? w = _wallets.FirstOrDefault(x => x.Login == login);
        if (w is null)
            return w;
            // todo throw new inexistent wallet exception
        return w;
    }
}