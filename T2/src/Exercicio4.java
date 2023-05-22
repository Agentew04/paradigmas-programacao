import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class Exercicio4 {
    public static void main(String[] args) {
        Scanner scan = new Scanner(System.in);
        List<Float> numeros = new ArrayList<>();

        for (int i = 0; i < 10; i++) {
            numeros.add(scan.nextFloat());
        }

        long intervalo1 = numeros.stream().filter(x -> x>=0 && x<=25).count();
        System.out.printf("Numeros entre [0,25]: %d\n", intervalo1);

        long intervalo2 = numeros.stream().filter(x -> x>=26 && x<=50).count();
        System.out.printf("Numeros entre [26,50]: %d\n", intervalo2);

        long intervalo3 = numeros.stream().filter(x -> x>=51 && x<=75).count();
        System.out.printf("Numeros entre [51,75]: %d\n", intervalo3);

        long intervalo4 = numeros.stream().filter(x -> x>=76 && x<=100).count();
        System.out.printf("Numeros entre [76,100]: %d\n", intervalo4);
    }
}
