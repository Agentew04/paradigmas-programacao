using System.Runtime.Serialization;

namespace BolsaValores.Exceptions; 

public class MissingAttributeException : Exception{
    public MissingAttributeException(string msg) : base(msg){
        
    }

    public MissingAttributeException(string msg, Exception inner): base(msg, inner) {
        
    }
    
}