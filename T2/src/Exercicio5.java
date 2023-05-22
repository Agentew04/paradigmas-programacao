import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class Exercicio5 {
    private static class Produto{
        public int codigo;
        public float preco;
    }

    public static void main(String[] args) {
        Scanner scan = new Scanner(System.in);
        float aumento = 0.2f;

        List<Produto> produtos = new ArrayList<>();
        while (true){
            int codigo = scan.nextInt();
            if(codigo < 0)
                break;
            float preco = scan.nextFloat();
            Produto p = new Produto();
            p.codigo = codigo;
            p.preco = preco * (1+aumento);
            produtos.add(p);
        }

        float mediaPreco = produtos.stream()
                .map(x -> x.preco)
                .reduce(0.0f, Float::sum) / produtos.size();
        System.out.printf("Media dos novos preços: %f", mediaPreco);
    }
}
