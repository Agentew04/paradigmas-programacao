namespace BolsaValores.Exceptions; 

public class WalletExistsException : Exception{
    
    public WalletExistsException(string msg) : base(msg){
        
    }

    public WalletExistsException(string msg, Exception inner): base(msg, inner) {
        
    }
}