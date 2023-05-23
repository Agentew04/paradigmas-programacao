namespace BolsaValores.Exceptions; 

public class InvalidWalletNameException : Exception{
    public InvalidWalletNameException(string msg) : base(msg){
        
    }

    public InvalidWalletNameException(string msg, Exception inner): base(msg, inner) {
        
    }
}