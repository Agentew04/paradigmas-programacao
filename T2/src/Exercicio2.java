import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class Exercicio2 {
    public static void main(String[] args) {
        Scanner scan = new Scanner(System.in);
        List<Integer> votos = new ArrayList<>();

        while (true){
            int voto = scan.nextInt();
            if(voto == 0)
                break;
            votos.add(voto);
        }

        long votosC1 = votos.stream().filter(x -> x==1).count();
        System.out.printf("Votos para candidato 1: %d\n", votosC1);

        long votosC2 = votos.stream().filter(x -> x==2).count();
        System.out.printf("Votos para candidato 2: %d\n", votosC2);

        long votosC3 = votos.stream().filter(x -> x==3).count();
        System.out.printf("Votos para candidato 3: %d\n", votosC3);

        long votosC4 = votos.stream().filter(x -> x==4).count();
        System.out.printf("Votos para candidato 4: %d\n", votosC4);

        long votosNulos =  votos.stream().filter(x -> x==5).count();
        System.out.printf("Votos nulos: %d\n", votosNulos);

        long votosBrancos =  votos.stream().filter(x -> x==6).count();
        System.out.printf("Votos em branco: %d\n", votosBrancos);
    }
}
